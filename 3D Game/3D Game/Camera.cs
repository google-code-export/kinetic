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
    /// The Camera class implements functionality for navigating the 3d environment
    /// </summary>
    public class Camera : Microsoft.Xna.Framework.GameComponent
    {
        // Fundamental
        public Matrix view { get; protected set; }
        public Matrix projection { get; protected set; }

        // Transform
        public Vector3 cameraPosition { get; protected set; }
        public Vector3 cameraDirection;
        public Vector3 cameraUp;
        Vector3 initPos;
        Vector3 initDirection;
        Vector3 initUp;
        float speed = 1;

        // Input/Controls
        KeyboardState kNow;
        KeyboardState kPrev;
        MouseState mNow;
        MouseState mPrev;

        // redundant ..
        Vector2 moveInitial;
        bool paused = false;

        public Camera(Game game, Vector3 pos, Vector3 target, Vector3 up)
            : base(game)
        {
            // Build camera view matrix
            cameraPosition = pos;
            cameraDirection = target - pos;
            cameraDirection.Normalize();
            cameraUp = up;
            initPos = cameraPosition;
            initDirection = cameraDirection;    // keep these the same
            initUp = cameraUp;                  // ...
            CreateLookAt();

            projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                (float)Game.Window.ClientBounds.Width / (float)Game.Window.ClientBounds.Height,
                1, 3000);
        }

        //// INITIALIZE
        public override void Initialize()
        {
            Mouse.SetPosition(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
            
            kPrev = kNow;
            mNow = Mouse.GetState();
            mPrev = mNow;
            base.Initialize();
        }

        //// UPDATE
        public override void Update(GameTime gameTime)
        {
            // Inputs
            mNow = Mouse.GetState();
            kNow = Keyboard.GetState();

            // keyboard stuff
            paused = (kNow.IsKeyUp(Keys.P) && kPrev.IsKeyDown(Keys.P)) ? !paused : paused;
            if (kNow.IsKeyDown(Keys.R))
            {
                cameraPosition = initPos;
                cameraDirection = initDirection;
                cameraUp = initUp;
                Mouse.SetPosition(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
            }

            // Keyboard controls
            if (Keyboard.GetState().IsKeyDown(Keys.W))  // forward
                cameraPosition += cameraDirection * speed;
            if (Keyboard.GetState().IsKeyDown(Keys.S))  // backward
                cameraPosition -= cameraDirection * speed;
            if (Keyboard.GetState().IsKeyDown(Keys.A))  // left
                cameraPosition -= Vector3.Cross(cameraDirection, cameraUp) * speed;
            if (Keyboard.GetState().IsKeyDown(Keys.D))  // right
                cameraPosition += Vector3.Cross(cameraDirection, cameraUp) * speed;
            if (Keyboard.GetState().IsKeyDown(Keys.Q))  // roll left
                cameraUp = Vector3.Transform(cameraUp, Matrix.CreateFromAxisAngle(cameraDirection, MathHelper.PiOver4 / 45));
            if (Keyboard.GetState().IsKeyDown(Keys.E))  // roll right
                cameraUp = Vector3.Transform(cameraUp, Matrix.CreateFromAxisAngle(cameraDirection, -MathHelper.PiOver4 / 45));

            // Mouse controsl
            if (mPrev.LeftButton == ButtonState.Released && mNow.LeftButton == ButtonState.Pressed)
            {

            }
            if (!paused && mNow.LeftButton==ButtonState.Pressed)
            {
                //cameraDirection = Vector3.Transform(initDirection,      // left-right rotate
                //    Matrix.CreateFromAxisAngle(cameraUp, (MathHelper.PiOver4 / 350) * (Game.Window.ClientBounds.Width / 2 - mNow.X)));
                //cameraDirection = Vector3.Transform(cameraDirection,    // up-down
                //    Matrix.CreateFromAxisAngle(Vector3.Cross(cameraUp, cameraDirection), (MathHelper.Pi / 350) * (mNow.Y - Game.Window.ClientBounds.Height / 2)));
                //cameraUp = Vector3.Transform(initUp,                    // up-down
                //    Matrix.CreateFromAxisAngle(Vector3.Cross(cameraUp, cameraDirection), (MathHelper.Pi / 350) * (mNow.Y - Game.Window.ClientBounds.Height / 2)));

                // TODO: NEED TO FREEZE VECTORS ON INITIAL CLICK
                cameraDirection = Vector3.Transform(cameraDirection,    // left-right rotate
                    Matrix.CreateFromAxisAngle(cameraUp, (MathHelper.PiOver4 / 150) * (mPrev.X - mNow.X)));
                cameraDirection = Vector3.Transform(cameraDirection,    // up-down rotate
                    Matrix.CreateFromAxisAngle(Vector3.Cross(cameraUp, cameraDirection), (MathHelper.Pi / 150) * (mNow.Y - mPrev.Y)));
                cameraUp = Vector3.Transform(cameraUp,                  // up-down rotate
                    Matrix.CreateFromAxisAngle(Vector3.Cross(cameraUp, cameraDirection), (MathHelper.Pi / 150) * (mNow.Y - mPrev.Y)));
                cameraDirection.Normalize();
                cameraUp.Normalize();
            }

            // Recreate view matrix
            CreateLookAt();

            // Save inputs
            kPrev = kNow;
            mPrev = mNow;

            base.Update(gameTime);
        }

        private void CreateLookAt()
        {
            view = Matrix.CreateLookAt(cameraPosition, cameraPosition + cameraDirection, cameraUp);
        }
    }
}
