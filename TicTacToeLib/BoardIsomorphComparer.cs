using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeLib
{
    public static class BoardIsomorphComparer
    {

        public static bool IsomorphDuplicate(Board board, List<Board> others) => others.Any(b => AreIsomorphs(board, b));
        public static Node? GetFirstIsomorphDuplicate(Board board, List<Node> others) => others.FirstOrDefault(b => AreIsomorphs(board, b.Board));

        public static bool AreIsomorphs(Board b1, Board b2)
        {
            Func<Func<int, int>, bool> IsoCheck = (t) =>
            {
                for (int i = 0; i < 9; i++)
                {
                    Move m1 = b1.State[i];
                    if (m1 == null) break;
                    if (b2.State.FirstOrDefault(m2 => m2?.Pos == t(m1.Pos))?.Piece != m1.Piece) return false;
                }
                return true;
            };

            if (IsoCheck(Equal)) return true;
            if (IsoCheck(Rotate90)) return true;
            if (IsoCheck(MirrorDiag1)) return true;
            if (IsoCheck(MirrorDiag2)) return true;
            if (IsoCheck(MirrorVert)) return true;
            if (IsoCheck(MirrorHoriz)) return true;
            if (IsoCheck(Rotate180)) return true;
            if (IsoCheck(Rotate270)) return true;
            return false;
        }

        public static int Equal(int pos) => pos;

        // 8|7|6
        // _____
        // 5|4|3
        // -----
        // 2|1|0
        public static int MirrorDiag2(int pos) => pos switch
        {
            0 => 0,
            1 => 3,
            2 => 6,
            3 => 1,
            4 => 4,
            5 => 7,
            6 => 2,
            7 => 5,
            8 => 8,
            _ => throw new NotImplementedException(),
        };

        public static int MirrorDiag1(int pos) => pos switch
        {
            0 => 8,
            1 => 5,
            2 => 2,
            3 => 7,
            4 => 4,
            5 => 1,
            6 => 6,
            7 => 3,
            8 => 0,
            _ => throw new NotImplementedException(),
        };

        public static int MirrorVert(int pos) => pos switch
        {
            4 => 4,
            0 => 6,
            1 => 7,
            2 => 8,
            5 => 5,
            3 => 3,
            8 => 2,
            7 => 1, 
            6 => 0,
            _ => throw new NotImplementedException(),
        };

        public static int MirrorHoriz(int pos) => pos switch
        {
            4 => 4,
            0 => 2,
            1 => 1,
            2 => 0,
            3 => 5,
            6 => 8,
            7 => 7,
            8 => 6,
            5 => 3,
            _ => throw new NotImplementedException(),
        };

        public static int Rotate90(int pos) => pos switch
        {
            4 => 4,
            0 => 2,
            1 => 5,
            2 => 8,
            5 => 7,
            8 => 6,
            7 => 3,
            6 => 0,
            3 => 1,
            _ => throw new NotImplementedException(),
        };
        public static int Rotate180(int pos) => Rotate90(Rotate90(pos));
        public static int Rotate270(int pos) => Rotate90(Rotate90(Rotate90(pos)));
    }
}
