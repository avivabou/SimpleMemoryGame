namespace MemoryGame
{
    public partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblFirstPlayerName = new System.Windows.Forms.Label();
            this.lblSecondPlayer = new System.Windows.Forms.Label();
            this.txtFirstPlayerName = new System.Windows.Forms.TextBox();
            this.txtSecondPlayerName = new System.Windows.Forms.TextBox();
            this.btnAgainstWho = new System.Windows.Forms.Button();
            this.lblBoardSize = new System.Windows.Forms.Label();
            this.btnBoardSize = new System.Windows.Forms.Button();
            this.btnStartGame = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblFirstPlayerName
            // 
            this.lblFirstPlayerName.AutoSize = true;
            this.lblFirstPlayerName.Location = new System.Drawing.Point(9, 7);
            this.lblFirstPlayerName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFirstPlayerName.Name = "lblFirstPlayerName";
            this.lblFirstPlayerName.Size = new System.Drawing.Size(92, 13);
            this.lblFirstPlayerName.TabIndex = 0;
            this.lblFirstPlayerName.Text = "First Player Name:";
            // 
            // lblSecondPlayer
            // 
            this.lblSecondPlayer.AutoSize = true;
            this.lblSecondPlayer.Location = new System.Drawing.Point(9, 35);
            this.lblSecondPlayer.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSecondPlayer.Name = "lblSecondPlayer";
            this.lblSecondPlayer.Size = new System.Drawing.Size(110, 13);
            this.lblSecondPlayer.TabIndex = 0;
            this.lblSecondPlayer.Text = "Second Player Name:";
            // 
            // txtFirstPlayerName
            // 
            this.txtFirstPlayerName.Location = new System.Drawing.Point(122, 5);
            this.txtFirstPlayerName.Margin = new System.Windows.Forms.Padding(2);
            this.txtFirstPlayerName.Name = "txtFirstPlayerName";
            this.txtFirstPlayerName.Size = new System.Drawing.Size(119, 20);
            this.txtFirstPlayerName.TabIndex = 1;
            this.txtFirstPlayerName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtSecondPlayerName
            // 
            this.txtSecondPlayerName.Enabled = false;
            this.txtSecondPlayerName.Location = new System.Drawing.Point(122, 35);
            this.txtSecondPlayerName.Margin = new System.Windows.Forms.Padding(2);
            this.txtSecondPlayerName.Name = "txtSecondPlayerName";
            this.txtSecondPlayerName.Size = new System.Drawing.Size(119, 20);
            this.txtSecondPlayerName.TabIndex = 1;
            this.txtSecondPlayerName.Text = "-computer-";
            this.txtSecondPlayerName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnAgainstWho
            // 
            this.btnAgainstWho.Location = new System.Drawing.Point(250, 32);
            this.btnAgainstWho.Margin = new System.Windows.Forms.Padding(2);
            this.btnAgainstWho.Name = "btnAgainstWho";
            this.btnAgainstWho.Size = new System.Drawing.Size(118, 23);
            this.btnAgainstWho.TabIndex = 2;
            this.btnAgainstWho.Text = "Against a Friend";
            this.btnAgainstWho.UseVisualStyleBackColor = true;
            this.btnAgainstWho.Click += new System.EventHandler(this.btnAgainstWho_Click);
            // 
            // lblBoardSize
            // 
            this.lblBoardSize.AutoSize = true;
            this.lblBoardSize.Location = new System.Drawing.Point(9, 66);
            this.lblBoardSize.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblBoardSize.Name = "lblBoardSize";
            this.lblBoardSize.Size = new System.Drawing.Size(61, 13);
            this.lblBoardSize.TabIndex = 0;
            this.lblBoardSize.Text = "Board Size:";
            // 
            // btnBoardSize
            // 
            this.btnBoardSize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnBoardSize.Location = new System.Drawing.Point(9, 82);
            this.btnBoardSize.Margin = new System.Windows.Forms.Padding(2);
            this.btnBoardSize.Name = "btnBoardSize";
            this.btnBoardSize.Size = new System.Drawing.Size(82, 56);
            this.btnBoardSize.TabIndex = 2;
            this.btnBoardSize.Text = "4x4";
            this.btnBoardSize.UseVisualStyleBackColor = false;
            this.btnBoardSize.Click += new System.EventHandler(this.btnBoardSize_Click);
            // 
            // btnStartGame
            // 
            this.btnStartGame.BackColor = System.Drawing.Color.LimeGreen;
            this.btnStartGame.Location = new System.Drawing.Point(250, 115);
            this.btnStartGame.Margin = new System.Windows.Forms.Padding(2);
            this.btnStartGame.Name = "btnStartGame";
            this.btnStartGame.Size = new System.Drawing.Size(118, 23);
            this.btnStartGame.TabIndex = 2;
            this.btnStartGame.Text = "Start!";
            this.btnStartGame.UseVisualStyleBackColor = false;
            this.btnStartGame.Click += new System.EventHandler(this.btnStartGame_OnClick);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(374, 151);
            this.Controls.Add(this.btnBoardSize);
            this.Controls.Add(this.btnStartGame);
            this.Controls.Add(this.btnAgainstWho);
            this.Controls.Add(this.txtSecondPlayerName);
            this.Controls.Add(this.txtFirstPlayerName);
            this.Controls.Add(this.lblBoardSize);
            this.Controls.Add(this.lblSecondPlayer);
            this.Controls.Add(this.lblFirstPlayerName);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Memory Game - Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFirstPlayerName;
        private System.Windows.Forms.Label lblSecondPlayer;
        private System.Windows.Forms.TextBox txtFirstPlayerName;
        private System.Windows.Forms.TextBox txtSecondPlayerName;
        private System.Windows.Forms.Button btnAgainstWho;
        private System.Windows.Forms.Label lblBoardSize;
        private System.Windows.Forms.Button btnBoardSize;
        private System.Windows.Forms.Button btnStartGame;
    }
}