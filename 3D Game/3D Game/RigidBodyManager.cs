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
    /// This is the collection of all RigidBody objects in the game. Objects
    /// that need physics must be associated with a RigidBody and added to the
    /// list here!
    /// 
    /// Currently this class isn't actually used for anything though ....
    /// </summary>
    public class RigidBodyManager : GameComponent
    {
        public List<RigidBody> rBodies;
        public bool paused { get; set; }

        public RigidBodyManager(Game game)
            : base(game)
        {
            rBodies = new List<RigidBody>();
            paused = false;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public void addBody(ref RigidBody body)
        {
            rBodies.Add(body);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (RigidBody rBody in rBodies)
            {
                rBody.Solve();
                rBody.Bounds();
            }
            base.Update(gameTime);
        }
    }
}
