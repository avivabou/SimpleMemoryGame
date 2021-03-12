using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace B20_Ex05_Aviv_207597980_Shir_307926238
{
    public partial class SettingsForm : Form
    {
        private readonly List<Size> r_BoardSizes;
        private int m_ChosenBoardSize = 0;

        public string FirstPlayerName
        {
            get
            {
                return txtFirstPlayerName.Text;
            }
        }

        public string SecondPlayerName
        {
            get
            {
                string name = txtSecondPlayerName.Text;
                if (!txtSecondPlayerName.Enabled)
                {
                    name = null;
                }

                return name;
            }
        }

        public Size BoardSize
        {
            get
            {
                return r_BoardSizes[m_ChosenBoardSize];
            }
        }
        
        /// <summary>
        /// SettingsForm constructor.
        /// </summary>
        public SettingsForm()
        {
            r_BoardSizes = new List<Size>();
            r_BoardSizes.Add(new Size(4, 4));
            r_BoardSizes.Add(new Size(4, 5));
            r_BoardSizes.Add(new Size(4, 6));
            r_BoardSizes.Add(new Size(5, 4));
            r_BoardSizes.Add(new Size(5, 6));
            r_BoardSizes.Add(new Size(6, 4));
            r_BoardSizes.Add(new Size(6, 5));
            r_BoardSizes.Add(new Size(6, 6));
            
            InitializeComponent();
        }

        /// <summary>
        /// Action for "Start Game" Button.
        /// It will close this form.
        /// </summary>
        /// <param name="sender">The object call this method.</param>
        /// <param name="e">Event arguments.</param>
        private void btnStartGame_OnClick(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Switch the second player (computer or human).
        /// </summary>
        /// <param name="sender">The object call this method.</param>
        /// <param name="e">Event arguments.</param>
        private void btnAgainstWho_Click(object sender, EventArgs e)
        {
            txtSecondPlayerName.Enabled = !txtSecondPlayerName.Enabled;
            if (txtSecondPlayerName.Enabled)
            {
                txtSecondPlayerName.Text = string.Empty;
                btnAgainstWho.Text = "Against Computer";
            }
            else
            {
                txtSecondPlayerName.Text = "-computer-";
                btnAgainstWho.Text = "Against a Friend";
            }
        }

        /// <summary>
        /// Switch the board size from the given sizes in BOARD_SIZES
        /// </summary>
        /// <param name="sender">The object call this method.</param>
        /// <param name="e">Event arguments.</param>
        private void btnBoardSize_Click(object sender, EventArgs e)
        {
            m_ChosenBoardSize = (m_ChosenBoardSize + 1) % r_BoardSizes.Count;
            Size boardSize = r_BoardSizes[m_ChosenBoardSize];
            btnBoardSize.Text = string.Format("{0}x{1}", boardSize.Width, boardSize.Height);
        }
    }
}
