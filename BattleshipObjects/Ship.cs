using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleshipUtils;

namespace BattleshipObjects
{
    public class Ship
    {
        /*
         * Byte Structure:
         * 1: width - 1
         * 2: height - 1
         * 3 to end: ship data (2d array of booleans)
        */
        public bool[,] ShipData { get; set; }

        public Ship(bool[,] shipData)
        {
            ShipData = shipData;
        }

        public Ship(byte[] bytes, int offset=0)
        {
            int width = bytes[offset] + 1;
            int height = bytes[offset + 1] + 1;
            ShipData = new bool[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int bitIndex = x * height + y;
                    int byteIndex = bitIndex / 8;
                    int bitOffset = bitIndex % 8;
                    ShipData[x, y] = BitUtils.GetBit(bytes[offset + 2 + byteIndex], bitOffset);
                }
            }
        }

        public byte[] ToBytes()
        {
            byte[] bytes = new byte[2 + (int)Math.Ceiling(ShipData.Length / 8f)];
            bytes[0] = (byte)(ShipData.GetLength(0) - 1);
            bytes[1] = (byte)(ShipData.GetLength(1) - 1);
            for (int x = 0; x < ShipData.GetLength(0); x++)
            {
                for (int y = 0; y < ShipData.GetLength(1); y++)
                {
                    int bitIndex = x * ShipData.GetLength(1) + y;
                    int byteIndex = bitIndex / 8;
                    int bitOffset = bitIndex % 8;
                    bytes[2 + byteIndex] = BitUtils.SetBit(bytes[2 + byteIndex], bitOffset, ShipData[x, y]);
                }
            }
            return bytes;
        }

        public bool IsConnected()
        {
            return CountFilled() == CountConnectedFilled();
        }

        public int CountFilled()
        {
            int filled = 0;
            for (int x = 0; x < ShipData.GetLength(0); x++)
            {
                for (int y = 0; y < ShipData.GetLength(1); y++)
                {
                    if (ShipData[x, y])
                    {
                        filled++;
                    }
                }
            }
            return filled;
        }

        public int CountConnectedFilled()
        {
            int startX = 0;
            int startY = 0;
            for (int x = 0; x < ShipData.GetLength(0); x++)
            {
                for (int y = 0; y < ShipData.GetLength(1); y++)
                {
                    if (ShipData[x, y])
                    {
                        (startX, startY) = (x, y);
                        break;
                    }
                }
            }

            bool[,] oldShipData = (bool[,])ShipData.Clone();
            int connectedFilled = CountConnectedFilled(startX, startY);
            ShipData = oldShipData;
            return connectedFilled;
        } 

        private int CountConnectedFilled(int x, int y)
        {
            if (x < 0 || x >= ShipData.GetLength(0) || y < 0 || y >= ShipData.GetLength(1))
            {
                return 0;
            }
            if (!ShipData[x, y])
            {
                return 0;
            }
            ShipData[x, y] = false;
            return 1 + CountConnectedFilled(x + 1, y) + CountConnectedFilled(x - 1, y) + CountConnectedFilled(x, y + 1) + CountConnectedFilled(x, y - 1);
        }

        public void Shrink()
        {
            bool removed = false;
            if (IsXEmpty(0))
            {
                RemoveX(0);
                removed = true;
            }
            if (IsXEmpty(ShipData.GetLength(0) - 1))
            {
                RemoveX(ShipData.GetLength(0) - 1);
                removed = true;
            }
            if (IsYEmpty(0))
            {
                RemoveY(0);
                removed = true;
            }
            if (IsYEmpty(ShipData.GetLength(1) - 1))
            {
                RemoveY(ShipData.GetLength(1) - 1);
                removed = true;
            }
            if (removed)
            {
                Shrink();
            }
        }

        private void RemoveX(int x)
        {
            bool[,] newShipData = new bool[ShipData.GetLength(0) - 1, ShipData.GetLength(1)];
            for (int i = 0; i < ShipData.GetLength(0); i++)
            {
                if (i < x)
                {
                    for (int j = 0; j < ShipData.GetLength(1); j++)
                    {
                        newShipData[i, j] = ShipData[i, j];
                    }
                }
                else if (i > x)
                {
                    for (int j = 0; j < ShipData.GetLength(1); j++)
                    {
                        newShipData[i - 1, j] = ShipData[i, j];
                    }
                }
            }
            ShipData = newShipData;
        }

        private void RemoveY(int y)
        {
            bool[,] newShipData = new bool[ShipData.GetLength(0), ShipData.GetLength(1) - 1];
            for (int i = 0; i < ShipData.GetLength(0); i++)
            {
                for (int j = 0; j < ShipData.GetLength(1); j++)
                {
                    if (j < y)
                    {
                        newShipData[i, j] = ShipData[i, j];
                    }
                    else if (j > y)
                    {
                        newShipData[i, j - 1] = ShipData[i, j];
                    }
                }
            }
            ShipData = newShipData;
        }

