using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeLib
{
    // 8|7|6
    // _____
    // 5|4|3
    // -----
    // 2|1|0
    // 
    // 0x0 == 0
    // 0x1 == 1
    // ...
    // 0x7 == 7
    // 0x8 == 8

    // Piece Value
    // 0x0 == empty
    // 0x1 == X
    // 0x2 == O

    // So, 0x71 is a move by X to position 7 on the grid
    public record Move(byte Value)
    {
        public int Pos => Value >> 4;
        public int Piece => Value & 0x0F;

        public enum PositionName { SE, S, SW, E, C, W, NE, N, NW };
        public static Move Get(int piece, int pos)  => AllMoves[(byte)((pos << 4) + piece)];
        public static Move Get(bool X, PositionName posName)  => AllMoves[(byte)(((int)posName << 4) + (X ? 0x1 : 0x2))];
        public static Dictionary<byte, Move> AllMoves = new();
        static Move()
        {
            for (int i = 0; i < 9; i++)
            {
                AllMoves.Add((byte)((i << 4) + 0x1), new Move((byte)((i << 4) + 0x1)));
                AllMoves.Add((byte)((i << 4) + 0x2), new Move((byte)((i << 4) + 0x2)));
            }
        }

        public override string ToString() => Piece == 1 ? "X" : "O";
    }
}
