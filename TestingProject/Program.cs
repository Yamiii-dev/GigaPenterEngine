using GigaPenterEngine;
using GigaPenterEngine.Audio.MiniAudio.Audio;
using GigaPenterEngine.BaseComponents;
using GigaPenterEngine.Core;
using GigaPenterEngine.Input;
using GigaPenterEngine.Renderer.PentaKit;
using GigaPenterEngine.Renderer.PentaKit.Input;
using GigaPenterEngine.Networking.Server;
using GigaPenterEngine.Networking.Client;
using GigaPenterEngine.Networking;
using Client = GigaPenterEngine.Networking.Client.Client;

namespace TestingProject;

/*
    Project made purely for testing the functionalities of our engine.
*/
class Program
{
    static bool server = false;

    // Helper enum for declaring what type of data were sending/recieving, has to start from 1 as 0 is used for the welcome packet
    enum PacketTypes
    {
        Test = 1
    }

    // Packet handlers for our test packet, first is the server's handler, the second the client's
    // (You usually only handle a packet type on a single side, but since here were sending the packet on button press no matter the role of the program, we handle it on both sides.)

    public static void RecievedTestServer(int _id, Packet _packet)
    {
        string value = _packet.ReadString();
        Console.WriteLine($"Received test value from {_id}: {value}");
    }

    public static void RecievedTestClient(Packet _packet)
    { 
        string value = _packet.ReadString();
        Console.WriteLine($"Received test value: {value}");
    }

    static void Main(string[] args)
    {
        var choice = Console.ReadLine();
        int width = 800;
        int height = 600;
        Game game = new Game();
        RendererSystem renderer = new RendererSystem(width, height, "Penter hra", game);
        AudioPlayer audioPlayer = new AudioPlayer();
        game.AddSystem(renderer);
        game.AddSystem(audioPlayer);
        game.AddSystem(new PlayerController(audioPlayer));
        // First number is the max players, second is the port
        ServerManager serverManager = new ServerManager(4, 6969);
        // This fires anytime a new client connects to the server, it returns the ID and Username of the client, usually used to spawn the client in the game world.
        serverManager.ClientConnected += OnConnect;
        // Networking has to be used as a System so the TCP threads have access to the main thread
        if (choice == "server")
            game.AddSystem(serverManager);
        else if (choice == "client")
            game.AddSystem(new ClientManager("pepega", "127.0.0.1", 6969));
        

        // Anytime we create a method that handles a packet type, we have to add it to our packet handlers.
        // As you can see, the AddPacketHandler method uses an int for the packet type, thus why the enum is only used as a helper and needs to start at 1.
        if (choice == "server")
        {
            server = true;
            Server.AddPacketHandler((int)PacketTypes.Test, RecievedTestServer);
        }
        else if (choice == "client")
        {
            server = false;
            Client.AddPacketHandler((int)PacketTypes.Test, RecievedTestClient);
        }
        Entity Player = new Entity();
        Player.AddComponent(new Transform());
        Player.AddComponent(new PlayerComponent());
        Player.AddComponent(new SpriteRenderer("images/test.png"));
        Player.AddComponent(new AudioSource());
        //Player.GetComponent<Transform>().Position = new Vector3(width / 2, height / 2, 0);
        game.SetFrameRate(144);
        game.Run();
        
    }

    // We declare a method that handles when a client connects to our server.
    private static void OnConnect(ClientConnectArgs args)
    {
        Console.WriteLine($"Kokot cislo {args.id} sa pripojil z menom {args.name}");
    }

    public class PlayerComponent : Component
    {
        
    }

    public class PlayerController : GameSystem
    {
        
        private float speed = 1f;
        private AudioPlayer audioPlayer;
        private AudioFile clip = new AudioFile("sounds/test.wav");

        public PlayerController(AudioPlayer audioPlayer)
        {
            this.audioPlayer = audioPlayer;
        }
        
        public override void Update()
        {
            PlayerComponent? player = ComponentRegistry.GetComponents<PlayerComponent>().FirstOrDefault();
            if (player != null)
            {
                Transform transform = player.Parent.GetComponent<Transform>();
                if (InputHandler.instance.KeyDown(Key.W))
                {
                    transform.Position.Y += speed * Game.DeltaTime;
                }
                else if (InputHandler.instance.KeyDown(Key.S))
                {
                    transform.Position.Y -= speed * Game.DeltaTime;
                }
                if (InputHandler.instance.KeyDown(Key.A))
                {
                    transform.Position.X -= speed * Game.DeltaTime;
                }
                else if (InputHandler.instance.KeyDown(Key.D))
                {
                    transform.Position.X += speed * Game.DeltaTime;
                }

                if (InputHandler.instance.KeyPressed(Key.Space))
                {
                    //_audioPlayer.PlaySound(_sound);
                    //audioPlayer.PlayClip(player.Parent.GetComponent<AudioSource>(), clip);
                    Packet packet = new GigaPenterEngine.Networking.Packet(1);
                    packet.Write("kys");

                    // Use ServerSend to send packets to clients when running as a server
                    // User ClientSend to send packets to the server when running as a client
                    if (server)
                        ServerSend.SendTCPDataToAll(packet);
                    else
                        ClientSend.SendTCPData(packet);
                }
            }
        }
    }
}