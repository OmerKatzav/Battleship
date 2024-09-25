using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipUtils
{
    public static class HashUtils
    {
        public static byte[] Hash(string input, byte[] salt)
        {
            SHA256 sha256 = SHA256.Create();
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(input).Concat(salt).ToArray());
        }
    }
}
