using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeLib
{
    public class TripleScore
    {
        public bool Unchallenged => _pieces.All(p => p == 1 || p == 0) || _pieces.All(p => p == 2 || p == 0);
        public int UnchallengedPiece => Unchallenged ? _pieces.FirstOrDefault(p => p != 0) : 0;
        public List<int> UnchallengedPositions()
        {
            List<int> unchallengedPositions = new();
            for(int i= 0; i < 3; i++)
            {
                if (_pieces[i] > 0) unchallengedPositions.Add(_positions[i]);
            }
            return unchallengedPositions;
        }

        private int[] _positions = new int[3];
        private int[] _pieces = new int[3];

        public TripleScore(int pos1, int pos2, int pos3)
        {
            _positions[0] = pos1;
            _positions[1] = pos2;
            _positions[2] = pos3;
        }

        public void SetPieceValues(Board board)
        {
            for (int i=0; i<3; i++)
            {
                int pos = _positions[i];
                _pieces[i] = board.State.FirstOrDefault(m => m?.Pos == pos)?.Piece ?? 0;
            }
        }
    }

    public static class BoardScorer
    {
        private static TripleScore[] _triples = new TripleScore[8]
        { 
            new TripleScore(2, 0, 1),
            new TripleScore(2, 5, 8),
            new TripleScore(6, 0, 3),
            new TripleScore(6, 7, 8),
            new TripleScore(4, 0, 8),
            new TripleScore(4, 1, 7),
            new TripleScore(4, 2, 6),
            new TripleScore(4, 3, 5) 
        };

        // general score algo per individual player:
        // Score(z) = (2^p * t(2)) + t(1)
        // where,
        // z = X or O
        // p = total number of pieces in unchallenged triples with 2 or more pieces
        // t(n) = total number of unchallenged triples with n pieces
        // if t(2) > 0 for a player in a board position where that player moves next,
        //      score is (-)500 (with positive for X and negative for O)
        // if t(3) > 0 for a player, score is (-)999 for a winning board
        // Otherwise, total score is Score(X) - Score(O)
        public static int GetScore(Board board)
        {
            foreach (TripleScore triple in _triples)
            {
                triple.SetPieceValues(board);
            }

            var winningTriples = _triples.Where(t => t.UnchallengedPiece > 0 && t.UnchallengedPositions().Count == 3);
            if (winningTriples.Any())
            {
                if (winningTriples.First().UnchallengedPiece == 1) return 999;
                else return -999;
            }

            int xScore = 0;
            var xTriples = _triples.Where(t => t.UnchallengedPiece == 1);
            var xDoubleTriples = xTriples.Where(t => t.UnchallengedPositions().Count > 1);
            if (board.PieceToMove == 1 && xDoubleTriples.Any()) return 500;
            int xDblUniquePosCount = xDoubleTriples.SelectMany(t => t.UnchallengedPositions()).Distinct().Count();
            xScore = (int)(Math.Pow(2, xDblUniquePosCount) * xDoubleTriples.Count()) + xTriples.Count(t => t.UnchallengedPositions().Count == 1);

            int oScore = 0;
            var oTriples = _triples.Where(t => t.UnchallengedPiece == 2);
            var oDoubleTriples = oTriples.Where(t => t.UnchallengedPositions().Count > 1);
            if (board.PieceToMove == 2 && oDoubleTriples.Any()) return -500;
            int oDblUniquePosCount = oDoubleTriples.SelectMany(t => t.UnchallengedPositions()).Distinct().Count();
            oScore = (int)(Math.Pow(2, oDblUniquePosCount) * oDoubleTriples.Count()) + oTriples.Count(t => t.UnchallengedPositions().Count == 1);

            return xScore - oScore;
        }

    }
}