using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ChessDatatypes;
using Raylib_cs;
using static System.Formats.Asn1.AsnWriter;

namespace AppRenderer
{
    internal interface IElement
    {
        public int XPos { get; set; }
        public int YPos { get; set; }
        public int Size { get; set; }

        public void Draw();

    }

    internal class UIBoard : IElement
    {
        public int XPos { get; set; }
        public int YPos { get; set; }
        public int Size { get; set; }

        public static Color WhiteColor = Color.BEIGE;
        public static Color BlackColor = Color.DARKBROWN;

        public Position CurrentPosition { get; set; }

        protected int _sectionFile { get; set; } = 0;
        protected int _sectionRank { get; set; } = 0;
        protected int _sectionSize { get; set; } = 8;
        public UIBoard(int xPos, int yPos, int size)
        {
            CurrentPosition = new Position();
            this.XPos = xPos;
            this.YPos = yPos;
            this.Size = size;
        }

        public void SetSection(int file, int rank, int size)
        {
            this._sectionFile = file;
            this._sectionRank = rank;
            this._sectionSize = size;
        }

        private bool IsInsideSection(int file, int rank)
        {
            return (file >= _sectionFile) && (rank >= _sectionRank) && (file < _sectionFile + _sectionSize) && (file < _sectionFile + _sectionSize);
        }

        private void DrawPieces()
        {
            const float localPieceScale = 0.7f;
            int squareSize = Size / 8;

            for (int rank = _sectionRank; rank  < _sectionRank + _sectionSize; rank++)
            {
                for (int file = _sectionFile; file < _sectionFile + _sectionSize; file++)
                {
                    if (!IsInsideSection(file, rank))
                    {
                        continue;
                    }
                    Pieces SquarePiece = CurrentPosition.Get(file, rank);

                    if (Renderer.PieceTextures.TryGetValue(SquarePiece, out var texture))
                    {
                        float absoluteScale = (float)squareSize / (float)texture.width * localPieceScale;
                        float scaledImageSize = texture.width * absoluteScale;
                        float scaledImageX = (file * squareSize) + (squareSize - scaledImageSize) / 2f;
                        float scaledImageY = (rank * squareSize) + (squareSize - scaledImageSize) / 2f;

                        Vector2 pos = new Vector2(scaledImageX + XPos, scaledImageY + YPos);

                        Raylib.DrawTextureEx(texture, pos, 0, absoluteScale, Color.WHITE);



                    }
                }
            }
        }

        private void DrawSquares()
        {
            int squareSize = Size / 8;

            for (int rank = _sectionRank; rank < _sectionRank + _sectionSize; rank++)
            {
                for (int file = _sectionFile; file < _sectionFile + _sectionSize; file++)
                {
                    if (!IsInsideSection(file, rank))
                    {
                        continue;
                    }
                    int squareY = (rank * squareSize) + YPos;
                    int squareX = (file * squareSize) + XPos;

                    if (Position.IsSquareWhite(file, rank))
                    {
                        Raylib.DrawRectangle(squareX, squareY, squareSize, squareSize, WhiteColor);
                    }
                    else
                    {
                        Raylib.DrawRectangle(squareX, squareY, squareSize, squareSize, BlackColor);

                    }

                }
            }
        }

        private void drawOutline(int thickness)
        {
            int outlineXPos = XPos - thickness;
            int outlineYPos = YPos - thickness;
            int outlineSize = Size + 2 * thickness;
            Raylib.DrawRectangle(outlineXPos, outlineYPos, outlineSize, outlineSize, Color.WHITE);
            Raylib.DrawRectangle(XPos, YPos, Size, Size, Color.BLACK); 
        }

        public void Draw() 
        {
            drawOutline(3); 
            DrawSquares();
            DrawPieces();
        }

        public void SetUpPosition(Position position)
        {
            CurrentPosition = position;
        }

    }

    
        
    internal class Renderer
    {
        public static Dictionary<Pieces, Texture2D> PieceTextures { get; private set; } = new Dictionary<Pieces, Texture2D>();

        public UIBoard Board { get; set; }
        
        public void DrawApp()
        {
            Board.Draw();
        }

        public void SetBoard(int xPos, int yPos, int size)
        {
            Board = new UIBoard(xPos, yPos, size);
        }

        public static void LoadPieceTextures()
        {
            PieceTextures = new Dictionary<Pieces, Texture2D>()
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


    }

    
}
