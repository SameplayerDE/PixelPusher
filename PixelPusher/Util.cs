using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace PixelPusher;

public static class Util
{
    public static Color FromHex(string hex)
    {
        var enumerable = SplitInParts(hex, 2);
        var rgb = enumerable.ToArray();
        var r = Convert.ToInt32(rgb[0], 16);
        var g = Convert.ToInt32(rgb[1], 16);
        var b = Convert.ToInt32(rgb[2], 16);
        return new Color(r, g, b);
    }

    private static IEnumerable<string> SplitInParts(this string s, int partLength) {
        if (s == null)
            throw new ArgumentNullException(nameof(s));
        if (partLength <= 0)
            throw new ArgumentException("Part length has to be positive.", nameof(partLength));

        for (var i = 0; i < s.Length; i += partLength)
            yield return s.Substring(i, Math.Min(partLength, s.Length - i));
    }
}