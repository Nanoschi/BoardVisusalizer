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

        Board ViewingBoard = new Board(20, 80, WIN_WIDTH - 100);

        int SectionTop = 1;
        int SectionLeft = 1;
        int SectionSize = 4;

        public App()
        {

        }

        private void DrawApp()
        {
            ViewingBoard.DrawSection(SectionLeft, SectionTop, SectionSize, SectionSize);
            Raylib.DrawText(ViewingBoard.GetHoveredSquare(SectionLeft, SectionTop, SectionSize, SectionSize).ToString(), 0, 0, 40, Color.RED);
        }

        public void Run()
        {
            Raylib.InitWindow(WIN_WIDTH, WIN_HEIGHT, "Board Visualizer");
            ViewingBoard.LoadTextures();
            ViewingBoard.SetUpFromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR");

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