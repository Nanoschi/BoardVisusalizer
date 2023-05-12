using Raylib_cs;
using System.IO.Compression;
using System.Numerics;
using InteractableBoard;

namespace BoardVisualizer
{   
    class App
    {
        const int WIN_WIDTH = 800;
        const int WIN_HEIGHT = 800;

        Board ViewingBoard = new Board(0, 0, WIN_WIDTH);

        int SectionSize = 8;
        int SectionTop = 0;
        int SectionLeft = 0;

        public App()
        {

        }

        private void DrawApp()
        {
            ViewingBoard.DrawSection(SectionLeft, SectionTop, SectionSize, SectionSize);
        }

        public void Run()
        {
            Raylib.InitWindow(WIN_WIDTH, WIN_HEIGHT, "Board Visualizer");
            ViewingBoard.LoadTextures();


            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.BLACK);

                DrawApp();

                Raylib.EndDrawing();



            }
        }
    }
    class Program
    {

        public static void Main(string[] args)
        {
            App app = new App();
            app.Run();

        }
    }
}