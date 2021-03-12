using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace MemoryGame
{
    public class GameBoardForm : Form
    {
        private readonly int r_ButtonSize = 60;
        private readonly int r_LabelHeight = 20;
        private readonly int r_ControlsDistance = 12;
        private readonly int r_HidingCellsInterval = 2000;
        private readonly Color r_FirstPlayerColor = Color.Lime;
        private readonly Color r_SecondPlayerColor = Color.Magenta;
        private readonly string r_ImagesURL = @"https://picsum.photos/80";
        private Dictionary<char, Bitmap> m_LettersToPics = new Dictionary<char, Bitmap>();
        private GameBoard m_GameBoard;
        private Button[,] m_BoardCellsButtons;
        private Label lblCurrentPlayer;
        private Label lblFirstPlayer;
        private Label lblSecondPlayer;
        private Timer m_TimerForRevealing;
        private bool m_IsDrawingNow = false; 

        /// <summary>
        /// When cell is revealed.
        /// Methods which will join to this event, will get known which cell is revealed and what was it letter.
        /// </summary>
        public event Action<int, int, char> CellRevealed;

        /// <summary>
        /// GameBoardForm constructor.
        /// </summary>
        /// <param name="i_boardSize">Board size.</param>
        /// <param name="i_firstPlayerName">First player name.</param>
        /// <param name="i_secondPlayerName">Second player name.</param>
        public GameBoardForm(Size i_boardSize, string i_firstPlayerName, string i_secondPlayerName)
        {
            Player[] players = new Player[2];
            Size clientSize;
            int additionHeight;

            m_GameBoard = new GameBoard(i_boardSize, players);
            m_GameBoard.NoMatch += waitAndHide;
            m_GameBoard.RevealedCell += showCell;
            m_GameBoard.OneTurnEnded += updateLabels;
            m_GameBoard.GameOver += showWinnerAndClose;
            players[0] = new Player(i_firstPlayerName);
            players[0].Color = r_FirstPlayerColor;
            if (i_secondPlayerName == null)
            {
                ComputerPlayer computerPlayer = new ComputerPlayer(i_boardSize);

                players[1] = computerPlayer;
                CellRevealed += computerPlayer.ShownCell;
                m_GameBoard.CoupleFound += computerPlayer.CoupleFound;
            }
            else
            {
                players[1] = new Player(i_secondPlayerName);
            }

            players[1].Color = r_SecondPlayerColor;
            MaximizeBox = false;
            MinimizeBox = false;
            Text = "Memory Game";
            SizeGripStyle = SizeGripStyle.Hide;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            StartPosition = FormStartPosition.CenterScreen;
            clientSize = initializeGameBoardButtons(i_boardSize);
            additionHeight = initializeLabels(clientSize.Height);
            clientSize.Height += additionHeight;
            ClientSize = clientSize;
            m_GameBoard.Start();
        }

        /// <summary>
        /// Initialize all buttons on form.
        /// </summary>
        /// <param name="i_boardSize">Board size.</param>
        /// <returns>The minimal client size for seeing all buttons.</returns>
        private Size initializeGameBoardButtons(Size i_boardSize)
        {
            Size clientSize = new Size();

            m_BoardCellsButtons = new Button[i_boardSize.Width, i_boardSize.Height];
            for (int i = 0; i < i_boardSize.Width; i++)
            {
                for (int j = 0; j < i_boardSize.Height; j++)
                {
                    m_BoardCellsButtons[i, j] = new Button();
                    m_BoardCellsButtons[i, j].Location = new Point(((i + 1) * r_ControlsDistance) + (i * r_ButtonSize), ((j + 1) * r_ControlsDistance) + (j * r_ButtonSize));
                    m_BoardCellsButtons[i, j].Height = r_ButtonSize;
                    m_BoardCellsButtons[i, j].Width = r_ButtonSize;
                    m_BoardCellsButtons[i, j].Tag = string.Format("{0},{1}", i, j);
                    m_BoardCellsButtons[i, j].Click += cellButton_Click;
                    m_BoardCellsButtons[i, j].DoubleClick += cellButton_Click;
                    Controls.Add(m_BoardCellsButtons[i, j]);
                }
            }

            clientSize.Width = ((i_boardSize.Width + 2) * r_ControlsDistance) + (i_boardSize.Width * r_ButtonSize);
            clientSize.Height = (i_boardSize.Height * r_ControlsDistance) + (i_boardSize.Height * r_ButtonSize);

            return clientSize;
        }

        /// <summary>
        /// Initialize all labels on form.
        /// </summary>
        /// <param name="i_startHeight">The first X cordinate for labels.</param>
        /// <returns>Minimal additional height for client size for seeing all labels.</returns>
        private int initializeLabels(int i_startHeight)
        {
            lblCurrentPlayer = new Label();
            lblCurrentPlayer.Text = "Current Player: ";
            lblCurrentPlayer.Tag = lblCurrentPlayer.Text;
            lblCurrentPlayer.Text += m_GameBoard.CurrentPlayer;
            lblCurrentPlayer.Location = new Point(r_ControlsDistance, i_startHeight + r_ControlsDistance);
            lblCurrentPlayer.Height = r_LabelHeight;
            lblCurrentPlayer.AutoSize = true;
            Controls.Add(lblCurrentPlayer);
            lblFirstPlayer = new Label();
            lblFirstPlayer.Text = string.Format("{0}: No pairs.", m_GameBoard.Players[0]);
            lblFirstPlayer.Location = new Point(r_ControlsDistance, lblCurrentPlayer.Location.Y + r_ControlsDistance + r_LabelHeight);
            lblFirstPlayer.BackColor = m_GameBoard.Players[0].Color;
            lblFirstPlayer.Height = r_LabelHeight;
            lblFirstPlayer.AutoSize = true;
            Controls.Add(lblFirstPlayer);
            lblSecondPlayer = new Label();
            lblSecondPlayer.Text = string.Format("{0}: No pairs.", m_GameBoard.Players[1]);
            lblSecondPlayer.Location = new Point(r_ControlsDistance, lblFirstPlayer.Location.Y + r_ControlsDistance + r_LabelHeight);
            lblSecondPlayer.BackColor = m_GameBoard.Players[1].Color;
            lblSecondPlayer.Height = r_LabelHeight;
            lblSecondPlayer.AutoSize = true;
            Controls.Add(lblSecondPlayer);

            return (r_LabelHeight * 3) + (r_ControlsDistance * 4);
        }

        /// <summary>
        /// Try to reveal the presented cell of the (button) sender.
        /// </summary>
        /// <param name="sender">The object call this method.</param>
        /// <param name="e">Event arguments.</param>
        private void cellButton_Click(object sender, EventArgs e)
        {
            if (!(m_GameBoard.CurrentPlayer is ComputerPlayer))
            {
                string tag = (sender as Button).Tag.ToString();
                int x = int.Parse(tag.Substring(0, tag.IndexOf(',')));
                int y = int.Parse(tag.Substring(tag.IndexOf(',') + 1));

                choosedCell(x, y);
            }
        }

        /// <summary>
        /// If there's no other drawing action, try to reveal the cell [i_x,i_y]
        /// </summary>
        /// <param name="i_x">X cordinate of the cell.</param>
        /// <param name="i_y">Y cordinate of the cell.</param>
        private void choosedCell(int i_x, int i_y)
        {
            if (!m_IsDrawingNow)
            {
                m_GameBoard.TryReveal(i_x, i_y);
            }
        }

        /// <summary>
        /// When 2 cells revealed and they ain't match, hide them after 2 seconds.
        /// Turning on m_IsDrawingNow until the cell will be hidden.
        /// </summary>
        /// <param name="i_firstSelction">First cell to hide.</param>
        /// <param name="i_secondSelction">Second cell to hide.</param>
        private void waitAndHide(Point i_firstSelction, Point i_secondSelction)
        {
            m_IsDrawingNow = true;
            m_TimerForRevealing = new Timer();
            m_TimerForRevealing.Enabled = true;
            m_TimerForRevealing.Interval = r_HidingCellsInterval;
            m_TimerForRevealing.Start();
            m_TimerForRevealing.Tick += (object sender, EventArgs e) => hideNow(i_firstSelction, i_secondSelction);
        }

        /// <summary>
        /// Hide now the given cells.
        /// When done, m_IsDrawingNow will be turned off.
        /// </summary>
        /// <param name="i_firstSelction">First cell to hide.</param>
        /// <param name="i_secondSelction">Second cell to hide.</param>
        private void hideNow(Point i_firstSelction, Point i_secondSelction)
        {
            Button firstSelction, secondSelction;

            m_TimerForRevealing.Enabled = false;
            m_TimerForRevealing.Stop();
            m_TimerForRevealing.Dispose();
            firstSelction = m_BoardCellsButtons[i_firstSelction.X, i_firstSelction.Y];
            firstSelction.Image = null;
            firstSelction.Tag = string.Format("{0},{1}", i_firstSelction.X, i_firstSelction.Y);
            firstSelction.BackColor = SystemColors.Control;
            secondSelction = m_BoardCellsButtons[i_secondSelction.X, i_secondSelction.Y];
            secondSelction.Image = null;
            secondSelction.BackColor = SystemColors.Control;
            secondSelction.Tag = string.Format("{0},{1}", i_secondSelction.X, i_secondSelction.Y);
            m_IsDrawingNow = false;
            m_GameBoard.UpdateTurn();
        }

        /// <summary>
        /// Revealing the given cell with the given letter.
        /// While revealing m_IsDrawingNow is on.
        /// </summary>
        /// <param name="i_x">X cordinate of the cell.</param>
        /// <param name="i_y">Y cordinate of the cell.</param>
        /// <param name="i_letter">The letter of the cell.</param>
        private void showCell(int i_x, int i_y, char i_letter)
        {
            m_IsDrawingNow = true;
            Button option = m_BoardCellsButtons[i_x, i_y];

            option.Image = GetImage(i_letter);
            option.BackColor = m_GameBoard.CurrentPlayer.Color;
            if (CellRevealed != null)
            {
                CellRevealed.Invoke(i_x, i_y, i_letter);
            }

            m_IsDrawingNow = false;
        }

        /// <summary>
        /// Showing message dialog to user with a winner message and question for playing again.
        /// </summary>
        private void showWinnerAndClose()
        {
            string winner;

            if (m_GameBoard.Players[0].Score == m_GameBoard.Players[1].Score)
            {
                winner = "It's a tie!";
            }
            else if (m_GameBoard.Players[0].Score > m_GameBoard.Players[1].Score)
            {
                winner = string.Format("The winner is {0}!", m_GameBoard.Players[0]);
            }
            else
            {
                winner = string.Format("The winner is {0}!", m_GameBoard.Players[1]);
            }

            winner = string.Format("Game Over!{0}{1}{0}Would you like to play again?", Environment.NewLine, winner);
            DialogResult = MessageBox.Show(winner, "Game Over!", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            Close();
        }

        /// <summary>
        /// Updating the labels with players information and current player turn.
        /// </summary>
        private void updateLabels()
        {
            lblFirstPlayer.Text = string.Format("{0}: {1} Pair(s)", m_GameBoard.Players[0].ToString(), m_GameBoard.Players[0].Score);
            lblSecondPlayer.Text = string.Format("{0}: {1} Pair(s)", m_GameBoard.Players[1].ToString(), m_GameBoard.Players[1].Score);
            lblCurrentPlayer.Text = string.Format("{0} {1}", lblCurrentPlayer.Tag.ToString(), m_GameBoard.CurrentPlayer.ToString());
            lblCurrentPlayer.BackColor = m_GameBoard.CurrentPlayer.Color;
        }

        /// <summary>
        /// Request from website to import picture.
        /// </summary>
        /// <param name="i_url">the URL of the image.</param>
        /// <returns>The image that imported from the given URL.</returns>
        private Bitmap imageWebRequest(string i_url)
        {
            System.Net.WebRequest request = System.Net.WebRequest.Create(i_url);
            System.Net.WebResponse response = request.GetResponse();
            System.IO.Stream responseStream = response.GetResponseStream();

            return new Bitmap(responseStream);
        }

        /// <summary>
        /// Comparing pixel by pixel both images.
        /// </summary>
        /// <param name="i_image1">First image to compare with.</param>
        /// <param name="i_image2">Second image to compare with.</param>
        /// <returns>True if both images are the same, otherwise false.</returns>
        private bool isTheSameImage(Bitmap i_image1, Bitmap i_image2)
        {
            bool isEquals = true;

            if ((i_image1.Width == i_image2.Width) && (i_image1.Height == i_image2.Height))
            {
                for (int i = 0; i < i_image1.Width; i++)
                {
                    for (int j = 0; j < i_image2.Height; j++)
                    {
                        if (i_image1.GetPixel(i, j) != i_image2.GetPixel(i, j))
                        {
                            isEquals = false;
                            break;
                        }
                    }
                }
            }
            else
            {
                isEquals = false;
            }

            return isEquals;
        }

        /// <summary>
        /// Importing new image for the new given letter.
        /// </summary>
        /// <param name="i_letter">The new letter.</param>
        private void newLetter(char i_letter)
        {
            Bitmap newImage;
            bool isExists;

            do
            {
                isExists = false;
                newImage = new Bitmap(imageWebRequest(r_ImagesURL), new Size(r_ButtonSize, r_ButtonSize));
                foreach (Bitmap image in m_LettersToPics.Values)
                {
                    if (isTheSameImage(image, newImage))
                    {
                        isExists = true;
                    }
                }
            }
            while (isExists);

            m_LettersToPics[i_letter] = newImage;
        }

        /// <summary>
        /// Getting the image that present the given letter.
        /// If there isn't image such that, the method creates a new one.
        /// </summary>
        /// <param name="i_letter">Some letter.</param>
        /// <returns>The image that present the given letter.</returns>
        public Bitmap GetImage(char i_letter)
        {
            if (!m_LettersToPics.ContainsKey(i_letter))
            {
                newLetter(i_letter);
            }

            return m_LettersToPics[i_letter];
        }
    }
}
