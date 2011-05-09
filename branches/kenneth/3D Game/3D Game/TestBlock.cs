#define SOLVER

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
        RigidBody pBody;
        Quaternion qRotate;
        float speed = 4f;

        public TestBlock(Game g, Model m, int seed)
            : base(g, m, "testBlock", Vector3.Zero)
        {
            Random random = new Random(seed);
            float rx = (float)random.NextDouble() * speed;
            float ry = (float)random.NextDouble() * speed / 2f;
            float rz = (float)random.NextDouble() * speed;
            qRotate = Quaternion.Identity;
            pBody = new RigidBody(Vector3.Zero, new Vector3(rx, ry, rz), 1.0f);

            // visual effects
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect be in mesh.Effects)
                {
                    be.DiffuseColor = new Vector3(0.2f, 0.2f, 0.9f);
                }
            }

            CreateBB();
        }

        public override void Update()
        {
#if SOLVER    
            pBody.Solve();
            pBody.Bounds();
            SetPos(pBody.pos);
#else
            position += velocity;
            SetPos(position);

            // temporary bounds hack. Will implement more accurate one with rest of physics ..
            if (Math.Abs(position.X) > 150)
            {
                velocity.X = -velocity.X;
            }
            if (Math.Abs(position.Y) > 100)
            {
                velocity.Y = -velocity.Y;
            }
            if (Math.Abs(position.Z) > 150)
            {
                velocity.Z = -velocity.Z;
            }
            ModelMesh mesh = model.Meshes[0];
            ModelMeshPart part = mesh.MeshParts[0];
            BasicEffect effect = (BasicEffect)part.Effect;  // serious hax ...
            effect.DiffuseColor = color;
#endif

            base.Update();
        }
        public VertexPositionColor[] DrawAxes()
        {
            VertexPositionColor[] vertices = new VertexPositionColor[6];
            vertices[0] = new VertexPositionColor(pBody.obb.center, Color.Red);
            vertices[1] = new VertexPositionColor(pBody.obb.center + Vector3.UnitX * pBody.obb.ex.X, Color.Red);
            vertices[2] = new VertexPositionColor(pBody.obb.center, Color.Green);
            vertices[3] = new VertexPositionColor(pBody.obb.center + Vector3.UnitY * pBody.obb.ex.Y, Color.Green);
            vertices[4] = new VertexPositionColor(pBody.obb.center, Color.Blue);
            vertices[5] = new VertexPositionColor(pBody.obb.center + Vector3.UnitZ * pBody.obb.ex.Z, Color.Blue);
            return vertices;
        }
        public void CreateBB()
        {
            BoundingBox bb = new BoundingBox();

            ModelMesh mesh = model.Meshes[0];
            ModelMeshPart part = mesh.MeshParts[0];
            VertexPositionNormalTexture[] vdata =
                new VertexPositionNormalTexture[part.NumVertices];
            part.VertexBuffer.GetData<VertexPositionNormalTexture>(vdata);

            Vector3[] vertices = new Vector3[vdata.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = vdata[i].Position;
            }

            bb = BoundingBox.CreateMerged(bb, BoundingBox.CreateFromPoints(vertices));

            Vector3 e = new Vector3(bb.Max.X, bb.Max.Y, bb.Max.Z);

            pBody.setBB(e);
        }
    }
}
