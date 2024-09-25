using System;
using BattleshipMessages;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using BattleshipMessages.Messages;
using System.Collections;
using BattleshipObjects;
using System.Net;

namespace BattleshipServer
{
    public class BattleshipServer
    {
        readonly Server TcpServer;
        readonly Database UserDatabase;
        readonly Dictionary<TcpClient, User> Users;
        readonly List<Game> Games;
        readonly int DefaultBoardWidth;
        readonly int DefaultBoardHeight;
        readonly ShipArray DefaultShips;

        public BattleshipServer(string connectionString, int port=1234, int DefaultBoardWidth=10, int DefaultBoardHeight=10, ShipArray DefaultShips=null)
        {
            TcpServer = new Server(port);
            UserDatabase = new Database(connectionString);
            Users = new Dictionary<TcpClient, User>();
            Games = new List<Game>();
            this.DefaultBoardWidth = DefaultBoardWidth;
            this.DefaultBoardHeight = DefaultBoardHeight;
            if (DefaultShips == null)
            {
                DefaultShips = new ShipArray(new Ship[] { new Ship(new bool[,] { { true, true, true, true, true } }),
                                                          new Ship(new bool[,] { { true, true, true, true } }), 
                                                          new Ship(new bool[,] { { true, true, true } }), 
                                                          new Ship(new bool[,] { { true, true, true } }),
                                                          new Ship(new bool[,] { { true, true } })});
            }
            this.DefaultShips = DefaultShips;
            TcpServer.MessageReceived += Server_MessageReceived;
            TcpServer.Disconnected += Server_ClientDisconnected;
            TcpServer.ExceptionReceived += Server_ExceptionReceived;
        }

        public void Start()
        {
            TcpServer.Start();
            UserDatabase.Connect();
        }

        public void Stop()
        {
            TcpServer.Stop();
            UserDatabase.Disconnect();
        }

        public void RemoveClient(TcpClient client)
        {
            if (client == null)
                return;
            if (!Users.ContainsKey(client))
                return;
            Game game = Games.FirstOrDefault(x => x.Player1 == Users[client] || x.Player2 == Users[client]);
            if (game != null)
            {
                if (game.Player2 != null)
                {
                    TcpClient otherClient = Users.Keys.FirstOrDefault(x => Users[x] == (game.Player1 == Users[client] ? game.Player2 : game.Player1));
                    TcpServer.Send(new OpponentLeftMessage(), otherClient);
                }
                Games.Remove(game);
            }
            Console.WriteLine(Users[client].Username + " logged out");
            Users.Remove(client);
        }

        public void Server_MessageReceived(object sender, (IMessage, TcpClient) e)
        {
            IMessage message = e.Item1;
            TcpClient client = e.Item2;
            switch (message.MessageType) 
            {
                case MessageTypes.PingRequest:
                    HandlePingRequest(client);
                    break;
                case MessageTypes.LoginRequest:
                    HandleLoginRequest((LoginRequest)message, client);
                    break;
                case MessageTypes.CitiesRequest:
                    HandleCitiesRequest(client);
                    break;
                case MessageTypes.RegisterRequest:
                    HandleRegisterRequest((RegisterRequest)message, client);
                    break;
                case MessageTypes.GetDataRequest:
                    HandleGetDataRequest(client);
                    break;
                case MessageTypes.OpenGamesRequest:
                    HandleOpenGamesRequest(client);
                    break;
                case MessageTypes.GameParamsRequest:
                    HandleGameParamsRequest((GameParamsRequest)message, client);
                    break;
                case MessageTypes.GameStartRequest:
                    HandleGameStartRequest((GameStartRequest)message, client);
                    break;
                case MessageTypes.GameRequest:
                    HandleGameRequest((GameRequest)message, client);
                    break;
                case MessageTypes.ChatRequest:
                    HandleChatRequest((ChatRequest)message, client);
                    break;
                default:
                    RemoveClient(client);
                    client.Close();
                    break;
            }
        }

        public void Server_ClientDisconnected(object sender, TcpClient client)
        {
            RemoveClient(client);
        }

        public void Server_ExceptionReceived(object sender, (Exception, TcpClient) e)
        {
            Console.WriteLine(e.Item1.Message);
            RemoveClient(e.Item2);
            e.Item2?.Close();
        }

        public void HandlePingRequest(TcpClient client)
        {
            TcpServer.Send(new PingResponse(), client);
        }

        public void HandleLoginRequest(LoginRequest request, TcpClient client)
        {
            if (Users.ContainsKey(client))
            {
                TcpServer.Send(new LoginResponse(false, "Already logged in to account"), client);
                Console.WriteLine(Users[client].Username + " failed to log in");
            }
            else if (Users.Keys.Any(x => Users[x].Username == request.Username))
            {
                TcpServer.Send(new LoginResponse(false, "Logged in on another computer"), client);
                Console.WriteLine(request.Username + " failed to log in");
            }
            else if (RegisterFilter.CheckUsername(request.Username) && RegisterFilter.CheckPassword(request.Password) && UserDatabase.IsExistingUser(request.Username) && UserDatabase.CheckPassword(request.Username, request.Password))
            {
                Users.Add(client, UserDatabase.GetUserData(request.Username));
                TcpServer.Send(new LoginResponse(true), client);
                Console.WriteLine(request.Username + " logged in");
            }
            else
            {
                TcpServer.Send(new LoginResponse(false, "Incorrect username or password"), client);
                Console.WriteLine(request.Username + " failed to log in");
            }
        }

