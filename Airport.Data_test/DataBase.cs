using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace Airport.Data_test
{
    class FlightInfo
    {
        
        public string flightCode;
        public string flightType;
        public string d_or_i;
        public string startTime;
        public string endTime;
        public string isTurnArround;
    }
    class StandInfo
    {
        public string standCode;
        public string near_or_far;
        public string priority;
    }
    class Prerules
    {
        public Dictionary<FlightInfo,StandInfo> preflightcode;
        public Dictionary<StandInfo, StandInfo> sCAdjCon;
        public Dictionary<string,int[]> Stnd_fl_type;
    }
}
