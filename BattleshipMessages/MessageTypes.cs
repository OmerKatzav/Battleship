using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipMessages
{
    public enum MessageTypes : byte
    {
        PingRequest,
        PingResponse,
        LoginRequest,
        LoginResponse,
        CitiesRequest,
        CitiesResponse,
        RegisterRequest,
        RegisterResponse,
        GetDataRequest,
        GetDataResponse,
        OpenGamesRequest,
        OpenGamesResponse,
        GameParamsRequest,
        GameParamsResponse,
        GameStartRequest,
        GameStartResponse,
        GameRequest,
        WaitingForOpponentMessage,
        GameResponse,
        ChatRequest,
        ChatResponse,
        OpponentLeftMessage,
    }
}