        public void HandleCitiesRequest(TcpClient client)
        {
            TcpServer.Send(new CitiesResponse(RegisterFilter.Cities), client);
        }

        public void HandleRegisterRequest(RegisterRequest request, TcpClient client)
        {
            bool success = false;
            if (UserDatabase.IsExistingUser(request.User.Username))
                TcpServer.Send(new RegisterResponse(false, "Username already exists"), client);
            else if (UserDatabase.IsExistingEmail(request.User.Email))
                TcpServer.Send(new RegisterResponse(false, "Email already exists"), client);
            else if (!RegisterFilter.CheckUsername(request.User.Username))
                TcpServer.Send(new RegisterResponse(false, "Invalid username"), client);
            else if (!RegisterFilter.CheckPassword(request.Password))
                TcpServer.Send(new RegisterResponse(false, "Invalid password"), client);
            else if (!RegisterFilter.CheckEmail(request.User.Email))
                TcpServer.Send(new RegisterResponse(false, "Invalid email"), client);
            else if (!RegisterFilter.CheckFirstName(request.User.FirstName))
                TcpServer.Send(new RegisterResponse(false, "Invalid first name"), client);
            else if (!RegisterFilter.CheckLastName(request.User.LastName))
                TcpServer.Send(new RegisterResponse(false, "Invalid last name"), client);
            else if (!RegisterFilter.CheckCity(request.User.City))
                TcpServer.Send(new RegisterResponse(false, "Invalid city"), client);
            else
            {
                success = true;
                UserDatabase.AddUser(request.User, request.Password);
                TcpServer.Send(new RegisterResponse(true), client);
                Console.WriteLine(request.User.Username + " registered");
            }
            if (!success)
                Console.WriteLine(request.User.Username + " failed to register");
        }

        public void HandleGetDataRequest(TcpClient client)
        {
            TcpServer.Send(new GetDataResponse(Users[client]), client);
        }

        public void HandleOpenGamesRequest(TcpClient client)
        {
            TcpServer.Send(new OpenGamesResponse(Games.Where(x => x.Player2 == null).Select(x => new GameInfo(x)).ToArray()), client);
        }

        public void HandleGameParamsRequest(GameParamsRequest request, TcpClient client)
        {
            int boardWidth = DefaultBoardWidth;
            int boardHeight = DefaultBoardHeight;
            ShipArray ships = DefaultShips;
            if (request.GameId != -1)
            {
                Game game = Games.FirstOrDefault(x => x.GameId == request.GameId);
                if (game != null)
                {
                    boardWidth = game.Board1.Width;
                    boardHeight = game.Board1.Height;
                    ships = new ShipArray(game.Board1.Ships.Keys.ToArray());
                }
            }
            TcpServer.Send(new GameParamsResponse(boardWidth, boardHeight, ships), client);
        }

