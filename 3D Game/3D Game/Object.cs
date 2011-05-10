using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3D_Game
{
    public class Object
    {
        public bool collidable { get; set; }
        BasicModel basicModel;
        RigidBody rBody;

        public Object(Model m, string name, Vector3 p)
        {
            basicModel = new BasicModel(m, name, p);
            rBody = null;
            collidable = false;
            //rBody = new RigidBody(p, v, mass);
            //CreateCollisionVolume();
        }

        public Object(Model m, Vector3 p, Vector3 v, float mass, int seed)
        {
            Random random = new Random(seed);   // random velocity seed. remove after level arrangements are done
            float rx = (float)random.NextDouble() * Globals.bSpeed;
            //float ry = (float)random.NextDouble() * Globals.bSpeed / 2f;
            float ry = 0.0f;
            float rz = (float)random.NextDouble() * Globals.bSpeed;

            basicModel = new TestBlock(m, p);
            rBody = new RigidBody(p, new Vector3(rx, ry, rz), mass);
            CreateCollisionVolume();
            collidable = true;
        }

        public void Update()
        {
            if (rBody != null)
            {
                rBody.Solve();
                rBody.Bounds();
                SetModelParams();
            }
        }

        public void Draw(Camera camera)
        {
            basicModel.Draw(camera);
        }

        public VertexPositionColor[] DrawAxes()
        {
            VertexPositionColor[] vertices = new VertexPositionColor[6];
            vertices[0] = new VertexPositionColor(rBody.obb.center, Color.Red);
            vertices[1] = new VertexPositionColor(rBody.obb.center + rBody.obb.axes[0] * rBody.obb.ex.X, Color.Red);
            vertices[2] = new VertexPositionColor(rBody.obb.center, Color.Green);
            vertices[3] = new VertexPositionColor(rBody.obb.center + rBody.obb.axes[1] * rBody.obb.ex.Y, Color.Green);
            vertices[4] = new VertexPositionColor(rBody.obb.center, Color.Blue);
            vertices[5] = new VertexPositionColor(rBody.obb.center + rBody.obb.axes[2] * rBody.obb.ex.Z, Color.Blue);
            return vertices;
        }

        public void CreateCollisionVolume()
        {
            BoundingBox bb = new BoundingBox();
            ModelMesh mesh = basicModel.model.Meshes[0];
            ModelMeshPart part = mesh.MeshParts[0];
            VertexPositionNormalTexture[] vData =
                new VertexPositionNormalTexture[part.NumVertices];
            part.VertexBuffer.GetData<VertexPositionNormalTexture>(vData);

            Vector3[] vertices = new Vector3[vData.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = vData[i].Position;
            }

            bb = BoundingBox.CreateMerged(bb, BoundingBox.CreateFromPoints(vertices));
            Vector3 e = new Vector3(bb.Max.X, bb.Max.Y, bb.Max.Z);
            rBody.SetBB(e);
        }

        public CollisionVolume GetOBB()
        {
            CollisionVolume returnVolume = rBody.obb;
            return rBody.obb;
        }

        public string GetName()
        {
            string returnString = basicModel.name;
            return returnString;
        }

        private void SetModelParams()
        {
            basicModel.ResetWorld();
            basicModel.SetRot(rBody.rot);   // rotate first!
            basicModel.SetPos(rBody.pos);   // then translate
        }
    }
}
