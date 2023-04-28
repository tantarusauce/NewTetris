using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewTetris
{
    public class Game
    {
        public int[,] placedMino = new int[17, 24];
        public int kind;
        public int hold;
        public int hidHold;
        public bool veryfast;
        public int score;
        public int deletedRow;
        public int level;
        public bool holded;
        public int combo;
        public int tickCount;
        public int fallTick;
        public int[] deletedRowNum = new int[4];
    }
}
