using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeGame
{
    public class GraphOptions
    {
        public int GraphLabelFontSize { get; set; }
        public int NodeLabelFontSize { get; set; }
        public string NodeFontColor { get; set; }
        public int RankSeparation { get; set; }
        public string XWinNodeColor { get; set; }
        public string OWinNodeColor { get; set; }
        public string DrawNodeColor { get; set; }
        public string ColorScheme { get; set; }
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
    }
}
