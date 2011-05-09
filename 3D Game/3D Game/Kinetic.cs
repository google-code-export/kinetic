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
    /// Kinetic, aka Angry Balls. Destroy towers by throwing balls at them.
    /// This is the main game class (duh)
    /// </summary>
    public class Game3D : Microsoft.Xna.Framework.Game
    {
        #region Fields

        // Input Device States
        KeyboardState kNow;
        KeyboardState kPrev;
        MouseState mNow;
        MouseState mPrev;

        // Drawing
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        RasterizerState rs;
        BasicEffect effect1;
        VertexBuffer vBuffer;
        VertexPositionColor[] verts;

        // Fonts
        public SpriteFont fontSansSerif;
        public SpriteFont fontSerif;
        public SpriteFont fontSystem;

        // Cameras
        public Camera camera { get; protected set; }

        // Objects
        ObjectManager modelManager;

        // Game Flags
        bool debug = true;
        bool paused = false;
        bool wires = false;

        #endregion

        #region Initizialize

        public Game3D()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // input devices
            kNow = Keyboard.GetState();
            kPrev = kNow;
            mNow = Mouse.GetState();
            mPrev = mNow;

            // graphics
            rs = new RasterizerState();
            rs.FillMode = FillMode.Solid;
            GraphicsDevice.RasterizerState = rs;
            graphics.PreferredBackBufferWidth = Globals.Width;
            graphics.PreferredBackBufferHeight = Globals.Height;
            graphics.ApplyChanges();
            effect1 = new BasicEffect(GraphicsDevice);
            vBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), 6, BufferUsage.None);

            // camera
            camera = new Camera(this, new Vector3(380, 125, 0), new Vector3(0, -20, 0), Vector3.Up);
            Components.Add(camera);

            // models
            modelManager = new ObjectManager(this);
            Components.Add(modelManager);

            // other stuff
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // load sprites
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // load fonts
            fontSystem = Content.Load<SpriteFont>(@"Fonts\system");
        }

        protected override void UnloadContent()
        {
        }

        #endregion

        #region Update

        //// UPDATE
        protected override void Update(GameTime gameTime)
        {
            // Keyboard controls
            kNow = Keyboard.GetState();
            if (kNow.IsKeyDown(Keys.Escape)) this.Exit();
            debug = (kNow.IsKeyUp(Keys.OemTilde) && kPrev.IsKeyDown(Keys.OemTilde)) ? !debug : debug;
            if (kNow.IsKeyUp(Keys.P) && kPrev.IsKeyDown(Keys.P))
            {
                paused = !paused;
                modelManager.paused = paused;
            }
            if (kNow.IsKeyUp(Keys.D1) && kPrev.IsKeyDown(Keys.D1))
            {
                rs = new RasterizerState();
                rs.FillMode = FillMode.WireFrame;
                rs.CullMode = CullMode.None;
                wires = true;
            }
            if (kNow.IsKeyUp(Keys.D2) && kPrev.IsKeyDown(Keys.D2))
            {
                rs = new RasterizerState();
                rs.FillMode = FillMode.Solid;
                rs.CullMode = CullMode.CullCounterClockwiseFace;
                wires = false;
            }

            // Mouse controls
            mNow = Mouse.GetState();

            // update input devices
            kPrev = kNow;
            mPrev = mNow;

            base.Update(gameTime);
        }

        //// DRAW
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            GraphicsDevice.RasterizerState = rs;

            base.Draw(gameTime);

            // debug
            if (debug)
            {
                if (wires)
                {
                    effect1.World = Matrix.Identity;
                    effect1.View = camera.view;
                    effect1.Projection = camera.projection;
                    effect1.VertexColorEnabled = true;

                    for (int i = 0; i < modelManager.GetObjCount() - 1; i++)
                    {
                        verts = modelManager.GetAxes(i);
                        //verts = modelManager.GetBlockData(i).DrawAxes();
                        vBuffer.SetData<VertexPositionColor>(verts);
                        GraphicsDevice.SetVertexBuffer(vBuffer);
                        foreach (EffectPass pass in effect1.CurrentTechnique.Passes)
                        {
                            pass.Apply();
                            GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, verts, 0, 3);
                        }
                    }
                }

                DrawDebug();
            }
        }

        #endregion

        #region Debug
        /// <summary>
        /// DrawDebug: format some information that will be good to know for debugging, then
        /// draw it onto the screen when debug flag is set.
        /// </summary>
        protected void DrawDebug()
        {
            int mg = 10;
            int lh = 12;

            // debug lines
            string dbg_game1 = (paused ? "\u25a0 paused" : "\u25a1 running");
            string dbg_cam1 = "cameraPos["
                + String.Format("{0,10: ####.0000 ;-####.0000 }", camera.cameraPosition.X)
                + String.Format("{0,10: ####.0000 ;-####.0000 }", camera.cameraPosition.Y)
                + String.Format("{0,10: ####.0000 ;-####.0000 }", camera.cameraPosition.Z)
                + "]";
            string dbg_cam2 = "cameraDir["
                + String.Format("{0,10: ####.0000 ;-####.0000 }", camera.cameraDirection.X)
                + String.Format("{0,10: ####.0000 ;-####.0000 }", camera.cameraDirection.Y)
                + String.Format("{0,10: ####.0000 ;-####.0000 }", camera.cameraDirection.Z)
                + "]";
            string dbg_cam3 = "cameraUp ["
                + String.Format("{0,10: ####.0000 ;-####.0000 }", camera.cameraUp.X)
                + String.Format("{0,10: ####.0000 ;-####.0000 }", camera.cameraUp.Y)
                + String.Format("{0,10: ####.0000 ;-####.0000 }", camera.cameraUp.Z)
                + "]";
            string dbg_mous1 = "mousePos ["
                + String.Format("{0,10: ####.0000 ;-####.0000 }", mNow.X)
                + String.Format("{0,10: ####.0000 ;-####.0000 }", mNow.Y)
                + "]";
            string dbg_mous2 = ((mNow.LeftButton == ButtonState.Pressed) ? "\u25a0" : "\u25a1") + " mouseLeft";
            string dbg_mous3 = ((mNow.RightButton == ButtonState.Pressed) ? "\u25a0" : "\u25a1") + " mouseRight";
            string dbg_draw1 = "window size " + Globals.Width + "\u2219" + Globals.Height;
            string dbg_ctrl1 =     "~ : toggle debug info";
            string dbg_ctrl2 =  "w/s/a/d : directional move ";
            string dbg_ctrl3 =   "r/f : up/down          ";
            string dbg_ctrl4 = "space : reset camera     ";
            string dbg_ctrl5 = "  1/2 : wires/solid      ";
            string dbg_mods1 = "objects: " + modelManager.GetObjCount();
            //string dbg_mods1 = "models: " + modelManager.GetModelCount();
            string dbg_mods2 = "obb1Pos["
                + String.Format("{0,10: ####.0000 ;-####.0000 }", camera.cameraPosition.X)
                + String.Format("{0,10: ####.0000 ;-####.0000 }", camera.cameraPosition.Y)
                + String.Format("{0,10: ####.0000 ;-####.0000 }", camera.cameraPosition.Z)
                + "]";

            // draw text
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                spriteBatch.DrawString(fontSystem, dbg_game1, new Vector2(mg, mg + 0*lh), paused ? Color.Red : Color.Blue);
                spriteBatch.DrawString(fontSystem, dbg_cam1,  new Vector2(mg, mg + 1*lh), Color.Black);
                spriteBatch.DrawString(fontSystem, dbg_cam2,  new Vector2(mg, mg + 2*lh), Color.Black);
                spriteBatch.DrawString(fontSystem, dbg_cam3,  new Vector2(mg, mg + 3*lh), Color.Black);
                spriteBatch.DrawString(fontSystem, dbg_mous1, new Vector2(mg, mg + 5*lh), Color.Black);
                spriteBatch.DrawString(fontSystem, dbg_mous2, new Vector2(mg, mg + 6*lh), Color.Black);
                spriteBatch.DrawString(fontSystem, dbg_mous3, new Vector2(mg, mg + 7*lh), Color.Black);
                spriteBatch.DrawString(fontSystem, "\u2609",  new Vector2(mNow.X, mNow.Y), Color.Black); // mouse cursor
                spriteBatch.DrawString(fontSystem, dbg_draw1, new Vector2(Globals.Width - mg - fontSystem.MeasureString(dbg_draw1).X, mg + 0*lh), Color.Black);
                spriteBatch.DrawString(fontSystem, dbg_ctrl1, new Vector2(Globals.Width - mg - fontSystem.MeasureString(dbg_ctrl1).X, mg + 1*lh), Color.Black);
                spriteBatch.DrawString(fontSystem, dbg_ctrl2, new Vector2(Globals.Width - mg - fontSystem.MeasureString(dbg_ctrl2).X, mg + 2*lh), Color.Black);
                spriteBatch.DrawString(fontSystem, dbg_ctrl3, new Vector2(Globals.Width - mg - fontSystem.MeasureString(dbg_ctrl3).X, mg + 3*lh), Color.Black);
                spriteBatch.DrawString(fontSystem, dbg_ctrl4, new Vector2(Globals.Width - mg - fontSystem.MeasureString(dbg_ctrl4).X, mg + 4*lh), Color.Black);
                spriteBatch.DrawString(fontSystem, dbg_ctrl5, new Vector2(Globals.Width - mg - fontSystem.MeasureString(dbg_ctrl5).X, mg + 5*lh), Color.Black);
                spriteBatch.DrawString(fontSystem, dbg_mods1, new Vector2(mg, Globals.Height - (mg + 1*lh) - fontSystem.MeasureString(dbg_mods1).Y), Color.Black);
                //spriteBatch.DrawString(fontSystem, dbg_mods2, new Vector2(10, Globals.Height - (mg + 0*lh) - fontSystem.MeasureString(dbg_mods2).Y), Color.Black);
            spriteBatch.End();

            // Fix depth stuff before drawing 3D !!
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        }
        #endregion
    }
}
