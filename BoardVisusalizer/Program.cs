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
        TimeToRemember,
        Guess,
        PieceInput

    }
    class App
    {
        Renderer renderer = new Renderer();
        float TimeInS = 0;

        const int BoardSize = 600;
        const int SelectionHeight = 50;

        const int WIN_WIDTH = BoardSize;
        const int WIN_HEIGHT = BoardSize + 2 * SelectionHeight;
        

        int QuadrantX = 0;
        int QuadrantY = 0;

        AppState State = AppState.FullView;

        Position RealPosition = new Position();
        Position GuessedPosition = new Position();
        Pieces SelectedPiece;

        float GuessTime = 5;

        public App()
        {
            Raylib.InitWindow(WIN_WIDTH, WIN_HEIGHT, "Board Visualizer");

            Renderer.LoadPieceTextures();

            RealPosition = Position.GetStartPosition();
            renderer.SetBoard(0, SelectionHeight, BoardSize);
            renderer.Board.SetUpPosition(RealPosition);
        }

        private void ApplyAppState()
        {
            switch (State)
            {
                case AppState.FullView:
                    renderer.Board.SetSection(0, 0, 8);
                    renderer.Board.SetUpPosition(RealPosition);
                    TimeInS = GuessTime;
                    break;
                case AppState.TimeToRemember:
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

                    TimeInS = GuessTime;
                    State = AppState.TimeToRemember;

                    ApplyAppState();
                }
            }

            if (State == AppState.TimeToRemember)
            {
                TimeInS -= Raylib.GetFrameTime();


                if (TimeInS <= 0f)
                {
                    State = AppState.Guess;
                    renderer.Board.SetUpPosition(GuessedPosition);
                }
            }

            if (State  != AppState.FullView) 
            {
                if (Raylib.IsKeyDown(KeyboardKey.KEY_R))
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

                if (State == AppState.TimeToRemember)
                {
                    renderer.DrawSeconds(TimeInS);
                }


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