using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipMessages.Messages
{
    public class CitiesResponse : IMessage
    {
        /*
         * Byte Structure:
         * 0: message type
         * 1 to 4: 1st city length
         * 5 to (1st city length + 4): 1st city
         * This pattern onwards
        */
        public MessageTypes MessageType => MessageTypes.CitiesResponse;
        public string[] Cities { get; set; }

        public CitiesResponse(string[] Cities)
        {
            this.Cities = Cities;
        }

        public CitiesResponse(byte[] response)
        {
            List<string> cities = new List<string>();
            int index = 1;
            while (index < response.Length)
            {
                int cityLength = BitConverter.ToInt32(response, index);
                cities.Add(Encoding.UTF8.GetString(response, index + 4, cityLength));
                index += cityLength + 4;
            }
            this.Cities = cities.ToArray();
        }

        public byte[] ToBytes()
        {
            List<byte> bytes = new List<byte> { (byte)MessageType };
            foreach (string city in Cities)
            {
                byte[] cityBytes = Encoding.UTF8.GetBytes(city);
                bytes.AddRange(BitConverter.GetBytes(cityBytes.Length));
                bytes.AddRange(cityBytes);
            }
            return bytes.ToArray();
        }
    }
}
