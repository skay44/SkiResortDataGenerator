using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Lift
    {
        public enum LiftType
        {
            magicCarpet,
            chairlift,
            ropeTow,
            gondola,
            unknown
        }

        int id;
        string zone;
        float bottom_altitude;
        float top_altitude;
        Lift.LiftType liftType;
        float length;
        float rideTime;
        int throughput;
        TimeSpan openingTime;
        TimeSpan closingTime;

        List<Slope> liftToSlopes;

        public Lift(int id, string zone, float bottom_altitude, float top_altitude, LiftType liftType, float length, float rideTime, int throughput, TimeSpan openingTime, TimeSpan closingTime)
        {
            this.id = id;
            this.zone = zone;
            this.bottom_altitude = bottom_altitude;
            this.top_altitude = top_altitude;
            this.liftType = liftType;
            this.length = length;
            this.rideTime = rideTime;
            this.throughput = throughput;
            this.openingTime = openingTime;
            this.closingTime = closingTime;

            liftToSlopes = new List<Slope>();
        }

        public static LiftType GetLiftType(String str)
        {
            str = str.ToLower();
            if (str == "magic carpet") return LiftType.magicCarpet;
            else if (str == "chairlift") return LiftType.chairlift;
            else if (str == "rope tow") return LiftType.ropeTow;
            else if (str == "gondola") return LiftType.gondola;
            else
            {
                Console.Error.WriteLine("unknown lift type: " + str);
                return LiftType.unknown;
            }
        }

        public override string ToString()
        {
            return "Lift:{ " + id.ToString() + " " + zone + " " + bottom_altitude.ToString() + " " + top_altitude.ToString() + " " + liftType.ToString() + " " + length.ToString() + " " + rideTime.ToString() + " " + throughput.ToString() + " " + openingTime.ToString() + " " + closingTime.ToString() + " }";
        }
    }
}
