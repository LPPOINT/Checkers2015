using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Classes.Implementation
{
    public class CheckerMove
    {
        public int OldX { get; set; }
        public int OldY { get; set; }
        public int NewX { get; set; }
        public int NewY { get; set; }

        public CheckerMove()
        {
            
        }

        public CheckerMove(int oldX, int oldY, int newX, int newY)
        {
            OldX = oldX;
            OldY = oldY;
            NewX = newX;
            NewY = newY;
        }

    }
}
