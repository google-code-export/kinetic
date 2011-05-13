using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3D_Game
{
    public class Collision
    {
        public enum cType { POINT = 1, EDGE = 2, FACE = 3 };
        int dummy;

        public Collision()
        {
            dummy = 0;
        }
    }
}
