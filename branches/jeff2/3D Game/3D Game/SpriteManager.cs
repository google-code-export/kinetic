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
    public class SpriteManager : DrawableGameComponent
    {
        Texture2D currentImage;
        Texture2D titleScreen;

        SpriteBatch spriteBatch;

        Boolean isDisplaying;

        public SpriteManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            isDisplaying = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            titleScreen = Game.Content.Load<Texture2D>("Kinetic");
            currentImage = titleScreen;

            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (isDisplaying)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(currentImage, new Vector2(0, 0), Color.White);
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }

        public void hideSpriteManager()
        {
            isDisplaying = false;
        }
    }
}
