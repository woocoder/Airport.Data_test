using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airport.Gate.Data.Dao;
using System.Data;
using System.Data.OracleClient;
using System.Configuration;



namespace Airport.Data_test
{
    class Program
    {
        static List<FlightInfo> readFlights(OrclDBManager orclDB_in,string starttimeBegin, string starttimeEnd)
        {
            List<FlightInfo> flightList = new List<FlightInfo>();
            try
            {
                string flightSql = "select * from V_FLIGHT where STARTTIME >= :starttimeBegin and STARTTIME <= :starttimeEnd";
                Dictionary<string, object> paramDic = new Dictionary<string, object>();
                paramDic.Add("starttimeBegin", Convert.ToDateTime("2015/08/13 19:20:00"));
                paramDic.Add("starttimeEnd", Convert.ToDateTime("2015/08/13 22:20:00"));
                DataTable a = orclDB_in.GetDataTable(flightSql, paramDic);

                for (int i = 0; i != a.Rows.Count; i++)
                {
                    FlightInfo fltemp = new FlightInfo();
                    fltemp.flightCode = a.Rows[i]["TASKID"].ToString();
                    fltemp.flightType = a.Rows[i]["AIRCRAFTTYPE"].ToString();
                    fltemp.d_or_i = a.Rows[i]["D_OR_I"].ToString();
                    fltemp.startTime = a.Rows[i]["STARTTIME"].ToString();
                    fltemp.endTime = a.Rows[i]["ENDTIME"].ToString();
                    fltemp.isTurnArround = a.Rows[i]["ISLINKED"].ToString();
                    //Console.WriteLine(fltemp.startTime);
                    flightList.Add(fltemp);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {

                //Console.Read();
            }
            return flightList;
        }
        static List<StandInfo> readStands(OrclDBManager orclDB_in)
        {
            List<StandInfo> standList = new List<StandInfo>();
            try
            {
                string standSql = "select * from V_STAND_POSITION where :standcode = :standcode order by STANDCODE";
                Dictionary<string, object> paramDic = new Dictionary<string, object>();
                paramDic.Add("standcode", "1");
                DataTable a = orclDB_in.GetDataTable(standSql, paramDic);

                for (int i = 0; i != a.Rows.Count; i++)
                {
                    StandInfo stdtemp = new StandInfo();
                    stdtemp.standCode = a.Rows[i]["STANDCODE"].ToString();
                    stdtemp.near_or_far = a.Rows[i]["POSITION"].ToString();
                    stdtemp.priority = a.Rows[i]["PRI"].ToString();
                    standList.Add(stdtemp);
                    //Console.WriteLine(stdtemp.standCode);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {

                //Console.Read();
            }
            return standList;
        }
        static Prerules readRules(OrclDBManager orclDB_in, List<StandInfo> standList, List<FlightInfo> flightList)
        {
            Prerules preRules = new Prerules();
            preRules.Stnd_fl_type = new Dictionary<string, int[]>();

            try
            {

                //设置机型-机位对应关系

                for (int i = 0; i != flightList.Count; i++)
                {
                    string match_flighCode = flightList[i].flightCode;
                    int[] match_stand = new int[standList.Count];
                    string sql_match = "select * from STAND_CONS where AIRCRAFTTYPE = :aircrafttype";
                    Dictionary<string, object> paramDic = new Dictionary<string, object>();
                    paramDic.Add("aircrafttype", flightList[i].flightType.ToString());
                    DataTable a = orclDB_in.GetDataTable(sql_match, paramDic);
                    List<string> a_stand = new List<string>();
                    for (int t = 0; t != a.Rows.Count; t++)
                    {
                        a_stand.Add(a.Rows[t]["STANDCODE"].ToString());
                    }

                    for (int j = 0; j != standList.Count; j++)
                    {
                        if (a_stand.Contains(standList[j].standCode))
                        {
                            match_stand[j] = 1;
                        }
                        else
                        {
                            match_stand[j] = 0;
                        }
                    }
                    preRules.Stnd_fl_type.Add(match_flighCode, match_stand);
                    a_stand.Clear();
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
            }



            return preRules;
        }

        static void Main(string[] args)
        {
            OrclDBManager orclDB_in = new OrclDBManager("gateDatain");

            string a = "2015 / 08 / 13 19:20:00", b = "2015 / 08 / 13 22:20:00";

            List<FlightInfo> flightList = readFlights(orclDB_in, a,b);
            List<StandInfo> standList = readStands(orclDB_in);
            
            Prerules preRules = readRules(orclDB_in, standList, flightList);
            
            foreach (string key in preRules.Stnd_fl_type.Keys)
            {
                Console.Write(key+": ");
                //Console.Write("key :{0} value:{1}\n", key, preRules.match[key][1]);
                for (int i = 0; i != preRules.Stnd_fl_type[key].Length; i++)
                {
                    Console.Write(preRules.Stnd_fl_type[key][i] + " ");

                }
                Console.Write("\n");
            }
            /*
            int count = 0;
            foreach (int i in preRules.match["1033544547"])
            {
                count += i;
            }
            Console.Write(count);
            */
            /*
            string key = "1033544547";
            Console.Write(key + ": ");
            //Console.Write("key :{0} value:{1}\n", key, preRules.match[key][1]);
            for (int i = 0; i != preRules.match[key].Length; i++)
            {
                Console.Write(standList[i].standCode + ": ");
                Console.Write(preRules.match[key][i] + " ");

            }
            Console.Write("\n");
            Console.Read();
            */

        }
    }
}