        public void HandleGameStartRequest(GameStartRequest request, TcpClient client)
        {
            Game participatingGame = Games.FirstOrDefault(x => x.Player1 == Users[client] || x.Player2 == Users[client]);
            if (participatingGame != null)
            {
                if (participatingGame.Player2 == null)
                {
                    Games.Remove(participatingGame);
                }
                else
                {
                    client.Close();
                    return;
                }
            }
            if (request.IsJoiningGame)
            {
                Game game = Games.FirstOrDefault(x => x.GameId == request.GameID);
                
                if (game != null && game.Player2 == null && request.Ships.Ships.Length == request.ShipLocations.Length)
                {
                    ShipArray gameShips = new ShipArray(game.Board1.Ships.Keys.ToArray());
                    if (!gameShips.IsEquals(request.Ships)) return;
                    Dictionary<Ship, (byte, byte)> ShipLocations = new Dictionary<Ship, (byte, byte)>();
                    for (int i = 0; i < request.ShipLocations.Length; i++)
                    {
                        ShipLocations.Add(request.Ships.Ships[i], request.ShipLocations[i]);
                    }
                    Board board = new Board(game.Board1.Width, game.Board1.Height, ShipLocations);
                    if (!board.IsValid())
                    {
                        return;
                    }
                    game.Player2 = Users[client];
                    game.Board2 = board;
                    TcpServer.Send(new GameStartResponse(!game.Player1Turn, game.Player1.Username), client);
                    TcpServer.Send(new GameStartResponse(game.Player1Turn, game.Player2.Username), Users.Keys.FirstOrDefault(x => Users[x] == game.Player1));
                    Console.WriteLine(game.Player1.Username + " and " + game.Player2.Username + " started a game");
                }
            }
            else if (request.IsCustomGame)
            {
                ShipArray ships = request.Ships;
                foreach (Ship ship in ships.Ships)
                {
                    ship.Shrink();
                }
                Game game = Games.FirstOrDefault(x => x.Player2 == null && x.IsCustom && x.Board1.Width == request.BoardWidth && x.Board1.Height == request.BoardHeight && ships.IsEquals(new ShipArray(x.Board1.Ships.Keys.ToArray())));
                Dictionary<Ship, (byte, byte)> ShipLocations = new Dictionary<Ship, (byte, byte)>();
                for (int i = 0; i < ships.Ships.Length; i++)
                {
                    ShipLocations.Add(ships.Ships[i], request.ShipLocations[i]);
                }
                Board board = new Board(request.BoardWidth, request.BoardHeight, ShipLocations);
                if (!board.IsValid())
                {
                    return;
                }
                if (game == null)
                {
                    game = new Game(Users[client], null, board, null);
                    Games.Add(game);
                    TcpServer.Send(new WaitingForOpponentMessage(game.GameId), client);
                    Console.WriteLine(Users[client].Username + " created a custom game");
                }
                else
                {
                    game.Player2 = Users[client];
                    TcpServer.Send(new GameStartResponse(!game.Player1Turn, game.Player1.Username), client);
                    TcpServer.Send(new GameStartResponse(game.Player1Turn, game.Player2.Username), Users.Keys.FirstOrDefault(x => Users[x] == game.Player1));
                    Console.WriteLine(game.Player1.Username + " and " + game.Player2.Username + " started a game");
                }
            }
            else
            {
                Game game = Games.FirstOrDefault(x => x.Player2 == null && !x.IsCustom);
                if (!DefaultShips.IsEquals(request.Ships)) return;
                Dictionary<Ship, (byte, byte)> ShipLocations = new Dictionary<Ship, (byte, byte)>();
                for (int i = 0; i < request.ShipLocations.Length; i++)
                {
                    ShipLocations.Add(request.Ships.Ships[i], request.ShipLocations[i]);
                }
                Board board = new Board(DefaultBoardWidth, DefaultBoardHeight, ShipLocations);
                if (!board.IsValid())
                {
                    return;
                }
                if (game == null)
                {
                    game = new Game(Users[client], null, board, null);
                    Games.Add(game);
                    TcpServer.Send(new WaitingForOpponentMessage(game.GameId), client);
                    Console.WriteLine(Users[client].Username + " created a game");
                }
                else
                {
                    game.Player2 = Users[client];
                    game.Board2 = board;
                    TcpServer.Send(new GameStartResponse(!game.Player1Turn, game.Player1.Username), client);
                    TcpServer.Send(new GameStartResponse(game.Player1Turn, game.Player2.Username), Users.Keys.FirstOrDefault(x => Users[x] == game.Player1));
                    Console.WriteLine(game.Player1.Username + " and " + game.Player2.Username + " started a game");
                }
            }
        }

        public void HandleGameRequest(GameRequest request, TcpClient client)
        {
            Game game = Games.FirstOrDefault(x => x.Player1 == Users[client] || x.Player2 == Users[client]);
            if (game == null)
            {
                return;
            }
            bool isPlayer1 = game.Player1 == Users[client];
            if ((isPlayer1 && !game.Player1Turn) || (!isPlayer1 && game.Player1Turn))
            {
                return;
            }
            bool hit = game.Hit(request.Location);
            Ship sunkShip = game.GetShipSunkByLastHit();
            bool isSunk = sunkShip != null;
            bool isGameOver = game.IsGameOver();
            Board opponentBoard = isPlayer1 ? game.Board2 : game.Board1;
            (byte, byte) sunkShipLocation = (0, 0);
            if (isSunk)
            {
                sunkShipLocation = opponentBoard.Ships[sunkShip];
            }
            TcpServer.Send(new GameResponse(true, hit, isSunk, isGameOver, request.Location, sunkShipLocation.Item1, sunkShipLocation.Item2, sunkShip), client);
            TcpClient otherClient = Users.Keys.FirstOrDefault(x => Users[x] == (isPlayer1 ? game.Player2 : game.Player1));
            TcpServer.Send(new GameResponse(false, hit, isSunk, isGameOver, request.Location), otherClient);
            if (isGameOver)
            {
                Games.Remove(game);
                Console.WriteLine(game.Player1.Username + " and " + game.Player2.Username + " finished a game");
            }
        }

        public void HandleChatRequest(ChatRequest request, TcpClient client)
        {
            Game game = Games.FirstOrDefault(x => x.Player1 == Users[client] || x.Player2 == Users[client]);
            if (game == null)
            {
                return;
            }
            TcpClient otherClient = Users.Keys.FirstOrDefault(x => Users[x] == (game.Player1 == Users[client] ? game.Player2 : game.Player1));
            if (otherClient != null)
            {
                TcpServer.Send(new ChatResponse(request.Message), otherClient);
            }
        }
    }
}
