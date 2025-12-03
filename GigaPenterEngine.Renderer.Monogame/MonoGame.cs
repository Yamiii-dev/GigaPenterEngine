using System;
using System.Linq;
using GigaPenterEngine.Renderer.Monogame.Input;
using GigaPenterEngine.Renderer.Monogame.Helper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GigaPenterEngine.Renderer.Monogame;

// The actual MonoGame instance
internal class MonoGame : Microsoft.Xna.Framework.Game
{
    public GraphicsDeviceManager _graphics;
    public SpriteBatch _spriteBatch;
    
    // Our engine instance
    GigaPenterEngine.Game engineGame;

    public MonoGame(GigaPenterEngine.Game _engineGame)
    {
        _graphics = new GraphicsDeviceManager(this);
        IsMouseVisible = true;
        engineGame = _engineGame;
        // If the framerate of our engine gets changed, change the framerate of the window
        engineGame.FrameRateChanged = (frameRate =>
        {
            TargetElapsedTime = TimeSpan.FromSeconds(1 / frameRate);
        });
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        // Create Texture2D instances for all our loaded Textures (required because we can't create a Texture2D without a GraphicsDevice
        TextureRegistry.RenderTextures(GraphicsDevice);
    }
    
    // Call the Draw function inside our Renderer system
    public Action<SpriteBatch>? OnDraw { get; set; }
    protected override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);

        OnDraw?.Invoke(_spriteBatch);
    }

    // If we close the window, notify the engine
    protected override void OnExiting(object sender, ExitingEventArgs args)
    {
        base.OnExiting(sender, args);
        if (engineGame != null)
            engineGame.RequestExit();
    }
}