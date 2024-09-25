using BattleshipObjects;
using BattleshipUtils;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleshipClient
{
    public partial class ShipCreator : Form
    {
        public Ship Ship { get; set; }
        public ShipCreator()
        {
            InitializeComponent();
            Ship = new Ship(new bool[(int)ShipXControl.Value, (int)ShipYControl.Value]);
            ShipBox.SizeMode = PictureBoxSizeMode.Zoom;
            ShipBox.Image = Ship.GetImage(ShipBox.Width, ShipBox.Height);
            ShipBox.MouseClick += ShipBox_MouseClick;
        }

        private void ShipXControl_ValueChanged(object sender, EventArgs e)
        {
            Ship.ChangeSize((int)ShipXControl.Value, (int)ShipYControl.Value);
            ShipBox.Image = Ship.GetImage(ShipBox.Width, ShipBox.Height);
        }

        private void ShipYControl_ValueChanged(object sender, EventArgs e)
        {
            Ship.ChangeSize((int)ShipXControl.Value, (int)ShipYControl.Value);
            ShipBox.Image = Ship.GetImage(ShipBox.Width, ShipBox.Height);
        }

        private void RotateBtn_Click(object sender, EventArgs e)
        {
            Ship.Rotate();
            ShipBox.Image = Ship.GetImage(ShipBox.Width, ShipBox.Height);
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (Ship.IsConnected())
            {
                Ship.Shrink();
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Ship must be connected");
            }
        }

        private void ShipBox_MouseClick(object sender, MouseEventArgs e)
        {
            Point point = PictureBoxUtils.GetPointHoveringOn(ShipBox, ShipBox.PointToScreen(e.Location), Ship.ShipData.GetLength(0), Ship.ShipData.GetLength(1));
            if (point.X < 0 || point.X >= Ship.ShipData.GetLength(0) || point.Y < 0 || point.Y >= Ship.ShipData.GetLength(1))
            {
                return;
            }
            Ship.ShipData[point.X, point.Y] = !Ship.ShipData[point.X, point.Y];
            ShipBox.Image = Ship.GetImage(ShipBox.Width, ShipBox.Height);
        }
    }
}
