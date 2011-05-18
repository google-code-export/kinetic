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
        public const float t = 0.08f;               // time step for solver - default 0.17f
        public const float g = -9.81f;              // gravity
        public const float cSpeed = 2.0f;           // default camera speed
        public const float bSpeed = 4.0f;           // default block speed
        public const float yEpsilon = 0.05f;        // for use in stacking hack
        public const float mEpsilon = 0.0000005f;   // for use in OBB collision test
        public const float cvEpsilon = 0.01f;       // used in FindContactVertices
        public const float vEpsilon = 0.5f;         // minimum distance betw contact vertices
        public const float eeEpsilon = 0.00001f;    // for closest edge-edge point 
        public const float xBound = 125f;
        public const float yBound = 92.5f;
        public const float zBound = 125f;

        // item parameters
        public const int blockCount = 10;

        // collision handling
        public enum colType { Point, Edge, Face };
    }
}
