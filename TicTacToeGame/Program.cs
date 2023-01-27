using TicTacToeLib;

namespace TicTacToeGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Board initialBoard = new Board();
            Node root = new(initialBoard);
            List<Node> leaves = new();
            GenerateFutureBoards(root, leaves);
            int xwins = leaves.Count(n => n.Board.GetResult() == GameResult.X_Win);
            int owins = leaves.Count(n => n.Board.GetResult() == GameResult.O_Win);
            int draws = leaves.Count(n => n.Board.GetResult() == GameResult.Draw);
            
        }

        static void GenerateFutureBoards(Node node, List<Node> leaves)
        {
            IEnumerable<Move> moves = node.Board.GetAvailableMoves();
            if (!moves.Any())
            {
                leaves.Add(node);
                return;
            }
            foreach (Move move in moves)
            {
                var newBoard = node.Board.Clone();
                newBoard.TryMove(move);
                Node newNode = new Node(newBoard);
                node.Futures.Add(newNode);
                GenerateFutureBoards(newNode, leaves);
            }
        }
    }
}