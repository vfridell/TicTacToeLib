using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToeLib;

namespace TicTacToeGame
{
    internal class Node
    {
        public Node(Board board)
        {
            Board = board;
        }

        public Board Board;
        public List<Node> Futures = new List<Node>();
    }
}
