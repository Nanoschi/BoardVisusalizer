using Raylib_cs;
using System.IO.Compression;
using System.Numerics;

namespace BoardVisualizer
{
    public enum Pieces
    {
        EMPTY,
        W_KING,
        W_QUEEN,
        W_ROOK,
        W_BISHOP,
        W_KNIGHT,
        W_PAWN,
        B_KING,
        B_QUEEN,
        B_KNIGHT,
        B_ROOK,
        B_BISHOP,
        B_PAWN,

    }
    class Board
    {
        const int WIN_WIDTH = 1600;
        const int WIN_HEIGHT = 1600;

        Color WHITE = Color.BEIGE;
        Color BLACK = Color.DARKBROWN;

        Pieces[,] BoardPosition = new Pieces[8, 8];

        Dictionary<Pieces, Texture2D> PIECE_TEXTURES;

        Dictionary<char, Pieces> FEN_PIECES = new Dictionary<char, Pieces>()
        {
            ['K'] = Pieces.W_KING,
            ['Q'] = Pieces.W_QUEEN,
            ['R'] = Pieces.W_ROOK,
            ['B'] = Pieces.W_BISHOP,
            ['N'] = Pieces.W_KNIGHT,
            ['P'] = Pieces.W_PAWN,
            ['k'] = Pieces.B_KING,
            ['q'] = Pieces.B_QUEEN,
            ['r'] = Pieces.B_ROOK,
            ['b'] = Pieces.B_BISHOP,
            ['n'] = Pieces.B_KNIGHT,
            ['p'] = Pieces.B_PAWN,
        };

        public Board(string startPosition)
        {
            PositionFromFEN(startPosition);
        }

        public void PositionFromFEN(string fen)
        {
            int rank = 0;
            int file = 0;

            foreach (var item in fen)
            {
                if (FEN_PIECES.TryGetValue(item, out var piece))
                {
                    BoardPosition[rank, file] = piece;
                    file++;
                }
                //else if (char.IsDigit(item))
                //{
                //    int empty = (int)item;
                //    file += empty;
                //}
                else if (int.TryParse($"{item}", out var value))
                {
                    file += value;
                }
                else if (item == '/')
                {
                    file = 0;
                    rank++;
                }
                else if (item == ' ')
                {
                    continue;
                }
                else
                {
                    throw new Exception($"Unexpected character {item} in FEN");
                }

            }
        }

        public void Init()
        {
            PIECE_TEXTURES = new Dictionary<Pieces, Texture2D>()
            {
                [Pieces.W_KING] = Raylib.LoadTexture("Images/512h/w_king_png_512px.png"),
                [Pieces.W_QUEEN] = Raylib.LoadTexture("Images/512h/w_queen_png_512px.png"),
                [Pieces.W_ROOK] = Raylib.LoadTexture("Images/512h/w_rook_png_512px.png"),
                [Pieces.W_BISHOP] = Raylib.LoadTexture("Images/512h/w_bishop_png_512px.png"),
                [Pieces.W_KNIGHT] = Raylib.LoadTexture("Images/512h/w_knight_png_512px.png"),
                [Pieces.W_PAWN] = Raylib.LoadTexture("Images/512h/w_pawn_png_512px.png"),
                [Pieces.B_KING] = Raylib.LoadTexture("Images/512h/b_king_png_512px.png"),
                [Pieces.B_QUEEN] = Raylib.LoadTexture("Images/512h/b_queen_png_512px.png"),
                [Pieces.B_ROOK] = Raylib.LoadTexture("Images/512h/b_rook_png_512px.png"),
                [Pieces.B_BISHOP] = Raylib.LoadTexture("Images/512h/b_bishop_png_512px.png"),
                [Pieces.B_KNIGHT] = Raylib.LoadTexture("Images/512h/b_knight_png_512px.png"),
                [Pieces.B_PAWN] = Raylib.LoadTexture("Images/512h/b_pawn_png_512px.png"),
            };
        }

        public bool IsInBounds((int file, int rank) square, int left, int top, int width, int height)
        {
            return (square.file >= left) && (square.file < left + width) && (square.rank >= top) && (square.rank < top + height);
        }

        public bool IsWhite(int rank, int file)
        {
            if (rank % 2 == 0)
            {
                return file % 2 == 0;
            }
            else
            {
                return file % 2 != 0;
            }
        }

        public void DrawBoard(int left, int top, int width, int height)
        {
            int squareSize = WIN_WIDTH / width;

            for (int row = 0; row < width; row++)
            {
                for (int file = 0; file < width; file++)
                {
                    if (IsWhite(row + top, file + left))
                    {
                        Raylib.DrawRectangle(file * squareSize, row * squareSize, squareSize, squareSize, WHITE);
                    }
                    else
                    {
                        Raylib.DrawRectangle(file * squareSize, row * squareSize, squareSize, squareSize, BLACK);
                    }


                }
            }
        }

        public void DrawPieces(int left, int top, int width, int height)
        {
            int squareSize = WIN_WIDTH / width;
            float pieceScale = 0.7f;

            for (int row = 0; row < height; row++)
            {
                for (int file = 0; file < width; file++)
                {

                    if (PIECE_TEXTURES.TryGetValue(BoardPosition[row + top, file + left], out var texture))
                    {
                        float scale = (float)squareSize / (float)texture.width * pieceScale;
                        float scaledImageSize = texture.width * scale;
                        Vector2 pos = new Vector2((file * squareSize) + (squareSize - scaledImageSize) / 2f, (row * squareSize) + (squareSize - scaledImageSize) / 2f);
                        Raylib.DrawTextureEx(texture, pos, 0, scale, Color.WHITE);
                    }

                }
            }
        }

        public void DrawSection(int left, int top, int width, int height)
        {
            DrawBoard(left, top, width, height);
            DrawPieces(left, top, width, height);

        }

        public void Run()
        {
            Raylib.InitWindow(WIN_WIDTH, WIN_HEIGHT, "Board Visualizer");

            Init();

            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.BLACK);

                DrawSection(0, 0, 4, 4);

                Raylib.EndDrawing();
            }
        }
    }
    class Program
    {
        public static void Main(string[] args)
        {
            Board board = new Board("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR");
            board.Run();

        }
    }
}