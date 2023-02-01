namespace TicTacToeLib
{
    public class Node
    {
        public Node(Board board)
        {
            Board = board;
        }

        public Board Board;
        public List<Node> Futures = new List<Node>();
    }
}
