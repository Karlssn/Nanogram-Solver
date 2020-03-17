using System;
using System.Collections.Generic;
using System.Text;

namespace NonogramSolver
{
    class Cell
    {
        public CellState state { get; set; }

        public Cell() {
            this.state = CellState.empty;
        }
    }
}
