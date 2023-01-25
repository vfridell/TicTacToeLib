namespace TicTacToeLib
{
    public class Board
    {
        // 8|7|6
        // _____
        // 5|4|3
        // -----
        // 2|1|0
        // 
        // 0x00 == empty
        // 0x1 == X
        // 0x2 == O
        public Move[] State { get; private set; } = new Move[9];

        public bool TryMove(Move move)
        {
            if (GetResult() != GameResult.Incomplete) return false;
            if (!IsValid(move)) return false;
            if (State.Any(m => m != null && m.Pos == move.Pos)) return false;
            State[NextMoveNum] = move;
            TryMakeFinalMove();
            return true;
        }

        public int NextMoveNum => State.Count(m => m != null);
        public int PieceToMove => (NextMoveNum % 2 == 0) ? 1 : 2;

        private bool TryMakeFinalMove()
        {
            if (NextMoveNum != 8) return false;
            int lastPos = -1;
            IEnumerable<int> positions = State.Where(m => m != null).Select(m => m.Pos);
            for (int i = 0; i < 8; i++)
            {
                if (!positions.Contains(i))
                {
                    lastPos = i;
                    break;
                }
            }
            State[8] = new Move((byte)((lastPos << 4) + 0x1));
            return true;
        }

        public bool IsValid(Move move)
        {
            if (NextMoveNum > 9) return false;
            if (!move.IsValid) return false;
            if (PieceToMove != move.Piece) return false;
            int moveDiff = State.Count(m => m != null && m.Piece == 1) - State.Count(m => m != null && m.Piece == 2);
            return moveDiff == 1 || moveDiff == 0;
        }


        public IEnumerable<Move> GetAvailableMoves()
        {
            if (GetResult() != GameResult.Incomplete) yield break;
            int piece = 2;
            if (NextMoveNum % 2 == 0) piece = 1; // X move
            for (int i = 0; i < 9; i++)
            {
                if(State[i] == null) yield return new Move((byte)((i << 4) + piece));
            }
        }

        public GameResult GetResult()
        {
            if (NextMoveNum < 5) return GameResult.Incomplete;

            int[] x = State.Where(m => m != null && m.Piece == 1).Select(m => m.Pos).ToArray();
            if (x.Contains(0) && x.Contains(1) && x.Contains(2)) return GameResult.X_Win;
            if (x.Contains(3) && x.Contains(4) && x.Contains(5)) return GameResult.X_Win;
            if (x.Contains(6) && x.Contains(7) && x.Contains(8)) return GameResult.X_Win;
            if (x.Contains(2) && x.Contains(5) && x.Contains(8)) return GameResult.X_Win;
            if (x.Contains(1) && x.Contains(4) && x.Contains(7)) return GameResult.X_Win;
            if (x.Contains(0) && x.Contains(3) && x.Contains(6)) return GameResult.X_Win;
            if (x.Contains(2) && x.Contains(4) && x.Contains(6)) return GameResult.X_Win;
            if (x.Contains(0) && x.Contains(4) && x.Contains(8)) return GameResult.X_Win;

            int[] o = State.Where(m => m != null && m.Piece == 2).Select(m => m.Pos).ToArray();
            if (o.Contains(0) && o.Contains(1) && o.Contains(2)) return GameResult.O_Win;
            if (o.Contains(3) && o.Contains(4) && o.Contains(5)) return GameResult.O_Win;
            if (o.Contains(6) && o.Contains(7) && o.Contains(8)) return GameResult.O_Win;
            if (o.Contains(2) && o.Contains(5) && o.Contains(8)) return GameResult.O_Win;
            if (o.Contains(1) && o.Contains(4) && o.Contains(7)) return GameResult.O_Win;
            if (o.Contains(0) && o.Contains(3) && o.Contains(6)) return GameResult.O_Win;
            if (o.Contains(2) && o.Contains(4) && o.Contains(6)) return GameResult.O_Win;
            if (o.Contains(0) && o.Contains(4) && o.Contains(8)) return GameResult.O_Win;

            if (NextMoveNum < 9) return GameResult.Incomplete;
            return GameResult.Draw;
        }

        public override string ToString()
        {
            string[] pieces = new string[9];
            for(int i=0; i<9; i++) pieces[i] = State[i]?.ToString() ?? " ";
            return 
$"{pieces[8]}|{pieces[7]}|{pieces[6]}\n-----\n{pieces[5]}|{pieces[4]}|{pieces[3]}\n-----\n{pieces[2]}|{pieces[1]}|{pieces[0]}";

        }


    }
}