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
    /// This is the collection of all physical objects in the game.
    /// </summary>
    public class ModelManager : DrawableGameComponent
    {
        List<BasicModel> models = new List<BasicModel>();
        Random random = new Random();
        public bool paused { get; set; }

        public ModelManager(Game game)
            : base(game)
        {
            paused = false;
        }
        public override void Initialize()
        {
            base.Initialize();
        }
        protected override void LoadContent()
        {
            Random random = new Random();
            for (int i = 0; i < Globals.blockCount; i++)
            {
                models.Add(new TestBlock(Game, Game.Content.Load<Model>(@"Models\box"), random.Next()));
            }
            models.Add(new BasicModel(Game, Game.Content.Load<Model>(@"Models\plane"), "plane", new Vector3(0, -100, 0)));

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

        #region debug

        public int GetModelCount()
        {
            return models.Count;
        }
        public int GetMeshCount()
        {
            return models[0].model.Meshes.Count;
        }
        public TestBlock GetBlockData(int i)
        {
            TestBlock block = (TestBlock)models[i];
            return block;
        }

        #endregion
    }
}
