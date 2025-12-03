using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GigaPenterEngine.BaseComponents;
using GigaPenterEngine.Core;
using GigaPenterEngine.Renderer.Monogame.Helper;
using GigaPenterEngine.Renderer.Monogame.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Game = GigaPenterEngine.Game;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Vector3 = Microsoft.Xna.Framework.Vector3;

namespace GigaPenterEngine.Renderer.Monogame;

public class RendererSystem : GameSystem
{
    private MonoGame game;

    public RendererSystem(Game _game)
    {
        game = new MonoGame(_game);
        game.OnDraw = Draw;
    }
    
    public override void Start()
    {
        // Run the MonoGame instance in its own thread as to not block our engine's game loop
        Thread renderThread = new Thread(() =>
        {
            game.Run();
        });
        renderThread.IsBackground = true;
        renderThread.Start();
    }

    public override void Update()
    {
        // Read the state of the keyboard, mouse and gamepad, and save their previous state for use in KeyDown
        InputHandler.lastKeyboardState = InputHandler.keyboard;
        InputHandler.lastMouseState = InputHandler.mouse;
        InputHandler.lastGamePadState = InputHandler.gamePad;
        InputHandler.keyboard = Keyboard.GetState();
        InputHandler.mouse = Mouse.GetState();
        InputHandler.gamePad = GamePad.GetState(PlayerIndex.One);
    }

    public void Draw(SpriteBatch _spriteBatch)
    {
        game.GraphicsDevice.Clear(Color.Black);
        
        // Check if we have an active camera
        CameraComponent? camera = ComponentRegistry.GetComponents<CameraComponent>().FirstOrDefault();
        if (camera != null)
        {
            // If so, start the sprite batch with the camera's transformation matrix
            Transform cameraTransform = camera.Parent.GetComponent<Transform>();
            Matrix cameraMatrix = Matrix.CreateTranslation(cameraTransform.Position.ToMonoGame());
            cameraMatrix *= Matrix.CreateRotationZ(cameraTransform.Rotation);
            cameraMatrix *= Matrix.CreateScale(camera.scale);
            _spriteBatch.Begin(transformMatrix: cameraMatrix);
        }
        else // If not, just start the sprite batch regularly
            _spriteBatch.Begin();
        
        // Get all the renderable components
        List<RenderableComponent> components = ComponentRegistry.GetComponents<RenderableComponent>();

        foreach (RenderableComponent component in components)
        {
            // Get the transform of the component's entity parent
            Transform transform = component.Parent.GetComponent<Transform>();
            if (transform == null)
            {
                // Don't continue if we don't have a Transform component
                Console.WriteLine("Render Entity doesn't have a transform!!");
                continue;
            }
            // Turn our custom Vector2 to the MonoGame one, and offset it so the sprite gets rendered in the middle of our position
            Vector2 position = new Vector2(transform.Position.X, transform.Position.Y) - (component.Texture.texture.Bounds.Size.ToVector2() / 2);
            // Check if our Texture2D got created, if yes, render our sprite
            if(component.Texture.texture != null)
                _spriteBatch.Draw(component.Texture.texture, position, null, component.Color, transform.Rotation, Vector2.Zero, transform.Scale.ToMonoGame(), SpriteEffects.None, 0f);
            else
            {
                // If not, create it
                Console.WriteLine("Texture not created.");
                TextureRegistry.RenderTexture(game.GraphicsDevice, component.Texture);
            }
        }

        _spriteBatch.End();
    }


    public GigaPenterEngine.Core.Vector2 GetWindowSize()
    {
        return new GigaPenterEngine.Core.Vector2(game.Window.ClientBounds.Width, game.Window.ClientBounds.Height);
    }

    public void SetWindowSize(GigaPenterEngine.Core.Vector2 windowSize)
    {
        game._graphics.PreferredBackBufferWidth = (int)windowSize.X;
        game._graphics.PreferredBackBufferHeight = (int)windowSize.Y;
        game._graphics.ApplyChanges();
    }
}