using System.Data;
using System.Diagnostics;
using GigaPenterEngine.Core;
using GigaPenterEngine.Input;

namespace GigaPenterEngine;

public class Game
{
    private readonly Stopwatch _timer = Stopwatch.StartNew();
    private readonly List<ISystem> _systems = new List<ISystem>();

    private bool _running = false;
    private int _frameRate = 60;
    private float _lastUpdate = 0;
    
    public static float DeltaTime = 0;

    public Game()
    {
        // Creates an event handler for the game getting closed (used for when a window of a renderer gets closed)
        // This even handler stops the game loop
        OnExit += () =>
        {
            _running = false;
        };
    }
    
    public void Run()
    {
        // Run Starting function of each system and start game loop, where we run the Update function of each system.
        _running = true;
        foreach (ISystem system in _systems)
        {
            system.Start();
        }

        while (_running)
        {
            Update();
        }
    }

    public void Stop()
    {
        // Stops the game loop
        _running = false;
    }

    public void Update()
    {
        // Calculate deltaTime (the time since our last Update)
        DeltaTime = (float)_timer.Elapsed.TotalSeconds - _lastUpdate;
        _lastUpdate = (float)_timer.Elapsed.TotalSeconds;
        // Call the Update function of each of our systems
        foreach (ISystem system in _systems)
        {
            system.Update();
        }
        // Caps the framerate
        Thread.Sleep((int)((1.0f / _frameRate) * 1000));
    }
    
    // Event for when the game window gets closed
    public event Action? OnExit;
    public void RequestExit()
    {
        OnExit?.Invoke();
    }
    
    public void SetFrameRate(int frameRate)
    {
        this._frameRate = frameRate;
        FrameRateChanged.Invoke(this._frameRate);
    }
    public Action<float>? FrameRateChanged { get; set; }
    
    public void AddSystem(ISystem system)
    {
        _systems.Add(system);
    }
}