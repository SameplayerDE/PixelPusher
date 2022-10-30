using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PixelPusher;

public class VirtualDisplay
{
    public event EventHandler<DisplayEventArgs> BufferChanged;

    private int _width;
    private int _height;

    private int _pixelCount;
    private Color[] _buffer;
    private Queue<DisplayCommand> _commands;

    public Color[] Buffer => _buffer;
    public int Width => _width;
    public int Height => _height;

    public VirtualDisplay(int width, int height)
    {
        _width = width;
        _height = height;
        _pixelCount = width * height;
        _commands = new Queue<DisplayCommand>();
        _buffer = Enumerable.Repeat(Color.Black, _pixelCount).ToArray();
    }

    public void SetPixel(int x, int y, int colorIndex = 5)
    {
        if (x < 0 || x >= _width || y < 0 || y >= _height)
        {
            return;
        }

        var index = _width * y + x;
        /*if (_buffer[index].Equals(color)) return;
        _buffer[index] = color;
        OnBufferChanged(new DisplayEventArgs()
        {
            VirtualDisplay = this
        });*/
        _commands.Enqueue(new DisplayCommand()
        {
            X = x,
            Y = y,
            Index = index,
            ColorIndex = colorIndex % 16
        });
    }

    public void SetPixel(float x, float y, int colorIndex = 5)
    {
        SetPixel((int)x, (int)y, colorIndex);
    }

    public void SetPixel(double x, double y, int colorIndex = 5)
    {
        SetPixel((int)x, (int)y, colorIndex);
    }

    public void ClearBuffer(int colorIndex = 0)
    {
        for (int i = _pixelCount - 1; i >= 0; i--)
        {
            _commands.Enqueue(new DisplayCommand()
            {
                X = i / _width,
                Y = i % _height,
                Index = i,
                ColorIndex = colorIndex
            });
        }
    }

    public void DrawRect(double x, double y, double xx, double yy, int colorIndex = 5)
    {
        for (var iy = y; iy <= yy; iy++)
        {
            for (var ix = x; ix <= xx; ix++)
            {
                SetPixel(ix, iy, colorIndex);
            }
        }
    }
    
    public void DrawCircle(int xm, int ym, int r, int colorIndex = 5)
    {
        if (r == 0)
        {
            SetPixel(xm, ym, colorIndex);
            return;
        }
        int x = -r, y = 0, err = 2-2*r; /* II. Quadrant */ 
        do {
            SetPixel(xm-x, ym+y, colorIndex); /*   I. Quadrant */
            SetPixel(xm-y, ym-x, colorIndex); /*  II. Quadrant */
            SetPixel(xm+x, ym-y, colorIndex); /* III. Quadrant */
            SetPixel(xm+y, ym+x, colorIndex); /*  IV. Quadrant */
            r = err;
            if (r >  x) err += ++x*2+1; /* e_xy+e_x > 0 */
            if (r <= y) err += ++y*2+1; /* e_xy+e_y < 0 */
        } while (x < 0);
    }
    
    public void DrawLine(int x0, int y0, int x1, int y1, int colorIndex = 5)
    {
        int dx =  Math.Abs (x1 - x0), sx = x0 < x1 ? 1 : -1;
        int dy = -Math.Abs (y1 - y0), sy = y0 < y1 ? 1 : -1; 
        int err = dx + dy, e2; /* error value e_xy */
 
        for (;;){  /* loop */
            SetPixel (x0,y0, colorIndex);
            if (x0 == x1 && y0 == y1) break;
            e2 = 2 * err;
            if (e2 >= dy) { err += dy; x0 += sx; } /* e_xy+e_x > 0 */
            if (e2 <= dx) { err += dx; y0 += sy; } /* e_xy+e_y < 0 */
        }
    }
    
    public void Apply()
    {
        while (_commands.Count > 0)
        {
            var command = _commands.Dequeue();
            //var index = _width * command.Y + command.X;
            _buffer[command.Index] = VirtualSystem.Colors[command.ColorIndex];
        }

        OnBufferChanged(new DisplayEventArgs()
        {
            VirtualDisplay = this
        });
    }

    protected virtual void OnBufferChanged(DisplayEventArgs args)
    {
        var handler = BufferChanged;
        handler?.Invoke(this, args);
    }
}