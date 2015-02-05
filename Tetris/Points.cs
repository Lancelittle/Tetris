using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class Points
    {
        public int ROW { get; set; }
        public int COL { get; set; }

        public Points() { }
        public Points(int row, int col)
        {
            ROW = row;
            COL = col;
        }

    }
}
