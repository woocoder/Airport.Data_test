using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ILOG.OPL;
using ILOG.Concert;
using ILOG.CP;


namespace Airport.Data_test
{
    internal class ApGPData:CustomOplDataSource
    {
        internal ApGPData(OplFactory oplF):base(oplF)
        {

        }
        public override void CustomRead()
        {
            int beginTime = 1920;//need to initialized
            int endTime = 2100;//need to initialized
            String marketCity = "SHA";//need to initialized
            /*
            1* FlightCode:{"1033538119", "1033538066"}
            2* FlightInfo:[<"738","D",1323,1800,0>,<"738","D",1341,1800,0>]
            3* StandCode:{"301","302"}
            4* StandInfo:[<"N",2>,<"N",2>]
            5* Stnd_fl_type:[0,1...]
            6* preflightcode:{"1033538119", "1033538066"}
            7* prestnd:
            8* SCAdjCon:[<"N207","N207L">,<"N207","N207R">,<"N206","N206L">]
             */

            OplDataHandler handler = getDataHandler();

            //1.FlightCode
            handler.StartElement("FlightCode");
            handler.StartSet();
            List<FlightInfo> FlightCode = new List<FlightInfo>();
            for (int i = 0; i < FlightCode.Count; i++)
            {
                handler.AddStringItem(FlightCode[i].flightCode);
            }
            handler.EndSet();
            handler.EndElement();

            //2.FlightInfo
            handler.StartElement("FlightInfo");
            handler.StartSet();
            List<FlightInfo> FlightInfo = new List<FlightInfo>();
            for (int i = 0; i < FlightInfo.Count; i++)
            {
                handler.StartTuple();
                handler.AddStringItem(FlightInfo[i].flightType);
                handler.AddStringItem(FlightInfo[i].d_or_i);
                handler.AddIntItem(int.Parse(FlightInfo[i].startTime));//是否要转为int？？居飞
                handler.AddIntItem(int.Parse(FlightInfo[i].endTime));
                handler.AddIntItem(int.Parse(FlightInfo[i].isTurnArround));
                handler.EndTuple();
            }
            handler.EndSet();
            handler.EndElement();

            //3.StandCode
            handler.StartElement("StandCode");
            handler.StartSet();
            List<StandInfo> StandCode = new List<StandInfo>();
            for (int i = 0; i < StandCode.Count; i++)
            {
                handler.AddStringItem(StandCode[i].standCode);
            }
            handler.EndSet();
            handler.EndElement();

            //4.StandInfo
            handler.StartElement("StandInfo");
            handler.StartSet();
            List<StandInfo> StandInfo = new List<StandInfo>();
            for (int i = 0; i < StandInfo.Count; i++)
            {
                handler.StartTuple();
                handler.AddStringItem(StandInfo[i].near_or_far);
                handler.AddIntItem(int.Parse(StandInfo[i].priority));
                handler.EndTuple();
            }
            handler.EndSet();
            handler.EndElement();

            //5.Stnd_fl_type
            handler.StartElement("Stnd_fl_type");
            handler.StartSet();

            //6.preflightcode
            handler.StartElement("preflightcode");
            handler.StartSet();

            //7.prestnd
            handler.StartElement("prestnd");
            handler.StartSet();

            //8.SCAdjCon
            handler.StartElement("SCAdjCon");
            handler.StartSet();

            Console.WriteLine("Read Over");
        }
    }
}
