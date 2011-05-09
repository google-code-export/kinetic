using System;

namespace _3D_Game
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Game3D game = new Game3D())
            {
                game.Run();
            }
        }
    }
#endif
}

