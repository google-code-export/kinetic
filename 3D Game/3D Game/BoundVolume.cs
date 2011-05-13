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
    public class BoundVolume
    {
        public Vector3 center;
        public Vector3[] axes;
        public Vector3 ex;

        public BoundVolume(Vector3 e)
        {
            center = Vector3.Zero;
            axes = new Vector3[3];
            axes[0] = new Vector3(1f, 0f, 0f);
            axes[1] = new Vector3(0f, 1f, 0f);
            axes[2] = new Vector3(0f, 0f, 1f);
            ex = e;
        }

        public void SetRot(Quaternion q)
        {
            axes[0] = Vector3.UnitX;
            axes[1] = Vector3.UnitY;
            axes[2] = Vector3.UnitZ;

            //axes[0] = Vector3.Transform(axes[0], q);
            //axes[1] = Vector3.Transform(axes[1], q);
            //axes[2] = Vector3.Transform(axes[2], q);
            for (int i = 0; i < 3; i++)
            {
                axes[i] = Vector3.Transform(axes[i], q);
                axes[i].Normalize();
            }
        }
    }
}
