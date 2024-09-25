using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipUtils
{
    public static class BitUtils
    {
        public static bool GetBit(byte b, int i)
        {
            return (b & (1 << i)) != 0;
        }

        public static byte SetBit(byte b, int i, bool bit)
        {
            if (bit) return (byte)(b | (1 << i));
            return (byte)(b & ~(1 << i));
        }
    }
}