        private bool IsXEmpty(int x)
        {
            for (int y = 0; y < ShipData.GetLength(1); y++)
            {
                if (ShipData[x, y])
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsYEmpty(int y)
        {
            for (int x = 0; x < ShipData.GetLength(0); x++)
            {
                if (ShipData[x, y])
                {
                    return false;
                }
            }
            return true;
        }

        public void Rotate()
        {
            ShipData = MatrixUtils.Rotate(ShipData);
        }

        public bool IsAt((byte, byte) location, (int, int) shipLocation)
        {
            if (location.Item1 < shipLocation.Item1 || location.Item1 >= shipLocation.Item1 + ShipData.GetLength(0) || location.Item2 < shipLocation.Item2 || location.Item2 >= shipLocation.Item2 + ShipData.GetLength(1))
            {
                return false;
            }
            return ShipData[location.Item1 - shipLocation.Item1, location.Item2 - shipLocation.Item2];
        }

        public bool IsAt(byte x, byte y, int shipX, int shipY)
        {
            return IsAt((x, y), (shipX, shipY));
        }

        public (byte, byte)[] GetFilledSpots((int, int) shipLocation)
        {
            List<(byte, byte)> filledSpots = new List<(byte, byte)>();
            for (byte x = 0; x < ShipData.GetLength(0); x++)
            {
                for (byte y = 0; y < ShipData.GetLength(1); y++)
                {
                    if (ShipData[x,y])
                    {
                        if (x + shipLocation.Item1 >= 0 && x + shipLocation.Item1 < 256 && y + shipLocation.Item2 >= 0 && y + shipLocation.Item2 < 256)
                            filledSpots.Add(((byte)(x + shipLocation.Item1), (byte)(y + shipLocation.Item2)));
                    }
                }
            }
            return filledSpots.ToArray();
        }

        public Ship GetShipOutline()
        {
            int length1 = Math.Min(ShipData.GetLength(0) + 2, 256);
            int length2 = Math.Min(ShipData.GetLength(1) + 2, 256);
            bool[,] outlineData = new bool[length1, length2];
            for (int x = 0; x < ShipData.GetLength(0); x++)
            {
                for (int y = 0; y < ShipData.GetLength(1); y++)
                {
                    if (ShipData[x, y])
                    {
                        foreach (int offset1 in new int[] { 0, 1, 2 })
                        {
                            foreach (int offset2 in new int[] { 0, 1, 2 })
                            {
                                if (x + offset1 >= 0 && x + offset1 < length1 && y + offset2 >= 0 && y + offset2 < length2)
                                {
                                    outlineData[x + offset1, y + offset2] = true;
                                }
                            }
                        }
                    }
                }
            }
            return new Ship(outlineData);
        }

        public void ChangeSize(int width, int height)
        {
            ShipData = MatrixUtils.ChangeSize(ShipData, width, height);
        }

        public Bitmap GetImage(int maxWidth, int maxHeight)
        {
            int width = ShipData.GetLength(0);
            int height = ShipData.GetLength(1);
            int scale = Math.Min(maxWidth / width, maxHeight / height);
            Bitmap image = new Bitmap(width * scale, height * scale);
            using (Graphics graphics = Graphics.FromImage(image))
            {
                using (Brush brush = new SolidBrush(Color.Gray))
                {
                    for (int x = 0; x < width; x++)
                    {
                        for (int y = 0; y < height; y++)
                        {
                            if (ShipData[x, y])
                            {
                                graphics.FillRectangle(brush, x * scale, y * scale, scale, scale);
                            }
                        }
                    }
                }
                using (Pen pen = new Pen(Color.Black))
                {
                    for (int x = 0; x <= width; x++)
                    {
                        graphics.DrawLine(pen, x * scale, 0, x * scale, height * scale);
                    }
                    for (int y = 0; y <= height; y++)
                    {
                        graphics.DrawLine(pen, 0, y * scale, width * scale, y * scale);
                    }
                }
            }
            
            return image;
        }
        
        public bool IsEquals(Ship ship)
        {
            bool[,] oldShipData = (bool[,])ShipData.Clone();
            for (int i = 0; i < 4; i++) { 
                if (MatrixUtils.AreEqual(ShipData, ship.ShipData))
                {
                    ShipData = oldShipData;
                    return true;
                }
                Rotate();
            }
            ShipData = oldShipData;
            return false;
        }

        public bool IsEqualsWithoutRotations(Ship ship)
        {
            return MatrixUtils.AreEqual(ShipData, ship.ShipData);
        }

       public Ship Clone()
        {
            return new Ship((bool[,])ShipData.Clone());
        }
    }
}
