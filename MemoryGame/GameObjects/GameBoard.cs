using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MemoryGame
{
    public class GameBoard
    {
        private readonly int r_ComputerRevealingIterval = 1000;
        private Cell[,] m_Board;
        private Point m_LastRevealed;
        private bool m_IsFirstReaveled = false;
        private int m_CurrentPlayerIndex;

        public Player[] Players { get; private set; }

        public Player CurrentPlayer
        {
            get
            {
                return Players[m_CurrentPlayerIndex];
            }
        }

        /// <summary>
        /// 2 matching cells were revealed.
        /// </summary>
        public event Action<Point, Point> CoupleFound;

        /// <summary>
        /// 2 non-matching cells were revealed.
        /// </summary>
        public event Action<Point, Point> NoMatch;

        /// <summary>
        /// Cell were revealed.
        /// </summary>
        public event Action<int, int, char> RevealedCell;

        /// <summary>
        /// Player done it's turn.
        /// </summary>
        public event Action OneTurnEnded;

        public event Action GameOver;

        /// <summary>
        /// GameBoard constructor.
        /// </summary>
        /// <param name="i_boardSize">Board size.</param>
        /// <param name="i_players">Players array.</param>
        public GameBoard(Size i_boardSize, Player[] i_players)
        {
            Players = i_players;
            m_Board = new Cell[i_boardSize.Width, i_boardSize.Height];
        }

        /// <summary>
        /// Shuffle the board and start the first turn.
        /// </summary>
        public void Start()
        {
            shuffle();
            UpdateTurn();
        }

        /// <summary>
        /// Check if there are unreveald cells.
        /// </summary>
        /// <returns>True if there are unrevealed cells, otherwise False.</returns>
        public bool IsThereHidden()
        {
            bool isHidden = false;

            foreach (Cell cell in m_Board)
            {
                if (cell.m_IsRevealed == false)
                {
                    isHidden = true;
                    break;
                }
            }

            return isHidden;
        }

        /// <summary>
        /// Reveal the chosen cell on board if it wasn't revealed.
        /// If the second revealing matchs the first, the current player score will be increased and turn won't change.
        /// </summary>
        /// <param name="i_x">X cordinate of the cell to reveal.</param>
        /// <param name="i_y">Y cordinate of the cell to reveal.</param>
        /// <returns>True if the given cordinates revealed new cell, otherwise False.</returns>
        public bool TryReveal(int i_x, int i_y)
        {
            bool success = true;

            if ((i_x == -1) || (i_y == -1))
            {
                success = false;
            }
            else if (m_Board[i_x, i_y].m_IsRevealed)
            {
                success = false;
            }
            else
            {
                reveal(i_x, i_y);
            }

            return success;
        }

        /// <summary>
        /// Update the turn to the next player turn.
        /// </summary>
        public void UpdateTurn()
        {
            m_CurrentPlayerIndex = (m_CurrentPlayerIndex + 1) % Players.Length;
            OneTurnEnded.Invoke();
            letComputerPlayerPlay();
        }

        /// <summary>
        /// Randomize the letters to be placed on the board.
        /// Also randomize the first player.
        /// </summary>
        private void shuffle()
        {
            Random rand = new Random();
            int lettersAmount = (m_Board.GetLength(0) * m_Board.GetLength(1)) / 2;
            List<char> letters = new List<char>();

            for (int i = 0; i < lettersAmount; i++)
            {
                char currentLetter = (char)('A' + i);
                letters.Add(currentLetter);
                letters.Add(currentLetter);
            }

            ///Puting the letters at board
            for (int i = 0; i < m_Board.GetLength(0); i++)
            {
                for (int j = 0; j < m_Board.GetLength(1); j++)
                {
                    int currentIndex = rand.Next(letters.Count);
                    m_Board[i, j].m_Letter = letters[currentIndex];
                    m_Board[i, j].m_IsRevealed = false;
                    letters.RemoveAt(currentIndex);
                }
            }

            m_CurrentPlayerIndex = rand.Next(Players.Length);
        }

        /// <summary>
        /// Revealing the cell with the given cordinates.
        /// If it is the second cell at same turn, the object will call the appropriate event.
        /// </summary>
        /// <param name="i_x">X cordinate of the cell to reveal.</param>
        /// <param name="i_y">Y cordinate of the cell to reveal.</param>
        private void reveal(int i_x, int i_y)
        {
            Point current = new Point(i_x, i_y);

            m_Board[i_x, i_y].m_IsRevealed = true;
            RevealedCell.Invoke(i_x, i_y, m_Board[i_x, i_y].m_Letter);
            m_IsFirstReaveled = !m_IsFirstReaveled;
            if (!m_IsFirstReaveled)
            {
                bool isMatch = m_Board[m_LastRevealed.X, m_LastRevealed.Y].m_Letter != m_Board[i_x, i_y].m_Letter;

                if (isMatch)
                {
                    areNotMatching(current);
                }
                else
                {
                    areMatching(current);
                }
            }
            else
            {
                m_LastRevealed = current;
            }
        }

        /// <summary>
        /// Change the players' turn, setting the cells as unrevealed and call the event NoMatch.
        /// </summary>
        /// <param name="i_current">The current cell cordinates were revealed.</param>
        private void areNotMatching(Point i_current)
        {
            m_Board[i_current.X, i_current.Y].m_IsRevealed = false;
            m_Board[m_LastRevealed.X, m_LastRevealed.Y].m_IsRevealed = false;
            if (NoMatch != null)
            {
                NoMatch.Invoke(m_LastRevealed, i_current);
            }
        }

        /// <summary>
        /// Raising the current player score and call the event CoupleFound.
        /// If there is no couple to reveal, this method call the event GameOver.
        /// </summary>
        /// <param name="i_current">The current cell cordinates were revealed.</param>
        private void areMatching(Point i_current)
        {
            m_Board[i_current.X, i_current.Y].m_IsRevealed = true;
            m_Board[m_LastRevealed.X, m_LastRevealed.Y].m_IsRevealed = true;
            Players[m_CurrentPlayerIndex].IncreaseScore();
            if (CoupleFound != null)
            {
                CoupleFound.Invoke(m_LastRevealed, i_current);
            }

            if (!IsThereHidden())
            {
                GameOver.Invoke();
            }
            else
            {
                OneTurnEnded.Invoke();
                letComputerPlayerPlay();
            }
        }

        /// <summary>
        /// Ask the computer for do its moves.
        /// </summary>
        private void letComputerPlayerPlay()
        {
            if (CurrentPlayer is ComputerPlayer)
            {
                ComputerPlayer computerPlayer = CurrentPlayer as ComputerPlayer;

                computerPlayer.SetThisTurn();
                for (int i = 0; i < 2; i++)
                {
                    System.Threading.Thread.Sleep(r_ComputerRevealingIterval);
                    Application.DoEvents();
                    Point selection = computerPlayer.GetCellsToReveal();
                    TryReveal(selection.X, selection.Y);
                }
            }
        }
    }
}
