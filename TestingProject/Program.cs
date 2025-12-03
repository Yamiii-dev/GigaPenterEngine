using GigaPenter.Renderer.Monogame;
using GigaPenter.Renderer.Monogame.Helper;
using GigaPenter.Renderer.Monogame.Input;
using GigaPenterEngine;
using GigaPenterEngine.BaseComponents;
using GigaPenterEngine.Core;
using GigaPenterEngine.Input;

namespace TestingProject;

/*
    Project made purely for testing the functionalities of our engine.
*/
class Program
{
    static float deltaTime = 0;
    static void Main(string[] args)
    {
        Game game = new Game();
        RendererSystem renderer = new RendererSystem(game);
        game.AddSystem(renderer);
        game.AddSystem(new PlayerController());
        Entity Player = new Entity();
        Player.AddComponent(new Transform());
        Player.AddComponent(new PlayerComponent());
        Player.AddComponent(new RenderableComponent(ContentLoader.LoadTexture("images/test.png")));
        Player.GetComponent<Transform>().Position = new Vector3(renderer.GetWindowSize().X / 2, renderer.GetWindowSize().Y / 2, 0);
        game.SetFrameRate(144);
        game.Run();
        
    }

    public class PlayerComponent : Component
    {
        
    }

    public class PlayerController : GameSystem
    {
        private float speed = 150f;
        public override void Update()
        {
            PlayerComponent? player = ComponentRegistry.GetComponents<PlayerComponent>().FirstOrDefault();
            if (player != null)
            {
                Transform transform = player.Parent.GetComponent<Transform>();
                if (InputHandler.KeyPressed(Key.W))
                {
                    transform.Position.Y -= speed * Game.DeltaTime;
                }
                else if (InputHandler.KeyPressed(Key.S))
                {
                    transform.Position.Y += speed * Game.DeltaTime;
                }
                if (InputHandler.KeyPressed(Key.A))
                {
                    transform.Position.X -= speed * Game.DeltaTime;
                }
                else if (InputHandler.KeyPressed(Key.D))
                {
                    transform.Position.X += speed * Game.DeltaTime;
                }
            }
        }
    }
}