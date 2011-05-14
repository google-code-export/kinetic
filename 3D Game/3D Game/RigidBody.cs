#define Q_DEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace _3D_Game
{
    /// <summary>
    /// This class contains the physics data for an object
    /// </summary>
    public class RigidBody
    {
        #region Fields

        public Vector3 pos { get; protected set; }
        public Quaternion rot { get; protected set; }
        public BoundVolume obb { get; protected set; }
        private Quaternion aVel;
        private Vector3 vel;
        private Vector3 accel;
        private Vector3 force;
        private float mass;

        #endregion

        #region Initialize

        // Constructor
        public RigidBody(Vector3 p, Vector3 v, Vector3 r, float s, float m)
        {
            pos = p;
            rot = Quaternion.Identity;
            //rot = Quaternion.CreateFromAxisAngle(new Vector3(1.0f, 0f, 0f), MathHelper.PiOver2);
            aVel = Quaternion.CreateFromAxisAngle(r, MathHelper.Pi*s / 180f);
            vel = v;
            force = Vector3.Zero;
            mass = m;
        }

        #endregion

        #region Solver + Collisions

        // Summary:
        //   ApplyForces: calculate aggregate force effect from all sources
        //   Takes initial state force, velocity, and position as arguments
        public void ApplyForces(Vector3 f0, Vector3 v0, Vector3 p0)
        {
            Vector3 result = Vector3.Zero;
            force = result;
            accel = force / mass + new Vector3(0f, Globals.g, 0f);
            // springs
            // other stuff?
        }

        // Summary:
        //   Explicit RK4 solver
        public void Solve()
        {
            Vector3 v1_dot, v2_dot, v3_dot, v4_dot, p1;

            ApplyForces(force, vel, pos);       // update current force
            v1_dot = vel + Globals.t * accel / 2.0f;// increment velocity with timestep
            p1 = pos + Globals.t * v1_dot / 2.0f;   // calculate position for k1

            ApplyForces(force, v1_dot, p1);     // K2 ...
            v2_dot = vel + Globals.t * accel / 2.0f;
            p1 = pos + Globals.t * v2_dot / 2.0f;

            ApplyForces(force, v2_dot, p1);     // K3 ...
            v3_dot = vel + Globals.t * accel;
            p1 = pos + Globals.t * v3_dot;

            ApplyForces(force, v3_dot, p1);     // K4 ...
            v4_dot = vel + Globals.t * accel;
            
            vel = v4_dot;
            pos = pos + Globals.t * (v1_dot + v2_dot*2f + v3_dot*2f + v4_dot) / 6.0f;

            rot *= aVel;
            rot = Quaternion.Normalize(rot);
            obb.center = pos;                   // IMPORTANT : update obb position
            obb.SetRot(rot);                    // IMPORTANT : update obb rotation

#if Q_DEBUG
            float qL = rot.Length();
            Vector3 XY = Vector3.Cross(obb.axes[0], obb.axes[1]);
            Vector3 Z = obb.axes[2];
            XY.Normalize();
            Z.Normalize();
            if (XY == Z)
            {

            }
            else
            {

            }
#endif
        }

        // Summary:
        //   apply environment bounds. VERY SLOPPY. will be improved later
        public void Bounds()
        {
            float? xB = null, yB = null, zB = null;

            if (Math.Abs(pos.X) > Globals.xBound)
            {
                vel.X = -vel.X;
                //xB = (pos.X > 0) ? 150 : -150;
            }
            if (Math.Abs(pos.Y) > Globals.yBound)
            {
                vel.Y = -vel.Y;
                //vel.Y = 0;
                yB = (pos.Y > 0) ? 100f : -92.5f;
            }
            if (Math.Abs(pos.Z) > Globals.zBound)
            {
                vel.Z = -vel.Z;
                //zB = (pos.Z > 0) ? 150 : -150;
            }

            pos = new Vector3(
                (xB == null) ? pos.X : (float)xB,
                (yB == null) ? pos.Y : (float)yB,
                (zB == null) ? pos.Z : (float)zB);
        }

        public void SetBB(Vector3 e)
        {
            obb = new BoundVolume(e);
            obb.SetRot(rot);
        }

        #endregion
    }
}
