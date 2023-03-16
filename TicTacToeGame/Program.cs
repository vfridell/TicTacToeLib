using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Drawing;
using System.Xml.Linq;
using TicTacToeLib;

namespace TicTacToeGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IHost _host = Host.CreateDefaultBuilder()
                .ConfigureServices(ConfigureTTTServices)
                .Build();

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
                RankSeparation = 10,

                GraphColorScheme = "piyg11",
                GraphBgColor = "6",
                
                NodeColorScheme = "",
                NodeFontColor = "/puor10/10",
                NodeDefaultFillColor = "/piyg11/6",
                TieNodeColor = "/puor10/3",

                XNodeColorScheme = "",
                XWinNodeColor = "/piyg11/2",
                XAlmostWinColor = "/piyg11/4",
                XAdvantageColor = "/piyg11/5",

                ONodeColorScheme = "",
                OWinNodeColor = "/piyg11/10",
                OAlmostWinColor = "/piyg11/8",
                OAdvantageColor = "/piyg11/7",

                EdgeColorScheme = "rdgy9",
                EdgeHighlightColor = "9",
                EdgeMinimalColor = "6",
                FontName = "cascadia code",
                ShowScore = false,
            };

            var graphviz = (GraphvizDriver)_host.Services.GetRequiredService(typeof(GraphvizDriver));
            string imageFilename;

            //GraphWriterFull graphWriterFull = new(options);
            //graphWriterFull.WriteDotFile("TicTacToe_multicolor.dot", root, treeLevels);
            //graphviz.TryGenerateGraph("TicTacToe_multicolor.dot", GraphvizDriver.OutputFormat.PDF, out imageFilename);

            //GraphWriterHighlightBestMove graphWriterHighlightBestMove = new(options);
            //graphWriterHighlightBestMove.WriteDotFile("TicTacToe_HighlightBestMove.dot", root, treeLevels);
            //graphviz.TryGenerateGraph("TicTacToe_HighlightBestMove.dot", GraphvizDriver.OutputFormat.PDF, out imageFilename);

            options.RankSeparation = 5;
            options.GraphLabel = $"<<table border=\"0\" cellborder=\"1\" color=\"{options.NodeFontColor}\"><tr><td sides=\"B\">Tic-Tac-Toe</td></tr><tr><td border=\"0\"><font point-size=\"36\">Best ply chains longer than five</font></td></tr><tr><td border=\"0\" align=\"right\"><font point-size=\"12\">&copy; 2023 Vern Fridell</font></td></tr></table>>";
            GraphWriterBestMovesOnlyTrimmed graphWriterBestMovesOnlyTrimmed = new(options);
            graphWriterBestMovesOnlyTrimmed.WriteDotFile("TicTacToe_bestMoveForestTrimmed.dot", root, treeLevels);
            graphviz.TryGenerateGraph("TicTacToe_bestMoveForestTrimmed.dot", GraphvizDriver.OutputFormat.PDF, out imageFilename);

            //options.RankSeparation = 5;
            //GraphWriterBestMovesOnlyForest graphWriterBestMovesOnlyForest = new(options);
            //graphWriterBestMovesOnlyForest.WriteDotFile("TicTacToe_bestMoveForest.dot", root, treeLevels);
            //graphviz.TryGenerateGraph("TicTacToe_bestMoveForest.dot", GraphvizDriver.OutputFormat.PDF, out imageFilename);

        }

        static void ConfigureTTTServices(HostBuilderContext context, IServiceCollection services)
        {
            var env = context.HostingEnvironment;
            services.Configure<TicTacToeGameOptions>(context.Configuration.GetSection(nameof(TicTacToeGameOptions)))
                .AddSingleton<GraphvizDriver>();
        }
    }
}