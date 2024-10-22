﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Skier
    {
        public enum State
        {
            skiing,
            ascending,
            waiting,
            idle
        }

        /* m/s */
        static float greenSpeed = 4.21f;
        static float blueSpeed = 7.16f;
        static float redSpeed = 8.11f;
        static float blackSpeed = 9.54f;
        static float doubleBlackSpeed = 5.27f;

        Slope currentSlope;
        Lift currentLift;
        Village currenVillage;
        float height;
        State state;
        Random rnd;
        Slope nextSlope;
        Lift nextLift;
        float NextAltitude;

        internal Slope CurrentSlope { get => currentSlope; set { 
                currentSlope = value;
                currentLift = null;
                currenVillage = null;
            } 
        }
        internal Lift CurrentLift { get => currentLift; set { 
                currentLift = value;
                currentSlope = null;
                currenVillage = null;
            } 
        }
        internal Village CurrentVillage { get => currenVillage; set { 
                currenVillage = value;
                currentSlope = null;
                currentLift = null;
            } 
        }
        internal Slope NextSlope
        {
            get => nextSlope; set
            {
                nextSlope = value;
                nextLift = null;
            }
        }
        internal Lift NextLift
        {
            get => nextLift; set
            {
                nextLift = value;
                nextSlope = null;
            }
        }

        public Skier(Village village, float height)
        {
            this.CurrentVillage = village;
            this.height = height;
            this.state = State.idle;
            rnd = new Random();
        }

        void tryGettingOnTheLift(Lift lift)
        {
            if(lift.currentUsers >= lift.throughput)
            {
                this.state = State.waiting;
                lift.liftQueue.Enqueue(this);
            }
            this.state = State.ascending;
        }

        void chooseFromVillage(Village village) {
            int options = village.lifts.Length;
            int random = rnd.Next(options);
            CurrentLift = village.lifts[random];
            chooseFromSlopes(CurrentLift.liftToSlopes);
            tryGettingOnTheLift(CurrentLift);
        }

        void chooseFromSlopes(List<Slope> slopes, List<float> altitude) //for slopes
        {
            int options = slopes.Count;
            int random = rnd.Next(options);
            NextSlope = slopes[random];
            NextAltitude = altitude[random];
            //this.state = State.skiing;
        }

        void chooseFromSlopes(List<Slope> slopes) //for lifts
        {
            int options = slopes.Count;
            int random = rnd.Next(options);
            NextSlope = slopes[random];
            //this.state = State.skiing;
        }

        //TODO change currents to nexts
        void chooseFromLifts(List<Lift> lifts)
        {
            int options = lifts.Count;
            int random = rnd.Next(options);
            NextLift = lifts[random];
            NextAltitude = NextLift.Bottom_altitude;
            //this.state = State.ascending;
        }

        //TODO change currents to nexts
        void chooseFromSlopesAndLifts(List<Slope> slopes, List<Lift> lifts, List<float> altitudes)
        {
            List<Slope> tmpSlopes = new List<Slope>();
            List<float> tmpAltitudes = new List<float>();
            for (int i = 0; i < slopes.Count; i++)
            {
                if (altitudes[i] < height)
                {
                    tmpSlopes.Add(slopes[i]);
                    tmpAltitudes.Add(altitudes[i]);
                }
            }
            List<Lift> tmpLifts = new List<Lift>();
            for (int i = 0; i < lifts.Count; i++)
            {
                if (lifts[i].Bottom_altitude < height)
                {
                    tmpLifts.Add(lifts[i]);
                }
            }
            int optionA = tmpSlopes.Count;
            int optionB = optionA + tmpLifts.Count;
            int random = rnd.Next(optionB);
            if(random < optionA)
            {
                chooseFromSlopes(tmpSlopes, tmpAltitudes);
            }
            else
            {
                chooseFromLifts(tmpLifts);
            }
        }

        void IdleTick(float delta)
        {
            if (CurrentVillage != null)
            {
                chooseFromVillage(CurrentVillage);
            }
            else if (CurrentLift != null)//na gorze liftu
            {
                //chooseFromSlopes(currentLift.liftToSlopes);
                CurrentSlope = NextSlope;
                chooseFromSlopesAndLifts(CurrentSlope.slopeToSlope, CurrentSlope.slopeToLifts, CurrentSlope.slopeToSlopeAltitude);
                state = State.skiing;
            }
            else if (CurrentSlope != null)//na dole slopeu
            {
                //chooseFromSlopesAndLifts(CurrentSlope.slopeToSlope, CurrentSlope.slopeToLifts);
                if (NextLift != null)
                {
                    CurrentLift = NextLift;
                    chooseFromSlopes(CurrentLift.liftToSlopes);
                    state = State.ascending;
                }
                else if (NextSlope != null)
                {
                    CurrentSlope = NextSlope;
                    chooseFromSlopesAndLifts(CurrentSlope.slopeToSlope, CurrentSlope.slopeToLifts, CurrentSlope.slopeToSlopeAltitude);
                    state = State.skiing;
                }
                else
                {
                    throw new Exception("Error69");
                }
            }
                
        }

        void WaitingTick(float delta)
        {

        }

        void AscendingTick(float delta)
        {
            if(CurrentLift != null)
            {
                float liftSpeed = CurrentLift.Length / CurrentLift.RideTime;
                height += liftSpeed * delta;
                if(height >= CurrentLift.Top_altitude)
                {
                    height = CurrentLift.Top_altitude;
                    state = State.idle;
                }
            }
            else
            {
                throw new Exception("Error1");
            }
        }

        void SkiingTick(float delta)
        {
            if (CurrentSlope != null)
            {
                float slopeSpeed = (CurrentSlope.Top_altitude - CurrentSlope.Bottom_altitude)/CurrentSlope.Length*getSlopeSpeed(CurrentSlope.DifficultyColor);
                height -= slopeSpeed * delta;
                if (height <= NextAltitude)
                {
                    height = NextAltitude;
                    state = State.idle;
                }
                
            }
            else 
            {
                throw new Exception("Error2");
            }
        }

        float getSlopeSpeed(Slope.Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Slope.Difficulty.green:
                    return greenSpeed;
                case Slope.Difficulty.blue:
                    return blueSpeed;
                case Slope.Difficulty.red:
                    return redSpeed;
                case Slope.Difficulty.black:
                    return blackSpeed;
                case Slope.Difficulty.doubleBlack:
                    return doubleBlackSpeed;
            }
            throw new Exception("unknown difficulty");
        }

        public void tick(float delta)
        {
            Console.WriteLine(getSkierData());
            switch (state)
            {
                case State.skiing:
                    SkiingTick(delta);
                    break;
                case State.ascending:
                    AscendingTick(delta);
                    break;
                case State.waiting:
                    WaitingTick(delta);
                    break;
                case State.idle:
                    IdleTick(delta);
                    break;
            }
        }

        string getSkierData()
        {
            string data = "TICK DATA: ";
            if (CurrentVillage != null)
            {
                data += CurrentVillage.ToString() + "   ";
            }
            else if (CurrentLift != null)//na gorze liftu
            {
                data += "lift: {" + CurrentLift.Id + "}  ";
            }
            else if (CurrentSlope != null)//na dole slopeu
            {
                data += "slope: {" + CurrentSlope.Id + "}   ";
            }
            data += height + "m ";
            data += "[ " + state + " ]";
            return data;
        }
    }


}
