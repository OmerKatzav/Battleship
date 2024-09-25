using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipMessages
{
    public interface IMessage
    {
        MessageTypes MessageType { get; }
        byte[] ToBytes();
    }
}
