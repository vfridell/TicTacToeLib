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
            if (!IsValid(move)) return false;
            State[NextMoveNum] = move;
            //TryMakeFinalMove();
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
            //State[8] = new Move((byte)((lastPos << 4) + 0x1));
            State[8] = Move.Get(0x01, lastPos);
            return true;
        }

        public bool IsValid(Move move)
        {
            return GetAvailableMoves().Contains(move);
        }

        public IEnumerable<Move> GetAvailableMoves()
        {
            if (GetResult() != GameResult.Incomplete) yield break;
            int piece = 2;
            if (NextMoveNum % 2 == 0) piece = 1; // X move
            IEnumerable<int> existing = State.Where(m => m != null).Select(m => m.Pos).ToArray();
            for (int i = 0; i<9; i++)
            {
                //if(!existing.Contains(i)) yield return new Move((byte)((i << 4) + piece));
                if(!existing.Contains(i)) yield return Move.Get(piece, i);
            }
        }

        public GameResult GetResult()
        {
            int nextMove = NextMoveNum;
            if (nextMove < 5) return GameResult.Incomplete;
            int _201 = 0, _258 = 0, _603 = 0, _678 = 0, _408 = 0, _417 = 0, _426 = 0, _435 = 0;
            foreach (Move m in State.Where(m => m != null))
            {
                int d = m.Piece == 1 ? 1 : -1;
                switch (m.Pos)
                {
                    case 0:
                        _201 += d;
                        _603 += d;
                        _408 += d;
                        break;
                    case 1:
                        _201 += d;
                        _417 += d;
                        break;
                    case 2:
                        _258 += d;
                        _201 += d;
                        _426 += d;
                        break;
                    case 3:
                        _603 += d;
                        _435 += d;
                        break;
                    case 4:
                        _408 += d;
                        _417 += d;
                        _426 += d;
                        _435 += d;
                        break;
                    case 5:
                        _258 += d;
                        _435 += d;
                        break;
                    case 6:
                        _603 += d;
                        _678 += d;
                        _426 += d;
                        break;
                    case 7:
                        _678 += d;
                        _417 += d;
                        break;
                    case 8:
                        _258 += d;
                        _678 += d;
                        _408 += d;
                        break;
                }
            }

            Func<int, GameResult?> f = (int t) =>
            {
                if (t == 3) return GameResult.X_Win;
                else if (t == -3) return GameResult.O_Win;
                else return null;
            };
            var r = f(_201);
            if (r != null) return r.Value;
            r = f(_258);
            if (r != null) return r.Value;
            r = f(_603);
            if (r != null) return r.Value;
            r = f(_678);
            if (r != null) return r.Value;
            r = f(_408);
            if (r != null) return r.Value;
            r = f(_417);
            if (r != null) return r.Value;
            r = f(_426);
            if (r != null) return r.Value;
            r = f(_435);
            if (r != null) return r.Value;
            if (nextMove < 9) return GameResult.Incomplete;
            return GameResult.Draw;
        }

        public override string ToString()
        {
            string[] pieces = new string[9];
            for(int i=0; i<9; i++) pieces[i] = State[i]?.ToString() ?? " ";
            return 
$"{pieces[8]}|{pieces[7]}|{pieces[6]}\n-----\n{pieces[5]}|{pieces[4]}|{pieces[3]}\n-----\n{pieces[2]}|{pieces[1]}|{pieces[0]}";

        }

        public Board Clone()
        {
            Board newBoard = new Board();
            newBoard.State = new Move[9];
            State.CopyTo(newBoard.State, 0);
            return newBoard;
        }

    }
}