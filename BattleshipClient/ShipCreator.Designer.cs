namespace BattleshipClient
{
    partial class ShipCreator
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
            this.ShipBox = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ShipXControl = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.ShipYControl = new System.Windows.Forms.NumericUpDown();
            this.RotateBtn = new System.Windows.Forms.Button();
            this.SaveBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ShipBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ShipXControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ShipYControl)).BeginInit();
            this.SuspendLayout();
            // 
            // ShipBox
            // 
            this.ShipBox.Location = new System.Drawing.Point(186, 12);
            this.ShipBox.Name = "ShipBox";
            this.ShipBox.Size = new System.Drawing.Size(324, 324);
            this.ShipBox.TabIndex = 0;
            this.ShipBox.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Ship Size:";
            // 
            // ShipXControl
            // 
            this.ShipXControl.Location = new System.Drawing.Point(73, 11);
            this.ShipXControl.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.ShipXControl.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ShipXControl.Name = "ShipXControl";
            this.ShipXControl.Size = new System.Drawing.Size(47, 20);
            this.ShipXControl.TabIndex = 2;
            this.ShipXControl.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.ShipXControl.ValueChanged += new System.EventHandler(this.ShipXControl_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(120, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(12, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "x";
            // 
            // ShipYControl
            // 
            this.ShipYControl.Location = new System.Drawing.Point(129, 12);
            this.ShipYControl.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.ShipYControl.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ShipYControl.Name = "ShipYControl";
            this.ShipYControl.Size = new System.Drawing.Size(47, 20);
            this.ShipYControl.TabIndex = 4;
            this.ShipYControl.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.ShipYControl.ValueChanged += new System.EventHandler(this.ShipYControl_ValueChanged);
            // 
            // RotateBtn
            // 
            this.RotateBtn.Location = new System.Drawing.Point(16, 47);
            this.RotateBtn.Name = "RotateBtn";
            this.RotateBtn.Size = new System.Drawing.Size(160, 31);
            this.RotateBtn.TabIndex = 5;
            this.RotateBtn.Text = "Rotate";
            this.RotateBtn.UseVisualStyleBackColor = true;
            this.RotateBtn.Click += new System.EventHandler(this.RotateBtn_Click);
            // 
            // SaveBtn
            // 
            this.SaveBtn.Location = new System.Drawing.Point(16, 305);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(160, 31);
            this.SaveBtn.TabIndex = 6;
            this.SaveBtn.Text = "Save";
            this.SaveBtn.UseVisualStyleBackColor = true;
            this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // ShipCreator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 348);
            this.Controls.Add(this.SaveBtn);
            this.Controls.Add(this.RotateBtn);
            this.Controls.Add(this.ShipYControl);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ShipXControl);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ShipBox);
            this.Name = "ShipCreator";
            this.Text = "ShipCreator";
            ((System.ComponentModel.ISupportInitialize)(this.ShipBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ShipXControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ShipYControl)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox ShipBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown ShipXControl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown ShipYControl;
        private System.Windows.Forms.Button RotateBtn;
        private System.Windows.Forms.Button SaveBtn;
    }
}