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
    /// This is the collection of all objects in the game.
    /// </summary>
    public class ObjectManager : DrawableGameComponent
    {
        List<Object> objects = new List<Object>();

        //List<BasicModel> models = new List<BasicModel>();
        Random random = new Random();
        public bool paused { get; set; }

        public ObjectManager(Game game)
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
                objects.Add(new Object(Game.Content.Load<Model>(@"Models\box"), Vector3.Zero, Vector3.Zero, 1.0f, random.Next()));
                //models.Add(new TestBlock(Game.Content.Load<Model>(@"Models\box")));
            }
            objects.Add(new Object(Game.Content.Load<Model>(@"Models\plane"), "ground_plane", new Vector3(0, -100, 0)));
            //models.Add(new BasicModel(Game.Content.Load<Model>(@"Models\plane"), "plane", new Vector3(0, -100, 0)));

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (!paused)
            {
                for (int i = 0; i < objects.Count; ++i)
                {
                    // if (!paused) models[i].Update();
                    if (!paused) objects[i].Update();
                }

                base.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            // draw each model
            //foreach (BasicModel bm in models)
            //{
            //    bm.Draw(((Game3D)Game).camera);
            //}

            foreach (Object obj in objects)
            {
                obj.Draw(((Game3D)Game).camera);
            }

            base.Draw(gameTime);
        }

        #region debug

        public int GetObjCount()
        {
            return objects.Count;
        }
        public VertexPositionColor[] GetAxes(int i)
        {
            if (i < objects.Count) return objects[i].DrawAxes();
            else return null;
        }
        public Object GetObject(int i)
        {
            Object obj = objects[i];
            return obj;
        }


        //public int GetModelCount()
        //{
        //    return models.Count;
        //}
        //public int GetMeshCount()
        //{
        //    return models[0].model.Meshes.Count;
        //}
        
        //public TestBlock GetBlockData(int i)
        //{
        //    TestBlock block = (TestBlock)models[i];
        //    return block;
        //}

        #endregion
    }
}
