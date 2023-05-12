using BoardVisualizer;
using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ChessDatatypes;

namespace InteractableBoard
{
    class Board
    {
        int BoardX;
        int BoardY;
        int BoardSize;

        public static Color WHITE = Color.BEIGE;
        public static Color BLACK = Color.DARKBROWN;

        public static Dictionary<Pieces, Texture2D> PIECE_TEXTURES { get; private set; }

        Position ActivePosition = new Position();

        public Board(int boardX, int boardY, int size)
        {
            this.BoardX = boardX;
            this.BoardY = boardY;
            this.BoardSize = size;

        }

        public void LoadTextures()
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

        public (int rank, int file) GetHoveredSquare(int left, int top, int width, int height)
        {
            int squareSize = BoardSize / width;

            Vector2 mousePos = Raylib.GetMousePosition();

            if (mousePos.X < BoardX || mousePos.X > BoardX + BoardSize || mousePos.Y < BoardY || mousePos.Y > BoardY + BoardSize)
            {
                return (-1, -1);
            }

            mousePos.X -= BoardX; 
            mousePos.Y -= BoardY;

            int hoverdFile = (int)(mousePos.X / squareSize + left);
            int hoverdRank = (int)(mousePos.Y / squareSize + top);

            return (hoverdFile, hoverdRank);
        }

        public bool IsSquareWhite(int rank, int file)
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
            int squareSize = BoardSize / width;

            for (int rank = 0; rank < height; rank++)
            {
                for (int file = 0; file < width; file++)
                {
                    int squareFile = (file * squareSize) + BoardX;
                    int squareRank = (rank * squareSize) + BoardY;

                    if (IsSquareWhite(rank + top, file + left))
                    {
                        Raylib.DrawRectangle(squareFile, squareRank, squareSize, squareSize, WHITE);
                    }
                    else
                    {
                        Raylib.DrawRectangle(squareFile, squareRank, squareSize, squareSize, BLACK);
                    }


                }
            }
        }

        public void DrawPieces(int left, int top, int width, int height)
        {
            int squareSize = BoardSize / width;
            const float pieceScale = 0.7f;

            for (int rank = 0; rank < height; rank++)
            {
                for (int file = 0; file < width; file++)
                {

                    if (PIECE_TEXTURES.TryGetValue(ActivePosition.Get(rank + top, file + left), out var texture))
                    {
                        float scale = (float)squareSize / (float)texture.width * pieceScale;
                        float scaledImageSize = texture.width * scale;
                        float scaledImageX = (file * squareSize) + (squareSize - scaledImageSize) / 2f;
                        float scaledImageY = (rank * squareSize) + (squareSize - scaledImageSize) / 2f;

                        Vector2 pos = new Vector2(scaledImageX + BoardX, scaledImageY + BoardY);

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

        public void SetUpFromFEN(string fen)
        {
            ActivePosition = Position.FromFEN(fen);
        }

    }

    
}
