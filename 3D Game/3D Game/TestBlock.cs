using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3D_Game
{
    public class TestBlock : BasicModel
    {
        //RigidBody pBody;
        Quaternion qRotate;
        //float speed = 4f;
        static int bNumber = 0;

        public TestBlock(Model m, Vector3 p)
            : base(m, "testBlock_" + bNumber, p)
        {
            //int seed = 1;
            //Random random = new Random(seed);
            //float rx = (float)random.NextDouble() * speed;
            //float ry = (float)random.NextDouble() * speed / 2f;
            //float rz = (float)random.NextDouble() * speed;
            qRotate = Quaternion.Identity;
            //pBody = new RigidBody(Vector3.Zero, new Vector3(rx, ry, rz), 1.0f);

            // visual effects
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect be in mesh.Effects)
                {
                    be.DiffuseColor = new Vector3(0.2f, 0.2f, 0.9f);
                }
            }

            bNumber++;
        }
    }
}
