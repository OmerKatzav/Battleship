using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipObjects
{
    public class OpponentBoard
    {
        public int Width;
        public int Height;
        public Dictionary<Ship, (byte, byte)> KnownShips { get; set; }
        public Dictionary<(byte, byte), bool> Shots { get; set; }

        public OpponentBoard(int Width, int Height) 
        {
            this.Width = Width;
            this.Height = Height;
            KnownShips = new Dictionary<Ship, (byte, byte)>();
            Shots = new Dictionary<(byte, byte), bool>();
        }

        public bool IsShipAt((byte, byte) position)
        {
            Shots.TryGetValue(position, out bool isShip);
            return isShip;
        }

        public bool IsShipAt(byte x, byte y)
        {
            return IsShipAt((x, y));
        }

        public bool IsOutlineAt((byte, byte) position)
        {
            foreach (Ship ship in KnownShips.Keys)
            {
                Ship outlineShip = ship.GetShipOutline();
                if (outlineShip.IsAt(position, (KnownShips[ship].Item1 - 1, KnownShips[ship].Item2 - 1))) return true;
            }
            return IsShipAt(position);
        }

        public bool IsOutlineAt(byte x, byte y)
        {
            return IsOutlineAt((x, y));
        }

        public Bitmap GetImage(int maxWidth, int maxHeight)
        {
            int scale = Math.Min(maxWidth / Width, maxHeight / Height);
            Bitmap image = new Bitmap(Width * scale, Height * scale);

            using (Graphics graphics = Graphics.FromImage(image))
            {
                using (Brush shotBrush = new SolidBrush(Color.Black))
                using (Brush hitBrush = new SolidBrush(Color.Red))
                using (Brush cantHitBrush = new SolidBrush(Color.LightGray))
                {
                    for (byte x = 0; x < Width; x++)
                    {
                        for (byte y = 0; y < Height; y++)
                        {
                            if (IsShipAt(x, y))
                            {
                                graphics.FillRectangle(hitBrush, x * scale, y * scale, scale, scale);
                            }
                            else if (Shots.ContainsKey((x, y)))
                            {
                                graphics.FillRectangle(cantHitBrush, x * scale, y * scale, scale, scale);
                            }
                            else if (IsOutlineAt(x, y))
                            {
                                graphics.FillRectangle(cantHitBrush, x * scale, y * scale, scale, scale);
                            }
                            if (Shots.ContainsKey((x, y)))
                            {
                                graphics.FillEllipse(shotBrush, x * scale + scale / 4, y * scale + scale / 4, scale / 2, scale / 2);
                            }
                        }
                    }
                }
                using (Pen pen = new Pen(Color.Black, 2))
                {
                    for (byte x = 0; x <= Width; x++)
                    {
                        graphics.DrawLine(pen, x * scale, 0, x * scale, Height * scale);
                    }
                    for (byte y = 0; y <= Height; y++)
                    {
                        graphics.DrawLine(pen, 0, y * scale, Width * scale, y * scale);
                    }
                }
            }
            return image;
        }
    }
}
