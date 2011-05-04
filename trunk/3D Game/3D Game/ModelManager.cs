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
    /// This is the collection of all drawable objects in the game.
    /// </summary>
    public class ModelManager : DrawableGameComponent
    {
        List<BasicModel> models = new List<BasicModel>();
        Random random = new Random();
        public bool paused { get; set; }
        int testBlockLimit = 50; // total number of blocks

        public ModelManager(Game game)
            : base(game)
        {
            paused = false;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// All models are to be loaded here. Don't load physics/collision detection meshes in here,
        /// as there will be a separate manager for that.
        /// </summary>
        protected override void LoadContent()
        {
            Random random = new Random();
            for (int i = 0; i < testBlockLimit; i++)
            {
                models.Add(new testBlock(Game.Content.Load<Model>(@"Models\box"), random.Next()));
            }
            models.Add(new BasicModel(Game.Content.Load<Model>(@"Models\plane"), "plane", new Vector3(0, -100, 0)));

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (!paused)
            {
                // update each models
                for (int i = 0; i < models.Count; ++i)
                {
                    if (!paused) models[i].Update();
                }

                base.Update(gameTime);
            }
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

        public int GetMeshCount()
        {
            return models[0].model.Meshes.Count;
        }
    }
}
