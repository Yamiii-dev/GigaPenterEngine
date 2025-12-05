using GigaPenterEngine;
using GigaPenterEngine.Audio.MiniAudio.Audio;
using GigaPenterEngine.BaseComponents;
using GigaPenterEngine.Core;
using GigaPenterEngine.Input;
using GigaPenterEngine.Renderer.PentaKit;
using GigaPenterEngine.Renderer.PentaKit.Input;

namespace TestingProject;

/*
    Project made purely for testing the functionalities of our engine.
*/
class Program
{
    static float deltaTime = 0;
    static void Main(string[] args)
    {
        int width = 800;
        int height = 600;
        Game game = new Game();
        RendererSystem renderer = new RendererSystem(width, height, "Penter hra", game);
        AudioPlayer audioPlayer = new AudioPlayer();
        game.AddSystem(renderer);
        game.AddSystem(audioPlayer);
        game.AddSystem(new PlayerController(audioPlayer));
        Entity Player = new Entity();
        Player.AddComponent(new Transform());
        Player.AddComponent(new PlayerComponent());
        Player.AddComponent(new SpriteRenderer("images/test.png"));
        Player.AddComponent(new AudioSource());
        //Player.GetComponent<Transform>().Position = new Vector3(width / 2, height / 2, 0);
        game.SetFrameRate(144);
        game.Run();
        
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
                    audioPlayer.PlayClip(player.Parent.GetComponent<AudioSource>(), clip);
                }
            }
        }
    }
}