using System.Drawing;
using System.Xml.Linq;
using TicTacToeLib;

namespace TicTacToeGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Board initialBoard = new Board();
            Node root = new(initialBoard, BoardScorer.GetScore(initialBoard));
            List<Node> leaves = new();
            Dictionary<int, List<Node>> treeLevels = new();
            TreeHelpers.GenerateFutureBoards(root, 0, treeLevels);
            int xwins = treeLevels[TreeHelpers.LeafLevel].Count(n => n.Board.GetResult() == GameResult.X_Win);
            int owins = treeLevels[TreeHelpers.LeafLevel].Count(n => n.Board.GetResult() == GameResult.O_Win);
            int draws = treeLevels[TreeHelpers.LeafLevel].Count(n => n.Board.GetResult() == GameResult.Draw);

            TreeHelpers.NegamaxAllTheNodes(root, treeLevels);

            GraphOptions options = new()
            {
                GraphLabel = "",
                GraphLabelFontSize = 96,
                NodeLabelFontSize = 24,
                NodeFontColor = "black",
                NodeDefaultFillColor = "transparent",
                RankSeparation = 10,
                EdgeHighlightColor = "9",
                EdgeMinimalColor = "3",
                DrawNodeColor = "darkturquoise",
                XWinNodeColor = "orangered",
                OWinNodeColor = "limegreen",
                XAlmostWinColor = "tomato",
                OAlmostWinColor = "springgreen",
                XAdvantageColor = "yellow",
                OAdvantageColor = "yellow",
                GraphBgColor = "2",
                ColorScheme = "oranges9",
                FontName = "courier new",
                ShowScore = true,
            };

            GraphWriterFull graphWriterFull = new(options);
            graphWriterFull.WriteDotFile("TicTacToe_multicolor.dot", root, treeLevels);

            GraphWriterHighlightBestMove graphWriterHighlightBestMove = new(options);
            graphWriterHighlightBestMove.WriteDotFile("TicTacToe_HighlightBestMove.dot", root, treeLevels);

            options.RankSeparation = 4;
            GraphWriterBestMovesOnlyTrimmed graphWriterBestMovesOnlyTrimmed = new(options);
            graphWriterBestMovesOnlyTrimmed.WriteDotFile("TicTacToe_bestMoveForestTrimmed.dot", root, treeLevels);

            options.RankSeparation = 5;
            GraphWriterBestMovesOnlyForest graphWriterBestMovesOnlyForest = new(options);
            graphWriterBestMovesOnlyForest.WriteDotFile("TicTacToe_bestMoveForest.dot", root, treeLevels);
        }

    }
}