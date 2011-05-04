#define SOLVER

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3D_Game
{
    class testBlock : BasicModel
    {
        Vector3 position;
        Vector3 velocity;
        Quaternion qRotate;
        BasicRigidBody pBody;
        float speed = 4f;

        int collide;

        public testBlock(Model m, int seed)
            : base(m, "testBlock", Vector3.Zero)
        {
            Random random = new Random(seed);
            float rx = (float)random.NextDouble() * speed;
            float ry = (float)random.NextDouble() * speed / 2f;
            float rz = (float)random.NextDouble() * speed;
            position = Vector3.Zero;
            velocity = new Vector3(rx, ry, rz);
            qRotate = Quaternion.Identity;
            collide = 0;

            pBody = new BasicRigidBody(position, velocity, 1.0f);

            // visual effects
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect be in mesh.Effects)
                {
                    be.DiffuseColor = new Vector3(0.2f, 0.2f, 0.9f);
                }
            }
        }

        public override void Update()
        {
#if SOLVER    
            pBody.Solve();
            pBody.Bounds();
            SetPos(pBody.pos);
            collide = 0;
#else
            position += velocity;
            SetPos(position);
            collide = Math.Max(collide - 1, 0);

            // temporary bounds hack. Will implement more accurate one with rest of physics ..
            if (Math.Abs(position.X) > 150)
            {
                velocity.X = -velocity.X;
                collide = 5;
            }
            if (Math.Abs(position.Y) > 100)
            {
                velocity.Y = -velocity.Y;
                collide = 5;
            }
            if (Math.Abs(position.Z) > 150)
            {
                velocity.Z = -velocity.Z;
                collide = 5;
            }

            Vector3 color = (collide != 0) ? new Vector3(1.0f, 0.5f, 0.5f) : new Vector3(0.2f, 0.2f, 0.9f);
            ModelMesh mesh = model.Meshes[0];
            ModelMeshPart part = mesh.MeshParts[0];
            BasicEffect effect = (BasicEffect)part.Effect;  // serious hax ...
            effect.DiffuseColor = color;
#endif

            base.Update();
        }
    }
}
