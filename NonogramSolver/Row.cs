using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace NonogramSolver
{
    class Row
    {
        public List<Cell> cells { get; set; }
        public RowState state { get; set; }
        public List<int> clues { get; set; }
        public Row Clone()
        {
            var clone = new Row(this.cells.Count);
            for (int i = 0; i < this.cells.Count; i++)
            {
                clone.cells[i].state = this.cells[i].state;
            }
            return clone;
          
        }
        public Row() {
            cells = new List<Cell>();
            this.state = new RowState();
        }
        public Row(int x) {
            cells = new List<Cell>();
            this.state = RowState.empty;
            for (int i = 0; i < x; i++)
            {
                cells.Add(new Cell());
            }
        }
    }
}
