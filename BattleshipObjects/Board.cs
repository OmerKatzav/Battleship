using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BattleshipObjects
{
    public class Board
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public Dictionary<Ship, (byte, byte)> Ships { get; set; }
        public List<(byte, byte)> Shots { get; set; } = new List<(byte, byte)>();
        
        public Board(int Width, int Height, Dictionary<Ship, (byte, byte)> Ships)
        {
            this.Width = Width;
            this.Height = Height;
            this.Ships = Ships;
        }

        public Board(int Width, int Height, ShipArray Ships, (byte, byte)[] locations)
        {
            this.Width = Width;
            this.Height = Height;
            this.Ships = new Dictionary<Ship, (byte, byte)>();
            for (int i = 0; i < Ships.Ships.Length; i++)
            {
                this.Ships[Ships.Ships[i]] = locations[i];
            }
        }

        public void AddShip(Ship ship, (byte, byte) location)
        {
            if (Ships.Keys.Contains(ship))
                ship = ship.Clone();
            Ships[ship] = location;
        }

        public void AddShip(Ship ship, byte x, byte y)
        {
            AddShip(ship, (x, y));
        }

        public void RemoveShip(Ship ship)
        {
            Ships.Remove(ship);
        }

        public void RemoveOneShipType(Ship ship)
        {
            foreach (Ship otherShip in Ships.Keys)
            {
                if (otherShip.IsEquals(ship))
                {
                    Ships.Remove(otherShip);
                    return;
                }
            }
        }

        public void RemoveShipType(Ship ship)
        {
            foreach (Ship otherShip in Ships.Keys)
            {
                if (otherShip.IsEquals(ship))
                {
                    Ships.Remove(otherShip);
                }
            }
        }

        public int CountShipType(Ship ship)
        {
            int count = 0;
            foreach (Ship otherShip in Ships.Keys)
            {
                if (otherShip.IsEquals(ship)) count++;
            }
            return count;
        }

        public bool IsOutlineAt((byte, byte) location, Ship exceptionShip=null)
        {
            foreach (Ship ship in Ships.Keys)
            {
                if (ship == exceptionShip) continue;
                Ship outlineShip = ship.GetShipOutline();
                if (outlineShip.IsAt(location, (Ships[ship].Item1 - 1, Ships[ship].Item2 - 1))) return true;
            }
            return false;
        }

        public bool IsOutlineAt(byte x, byte y)
        {
            return IsOutlineAt((x, y));
        }

        public bool IsValidLocation(Ship ship, (byte, byte) location, Ship exceptionShip=null)
        {
            if (location.Item1 + ship.ShipData.GetLength(0) > Width || location.Item2 + ship.ShipData.GetLength(1) > Height) return false;
            foreach ((byte, byte) ShipPoint in ship.GetFilledSpots(location))
            {
                if (IsOutlineAt(ShipPoint, exceptionShip)) return false;
            }
            Ship outlineShip = ship.GetShipOutline();
            (int, int) outlineLocation = (location.Item1 - 1, location.Item2 - 1);
            foreach ((byte, byte) ShipPoint in outlineShip.GetFilledSpots(outlineLocation))
            {
                if (IsShipAt(ShipPoint, exceptionShip)) return false;
            }
            return true;
        }

        public bool IsValidLocation(Ship ship, byte x, byte y)
        {
            return IsValidLocation(ship, (x, y));
        }

        public bool IsValid()
        {
            if (Ships.Count == 0) return false;
            if (Width == 0 || Height == 0 || Width > 255 || Height > 255) return false;
            foreach (Ship ship in Ships.Keys)
            {
                if (!ship.IsConnected()) return false;
            }
            foreach (Ship ship in Ships.Keys)
            {
                if (!IsValidLocation(ship, Ships[ship], ship)) return false;
            }
            return true;
        }

        public Ship ShipAt((byte, byte) location, Ship exceptionShip = null)
        {
            foreach (Ship ship in Ships.Keys)
            {
                if (ship == exceptionShip) continue;
                if (ship.IsAt(location, Ships[ship])) return ship;
            }
            return null;
        }

        public Ship ShipAt(byte x, byte y, Ship exceptionShip = null)
        {
            return ShipAt((x, y), exceptionShip);
        }

        public bool IsShipAt((byte, byte) location, Ship exceptionShip = null)
        {
            return ShipAt(location, exceptionShip) != null;
        }

        public bool IsShipAt(byte x, byte y, Ship exceptionShip = null)
        {
            return IsShipAt((x, y), exceptionShip);
        }

        public bool Hit((byte, byte) location)
        {
            if (Shots.Contains(location)) return false;
            Shots.Add(location);
            return IsShipAt(location);
        }

        public bool Hit(byte x, byte y)
        {
            return Hit((x, y));
        }

        public bool IsShipSunk(Ship ship)
        {
            int hits = 0;
            foreach ((byte, byte) hit in Shots)
            {
                if (ship.IsAt(hit, Ships[ship])) hits++;
            }
            return hits == ship.CountFilled();
        }

        public Ship[] GetSunkShips()
        {
            List<Ship> sunkShips = new List<Ship>();
            foreach (Ship ship in Ships.Keys)
            {
                if (IsShipSunk(ship)) sunkShips.Add(ship);
            }
            return sunkShips.ToArray();
        }

        public Ship GetShipSunkByLastHit()
        {
            Ship ship = ShipAt(Shots.Last());
            if (ship == null) return null;
            if (IsShipSunk(ship)) return ship;
            return null;
        }

        public bool IsGameOver()
        {
            foreach (Ship ship in Ships.Keys)
            {
                if (!IsShipSunk(ship)) return false;
            }
            return true;
        }

        public Bitmap GetImage(int maxWidth, int maxHeight)
        {
            int scale = Math.Min(maxWidth / Width, maxHeight / Height);
            Bitmap image = new Bitmap(Width * scale, Height * scale);
            using (Graphics graphics = Graphics.FromImage(image))
            {
                using (Brush shotBrush = new SolidBrush(Color.Black))
                using (Brush shipBrush = new SolidBrush(Color.Gray))
                using (Brush hitBrush = new SolidBrush(Color.Red))
                using (Brush cantHitBrush = new SolidBrush(Color.LightGray))
                {
                    for (byte x = 0; x < Width; x++)
                    {
                        for (byte y = 0; y < Height; y++)
                        {
                            if (Shots.Contains((x, y)) && IsShipAt(x, y))
                            {
                                graphics.FillRectangle(hitBrush, x * scale, y * scale, scale, scale);
                            }
                            else if (Shots.Contains((x, y)))
                            {
                                graphics.FillRectangle(cantHitBrush, x * scale, y * scale, scale, scale);
                            }
                            else if (IsShipAt(x, y))
                            {
                                graphics.FillRectangle(shipBrush, x * scale, y * scale, scale, scale);
                            }
                            else if (IsOutlineAt(x, y))
                            {
                                graphics.FillRectangle(cantHitBrush, x * scale, y * scale, scale, scale);
                            }
                            if (Shots.Contains((x, y)))
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
