namespace BattleshipClient
{
    partial class GamesViewer
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
            this.GamesBox = new System.Windows.Forms.ListBox();
            this.JoinBtn = new System.Windows.Forms.Button();
            this.RefreshBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // GamesBox
            // 
            this.GamesBox.FormattingEnabled = true;
            this.GamesBox.Location = new System.Drawing.Point(13, 13);
            this.GamesBox.Name = "GamesBox";
            this.GamesBox.Size = new System.Drawing.Size(310, 316);
            this.GamesBox.TabIndex = 0;
            // 
            // JoinBtn
            // 
            this.JoinBtn.Location = new System.Drawing.Point(348, 306);
            this.JoinBtn.Name = "JoinBtn";
            this.JoinBtn.Size = new System.Drawing.Size(75, 23);
            this.JoinBtn.TabIndex = 1;
            this.JoinBtn.Text = "Join";
            this.JoinBtn.UseVisualStyleBackColor = true;
            this.JoinBtn.Click += new System.EventHandler(this.JoinBtn_Click);
            // 
            // RefreshBtn
            // 
            this.RefreshBtn.Location = new System.Drawing.Point(348, 13);
            this.RefreshBtn.Name = "RefreshBtn";
            this.RefreshBtn.Size = new System.Drawing.Size(75, 23);
            this.RefreshBtn.TabIndex = 2;
            this.RefreshBtn.Text = "Refresh";
            this.RefreshBtn.UseVisualStyleBackColor = true;
            this.RefreshBtn.Click += new System.EventHandler(this.RefreshBtn_Click);
            // 
            // OpenGamesViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 344);
            this.Controls.Add(this.RefreshBtn);
            this.Controls.Add(this.JoinBtn);
            this.Controls.Add(this.GamesBox);
            this.Name = "OpenGamesViewer";
            this.Text = "OpenGamesViewer";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox GamesBox;
        private System.Windows.Forms.Button JoinBtn;
        private System.Windows.Forms.Button RefreshBtn;
    }
}