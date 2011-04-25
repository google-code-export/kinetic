using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3D_Game
{
    class BasicModel
    {
        public Model model { get; protected set; }      // to represent 3D model
        protected Matrix world = Matrix.Identity;       // transform matrix

        //// CONSTRUCTOR
        public BasicModel(Model m)
        {
            model = m;
        }

        //// UPDATE
        public virtual void Update()
        {
        }

        //// DRAW
        public void Draw(Camera camera)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect be in mesh.Effects)
                {
                    be.EnableDefaultLighting();
                    be.Projection = camera.projection;
                    be.View = camera.view;
                    be.World = GetWorld() * mesh.ParentBone.Transform;
                }

                mesh.Draw();
            }
        }

        //// GETWORLD
        public virtual Matrix GetWorld()
        {
            return world;
        }


    }
}
