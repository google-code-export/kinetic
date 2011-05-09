using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace _3D_Game
{
    public static class Globals
    {
        // game window
        public const int Width = 1400;
        public const int Height = 900;
        
        // environment and physics
        public const float t = 0.22f;   // time step for solver
        public const float g = -9.81f;   // gravity
        public const int xBound = 125;
        public const int yBound = 100;
        public const int zBound = 125;

        // item parameters
        public const int blockCount = 10;
    }
}
