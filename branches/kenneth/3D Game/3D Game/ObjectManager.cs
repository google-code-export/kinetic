using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace _3D_Game
{
    /// <summary>
    /// This is the collection of all objects in the game.
    /// </summary>
    public class ObjectManager : DrawableGameComponent
    {
        public bool paused { get; set; }
        public bool visible { get; set; }
        public bool hit { get; set; }
        List<Object> objects = new List<Object>();
        Random random = new Random();
        

        public ObjectManager(Game game)
            : base(game)
        {
            paused = false;
            hit = false;
            visible = true;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Random random = new Random();

            for (int i = 0; i < Globals.blockCount; i++)
            {
                //objects.Add(
                //    new Object(Game.Content.Load<Model>(@"Models\box"),
                //        new Vector3(0f, -Globals.yBound, 0f),
                //        Vector3.Zero,
                //        1.0f,
                //        random.Next()));
                objects.Add(
                    new Object(Game.Content.Load<Model>(@"Models\box"),     // model
                        new Vector3(0f, (float)random.NextDouble() * 5f, 0f),            // position
                        1.0f,                                               // mass
                        random.Next()));                                    // seed for velocity
            }
            objects.Add(new Object(Game.Content.Load<Model>(@"Models\plane"), "ground_plane", new Vector3(0, -100, 0)));

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (!paused)
            {
                for (int i = 0; i < objects.Count; ++i)
                {
                    if (!paused) objects[i].Update();
                }

                hit = false;
                CollisionDetect();

                base.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (visible)
            {
                foreach (Object obj in objects)
                {
                    obj.Draw(((Game3D)Game).camera);
                }
            }

            base.Draw(gameTime);
        }

        private void CollisionDetect()
        {
            // here it is .. the n^2 CPU killer
            foreach (Object A in objects)
            {
                if (A.collidable)
                {
                    foreach (Object B in objects)
                    {
                        if (B.collidable && (A.GetName() != B.GetName()))
                        {
                            Collision col;
                            if (CollisionSolver.ObbSAT(A.GetOBB(), B.GetOBB(), out col))
                            {
                                hit = true;
                                CollisionResolve(A, B);
                            }

                            //if (CollisionTest(A, B))
                            //{
                            //    hit = true;
                            //    CollisionResolve(A, B);
                            //}
                        }
                    }
                }
            }
        }

        // Summary:
        //   OBB Collision based on Separating Axis theorem
        //   Code ported from Christer Ericson's C++ version
        //   There is probably a more concise way to code this
        private bool CollisionTest(Object A, Object B)
        {
            //BoundVolume a = A.GetOBB();
            //BoundVolume b = B.GetOBB();
            //float rA, rB;                       // projection of radii onto test axis
            //float[,] R = new float[3,3];
            //float[,] AbsR = new float[3,3];
            //float[] aEx = new float[3];
            //float[] bEx = new float[3];

            //// Because C# won't let me access Vector3 elements with []s ...
            //aEx[0] = a.ex.X;
            //aEx[1] = a.ex.Y;
            //aEx[2] = a.ex.Z;
            //bEx[0] = b.ex.X;
            //bEx[1] = b.ex.Y;
            //bEx[2] = b.ex.Z;

            //// compute rotation matrix expressing b in a's coordinate frame
            //for (int i = 0; i < 3; i++)
            //    for (int j = 0; j < 3; j++)
            //        R[i, j] = Vector3.Dot(a.axes[i], b.axes[j]);

            //// Compute translation vector t, and bring into a's coordinate frame
            ////Vector3 t = b.center - a.center;
            //Vector3 tt = b.center - a.center;
            //float[] t = new float[3];
            //t[0] = Vector3.Dot(tt, a.axes[0]);
            //t[1] = Vector3.Dot(tt, a.axes[1]);
            //t[2] = Vector3.Dot(tt, a.axes[2]);

            //// Compute common subexps; Add epsilon to prevent cross products from exploding
            //for (int i = 0; i < 3; i++)
            //    for (int j = 0; j < 3; j++)
            //        AbsR[i,j] = Math.Abs(R[i,j]) + Globals.mEpsilon;

            //// Test axes L = A0, L = A1, L = A2
            //for (int i = 0; i < 3; i++)
            //{
            //    rA = aEx[i];
            //    rB = bEx[0] * AbsR[i, 0] + bEx[1] * AbsR[i, 1] + bEx[2] * AbsR[i, 2];
            //    if (Math.Abs(t[i]) > rA + rB) return false;
            //}

            //// Test axes L = B0, L = B1, L = B2
            //for (int i = 0; i < 3; i++)
            //{
            //    rA = aEx[0] * AbsR[0, i] + aEx[1] * AbsR[1, i] + aEx[2] * AbsR[2, i];
            //    rB = bEx[i];
            //    if (Math.Abs(t[0] * R[0,i] + t[1] * R[1,i] + t[2] * R[2,i]) > rA + rB) return false;
            //}

            //// Test axis L = A0 x B0    (7)
            //rA = aEx[1] * AbsR[2, 0] + aEx[2] * AbsR[1, 0];
            //rB = bEx[1] * AbsR[0, 2] + bEx[2] * AbsR[0, 1];
            //if (Math.Abs(t[2] * R[1, 0] - t[1] * R[2, 0]) > rA + rB) return false;

            //// Test axis L = A0 x B1    (8)
            //rA = aEx[1] * AbsR[2, 1] + aEx[2] * AbsR[1, 1];
            //rB = bEx[0] * AbsR[0, 2] + bEx[2] * AbsR[0, 0];
            //if (Math.Abs(t[2] * R[1, 1] - t[1] * R[2, 1]) > rA + rB) return false;

            //// Test axis L = A0 x B2    (9)
            //rA = aEx[1] * AbsR[2, 2] + aEx[2] * AbsR[1, 2];
            //rB = bEx[0] * AbsR[0, 1] + bEx[1] * AbsR[0, 0];
            //if (Math.Abs(t[2] * R[1, 2] - t[1] * R[2, 2]) > rA + rB) return false;

            //// Test axis L = A1 x B0    (10)
            //rA = aEx[0] * AbsR[2, 0] + aEx[2] * AbsR[0, 0];
            //rB = bEx[1] * AbsR[1, 2] + bEx[2] * AbsR[1, 1];
            //if (Math.Abs(t[0] * R[2, 0] - t[2] * R[0, 0]) > rA + rB) return false;

            //// Test axis L = A1 x B1    (11)
            //rA = aEx[0] * AbsR[2, 1] + aEx[2] * AbsR[0, 1];
            //rB = bEx[0] * AbsR[1, 2] + bEx[2] * AbsR[1, 0];
            //if (Math.Abs(t[0] * R[2, 1] - t[2] * R[0, 1]) > rA + rB) return false;

            //// Test axis L = A1 x B2    (12)
            //rA = aEx[0] * AbsR[2, 2] + aEx[2] * AbsR[0, 2];
            //rB = bEx[0] * AbsR[1, 2] + bEx[2] * AbsR[1, 0];
            //if (Math.Abs(t[0] * R[2, 2] - t[2] * R[0, 2]) > rA + rB) return false;

            //// Test axis L = A2 x B0    (13)
            //rA = aEx[0] * AbsR[1, 0] + aEx[1] * AbsR[0, 0];
            //rB = bEx[1] * AbsR[2, 2] + bEx[2] * AbsR[2, 1];
            //if (Math.Abs(t[1] * R[0, 0] - t[0] * R[1, 0]) > rA + rB) return false;

            //// Test axis L = A2 x B1    (14)
            //rA = aEx[0] * AbsR[1, 1] + aEx[1] * AbsR[0, 1];
            //rB = bEx[0] * AbsR[2, 2] + bEx[2] * AbsR[2, 0];
            //if (Math.Abs(t[1] * R[0, 1] - t[0] * R[1, 1]) > rA + rB) return false;

            //// Test axis L = A2 x B2    (15)
            //rA = aEx[0] * AbsR[1, 2] + aEx[1] * AbsR[0, 2];
            //rB = bEx[0] * AbsR[2, 1] + bEx[1] * AbsR[2, 0];
            //if (Math.Abs(t[1] * R[0, 2] - t[0] * R[1, 2]) > rA + rB) return false;

            //return true;
            Collision col;
            return CollisionSolver.ObbSAT(A.GetOBB(), B.GetOBB(), out col);
        }

        public void toggleVis()
        {
            visible = !visible;
        }

        #region debug

        public int GetObjCount()
        {
            return objects.Count;
        }
        public VertexPositionColor[] GetAxes(int i)
        {
            if (i < objects.Count) return objects[i].DrawAxes();
            else return null;
        }
        public Object GetObject(int i)
        {
            Object obj = objects[i];
            return obj;
        }

        #endregion
    }
}
