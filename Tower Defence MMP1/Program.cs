//MultiMediaTechnology 
//FHS 45891
//MultiMediaProjekt 1
//Benjamin Kunz

using System;

namespace Tower_Defence
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Game1())
                game.Run();
        }
    }
}
