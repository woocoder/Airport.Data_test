using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Airport.Data_test
{
    class FlightInfo
    {
        public
        string flightCode;
        string flightType;
        string d_or_i;
        string startTime;
        string endTime;
        string isTurnArround;
    }
    class StandInfo
    {
        string standCode;
        string near_or_far;
        string priority;
    }
    class Prerules
    {
        List<Dictionary<FlightInfo,StandInfo>> finishTask;
        List<Dictionary<StandInfo, StandInfo>> sCAdjCon;
    }
}
