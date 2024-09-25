namespace BattleshipClient
{
    partial class Homepage
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
            this.WelcomeLabel = new System.Windows.Forms.Label();
            this.StartBtn = new System.Windows.Forms.Button();
            this.OpenGamesBtn = new System.Windows.Forms.Button();
            this.ViewDataBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // WelcomeLabel
            // 
            this.WelcomeLabel.AutoSize = true;
            this.WelcomeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.WelcomeLabel.Location = new System.Drawing.Point(258, 39);
            this.WelcomeLabel.Name = "WelcomeLabel";
            this.WelcomeLabel.Size = new System.Drawing.Size(223, 31);
            this.WelcomeLabel.TabIndex = 0;
            this.WelcomeLabel.Text = "Welcome, Gamer";
            // 
            // StartBtn
            // 
            this.StartBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.StartBtn.Location = new System.Drawing.Point(264, 123);
            this.StartBtn.Name = "StartBtn";
            this.StartBtn.Size = new System.Drawing.Size(217, 45);
            this.StartBtn.TabIndex = 1;
            this.StartBtn.Text = "Start";
            this.StartBtn.UseVisualStyleBackColor = true;
            this.StartBtn.Click += new System.EventHandler(this.StartBtn_Click);
            // 
            // OpenGamesBtn
            // 
            this.OpenGamesBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.OpenGamesBtn.Location = new System.Drawing.Point(264, 204);
            this.OpenGamesBtn.Name = "OpenGamesBtn";
            this.OpenGamesBtn.Size = new System.Drawing.Size(217, 45);
            this.OpenGamesBtn.TabIndex = 2;
            this.OpenGamesBtn.Text = "Open Games";
            this.OpenGamesBtn.UseVisualStyleBackColor = true;
            this.OpenGamesBtn.Click += new System.EventHandler(this.OpenGamesBtn_Click);
            // 
            // ViewDataBtn
            // 
            this.ViewDataBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.ViewDataBtn.Location = new System.Drawing.Point(264, 287);
            this.ViewDataBtn.Name = "ViewDataBtn";
            this.ViewDataBtn.Size = new System.Drawing.Size(217, 45);
            this.ViewDataBtn.TabIndex = 3;
            this.ViewDataBtn.Text = "View Data";
            this.ViewDataBtn.UseVisualStyleBackColor = true;
            this.ViewDataBtn.Click += new System.EventHandler(this.ViewDataBtn_Click);
            // 
            // Homepage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(769, 403);
            this.Controls.Add(this.ViewDataBtn);
            this.Controls.Add(this.OpenGamesBtn);
            this.Controls.Add(this.StartBtn);
            this.Controls.Add(this.WelcomeLabel);
            this.Name = "Homepage";
            this.Text = "Homepage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label WelcomeLabel;
        private System.Windows.Forms.Button StartBtn;
        private System.Windows.Forms.Button OpenGamesBtn;
        private System.Windows.Forms.Button ViewDataBtn;
    }
}