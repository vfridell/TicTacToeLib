namespace TicTacToeLib
{
    public class Board
    {
        // board positions
        // 8|7|6
        // _____
        // 5|4|3
        // -----
        // 2|1|0
        // 
        // 0x00 == empty
        // 0x1 == X
        // 0x2 == O

        // the State array index is in order of turns 0-8. 
        // So the first move by X is State[0], first by O is State[1] and so on.
        public Move[] State { get; private set; } = new Move[9];

        public bool TryMove(Move move)
        {
            if (!IsValid(move)) return false;
            State[NextMoveNum] = move;
            return true;
        }

        public int NextMoveNum => State.Count(m => m != null);
        public int PieceToMove => (NextMoveNum % 2 == 0) ? 1 : 2;

        public bool TryMakeFinalMove()
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

        public string[] GetPiecesByPosition(string emptyVal = " ")
        {
            string[] pieces = new string[9] { emptyVal, emptyVal, emptyVal, emptyVal, emptyVal, emptyVal, emptyVal, emptyVal, emptyVal };
            for (int i = 0; i < 9; i++)
            {
                if (State[i] == null) continue;
                else pieces[State[i].Pos] = State[i].ToString();
            }
            return pieces;
        }

        public override string ToString()
        {
            string[] pieces = GetPiecesByPosition();
            return 
$"{pieces[8]}|{pieces[7]}|{pieces[6]}\n-----\n{pieces[5]}|{pieces[4]}|{pieces[3]}\n-----\n{pieces[2]}|{pieces[1]}|{pieces[0]}";

        }

        public string KeyString()
        {
            string[] pieces = GetPiecesByPosition("_");
            return $"{pieces[8]}{pieces[7]}{pieces[6]}{pieces[5]}{pieces[4]}{pieces[3]}{pieces[2]}{pieces[1]}{pieces[0]}";
        }

        public Board Clone()
        {
            Board newBoard = new Board();
            newBoard.State = new Move[9];
            State.CopyTo(newBoard.State, 0);
            return newBoard;
        }

        public object GetLabel()
        {
            string[] pieces = GetPiecesByPosition();
            string color = "";
            var result = GetResult();
            switch (result)
            {
                case GameResult.Draw:
                    color = "bgcolor=\"aqua\"";
                    break;
                case GameResult.O_Win:
                    color = "bgcolor=\"yellowgreen\"";
                    break;
                case GameResult.X_Win:
                    color = "bgcolor=\"tomato\"";
                    break;
                default:
                    color = "";
                    break;

            };
            return $"<<table {color} border=\"0\" cellborder=\"1\" cellspacing=\"0\"><tr><td port=\"p8\">{pieces[8]}</td><td port=\"p7\">{pieces[7]}</td><td port=\"p6\">{pieces[6]}</td></tr><tr><td port=\"p5\">{pieces[5]}</td><td port=\"p4\">{pieces[4]}</td><td port=\"p3\">{pieces[3]}</td></tr><tr><td port=\"p2\">{pieces[2]}</td><td port=\"p1\">{pieces[1]}</td><td port=\"p0\">{pieces[0]}</td></tr></table>>";
        }
    }
}