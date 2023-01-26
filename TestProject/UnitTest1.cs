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
    }
}