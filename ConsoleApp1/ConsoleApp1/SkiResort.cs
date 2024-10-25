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

        List<Village> villages;
        List<Skier> skiers;
        Random random;
        DateTime currentTime;

        List<Person> customers;

        public SkiResort()
        {
            lifts = new List<Lift>();
            slopes = new List<Slope>();
            liftToSlopes = new List<LiftToSlope>();
            slopeToSlopes = new List<SlopeToSlope>();
            slopeToLifts = new List<SlopeToLift>();
            villages = new List<Village>();
            skiers = new List<Skier>();
            random = new Random();
            currentTime = new DateTime(2017, 11, 15, 7, 0, 0);
            customers = new List<Person>();
        }

        public DateTime CurrentTime { get => currentTime; set => currentTime = value; }
        public void ApplyConnections()
        {
            foreach(LiftToSlope LTS in liftToSlopes)
            {
                int liftID = LTS.From;
                int slopeID = LTS.To;
                Lift from = null;
                Slope to = null;
                foreach(Lift l in lifts)
                {
                    if(l.Id == liftID)
                    {
                        from = l;
                        break;
                    }
                }
                foreach (Slope s in slopes)
                {
                    if(s.Id == slopeID)
                    {
                        to = s;
                        break;
                    }
                }
                if(from != null && to != null)
                {
                    from.liftToSlopes.Add(to);
                }
            }
            foreach(SlopeToLift STL in slopeToLifts)
            {
                int slopeID = STL.From; 
                int liftID = STL.To;
                Slope from = null;
                Lift to = null;
                foreach (Slope s in slopes)
                {
                    if (s.Id == slopeID)
                    {
                        from = s;
                        break;
                    }
                }
                foreach (Lift l in lifts)
                {
                    if (l.Id == liftID)
                    {
                        to = l;
                        break;
                    }
                }
                if (from != null && to != null)
                {
                    from.slopeToLifts.Add(to);
                }
            }
            foreach(SlopeToSlope STS in slopeToSlopes)
            {
                int slopeFrom = STS.From;
                int slopeTo = STS.To;
                Slope from = null;
                Slope to = null;
                foreach(Slope s in slopes)
                {
                    if(s.Id == slopeFrom)
                    {
                        from = s;
                    }
                    if(s.Id == slopeTo)
                    {
                        to = s;
                    }
                    if(from != null && to != null)
                    {
                        break;
                    }
                }
                if (from != null && to != null)
                {
                    from.slopeToSlope.Add(to);
                    from.slopeToSlopeAltitude.Add(STS.Altitude);
                }
            }
        }

        public void AddLift(Lift liftToAdd)
        {
            liftToAdd.Resort = this;
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

        public void addVillage(VillageData data)
        {
            Lift[] villageLifts = new Lift[data.liftIDs.Length];
            int liter = 0;
            Slope[] villageSlopes = new Slope[data.slopeIDs.Length];
            int siter = 0;
            foreach(int LID in data.liftIDs)
            {
                foreach(Lift lift in lifts)
                {
                    if (lift.Id == LID)
                    {
                        villageLifts[liter] = lift;
                        liter++;
                    }
                }
            }
            foreach(int SID in data.slopeIDs)
            {
                foreach (Slope slope in slopes)
                {
                    if(slope.Id == SID)
                    {
                        villageSlopes[siter] = slope;
                        siter++;
                    }
                }
            }
            if(data.liftIDs.Length != liter || data.slopeIDs.Length != siter)
            {
                throw new Exception("nie wczytano wszystkich danych do zaalokowanej pamięci wioski");
            }
            Village newVillage = new Village(villageLifts, villageSlopes, data.pupularity, data.villageName);
            villages.Add(newVillage);
        }

        public void WriteAllVillages(int depth)
        {
            foreach (var village in villages)
            {
                addDepth(depth);
                Console.WriteLine(village);
                Console.Write(village.GetLiftStr(depth + 1));
                Console.Write(village.GetSlopeStr(depth + 1));
            }
        }

        public void WriteAllVillages()
        {
            WriteAllVillages(0);
        }

        public void WriteAllLifts(int depth)
        {
            foreach (var lift in lifts) {
                addDepth(depth);
                Console.WriteLine(lift);
                Console.Write(lift.GetSlopesStr(depth+1));
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
                Console.Write(slope.GetLiftsStr(depth + 1));
                Console.Write(slope.GetSlopesStr(depth + 1));
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
            Console.WriteLine("Villages: ");
            WriteAllVillages(depth + 1);
        }

        public void GenerateCustomers(float delta)
        {
            for (int i = 0; i < delta; i++)
            {
                Person s = new Person(i, random);
                customers.Add(s);
                Console.WriteLine(s.name + " " + s.surname + " from " + s.countryOfOrigin + " was born in " + s.dateOfBirth);
            }
        }

        public void GeneratePasses(float number, int year)
        {
            for (int i = 0; i < number; i++)
            {
                int duration = GenerateDuration();
                DateTime startDate = new DateTime(year, 11, 15).AddDays(random.Next(0, 120 - duration + 1));
                
                int zones = GenerateZones();
                SkiPass s = new SkiPass(i, startDate, zones, duration);

                bool valid = false;
                while(!valid)
                {
                    int r = random.Next(0, customers.Count);
                    bool b = true;
                    foreach(SkiPass pass in customers[r].passList)
                    {
                        if ((pass.dateOfIssue >= s.dateOfIssue && pass.dateOfIssue <= s.expirationDate) || (pass.expirationDate >= s.dateOfIssue && pass.expirationDate <= s.expirationDate))
                        {
                            b = false;
                        }
                    }
                    if (b == true)
                    {
                        customers[r].passList.Add(s);
                        valid = true;
                    }
                }
            }
        }

        public int GenerateDuration()
        {
            int duration = 120;
            double randomNum = random.NextDouble();
            List<double> chances = new List<double>() {0.02,  0.02, 0.29, 0.25, 0.18, 0.1, 0.09, 0.02, 0.03};
            List<int> durations = new List<int>() {1, 2, 5, 7, 10, 14, 30, 60, 120 };

            for (int i = 0; i < chances.Count; i++)
            {
                double sum = 0;
                for (int j = 0; j <= i; j++)
                {
                    sum += chances[j];
                }
                if (randomNum < sum)
                {
                    duration = durations[i];
                    break;
                }
            }

            return duration;
        }

        public int GenerateZones()
        {
            int duration = 120;
            double randomNum = random.NextDouble();
            List<double> chances = new List<double>() { 0.05, 0.05, 0.05, 0.07, 0.13, 0.15, 0.5 };
            List<int> durations = new List<int>() { 1, 2, 4, 8, 3, 12, 15 };

            for (int i = 0; i < chances.Count; i++)
            {
                double sum = 0;
                for (int j = 0; j <= i; j++)
                {
                    sum += chances[j];
                }
                if (randomNum < sum)
                {
                    duration = durations[i];
                    break;
                }
            }

            return duration;
        }

        public void spawnNewSkiers()
        {
            int i = 0;
            foreach (Person customer in customers)
            {
                foreach (SkiPass skiPass in customer.passList)
                {
                    if (skiPass.dateOfIssue <= currentTime && skiPass.expirationDate >= currentTime)
                    {
                        Skier s = new Skier(villages[random.Next(0, 4)], 1000, random);
                        s.Person = customer;
                        s.Pass = skiPass;
                        skiers.Add(s);
                        i++;
                        break;
                    }
                }
            }
            Console.WriteLine("Spawned " + i + " skiers, on day " + currentTime);
        }

        public void ResetSkiers()
        {
            skiers.Clear();
        }

        public void tick(float delta)
        {
            
            foreach(Skier skier in skiers)
            {
                skier.tick(delta);
            }
            foreach(Lift lift in lifts)
            {
                lift.LiftTick(delta);
            }
            currentTime = currentTime.AddSeconds(delta);
        }

        public void WriteAllData()
        {
            WriteAllData(0);
        }
    }
}
