using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3D_Game
{
    /// <summary>
    /// This class contains the physics data for an object
    /// </summary>
    class BasicRigidBody
    {
        float TSTEP = 0.22f; // i have no idea
        Vector3 GRAV = new Vector3(0f, -9.81f, 0f);

        #region Fields

        public Vector3 pos { get; protected set; }      // equivalent to center point
        Vector3 vel;
        Vector3 accel;
        Vector3 force;
        float mass;
        protected Matrix world = Matrix.Identity;

        #endregion

        #region Initialize

        public BasicRigidBody(Vector3 p, Vector3 v, float m)
        {
            pos = p;
            vel = v;
            //accel = Vector3.Zero;
            force = Vector3.Zero;
            mass = m;
        }

        #endregion

        #region Physics Solver

        // ApplyForces: calculate aggregate force effect from all sources
        public void ApplyForces(Vector3 f0, Vector3 v0, Vector3 p0)
        {
            Vector3 result = Vector3.Zero;
            force = result;
            accel = force / mass + GRAV;
            // springs
            // other stuff?
        }

        // Solve: Explicit RK4 solver
        public void Solve()
        {
            Vector3 v1_dot, v2_dot, v3_dot, v4_dot, p1;

            ApplyForces(force, vel, pos);       // update current force
            v1_dot = vel + TSTEP * accel / 2.0f;// increment velocity with timestep
            p1 = pos + TSTEP * v1_dot / 2.0f;   // calculate position for k1

            ApplyForces(force, v1_dot, p1);     // K2 ...
            v2_dot = vel + TSTEP * accel / 2.0f;
            p1 = pos + TSTEP * v2_dot / 2.0f;

            ApplyForces(force, v2_dot, p1);     // K3 ...
            v3_dot = vel + TSTEP * accel;
            p1 = pos + TSTEP * v3_dot;

            ApplyForces(force, v3_dot, p1);     // K4 ...
            v4_dot = vel + TSTEP * accel;
           
            vel = v4_dot;
            pos = pos + TSTEP * (v1_dot + v2_dot*2f + v3_dot*2f + v4_dot) / 6.0f;
        }

        // apply environment bounds
        public void Bounds()
        {
            float? xB = null, yB = null, zB = null;

            if (Math.Abs(pos.X) > 150)
            {
                vel.X = -vel.X;
                //xB = (pos.X > 0) ? 150 : -150;
            }
            if (Math.Abs(pos.Y) > 100)
            {
                vel.Y = -vel.Y;
                yB = (pos.Y > 0) ? 100 : -100;
            }
            if (Math.Abs(pos.Z) > 150)
            {
                vel.Z = -vel.Z;
                //zB = (pos.Z > 0) ? 150 : -150;
            }

            pos = new Vector3(
                (xB == null) ? pos.X : (float)xB,
                (yB == null) ? pos.Y : (float)yB,
                (zB == null) ? pos.Z : (float)zB);
        }

        #endregion
    }
}
