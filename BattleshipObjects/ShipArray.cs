using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BattleshipObjects
{
    public class ShipArray
    {
        /*
         * Byte Structure:
         * 0 to (1+ceil(shipWidth*shipHeight/8)): ship 1
         * repeat for each ship
        */
        public Ship[] Ships { get; set; }

        public ShipArray(Ship[] ships)
        {
            this.Ships = ships;
        }

        public ShipArray(byte[] bytes, int offset=0, int length=0)
        {
            if (length == 0) length = bytes.Length - offset;
            List<Ship> shipList = new List<Ship>();
            int newOffset = 0;
            while (newOffset < length)
            {
                Ship ship = new Ship(bytes, offset + newOffset);
                shipList.Add(ship);
                newOffset += 2 + (int)Math.Ceiling(ship.ShipData.Length / 8f);
            }
            Ships = shipList.ToArray();
        }

        public int GetCount(Ship ship)
        {
            int count = 0;
            foreach (Ship existingShip in Ships)
            {
                if (existingShip.Equals(ship))
                {
                    count++;
                }
            }
            return count;
        }

        public Dictionary<Ship, int> GetShipCounts()
        {
            Dictionary<Ship, int> shipCounts = new Dictionary<Ship, int>();
            foreach (Ship ship in Ships)
            {
                bool found = false;
                foreach (Ship existingShip in shipCounts.Keys)
                {
                    if (existingShip.IsEquals(ship))
                    {
                        shipCounts[existingShip]++;
                        found = true;
                        break;
                    }
                }
                if (!found)
                    shipCounts[ship] = 1;
            }
            return shipCounts;
        }

        public byte[] ToBytes()
        {
            int totalLength = 0;
            foreach (Ship ship in Ships)
            {
                totalLength += 2 + (int)Math.Ceiling(ship.ShipData.Length / 8f);
            }
            byte[] bytes = new byte[totalLength];
            int offset = 0;
            foreach (Ship ship in Ships)
            {
                byte[] shipBytes = ship.ToBytes();
                Array.Copy(shipBytes, 0, bytes, offset, shipBytes.Length);
                offset += shipBytes.Length;
            }
            return bytes;
        }

        public void RemoveShipType(Ship ship)
        {
            List<Ship> newShips = new List<Ship>();
            foreach (Ship existingShip in Ships)
            {
                if (!existingShip.Equals(ship))
                {
                    newShips.Add(existingShip);
                }
            }
            Ships = newShips.ToArray();
        }

        public void RemoveOneShipType(Ship ship)
        {
            bool found = false;
            List<Ship> newShips = new List<Ship>();
            foreach (Ship existingShip in Ships)
            {
                if (!existingShip.Equals(ship) || found)
                {
                    newShips.Add(existingShip);
                }
                else
                {
                    found = true;
                }
            }
            Ships = newShips.ToArray();
        }

        public void AddShip(Ship ship)
        {
            List<Ship> newShips = new List<Ship>(Ships)
            {
                ship
            };
            Ships = newShips.ToArray();
        }

        public bool IsEquals(ShipArray other)
        {
            if (Ships.Length != other.Ships.Length) return false;
            Dictionary<Ship, int> shipCounts = GetShipCounts();
            Dictionary<Ship, int> otherShipCounts = other.GetShipCounts();
            bool found;
            foreach (Ship ship in shipCounts.Keys)
            {  
                found = false;
                foreach (Ship otherShip in otherShipCounts.Keys)
                {
                    if (ship.IsEquals(otherShip))
                    {
                        if (shipCounts[ship] != otherShipCounts[otherShip])
                        {
                            return false;
                        }
                        found = true;
                        break;
                    }
                }
                if (!found) return false;
            }
            return true;
        }

        public bool IsEqualsWithoutRotations(ShipArray other)
        {
            if (Ships.Length != other.Ships.Length) return false;
            Dictionary<Ship, int> shipCounts = GetShipCounts();
            Dictionary<Ship, int> otherShipCounts = other.GetShipCounts();
            bool found;
            foreach (Ship ship in shipCounts.Keys)
            {
                found = false;
                foreach (Ship otherShip in otherShipCounts.Keys)
                {
                    if (ship.IsEqualsWithoutRotations(otherShip))
                    {
                        if (shipCounts[ship] != otherShipCounts[otherShip])
                        {
                            return false;
                        }
                        found = true;
                        break;
                    }
                }
                if (!found) return false;
            }
            return true;
        }
    }
}
