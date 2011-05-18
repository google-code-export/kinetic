using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace _3D_Game
{
    public class Collision
    {
        //public enum cType { POINT = 1, EDGE = 2, FACE = 3 };

        public float pDist;              // (p) penetration distance
        public Vector3 pNorm;            // (h) collision axis
        public List<Vector3> points;    // (hitPoints) list of contact points
        public int pIndex;            // (numHitPoints) index of contact points [??]

        public Collision()
        {
        }

        public Collision(float p, Vector3 h, Vector3[] hitPoints, int numHitPoints)
        {
            pDist = p;
            pNorm = h;
            points = new List<Vector3>();
            for (int i = 0; i < numHitPoints; i++)
            {
                points.Add(hitPoints[i]);
            }

            pIndex = numHitPoints;
        }
    }
}
