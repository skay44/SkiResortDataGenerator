using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ConsoleApp1.Lift;

namespace ConsoleApp1
{
    internal class Slope
    {
        public enum Difficulty
        {
            green,
            blue,
            red,
            black,
            doubleBlack,
            unknown
        }

        int id;
        string zone;
        float bottom_altitude;
        float top_altitude;
        float length;
        Difficulty difficulty;

        List<Lift> slopeToLifts;
        List<Slope> slopeToSlope;

        public Slope(int id, string zone, float bottom_altitude, float top_altitude, float length, Difficulty difficulty)
        {
            this.id = id;
            this.zone = zone;
            this.bottom_altitude = bottom_altitude;
            this.top_altitude = top_altitude;
            this.length = length;
            this.difficulty = difficulty;

            slopeToLifts = new List<Lift>();
            slopeToSlope = new List<Slope>();
        }

        public static Difficulty GetLiftType(String str)
        {
            str = str.ToLower();
            if (str == "gr") return Difficulty.green;
            else if (str == "bl") return Difficulty.blue;
            else if (str == "re") return Difficulty.red;
            else if (str == "1b") return Difficulty.black;
            else if (str == "2b") return Difficulty.doubleBlack;
            else
            {
                Console.Error.WriteLine("unknown lift type: " + str);
                return Difficulty.unknown;
            }
        }

        public override string ToString()
        {
            return "Slope:{ " + id.ToString() + " " + zone + " " + bottom_altitude.ToString() + " " + top_altitude.ToString() + " " + length.ToString() + " " + difficulty.ToString() + " }";
        }
    }
}
