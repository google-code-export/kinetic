using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace _3D_Game
{
    /// <summary>
    /// OBB collision volume object to be used in collision detection and
    /// resolution
    /// </summary>
    public class CollisionVolume
    {
        public Vector3 center;
        public Vector3[] axes;
        public Vector3 ex;

        public CollisionVolume(Vector3 e)
        {
            center = Vector3.Zero;
            axes = new Vector3[3];
            axes[0] = new Vector3(1f, 0f, 0f);
            axes[1] = new Vector3(0f, 1f, 0f);
            axes[2] = new Vector3(0f, 0f, 1f);
            ex = e;
        }
    }
}
