using Newtonsoft.Json;

namespace TicTacToeLib
{
    public class Node
    {
        public Node(Board board)
        {
            Board = board;
            Score = null;
        }

        public Node(Board board, int score)
        {
            Board = board;
            Score = score;
        }

        public Board Board;
        public int? Score;
        public List<Node> Futures = new List<Node>();
        public Node? BestFuture;

        [JsonIgnore]
        public List<Node> BestParents = new List<Node>();

    }
}
