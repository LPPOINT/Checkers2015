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


        public int VictimX { get; set; }
        public int VictimY { get; set; }

        public bool HasVictim
        {
            get { return VictimX != -1 && VictimY != -1; }
        }

        public CheckerMove()
        {
            
        }


        public CheckerMove(Checker checker, int newX, int newY) : this(checker.X, checker.Y, newX, newY)
        {
            
        }
        public CheckerMove(int oldX, int oldY, int newX, int newY)
        {
            OldX = oldX;
            OldY = oldY;
            NewX = newX;
            NewY = newY;
            VictimX = -1;
            VictimY = -1;
        }

        public CheckerMove(int oldX, int oldY, int newX, int newY, int victimX, int victimY)
        {
            OldX = oldX;
            OldY = oldY;
            NewX = newX;
            NewY = newY;
            VictimX = victimX;
            VictimY = victimY;
        }

        protected bool Equals(CheckerMove other)
        {
            return OldX == other.OldX && OldY == other.OldY && NewX == other.NewX && NewY == other.NewY && VictimY == other.VictimY && VictimX == other.VictimX;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CheckerMove) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = OldX;
                hashCode = (hashCode*397) ^ OldY;
                hashCode = (hashCode*397) ^ NewX;
                hashCode = (hashCode*397) ^ NewY;
                hashCode = (hashCode*397) ^ VictimY;
                hashCode = (hashCode*397) ^ VictimX;
                return hashCode;
            }
        }
    }
}
