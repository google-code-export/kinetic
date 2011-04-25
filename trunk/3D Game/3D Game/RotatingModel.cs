using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3D_Game
{
    class RotatingModel : BasicModel
    {
        Matrix rotation = Matrix.Identity;

        public RotatingModel(Model m)
            : base(m) { }

        public override void Update()
        {
            rotation *= Matrix.CreateRotationY(MathHelper.Pi / 180);
        }

        public override Matrix GetWorld()
        {
            return world * rotation;
        }

    }
}
