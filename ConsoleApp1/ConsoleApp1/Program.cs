using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Xml.Serialization;

namespace ConsoleApp1
{
    internal class Program
    {

        static SqlDataReader ReadTableFormDB(SqlConnection connection, string tableName)
        {
            String str;
            str = "SELECT * FROM " + tableName;
            SqlCommand myCommand = new SqlCommand(str, connection);
            SqlDataReader data = null;
            try
            {
                data = myCommand.ExecuteReader();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            return data;
        }

        static void ReadLifts(SqlConnection myConn, SkiResort skissueSkiResort)
        {
            myConn.Open();
            SqlDataReader data = ReadTableFormDB(myConn, "Ski_Lifts");
            while (data.Read())
            {
                int id = data.GetInt32(0);
                string zone = data.GetString(1);
                float bottom_altitude = (float)data.GetDecimal(2);
                float top_altitude = (float)data.GetDecimal(3);
                Lift.LiftType liftType = Lift.GetLiftType(data.GetString(4));
                float length = (float)data.GetDecimal(5);
                float rideTime = (float)data.GetDecimal(6);
                int throughput = data.GetInt32(7);
                TimeSpan openingTime = data.GetTimeSpan(8);
                TimeSpan closingTime = data.GetTimeSpan(9);
                Lift newLift = new Lift(id, zone, bottom_altitude, top_altitude, liftType, length, rideTime, throughput, openingTime, closingTime);
                skissueSkiResort.AddLift(newLift);
            }
            myConn.Close();
        }

        static void ReadSlopes(SqlConnection myConn, SkiResort skissueSkiResort)
        {
            myConn.Open();
            SqlDataReader data = ReadTableFormDB(myConn, "Ski_Slopes");
            while (data.Read())
            {
                int id = data.GetInt32(0);
                string zone = data.GetString(1);
                float bottom_altitude = (float)data.GetDecimal(2);
                float top_altitude = (float)data.GetDecimal(3);
                float length = (float)data.GetDecimal(4);
                Slope.Difficulty slopeDifficylty = Slope.GetLiftType(data.GetString(5));
                Slope newSLope = new Slope(id, zone, bottom_altitude, top_altitude, length, slopeDifficylty);
                skissueSkiResort.AddSlope(newSLope);
            }
            myConn.Close();
        }

        static void ReadSlopeToLift(SqlConnection myConn, SkiResort skissueSkiResort)
        {
            myConn.Open();
            SqlDataReader data = ReadTableFormDB(myConn, "Slope_to_Lift");
            while (data.Read())
            {
                int slopeId = data.GetInt32(0);
                int liftId = data.GetInt32(1);
                SlopeToLift slopeToLift = new SlopeToLift(slopeId, liftId);
                skissueSkiResort.AddSlopeToLift(slopeToLift);
            }
            myConn.Close();
        }

        static void ReadLiftToSlope(SqlConnection myConn, SkiResort skissueSkiResort)
        {
            myConn.Open();
            SqlDataReader data = ReadTableFormDB(myConn, "Lift_to_Slope");
            while (data.Read())
            {
                int liftId = data.GetInt32(0);
                int slopeId = data.GetInt32(1);
                LiftToSlope slopeToLift = new LiftToSlope(liftId, slopeId);
                skissueSkiResort.AddLiftToSlope(slopeToLift);
            }
            myConn.Close();
        }

        static void ReadSlopeToSlope(SqlConnection myConn, SkiResort skissueSkiResort)
        {
            myConn.Open();
            SqlDataReader data = ReadTableFormDB(myConn, "Slope_to_Slope");
            while (data.Read())
            {
                int topSlopeId = data.GetInt32(0);
                int bottomSlopeId = data.GetInt32(1);
                float altitude = (float)data.GetDecimal(2);
                SlopeToSlope slopeToSlope = new SlopeToSlope(topSlopeId, bottomSlopeId, altitude);
                skissueSkiResort.AddSlopeToSlope(slopeToSlope);
            }
            myConn.Close();
        }
        
        static void AddVillages(SkiResort resort)
        {
            VillageData[] vData = new VillageData[4];
            vData[0].pupularity = 10;
            vData[0].liftIDs = new int[] { 1, 2 };
            vData[0].slopeIDs = new int[] { 1, 3 };
            vData[0].villageName = "Village A";

            vData[1].pupularity = 25;
            vData[1].liftIDs = new int[] { 7, 8 };
            vData[1].slopeIDs = new int[] { 13, 15 };
            vData[1].villageName = "Village B";

            vData[2].pupularity = 20;
            vData[2].liftIDs = new int[] { 13, 14 };
            vData[2].slopeIDs = new int[] { 24, 47 };
            vData[2].villageName = "Village C";

            vData[3].pupularity = 10;
            vData[3].liftIDs = new int[] { 23, 25, 26 };
            vData[3].slopeIDs = new int[] { 38, 41, 45 };
            vData[3].villageName = "Village D";

            resort.addVillage(vData[0]);
            resort.addVillage(vData[1]);
            resort.addVillage(vData[2]);
            resort.addVillage(vData[3]);
        }

        static void Simulate(int seasons, SkiResort resort, float tickLength)
        {
            resort.GenerateCustomers(new Random().Next(10000, 25001));
            
            for (int j = 0; j < seasons; j++)
            {
                resort.CurrentTime = new DateTime(2017, 11, 15, 7, 0, 0).AddYears(j);
                //todo generate skipassed and set date for new opening

                for (int k = 0; k < 120; k++)
                {
                    resort.ResetSkiers();
                    resort.spawnNewSkiers(new Random().Next(100, 1001));

                    for (int i = 0; i < 660; i++)
                    {
                        resort.tick(tickLength);
                    }
                    resort.CurrentTime.AddHours(13);
                }
            }
            Console.WriteLine("done");
        }

        static void Main(string[] args)
        {
            SkiResort skissueSkiResort;
            skissueSkiResort = new SkiResort();
            SqlConnection myConn = new SqlConnection("Server=EPICGAMERPUTER;Integrated security=SSPI;database=wyciagi_stoki");
            
            ReadLifts(myConn, skissueSkiResort);
            ReadSlopes(myConn, skissueSkiResort);
            ReadLiftToSlope(myConn, skissueSkiResort);
            ReadSlopeToLift(myConn, skissueSkiResort);
            ReadSlopeToSlope(myConn, skissueSkiResort);

            skissueSkiResort.ApplyConnections();

            AddVillages(skissueSkiResort);

            Simulate(5, skissueSkiResort, 60);

            //skissueSkiResort.WriteAllLifts();
            //skissueSkiResort.WriteAllSlopes();
            //skissueSkiResort.WriteAllVillages();

            Console.ReadLine();
        }
    }
}
