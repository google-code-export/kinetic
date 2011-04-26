using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3D_Game
{
    /// <summary>
    /// This class will serve as a intermediate shell for holding collision detection functions ...
    /// </summary>
    class Collider
    {
        // VERY IMPORTANT
        // broad phase detector
        public void RunBroadPhaseDetection()
        {
            // return list of blocks that we care about
        }

        // fast bounding sphere test to follow broad-phase
        public bool SphereTest(int obj1, int obj2)
        {
            // run on two objects or something, currently two indices to model/mesh array or list
            // return boolean to indicate collision or not
            return false;
        }

        // find 'exact' collision point
        public Vector3? FindCollisionPoint()
        {
            // find exact point of collision
            return Vector3.Zero;
        }

        // VERY IMPORTANT
        // collision solver
        public bool ResolveCollision(int obj1, int obj2, Vector3? collidePoint)
        {
            // can probably run FindCollisionPoint() directly inside function call for this
            if (collidePoint != null)
            {
                // MEAT OF THE PHYSICS HAPPENS IN HERE
                // probably will have calls to other helper functions

                // calculate new position and velocity of obj1 and obj2

                // collided, objects taken care of
                return true;
            }

            // didn't actually collide, so return false
            return false;
        }

    }
}
