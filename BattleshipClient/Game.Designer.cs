namespace BattleshipClient
{
    partial class Game
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
            this.OpponentBoardBox = new System.Windows.Forms.PictureBox();
            this.BoardBox = new System.Windows.Forms.PictureBox();
            this.OpponentUsernameLabel = new System.Windows.Forms.Label();
            this.UsernameLabel = new System.Windows.Forms.Label();
            this.ChatDisplayBox = new System.Windows.Forms.ListBox();
            this.SendBtn = new System.Windows.Forms.Button();
            this.ChatBox = new System.Windows.Forms.TextBox();
            this.PlayerTurnLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.OpponentBoardBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BoardBox)).BeginInit();
            this.SuspendLayout();
            // 
            // OpponentBoardBox
            // 
            this.OpponentBoardBox.Location = new System.Drawing.Point(13, 13);
            this.OpponentBoardBox.Name = "OpponentBoardBox";
            this.OpponentBoardBox.Size = new System.Drawing.Size(250, 250);
            this.OpponentBoardBox.TabIndex = 0;
            this.OpponentBoardBox.TabStop = false;
            // 
            // BoardBox
            // 
            this.BoardBox.Location = new System.Drawing.Point(13, 269);
            this.BoardBox.Name = "BoardBox";
            this.BoardBox.Size = new System.Drawing.Size(250, 250);
            this.BoardBox.TabIndex = 1;
            this.BoardBox.TabStop = false;
            // 
            // OpponentUsernameLabel
            // 
            this.OpponentUsernameLabel.AutoSize = true;
            this.OpponentUsernameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.OpponentUsernameLabel.Location = new System.Drawing.Point(269, 13);
            this.OpponentUsernameLabel.Name = "OpponentUsernameLabel";
            this.OpponentUsernameLabel.Size = new System.Drawing.Size(70, 25);
            this.OpponentUsernameLabel.TabIndex = 2;
            this.OpponentUsernameLabel.Text = "label1";
            // 
            // UsernameLabel
            // 
            this.UsernameLabel.AutoSize = true;
            this.UsernameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.UsernameLabel.Location = new System.Drawing.Point(269, 269);
            this.UsernameLabel.Name = "UsernameLabel";
            this.UsernameLabel.Size = new System.Drawing.Size(70, 25);
            this.UsernameLabel.TabIndex = 3;
            this.UsernameLabel.Text = "label2";
            // 
            // ChatDisplayBox
            // 
            this.ChatDisplayBox.FormattingEnabled = true;
            this.ChatDisplayBox.Location = new System.Drawing.Point(587, 13);
            this.ChatDisplayBox.Name = "ChatDisplayBox";
            this.ChatDisplayBox.Size = new System.Drawing.Size(240, 472);
            this.ChatDisplayBox.TabIndex = 4;
            // 
            // SendBtn
            // 
            this.SendBtn.Location = new System.Drawing.Point(785, 498);
            this.SendBtn.Name = "SendBtn";
            this.SendBtn.Size = new System.Drawing.Size(42, 20);
            this.SendBtn.TabIndex = 5;
            this.SendBtn.Text = "Send";
            this.SendBtn.UseVisualStyleBackColor = true;
            this.SendBtn.Click += new System.EventHandler(this.SendBtn_Click);
            // 
            // ChatBox
            // 
            this.ChatBox.Location = new System.Drawing.Point(587, 498);
            this.ChatBox.Name = "ChatBox";
            this.ChatBox.Size = new System.Drawing.Size(192, 20);
            this.ChatBox.TabIndex = 6;
            // 
            // PlayerTurnLabel
            // 
            this.PlayerTurnLabel.AutoSize = true;
            this.PlayerTurnLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.PlayerTurnLabel.Location = new System.Drawing.Point(269, 498);
            this.PlayerTurnLabel.Name = "PlayerTurnLabel";
            this.PlayerTurnLabel.Size = new System.Drawing.Size(70, 25);
            this.PlayerTurnLabel.TabIndex = 7;
            this.PlayerTurnLabel.Text = "label1";
            // 
            // Game
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(839, 532);
            this.Controls.Add(this.PlayerTurnLabel);
            this.Controls.Add(this.ChatBox);
            this.Controls.Add(this.SendBtn);
            this.Controls.Add(this.ChatDisplayBox);
            this.Controls.Add(this.UsernameLabel);
            this.Controls.Add(this.OpponentUsernameLabel);
            this.Controls.Add(this.BoardBox);
            this.Controls.Add(this.OpponentBoardBox);
            this.Name = "Game";
            this.Text = "Game";
            ((System.ComponentModel.ISupportInitialize)(this.OpponentBoardBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BoardBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox OpponentBoardBox;
        private System.Windows.Forms.PictureBox BoardBox;
        private System.Windows.Forms.Label OpponentUsernameLabel;
        private System.Windows.Forms.Label UsernameLabel;
        private System.Windows.Forms.ListBox ChatDisplayBox;
        private System.Windows.Forms.Button SendBtn;
        private System.Windows.Forms.TextBox ChatBox;
        private System.Windows.Forms.Label PlayerTurnLabel;
    }
}