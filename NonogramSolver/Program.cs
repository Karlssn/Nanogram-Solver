using System;

namespace NonogramSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            // clues [1,2,3]
            // clues = [3,2,1]

            /*
             * Sudo Code
             * row.cell = cell[];
             * row.clues = clues;
             * 
             * // Different FUNC
             * // Smarter way to do foreach, while(jobs in joblist)
             *
             * foreach rows
             *      left = leftmostsol(row)
             *      right = righmostsol(row)
             *      
             *      foreach cell in rows
             *          if(right[cell].state==left[cell].state && right[cell].state != "empty")
             *              rows[cell].state = left.state;
             *              if(rows[cell].state =="fill")
             *                  update(cell ? ); // Add the col/row-equivalent
             *                  
             *                  
             *  row leftmostsol(row){
             *      if(row.complete){
             *          return row;
             *      }
             *      if(row.started){
             *          // TODO
             *           1. If a cell is filled, then fit clues around the filled blocks
             *              1.1 Do the leftmost solution so that the closest clue fits the leftmost solution
             *              1.2 Calculate if the filled cells can 
             *              1.3 
             *           2. If a cell is crossed, move the leftmost solution futher to the left
             *      }
             *          
             *          
             *      }
             *      else {
             *          currentIndex=0
             *          foreach clue{
             *              for(int i=0; i<clue; i++){
             *                  row[currentIndex+i].fill;
             *              }
             *              currentIndex+=i+1
             *              row[currentIndex-1].cross
             *          }
             *          
             *          // Check if the row is complete
             *          if(currentIndex=MATRIX.LENGTH){
             *              row.complete;
             *              return row;
             *          }
             *      }
             *  }
             */



        }
    }
}
