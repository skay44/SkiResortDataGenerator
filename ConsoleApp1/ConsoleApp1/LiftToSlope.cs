﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class LiftToSlope
    {
        int from;
        int to;

        public LiftToSlope(int from, int to)
        {
            this.from = from;
            this.to = to;
        }
        public override string ToString()
        {
            return "LiftToSlope:{ " + from + " " + to + "}";
        }
    }


}
