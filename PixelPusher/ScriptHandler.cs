using System;
using System.Collections.Generic;
using System.IO;
using NLua;

namespace PixelPusher;

public static class ScriptHandler
{
    private static string _root;
    public static string ScriptPath = string.Empty;
    public static readonly Lua Lua;
    
    private static string[] _predefinedFunctions;
    private static Dictionary<string, LuaFunction> _referencedFunctions;

    static ScriptHandler()
    {
        Lua = new Lua();
        Setup();
    }

    private static void Setup()
    {
        _root = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        _root = Path.Combine(_root, "cicho-8");
        Directory.CreateDirectory(_root);
        _predefinedFunctions = new[]
        {
            "_init",
            "_update",
            "_draw"
        };
        _referencedFunctions = new Dictionary<string, LuaFunction>();
        RegisterFunctions();
    }
    
    public static void Load(string scriptPath)
    {
        if (!File.Exists(Path.Combine(_root, scriptPath)))
        {
            var path = Path.Combine(_root, scriptPath);
            Console.WriteLine(path);
            throw new FileNotFoundException();
        }   
        ScriptPath = Path.Combine(_root, scriptPath);
    }

    public static void Run(string args = "")
    {
        if (!File.Exists(ScriptPath))
        {
            throw new FileNotFoundException();
        }
        _referencedFunctions.Clear();
        Lua.DoFile(ScriptPath);
        foreach (var function in _predefinedFunctions)
        {
            if (Lua[function] is LuaFunction func)
            {
                _referencedFunctions[function] = func;
            }
        }
    }

    public static bool Contains(string func)
    {
        return _referencedFunctions.ContainsKey(func);
    }
    
    public static bool Invoke(string func)
    {
        if (!_referencedFunctions.ContainsKey(func)) return false;
        _referencedFunctions[func].Call();
        return true;
    }
    
    private static void RegisterFunctions()
    {
    }
}