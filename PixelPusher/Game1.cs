using System;
using System.Linq;
using Calendula.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PixelPusher;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphicsDeviceManager;
    private SpriteBatch _spriteBatch;
    
    public bool IsFullScreen
    {
        get => _graphicsDeviceManager.IsFullScreen;
        set
        {
            _graphicsDeviceManager.IsFullScreen = value;
            _graphicsDeviceManager.ApplyChanges();
        }
    }

    private Texture2D _display;
    private Rectangle _destination;

    public Game1()
    {
        _graphicsDeviceManager = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphicsDeviceManager.PreferredBackBufferWidth = 512;
        _graphicsDeviceManager.PreferredBackBufferHeight = 512;
        _graphicsDeviceManager.PreferredBackBufferFormat = SurfaceFormat.Color;
        _graphicsDeviceManager.PreferredDepthStencilFormat = DepthFormat.Depth24; // <-- set depth here
        
        _graphicsDeviceManager.HardwareModeSwitch = false;
        _graphicsDeviceManager.PreferMultiSampling = false;
        _graphicsDeviceManager.IsFullScreen = false;
        _graphicsDeviceManager.ApplyChanges();
        
        VirtualSystem.Boot();
        var w = VirtualSystem.Display.Width;
        var h = VirtualSystem.Display.Height;
        _display = new Texture2D(GraphicsDevice, w, h);
        _display.SetData(VirtualSystem.Display.Buffer);

        VirtualSystem.Display.BufferChanged += UpdateTexture;

        int color = Convert.ToInt32("16", 16);
        Console.WriteLine(color);

        ScriptHandler.Load("test.lua");
        ScriptHandler.Run();

        Window.AllowUserResizing = true;
        Window.ClientSizeChanged += OnResize;

        PerformScreenFit();

        base.Initialize();
    }

    private void PerformScreenFit()
    {
        var rectangle = GraphicsDevice.Viewport.Bounds;
        var width = rectangle.Width;
        var height = rectangle.Height;
        var aspectRatioViewport = GraphicsDevice.Viewport.AspectRatio;
        var aspectRatioVirtualDisplay = (float)VirtualSystem.Display.Width / VirtualSystem.Display.Height;

        float rx = 0f;
        float ry = 0f;
        float rw = width;
        float rh = height;
        
        if (aspectRatioViewport > aspectRatioVirtualDisplay)
        {
            rw = rh * aspectRatioVirtualDisplay;
            rx = (width - rw) / 2f;
        }
        else if (aspectRatioViewport < aspectRatioVirtualDisplay)
        {
            rh = rw / aspectRatioVirtualDisplay;
            ry = (height - rh) / 2f;
        }

        _destination = new Rectangle((int)rx, (int)ry, (int)rw, (int)rh);
    }
    
    private void OnResize(object sender, EventArgs e)
    {
        PerformScreenFit();
    }

    private void UpdateTexture(object sender, DisplayEventArgs e)
    {
        _display.SetData(e.VirtualDisplay.Buffer);
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        HxKeyboard.Update(gameTime);

        if (HxKeyboard.IsKeyDown(Keys.LeftAlt))
        {
            if (HxKeyboard.IsKeyDownOnce(Keys.Enter))
            {
                IsFullScreen = !IsFullScreen;
            }
        }
        
        ScriptHandler.Invoke("_update");
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        /*
         * Render Virtual System
         */

        ScriptHandler.Invoke("_draw");
        
        VirtualSystem.Display.Apply();

        /*
         * 
         */

        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _spriteBatch.Draw(_display, _destination, Color.White);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}