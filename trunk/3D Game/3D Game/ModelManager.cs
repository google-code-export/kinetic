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
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class ModelManager : DrawableGameComponent
    {
        List<BasicModel> models = new List<BasicModel>();   // change this to array later?
        public bool paused { get; set; }

        public ModelManager(Game game)
            : base(game)
        {
            paused = false;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            //models.Add(new BasicModel(Game.Content.Load<Model>(@"Models\tower"), "dummy"));
            models.Add(new BasicModel(Game.Content.Load<Model>(@"Models\box"), "box", new Vector3(0, 10, 0)));
            models.Add(new BasicModel(Game.Content.Load<Model>(@"Models\plane"), "plane", new Vector3(0, -1, 0)));

            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // update each models
            for (int i = 0; i < models.Count; ++i)
            {
                if (!paused) models[i].Update();
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            // draw each model
            foreach (BasicModel bm in models)
            {
                bm.Draw(((Game3D)Game).camera);
            }

            base.Draw(gameTime);
        }

        public int GetModelCount()
        {
            return models.Count;
        }
    }
}
