﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

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
                SlopeToSlope slopeToLift = new SlopeToSlope(topSlopeId, bottomSlopeId);
                skissueSkiResort.AddSlopeToSlope(slopeToLift);
            }
            myConn.Close();
        }

        static void Main(string[] args)
        {
            SkiResort skissueSkiResort;
            skissueSkiResort = new SkiResort();
            SqlConnection myConn = new SqlConnection("Server=DESKTOP-A2OQMJO;Integrated security=SSPI;database=wyciagi_stoki");

            ReadLifts(myConn, skissueSkiResort);
            ReadSlopes(myConn, skissueSkiResort);
            ReadLiftToSlope(myConn, skissueSkiResort);
            ReadSlopeToLift(myConn, skissueSkiResort);
            ReadSlopeToSlope(myConn, skissueSkiResort);


            skissueSkiResort.WriteAllData();

            Console.ReadLine();
        }
    }
}
