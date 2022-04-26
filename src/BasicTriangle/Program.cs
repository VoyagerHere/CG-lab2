using System;
using OpenTK;
using OpenTK.Graphics.OpenGL4;



namespace Lab2
{
    sealed class Program : GameWindow
    {

        [STAThread]
        static void Main()
        {
            using (Window game = new Window(800, 600, "LearnOpenTK"))
            {
                //Run takes a double, which is how many frames per second it should strive to reach.
                //You can leave that out and it'll just update as fast as the hardware will allow it.
                game.Run(60.0);
            }
            // var program = new Program();
            // program.Run();
        }
    }
}
