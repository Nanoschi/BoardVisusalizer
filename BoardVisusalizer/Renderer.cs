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
        public int Width { get; set; }

        public void Draw();

    }

    internal class UIBoard : IElement
    {
        public int XPos { get; set; }
        public int YPos { get; set; }
        public int Width { get; set; }

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
            this.Width = size;
        }

        public void SetSection(int file, int rank, int size)
        {
            this._sectionFile = file;
            this._sectionRank = rank;
            this._sectionSize = size;
        }

        public bool IsInsideSection(int file, int rank)
        {
            return (file >= _sectionFile) && (rank >= _sectionRank) && (file < _sectionFile + _sectionSize) && (file < _sectionFile + _sectionSize);
        }

        private void DrawPieces()
        {
            const float localPieceScale = 0.7f;
            int squareSize = Width / 8;

            for (int rank = _sectionRank; rank  < _sectionRank + _sectionSize; rank++)
            {
                for (int file = _sectionFile; file < _sectionFile + _sectionSize; file++)
                {
                    if (!IsInsideSection(file, rank))
                    {
                        continue;
                    }
                    Pieces SquarePiece = CurrentPosition.Get(file, rank);

                    if (SquarePiece == Pieces.EMPTY)
                    {
                        continue;
                    }

                    
                    Texture2D texture = Renderer.PieceTextures[SquarePiece];
                    float absoluteScale = (float)squareSize / (float)texture.width * localPieceScale;
                    float scaledImageSize = texture.width * absoluteScale;
                    float scaledImageX = (file * squareSize) + (squareSize - scaledImageSize) / 2f;
                    float scaledImageY = (rank * squareSize) + (squareSize - scaledImageSize) / 2f;

                    Vector2 pos = new Vector2(scaledImageX + XPos, scaledImageY + YPos);

                    Raylib.DrawTextureEx(texture, pos, 0, absoluteScale, Color.WHITE);



                    
                }
            }
        }

        private void DrawSquares()
        {
            int squareSize = Width / 8;

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
            int outlineSize = Width + 2 * thickness;
            Raylib.DrawRectangle(outlineXPos, outlineYPos, outlineSize, outlineSize, Color.WHITE);
            Raylib.DrawRectangle(XPos, YPos, Width, Width, Color.BLACK); 
        }

        public (int file, int rank) GetSquareAtPosition(int x, int y)
        {
            int squareSize = Width / 8;
            if ((x < XPos || y < YPos) || (x > XPos + Width || y > YPos + Width))
            {
                return (-1, -1);
            }
            int file = (x - XPos) / squareSize;
            int rank = (y - YPos) / squareSize;
            return (file, rank);
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

    internal class UISelector : IElement
    {
        public int XPos { get; set; }
        public int YPos { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public bool IsWhite { get; set; }

        private Color _backgroundColor = new Color(36, 37, 38, 255);

        private static Pieces[] _whitePieces =
        {
            Pieces.W_PAWN,
            Pieces.W_KNIGHT,
            Pieces.W_BISHOP,
            Pieces.W_ROOK,
            Pieces.W_QUEEN,
            Pieces.W_KING,
        };

        private static Pieces[] _blackPieces =
        {
            Pieces.B_PAWN,
            Pieces.B_KNIGHT,
            Pieces.B_BISHOP,
            Pieces.B_ROOK,
            Pieces.B_QUEEN,
            Pieces.B_KING,
        };

        public UISelector(int xPos, int yPos, int width, int height, bool isWhite)
        {
            Height = height;
            XPos = xPos;
            YPos = yPos;
            IsWhite = isWhite;
            Width = width;
        }

        public void Draw()
        {
            Raylib.DrawRectangle(XPos, YPos, Width, Height, _backgroundColor);
        }
    }

    
        
    internal class Renderer
    {
        public static Dictionary<Pieces, Texture2D> PieceTextures { get; private set; } = new Dictionary<Pieces, Texture2D>();

        private int _timeFontSize = 40;
        private Color _timeFontColor = Color.SKYBLUE;

        public UIBoard Board { get; set; } = new UIBoard(0, 0, 100);
        public UISelector WhiteSelector { get; set; } = new UISelector(0, 0, 100, 50, true);

        public static bool ArePiecesLoaded { get; private set;  } = false;

        public void DrawSeconds(float timeInS)
        {
            string formatedTime = timeInS.ToString("0.0");
            int textSize = Raylib.MeasureText(formatedTime, _timeFontSize);
            int YPos = Board.YPos + 5;
            int XPos = (Board.Width / 2) - (textSize / 2) + Board.XPos;
            Raylib.DrawText(formatedTime, XPos, YPos, _timeFontSize, _timeFontColor);

        }
        
        public void DrawApp()
        {
            Board.Draw();
            WhiteSelector.Draw();
        }

        public void SetBoard(int xPos, int yPos, int size)
        {
            Board = new UIBoard(xPos, yPos, size);
        }

        public void SetWhiteSelector(int xPos, int yPos, int width, int height)
        {
            WhiteSelector = new UISelector(xPos, yPos, width, height, true);
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
            ArePiecesLoaded = true;
        }


    }

    
}
