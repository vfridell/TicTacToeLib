using TicTacToeLib;

namespace TestProject
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void InitialBoardStateCount()
        {
            Board board = new Board();
            Assert.AreEqual(0, board.NextMoveNum);
        }

        [TestMethod]
        public void BoardStateCount1()
        {
            Board board = new Board();
            Assert.IsTrue(board.TryMove(Move.Get(true, Move.PositionName.C)));
            Assert.AreEqual(1, board.NextMoveNum);
        }

        [TestMethod]
        public void BoardBadMoves1()
        {
            Board board = new Board();
            Assert.IsTrue(board.TryMove(Move.Get(true, Move.PositionName.C)));
            Assert.IsFalse(board.TryMove(Move.Get(true, Move.PositionName.C)));
            Assert.IsFalse(board.TryMove(Move.Get(false, Move.PositionName.C)));
        }

        [TestMethod]
        public void BoardBadMoves2()
        {
            Board board = new Board();
            Assert.IsTrue(board.TryMove(Move.Get(true, Move.PositionName.C)));
            Assert.IsFalse(board.TryMove(Move.Get(true, Move.PositionName.E)));
        }

        [TestMethod]
        public void DrawGame1()
        {
            Board board = new Board();
            Assert.IsTrue(board.TryMove(Move.Get(true, Move.PositionName.C)));
            Assert.IsTrue(board.TryMove(Move.Get(false, Move.PositionName.SW)));
            Assert.IsTrue(board.TryMove(Move.Get(true, Move.PositionName.NW)));
            Assert.IsTrue(board.TryMove(Move.Get(false, Move.PositionName.SE)));
            Assert.IsTrue(board.TryMove(Move.Get(true, Move.PositionName.S)));
            Assert.IsTrue(board.TryMove(Move.Get(false, Move.PositionName.N)));
            Assert.IsTrue(board.TryMove(Move.Get(true, Move.PositionName.E)));
            Assert.IsTrue(board.TryMove(Move.Get(false, Move.PositionName.W)));
            Assert.IsTrue(board.TryMove(Move.Get(true, Move.PositionName.NE)));
            Assert.AreEqual(GameResult.Draw, board.GetResult());
        }

        [TestMethod]
        public void XWinGame1()
        {
            Board board = new Board();
            Assert.IsTrue(board.TryMove(Move.Get(true, Move.PositionName.C)));
            Assert.IsTrue(board.TryMove(Move.Get(false, Move.PositionName.E)));
            Assert.IsTrue(board.TryMove(Move.Get(true, Move.PositionName.NE)));
            Assert.IsTrue(board.TryMove(Move.Get(false, Move.PositionName.SW)));
            Assert.IsTrue(board.TryMove(Move.Get(true, Move.PositionName.NW)));
            Assert.IsTrue(board.TryMove(Move.Get(false, Move.PositionName.N)));
            Assert.IsTrue(board.TryMove(Move.Get(true, Move.PositionName.SE)));
            Assert.AreEqual(GameResult.X_Win, board.GetResult());
        }


        [TestMethod]
        public void OWinGame1()
        {
            Board board = new Board();
            Assert.IsTrue(board.TryMove(Move.Get(true, Move.PositionName.SW)));
            Assert.IsTrue(board.TryMove(Move.Get(false, Move.PositionName.C)));
            Assert.IsTrue(board.TryMove(Move.Get(true, Move.PositionName.E)));
            Assert.IsTrue(board.TryMove(Move.Get(false, Move.PositionName.NW)));
            Assert.IsTrue(board.TryMove(Move.Get(true, Move.PositionName.S)));
            Assert.IsTrue(board.TryMove(Move.Get(false, Move.PositionName.SE)));
            Assert.AreEqual(GameResult.O_Win, board.GetResult());
        }

        [TestMethod]
        public void XWinGame2()
        {
            Board board = new Board();
            Assert.IsTrue(board.TryMove(Move.Get(true, Move.PositionName.C)));
            Assert.IsTrue(board.TryMove(Move.Get(false, Move.PositionName.E)));
            Assert.IsTrue(board.TryMove(Move.Get(true, Move.PositionName.NE)));
            Assert.IsTrue(board.TryMove(Move.Get(false, Move.PositionName.SE)));
            Assert.IsTrue(board.TryMove(Move.Get(true, Move.PositionName.SW)));
            Assert.AreEqual(GameResult.X_Win, board.GetResult());
        }

        [TestMethod]
        public void IsomorphTest1()
        {
            //  | |X
            // _____
            //  |X|O
            // -----
            //  | | 
            Board board1 = new Board();
            Assert.IsTrue(board1.TryMove(Move.Get(true, Move.PositionName.C)));
            Assert.IsTrue(board1.TryMove(Move.Get(false, Move.PositionName.E)));
            Assert.IsTrue(board1.TryMove(Move.Get(true, Move.PositionName.NE)));
            //  | |
            // _____
            //  |X|
            // -----
            //  |O|X
            Board board2 = new Board();
            Assert.IsTrue(board2.TryMove(Move.Get(true, Move.PositionName.C)));
            Assert.IsTrue(board2.TryMove(Move.Get(false, Move.PositionName.S)));
            Assert.IsTrue(board2.TryMove(Move.Get(true, Move.PositionName.SE)));

            Assert.IsTrue(BoardIsomorphComparer.AreIsomorphs(board1, board2));
        }

        [TestMethod]
        public void IsomorphTest2()
        {
            //  |X| 
            // _____
            //  |X|O
            // -----
            //  | | 
            Board board1 = new Board();
            Assert.IsTrue(board1.TryMove(Move.Get(true, Move.PositionName.C)));
            Assert.IsTrue(board1.TryMove(Move.Get(false, Move.PositionName.E)));
            Assert.IsTrue(board1.TryMove(Move.Get(true, Move.PositionName.N)));
            //  | |
            // _____
            //  |X|
            // -----
            //  |O|X
            Board board2 = new Board();
            Assert.IsTrue(board2.TryMove(Move.Get(true, Move.PositionName.C)));
            Assert.IsTrue(board2.TryMove(Move.Get(false, Move.PositionName.S)));
            Assert.IsTrue(board2.TryMove(Move.Get(true, Move.PositionName.SE)));

            Assert.IsFalse(BoardIsomorphComparer.AreIsomorphs(board1, board2));
        }

        [TestMethod]
        public void IsomorphTest3()
        {
            //  | | 
            // _____
            // X|X|O
            // -----
            //  | | 
            Board board1 = new Board();
            Assert.IsTrue(board1.TryMove(Move.Get(true, Move.PositionName.C)));
            Assert.IsTrue(board1.TryMove(Move.Get(false, Move.PositionName.E)));
            Assert.IsTrue(board1.TryMove(Move.Get(true, Move.PositionName.W)));
            //  | |
            // _____
            // O|X|X
            // -----
            //  | | 
            Board board2 = new Board();
            Assert.IsTrue(board2.TryMove(Move.Get(true, Move.PositionName.C)));
            Assert.IsTrue(board2.TryMove(Move.Get(false, Move.PositionName.W)));
            Assert.IsTrue(board2.TryMove(Move.Get(true, Move.PositionName.E)));

            Assert.IsTrue(BoardIsomorphComparer.AreIsomorphs(board1, board2));
        }

        [TestMethod]
        public void IsomorphTest4()
        {
            //  | |O 
            // _____
            //  |X|
            // -----
            // X| | 
            Board board1 = new Board();
            Assert.IsTrue(board1.TryMove(Move.Get(true, Move.PositionName.C)));
            Assert.IsTrue(board1.TryMove(Move.Get(false, Move.PositionName.NE)));
            Assert.IsTrue(board1.TryMove(Move.Get(true, Move.PositionName.SW)));
            //  | |X
            // _____
            //  |X| 
            // -----
            // O| | 
            Board board2 = new Board();
            Assert.IsTrue(board2.TryMove(Move.Get(true, Move.PositionName.C)));
            Assert.IsTrue(board2.TryMove(Move.Get(false, Move.PositionName.SW)));
            Assert.IsTrue(board2.TryMove(Move.Get(true, Move.PositionName.NE)));

            Assert.IsTrue(BoardIsomorphComparer.AreIsomorphs(board1, board2));
        }

        [TestMethod]
        public void IsomorphTest5()
        {
            //  |O|O
            // _____
            // O|X|
            // -----
            // X|X|X
            Board board1 = new Board();
            Assert.IsTrue(board1.TryMove(Move.Get(true, Move.PositionName.C)));
            Assert.IsTrue(board1.TryMove(Move.Get(false, Move.PositionName.W)));
            Assert.IsTrue(board1.TryMove(Move.Get(true, Move.PositionName.S)));
            Assert.IsTrue(board1.TryMove(Move.Get(false, Move.PositionName.N)));
            Assert.IsTrue(board1.TryMove(Move.Get(true, Move.PositionName.SW)));
            Assert.IsTrue(board1.TryMove(Move.Get(false, Move.PositionName.NE)));
            Assert.IsTrue(board1.TryMove(Move.Get(true, Move.PositionName.SE)));
            // X|X|X
            // _____
            //  |X|O
            // -----
            // O|O| 
            Board board2 = new Board();
            Assert.IsTrue(board2.TryMove(Move.Get(true, Move.PositionName.C)));
            Assert.IsTrue(board2.TryMove(Move.Get(false, Move.PositionName.E)));
            Assert.IsTrue(board2.TryMove(Move.Get(true, Move.PositionName.N)));
            Assert.IsTrue(board2.TryMove(Move.Get(false, Move.PositionName.S)));
            Assert.IsTrue(board2.TryMove(Move.Get(true, Move.PositionName.NE)));
            Assert.IsTrue(board2.TryMove(Move.Get(false, Move.PositionName.SW)));
            Assert.IsTrue(board2.TryMove(Move.Get(true, Move.PositionName.NW)));

            Assert.IsTrue(BoardIsomorphComparer.AreIsomorphs(board1, board2));
        }

        [TestMethod]
        public void IsomorphTest6_SameBoard()
        {
            //  |O|O
            // _____
            // O|X|
            // -----
            // X|X|X
            Board board1 = new Board();
            Assert.IsTrue(board1.TryMove(Move.Get(true, Move.PositionName.C)));
            Assert.IsTrue(board1.TryMove(Move.Get(false, Move.PositionName.W)));
            Assert.IsTrue(board1.TryMove(Move.Get(true, Move.PositionName.S)));
            Assert.IsTrue(board1.TryMove(Move.Get(false, Move.PositionName.N)));
            Assert.IsTrue(board1.TryMove(Move.Get(true, Move.PositionName.SW)));
            Assert.IsTrue(board1.TryMove(Move.Get(false, Move.PositionName.NE)));
            Assert.IsTrue(board1.TryMove(Move.Get(true, Move.PositionName.SE)));
            //  |O|O
            // _____
            // O|X|
            // -----
            // X|X|X
            Board board2 = new Board();
            Assert.IsTrue(board2.TryMove(Move.Get(true, Move.PositionName.C)));
            Assert.IsTrue(board2.TryMove(Move.Get(false, Move.PositionName.W)));
            Assert.IsTrue(board2.TryMove(Move.Get(true, Move.PositionName.S)));
            Assert.IsTrue(board2.TryMove(Move.Get(false, Move.PositionName.N)));
            Assert.IsTrue(board2.TryMove(Move.Get(true, Move.PositionName.SW)));
            Assert.IsTrue(board2.TryMove(Move.Get(false, Move.PositionName.NE)));
            Assert.IsTrue(board2.TryMove(Move.Get(true, Move.PositionName.SE)));

            Assert.IsTrue(BoardIsomorphComparer.AreIsomorphs(board1, board2));
        }

        [TestMethod]
        public void GetMove()
        {
            Move m = Move.Get(false, Move.PositionName.E);
            Assert.AreEqual(3, m.Pos);
            Assert.AreEqual(2, m.Piece);
        }

        [TestMethod]
        public void GetAvailableMoves1()
        {
            Board board = new Board();
            List<Move> moves = board.GetAvailableMoves().ToList();
            Assert.AreEqual(9, moves.Count);
            Assert.IsTrue(moves.All(m => m.Piece == 1));
            for (int i = 0; i < 9; i++) Assert.IsTrue(moves.Any(m => m.Pos == i));
        }

        [TestMethod]
        public void GetAvailableMoves2()
        {
            Board board = new Board();
            List<Move> moves = board.GetAvailableMoves().ToList();
            int predict = 9;
            while (moves.Count > 0)
            {
                Assert.AreEqual(predict, moves.Count);
                Assert.IsTrue(moves.All(m => m.Piece == board.PieceToMove));
                Assert.IsTrue(board.TryMove(moves[0]));
                moves = board.GetAvailableMoves().ToList();
                predict--;
            }
            Assert.AreNotEqual(GameResult.Incomplete, board.GetResult());
        }

        [TestMethod]
        public void PerfectGame1()
        {
            Board board = new Board();
            List<Move> bestMoves = new();
            while (board.GetResult() == GameResult.Incomplete)
            {
                int bestScore = board.PieceToMove == 1 ? int.MinValue : int.MaxValue;
                Move? bestMove = null;
                foreach (Move move in board.GetAvailableMoves())
                {
                    Board clone = board.Clone();
                    clone.TryMove(move);
                    int score = BoardScorer.GetScore(clone);
                    if (board.PieceToMove == 1)
                    {
                        if (score >= bestScore)
                        {
                            bestMove = move;
                            bestScore = score;
                        }
                    }
                    else
                    {
                        if (score <= bestScore)
                        {
                            bestMove = move;
                            bestScore = score;
                        }
                    }
                }
                Assert.IsNotNull(bestMove);
                if (bestMove != null)
                {
                    bestMoves.Add(bestMove);
                    board.TryMove(bestMove);
                }
            }
            Assert.AreEqual(GameResult.Draw, board.GetResult());
        }

        [TestMethod]
        public void Score1()
        {
            //  |O|X
            // _____
            //  |X|
            // -----
            // O| |X
            Board board1 = new Board();
            Assert.IsTrue(board1.TryMove(Move.Get(true, Move.PositionName.C)));
            int score = BoardScorer.GetScore(board1);
            Assert.AreEqual(4, score);
            
            Assert.IsTrue(board1.TryMove(Move.Get(false, Move.PositionName.N)));
            score = BoardScorer.GetScore(board1);
            Assert.AreEqual(2, score);
            
            Assert.IsTrue(board1.TryMove(Move.Get(true, Move.PositionName.NE)));
            score = BoardScorer.GetScore(board1);
            Assert.AreEqual(7, score);

            Assert.IsTrue(board1.TryMove(Move.Get(false, Move.PositionName.SW)));
            score = BoardScorer.GetScore(board1);
            Assert.AreEqual(1, score);
            
            Assert.IsTrue(board1.TryMove(Move.Get(true, Move.PositionName.SE)));
            score = BoardScorer.GetScore(board1);
            Assert.AreEqual(16, score);

        }

        [TestMethod]
        public void Score2()
        {
            //  |O|O
            // _____
            // O|X|
            // -----
            // X|X|X
            Board board1 = new Board();
            Assert.IsTrue(board1.TryMove(Move.Get(true, Move.PositionName.C)));
            Assert.IsTrue(board1.TryMove(Move.Get(false, Move.PositionName.W)));
            Assert.IsTrue(board1.TryMove(Move.Get(true, Move.PositionName.S)));
            Assert.IsTrue(board1.TryMove(Move.Get(false, Move.PositionName.N)));
            Assert.IsTrue(board1.TryMove(Move.Get(true, Move.PositionName.SW)));
            Assert.IsTrue(board1.TryMove(Move.Get(false, Move.PositionName.NE)));
            Assert.IsTrue(board1.TryMove(Move.Get(true, Move.PositionName.SE)));
            int score = BoardScorer.GetScore(board1);
            Assert.AreEqual(999, score);

        }
    }
}