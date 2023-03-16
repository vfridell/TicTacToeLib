using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeGame
{
    public class GraphOptions
    {
        public double GraphLabelFontSize { get; set; }
        public double NodeLabelFontSize { get; set; }
        public string NodeFontColor { get; set; }
        public double RankSeparation { get; set; }
        public string XWinNodeColor { get; set; }
        public string OWinNodeColor { get; set; }
        public string TieNodeColor { get; set; }
        public string NodeColorScheme { get; set; }
        public string GraphColorScheme { get; set; }
        public string EdgeColorScheme { get; set; }
        public bool ShowScore { get; set; }
        public string XAlmostWinColor { get; set; }
        public string OAlmostWinColor { get; set; }
        public string XAdvantageColor { get; set; }
        public string OAdvantageColor { get; set; }
        public string GraphBgColor { get; set; }
        public string GraphLabel { get; set; }
        public string FontName { get; set; }
        public string NodeDefaultFillColor { get; internal set; }
        public string EdgeHighlightColor { get; internal set; }
        public string EdgeMinimalColor { get; internal set; }
        public string XNodeColorScheme { get; internal set; }
        public string ONodeColorScheme { get; internal set; }
    }
}
