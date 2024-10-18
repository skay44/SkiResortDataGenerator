using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class SkiResort
    {
        List<Lift> lifts;
        List<Slope> slopes;

        List<LiftToSlope> liftToSlopes;
        List<SlopeToLift> slopeToLifts;
        List<SlopeToSlope> slopeToSlopes;

        public SkiResort()
        {
            lifts = new List<Lift>();
            slopes = new List<Slope>();
            liftToSlopes = new List<LiftToSlope>();
            slopeToSlopes = new List<SlopeToSlope>();
            slopeToLifts = new List<SlopeToLift>();
        }

        public void AddLift(Lift liftToAdd)
        {
            lifts.Add(liftToAdd);
        }

        public void AddSlope(Slope slopeToAdd)
        {
            slopes.Add(slopeToAdd);
        }

        public void AddLiftToSlope(LiftToSlope toAdd)
        {
            liftToSlopes.Add(toAdd);
        }

        public void AddSlopeToLift(SlopeToLift toAdd)
        {
            slopeToLifts.Add(toAdd);
        }

        public void AddSlopeToSlope(SlopeToSlope toAdd)
        {
            slopeToSlopes.Add(toAdd);
        }

        private void addDepth(int depth)
        {
            for (int i = 0; i < depth; i++)
                Console.Write("  ");
        }

        public void WriteAllLifts(int depth)
        {
            foreach (var lift in lifts) {
                addDepth(depth);
                Console.WriteLine(lift);
            }
        }

        public void WriteAllLifts()
        {
            WriteAllLifts(0);
        }

        public void WriteAllSlopes(int depth)
        {
            foreach (var slope in slopes)
            {
                addDepth(depth);
                Console.WriteLine(slope);
            }
        }

        public void WriteAllSlopes()
        {
            WriteAllSlopes(0);
        }

        public void WriteAllLiftToSlopes(int depth)
        {
            foreach (var liftToSlope in liftToSlopes)
            {
                addDepth(depth);
                Console.WriteLine(liftToSlope);
            }
        }

        public void WriteAllLiftToSlopes()
        {
            WriteAllLiftToSlopes(0);
        }

        public void WriteAllSlopeToLifts(int depth)
        {
            foreach (var slopeToLift in slopeToLifts)
            {
                addDepth(depth);
                Console.WriteLine(slopeToLift);
            }
        }

        public void WriteAllSlopeToLifts()
        {
            WriteAllSlopeToLifts(0);
        }

        public void WriteAllSlopeToSlopes(int depth)
        {
            foreach (var slopeToSlope in slopeToSlopes)
            {
                addDepth(depth);
                Console.WriteLine(slopeToSlope);
            }
        }
        public void WriteAllSlopeToSlopes()
        {
            WriteAllSlopeToSlopes(0);
        }

        public void WriteAllData(int depth)
        {
            addDepth(depth);
            Console.WriteLine("Lifts: ");
            WriteAllLifts(depth + 1);
            addDepth(depth);
            Console.WriteLine("Slopes: ");
            WriteAllSlopes(depth + 1);
            addDepth(depth);
            Console.WriteLine("Lift to Slope: ");
            WriteAllLiftToSlopes(depth + 1);
            addDepth(depth);
            Console.WriteLine("Slope to Lift: ");
            WriteAllSlopeToLifts(depth + 1);
            addDepth(depth);
            Console.WriteLine("Slope to Slope: ");
            WriteAllSlopeToSlopes(depth + 1);
        }

        public void WriteAllData()
        {
            WriteAllData(0);
        }
    }
}
