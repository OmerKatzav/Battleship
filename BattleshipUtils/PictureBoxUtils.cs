using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BattleshipUtils
{
    public static class PictureBoxUtils
    {
        public static Point GetPointHoveringOn(PictureBox pictureBox, Point screenMouseLocation, int width, int height)
        {
            PointF point = pictureBox.PointToClient(screenMouseLocation);

            if (pictureBox.Image.Width / pictureBox.Image.Height > pictureBox.Width / pictureBox.Height)
            {
                float scale = (float)pictureBox.Width / pictureBox.Image.Width;
                float BlankSpace = (pictureBox.Height - pictureBox.Image.Height * scale) / 2;
                point.Y -= BlankSpace;
                point.X /= scale;
                point.Y /= scale;
            }
            else
            {
                float scale = (float)pictureBox.Height / pictureBox.Image.Height;
                float BlankSpace = (pictureBox.Width - pictureBox.Image.Width * scale) / 2;
                point.X -= BlankSpace / 2;
                point.X /= scale;
                point.Y /= scale;
            }
            int objectScale = Math.Min(pictureBox.Width / width, pictureBox.Height / height);
            return new Point((int)(point.X/objectScale), (int)(point.Y/objectScale));
        }
    }
}
