﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3D_Game
{
    /// <summary>
    /// This class contains a drawable object model and handles the
    /// draw effects associated with it
    /// </summary>
    class BasicModel
    {
        public string name { get; protected set; }      // name of model
        public Model model { get; protected set; }      // to represent 3D model
        protected Matrix world = Matrix.Identity;       // transform matrix

        //// CONSTRUCTOR
        public BasicModel(Model m, string n, Vector3 position)
        {
            model = m;
            name = n;
            Translate(position);
        }
        public virtual void Update()
        {
        }
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

        public void Translate(Vector3 trans)
        {
            world *= Matrix.CreateTranslation(trans);
        }
        public void SetPos(Vector3 newPos)
        {
            world = Matrix.CreateTranslation(newPos);
        }
        public Matrix GetWorld()
        {
            return world;
        }
    }
}
