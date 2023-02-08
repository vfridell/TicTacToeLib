using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeGame
{
    internal class ColorPicker
    {
        private List<string> _colors;
        private int _currentIndex;

        public ColorPicker(params string[] colors)
        {
            _colors = new(colors);
            _currentIndex = 0;
        }

        public ColorPicker(List<string> colors)
        {
            _colors = new(colors);
            _currentIndex = 0;
        }

        public string Next()
        {
            if (_currentIndex > _colors.Count - 1) _currentIndex = 0;
            return _colors[_currentIndex++];
        }
    }
}
