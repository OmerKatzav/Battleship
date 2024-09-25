using BattleshipMessages.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipMessages
{
    public class MessageFactory
    {
        public static IMessage FromBytes(byte[] message)
        {
            MessageTypes messageType = (MessageTypes)message[0];
            switch (messageType)
            {
                case MessageTypes.PingRequest:
                    return new PingRequest(message);
                case MessageTypes.PingResponse:
                    return new PingResponse(message);
                case MessageTypes.LoginRequest:
                    return new LoginRequest(message);
                case MessageTypes.LoginResponse:
                    return new LoginResponse(message);
                case MessageTypes.CitiesRequest:
                    return new CitiesRequest(message);
                case MessageTypes.CitiesResponse:
                    return new CitiesResponse(message);
                case MessageTypes.RegisterRequest:
                    return new RegisterRequest(message);
                case MessageTypes.RegisterResponse:
                    return new RegisterResponse(message);
                case MessageTypes.GetDataRequest:
                    return new GetDataRequest(message);
                case MessageTypes.GetDataResponse:
                    return new GetDataResponse(message);
                case MessageTypes.OpenGamesRequest:
                    return new OpenGamesRequest(message);
                case MessageTypes.OpenGamesResponse:
                    return new OpenGamesResponse(message);
                case MessageTypes.GameParamsRequest:
                    return new GameParamsRequest(message);
                case MessageTypes.GameParamsResponse:
                    return new GameParamsResponse(message);
                case MessageTypes.GameStartRequest:
                    return new GameStartRequest(message);
                case MessageTypes.GameStartResponse:
                    return new GameStartResponse(message);
                case MessageTypes.GameRequest:
                    return new GameRequest(message);
                case MessageTypes.WaitingForOpponentMessage:
                    return new WaitingForOpponentMessage(message);
                case MessageTypes.GameResponse:
                    return new GameResponse(message);
                case MessageTypes.ChatRequest:
                    return new ChatRequest(message);
                case MessageTypes.ChatResponse:
                    return new ChatResponse(message);
                case MessageTypes.OpponentLeftMessage:
                    return new OpponentLeftMessage(message);
                default:
                    throw new Exception("Invalid message type");
            }
        }
    }
}
