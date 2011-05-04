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
    /// </summary>
    public class RigidBodyManager : Microsoft.Xna.Framework.GameComponent
    {
        List<BasicRigidBody> rBodies = new List<BasicRigidBody>();
        public bool paused { get; set; }

        public RigidBodyManager(Game game)
            : base(game)
        {
            paused = false;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
