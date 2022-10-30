using Calendula.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PixelPusher;

public static class VirtualSystem
{
    public static Color[] Colors { get; } =
    {
        Util.FromHex("16171a"),
        Util.FromHex("7f0622"),
        Util.FromHex("d62411"),
        Util.FromHex("ff8426"),
        Util.FromHex("ffd100"),
        Util.FromHex("fafdff"),
        Util.FromHex("ff80a4"),
        Util.FromHex("ff2674"),
        Util.FromHex("94216a"),
        Util.FromHex("430067"),
        Util.FromHex("234975"),
        Util.FromHex("68aed4"),
        Util.FromHex("bfff3c"),
        Util.FromHex("10d275"),
        Util.FromHex("007899"),
        Util.FromHex("002859"),
    };

    public static VirtualDisplay Display;
    private static bool _running;

    public static bool IsRunning => _running;

    public static void Boot()
    {
        Display = new VirtualDisplay(128, 128);
        Display.ClearBuffer();
        
        /*ScriptHandler.Lua.RegisterFunction("cls", Display, typeof(VirtualDisplay).GetMethod("ClearBuffer"));
        ScriptHandler.Lua.RegisterFunction("plot", Display, typeof(VirtualDisplay).GetMethod("SetPixel"));
        ScriptHandler.Lua.RegisterFunction("rect", Display, typeof(VirtualDisplay).GetMethod("DrawRect"));
        ScriptHandler.Lua.RegisterFunction("circ", Display, typeof(VirtualDisplay).GetMethod("DrawCircle"));*/
        
        ScriptHandler.Lua.RegisterFunction("btn", typeof(VirtualSystem).GetMethod("Button"));
        
        ScriptHandler.Lua["display"] = Display;
        ScriptHandler.Load("api.lua");
        ScriptHandler.Run();
        
        _running = true;
    }

    public static void Shutdown()
    {
        _running = false;
    }
    
    public static bool Button(int index)
    {
        return index switch
        {
            0 => HxKeyboard.IsKeyDown(Keys.Left),
            1 => HxKeyboard.IsKeyDown(Keys.Right),
            2 => HxKeyboard.IsKeyDown(Keys.Up),
            3 => HxKeyboard.IsKeyDown(Keys.Down),
            4 => HxKeyboard.IsKeyDown(Keys.X),
            5 => HxKeyboard.IsKeyDown(Keys.C),
            _ => false
        };
    }
    
}