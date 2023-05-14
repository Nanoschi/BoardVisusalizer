using Raylib_cs;
using System.IO.Compression;
using System.Numerics;
using AppRenderer;
using ChessDatatypes;

namespace BoardVisualizer
{   
    enum AppState
    {
        FullView,
        QuadrantView,

    }
    class App
    {
        Renderer renderer = new Renderer();
        const int BoardSize = 600;
        const int SelectionHeight = 50;

        const int WIN_WIDTH = BoardSize;
        const int WIN_HEIGHT = BoardSize + 2 * SelectionHeight;

        

        int SectionTop = 0;
        int SectionLeft = 0;
        int SectionSize = 8;

        int QuadrantX = 0;
        int QuadrantY = 0;

        AppState State = AppState.FullView;

        public App()
        {
            Raylib.InitWindow(WIN_WIDTH, WIN_HEIGHT, "Board Visualizer");

            Renderer.LoadPieceTextures();

            renderer.SetBoard(0, SelectionHeight, BoardSize);
            renderer.Board.SetUpPosition(Position.GetStartPosition());
        }

        private void ApplyAppState()
        {
            switch (State)
            {
                case AppState.FullView:
                    renderer.Board.SetSection(0, 0, 8);
                    break;
                case AppState.QuadrantView:
                    renderer.Board.SetSection(QuadrantX * 4, QuadrantY * 4, 4);
                    break;
            }
        }

        private void GetUserInput()
        {
            if (State == AppState.FullView)
            {
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER))
                {
                    Random rand = new Random();
                    QuadrantX = rand.Next(0, 2);
                    QuadrantY = rand.Next(0, 2);

                    State = AppState.QuadrantView;

                    ApplyAppState();
                }
            }

            else if (State == AppState.QuadrantView)
            {
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_F))
                {
                    State = AppState.FullView;
                    ApplyAppState();

                }
            }
        }

        public void Run()
        {
            
            

            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.BLACK);

                GetUserInput();
                renderer.DrawApp();

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