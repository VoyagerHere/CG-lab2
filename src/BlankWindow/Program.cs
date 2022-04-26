using System;
using OpenTK;

namespace BlankWindow
{
    sealed class Program : GameWindow // Базовый класс opentk
    {
        [STAThread]
        static void Main()
        {
            var program = new Program();
            program.Run();
        }
    }
}
