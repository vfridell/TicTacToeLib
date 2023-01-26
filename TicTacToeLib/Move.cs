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
        public bool IsValid
        {
            get
            {
                if (Pos > 8 || (Piece != 1 && Piece != 2)) return false;
                return true;
            }
        }

        public int Pos => Value >> 4;
        public int Piece => Value & 0x0F;

        public enum PositionName { SE, S, SW, E, C, W, NE, N, NW };
        public static Move Get(bool X, PositionName posName)  => new Move((byte)(((int)posName << 4) + (X ? 0x1 : 0x2)));

        public override string ToString() => Piece == 1 ? "X" : "O";
    }
}
