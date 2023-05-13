using Raylib_cs;
using System.IO.Compression;
using System.Numerics;
using InteractableBoard;

namespace BoardVisualizer
{   
    enum AppState
    {
        FullView,
        QuadrantView,

    }
    class App
    {
        const int BOARD_SIZE = 700;
        const int SELECTION_HEIGHT = 100;

        const int WIN_WIDTH = BOARD_SIZE;
        const int WIN_HEIGHT = BOARD_SIZE + 2 * SELECTION_HEIGHT;

        Board ViewingBoard;

        int SectionTop = 0;
        int SectionLeft = 0;
        int SectionSize = 8;

        int QuadrantX = 0;
        int QuadrantY = 0;

        AppState State = AppState.FullView;

        public App()
        {
            ViewingBoard = new Board(0, SELECTION_HEIGHT, BOARD_SIZE);
        }

        private void DrawApp()
        {
            switch (State)
            {
                case AppState.FullView:
                    ViewingBoard.DrawSection(0, 0, 8, 8);
                    break;
                case AppState.QuadrantView:
                    ViewingBoard.DrawSection(QuadrantX, QuadrantY, 4, 4);
                    break;
            }
        }

        private void GetInput()
        {
            if (State == AppState.FullView)
            {
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER))
                {
                    Random rand = new Random();
                    QuadrantX = rand.Next(0, 2);
                    QuadrantY = rand.Next(0, 2);

                    State = AppState.QuadrantView;
                    Console.WriteLine("sdfsdfsdf");
                }
            }
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

                GetInput();
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