﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ChessDatatypes
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

    class Position
    {
        public Pieces[,] PositionArr { get; private set;  }

        public static Dictionary<char, Pieces> FenPieces = new Dictionary<char, Pieces>()
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

        public Position()
        {
            PositionArr = new Pieces[8, 8];

            for (int i = 0;  i < 8; i++)
            {
                for (int j = 0;  j < 8; j++)
                {
                    PositionArr[i, j] = Pieces.EMPTY;
                }
            }
        }

        public void Set(int file, int rank, Pieces piece)
        {
            PositionArr[rank, file] = piece;
        }

        public Pieces Get(int file, int rank)
        {
            return PositionArr[rank, file];
        }
        
        public Pieces[,] GetSection(int left, int top, int width, int height)
        {
            Pieces[,] section = new Pieces[height, width];

            for (int rank = left; rank < left + width; rank++)
            {
                for (int file = top; file < top + height;  file++)
                {
                    section[rank, file] = Get(rank, file);
                }
            }

            return section;
        }
        public static Position FromFEN(string fen)
        {
            Position pos = new Position();
            int rank = 0;
            int file = 0;

            foreach (var item in fen)
            {
                if (FenPieces.TryGetValue(item, out var piece))
                {
                    pos.Set(file, rank, piece);
                    file++;
                }
                else if (char.IsDigit(item))
                {
                    int empty = (int)item;
                    file += empty;
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

            return pos;
        }

        public static Position GetStartPosition()
        {
            return FromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR");
        }

        public static bool IsSquareWhite(int file, int rank)
        {
            if (rank % 2 == 0)
            {
                return file % 2 == 0;
            }
            else
            {
                return !(file % 2 == 0);
            }
        }

    }
    
}
