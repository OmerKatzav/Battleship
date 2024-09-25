namespace BattleshipClient
{
    partial class GameEditor
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
            this.BoardBox = new System.Windows.Forms.PictureBox();
            this.StartBtn = new System.Windows.Forms.Button();
            this.CustomShipBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.BoardXControl = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.BoardYControl = new System.Windows.Forms.NumericUpDown();
            this.RotateBtn = new System.Windows.Forms.Button();
            this.AddBtn = new System.Windows.Forms.Button();
            this.RemoveBtn = new System.Windows.Forms.Button();
            this.DeleteBtn = new System.Windows.Forms.Button();
            this.ShipListView = new System.Windows.Forms.ListView();
            this.DefaultBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.BoardBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BoardXControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BoardYControl)).BeginInit();
            this.SuspendLayout();
            // 
            // BoardBox
            // 
            this.BoardBox.Location = new System.Drawing.Point(258, 12);
            this.BoardBox.Name = "BoardBox";
            this.BoardBox.Size = new System.Drawing.Size(426, 426);
            this.BoardBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.BoardBox.TabIndex = 1;
            this.BoardBox.TabStop = false;
            // 
            // StartBtn
            // 
            this.StartBtn.BackColor = System.Drawing.Color.Red;
            this.StartBtn.Location = new System.Drawing.Point(13, 403);
            this.StartBtn.Name = "StartBtn";
            this.StartBtn.Size = new System.Drawing.Size(239, 34);
            this.StartBtn.TabIndex = 2;
            this.StartBtn.Text = "Start Game";
            this.StartBtn.UseVisualStyleBackColor = false;
            this.StartBtn.Click += new System.EventHandler(this.StartBtn_Click);
            // 
            // CustomShipBtn
            // 
            this.CustomShipBtn.Location = new System.Drawing.Point(13, 323);
            this.CustomShipBtn.Name = "CustomShipBtn";
            this.CustomShipBtn.Size = new System.Drawing.Size(239, 34);
            this.CustomShipBtn.TabIndex = 3;
            this.CustomShipBtn.Text = "Add Custom Ship";
            this.CustomShipBtn.UseVisualStyleBackColor = true;
            this.CustomShipBtn.Click += new System.EventHandler(this.CustomShipBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 299);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Board Size:";
            // 
            // BoardXControl
            // 
            this.BoardXControl.Location = new System.Drawing.Point(80, 297);
            this.BoardXControl.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.BoardXControl.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.BoardXControl.Name = "BoardXControl";
            this.BoardXControl.Size = new System.Drawing.Size(47, 20);
            this.BoardXControl.TabIndex = 5;
            this.BoardXControl.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.BoardXControl.ValueChanged += new System.EventHandler(this.BoardXControl_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(133, 299);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(12, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "x";
            // 
            // BoardYControl
            // 
            this.BoardYControl.Location = new System.Drawing.Point(151, 297);
            this.BoardYControl.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.BoardYControl.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.BoardYControl.Name = "BoardYControl";
            this.BoardYControl.Size = new System.Drawing.Size(47, 20);
            this.BoardYControl.TabIndex = 7;
            this.BoardYControl.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.BoardYControl.ValueChanged += new System.EventHandler(this.BoardYControl_ValueChanged);
            // 
            // RotateBtn
            // 
            this.RotateBtn.Location = new System.Drawing.Point(97, 268);
            this.RotateBtn.Name = "RotateBtn";
            this.RotateBtn.Size = new System.Drawing.Size(75, 23);
            this.RotateBtn.TabIndex = 8;
            this.RotateBtn.Text = "Rotate";
            this.RotateBtn.UseVisualStyleBackColor = true;
            this.RotateBtn.Click += new System.EventHandler(this.RotateBtn_Click);
            // 
            // AddBtn
            // 
            this.AddBtn.Location = new System.Drawing.Point(178, 268);
            this.AddBtn.Name = "AddBtn";
            this.AddBtn.Size = new System.Drawing.Size(23, 23);
            this.AddBtn.TabIndex = 9;
            this.AddBtn.Text = "+";
            this.AddBtn.UseVisualStyleBackColor = true;
            this.AddBtn.Click += new System.EventHandler(this.AddBtn_Click);
            // 
            // RemoveBtn
            // 
            this.RemoveBtn.Location = new System.Drawing.Point(207, 268);
            this.RemoveBtn.Name = "RemoveBtn";
            this.RemoveBtn.Size = new System.Drawing.Size(23, 23);
            this.RemoveBtn.TabIndex = 10;
            this.RemoveBtn.Text = "-";
            this.RemoveBtn.UseVisualStyleBackColor = true;
            this.RemoveBtn.Click += new System.EventHandler(this.RemoveBtn_Click);
            // 
            // DeleteBtn
            // 
            this.DeleteBtn.Location = new System.Drawing.Point(16, 268);
            this.DeleteBtn.Name = "DeleteBtn";
            this.DeleteBtn.Size = new System.Drawing.Size(75, 23);
            this.DeleteBtn.TabIndex = 11;
            this.DeleteBtn.Text = "Delete";
            this.DeleteBtn.UseVisualStyleBackColor = true;
            this.DeleteBtn.Click += new System.EventHandler(this.DeleteBtn_Click);
            // 
            // ShipListView
            // 
            this.ShipListView.HideSelection = false;
            this.ShipListView.Location = new System.Drawing.Point(13, 13);
            this.ShipListView.Name = "ShipListView";
            this.ShipListView.Size = new System.Drawing.Size(239, 249);
            this.ShipListView.TabIndex = 12;
            this.ShipListView.UseCompatibleStateImageBehavior = false;
            // 
            // DefaultBtn
            // 
            this.DefaultBtn.Location = new System.Drawing.Point(13, 363);
            this.DefaultBtn.Name = "DefaultBtn";
            this.DefaultBtn.Size = new System.Drawing.Size(239, 34);
            this.DefaultBtn.TabIndex = 13;
            this.DefaultBtn.Text = "Restore Defaults";
            this.DefaultBtn.UseVisualStyleBackColor = true;
            this.DefaultBtn.Click += new System.EventHandler(this.DefaultBtn_Click);
            // 
            // GameEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(698, 450);
            this.Controls.Add(this.DefaultBtn);
            this.Controls.Add(this.ShipListView);
            this.Controls.Add(this.DeleteBtn);
            this.Controls.Add(this.RemoveBtn);
            this.Controls.Add(this.AddBtn);
            this.Controls.Add(this.RotateBtn);
            this.Controls.Add(this.BoardYControl);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.BoardXControl);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CustomShipBtn);
            this.Controls.Add(this.StartBtn);
            this.Controls.Add(this.BoardBox);
            this.Name = "GameEditor";
            this.Text = "GameEditor";
            ((System.ComponentModel.ISupportInitialize)(this.BoardBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BoardXControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BoardYControl)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox BoardBox;
        private System.Windows.Forms.Button StartBtn;
        private System.Windows.Forms.Button CustomShipBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown BoardXControl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown BoardYControl;
        private System.Windows.Forms.Button RotateBtn;
        private System.Windows.Forms.Button AddBtn;
        private System.Windows.Forms.Button RemoveBtn;
        private System.Windows.Forms.Button DeleteBtn;
        private System.Windows.Forms.ListView ShipListView;
        private System.Windows.Forms.Button DefaultBtn;
    }
}