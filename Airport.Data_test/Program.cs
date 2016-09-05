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
        List<FlightInfo> flightList(string starttimeBegin, string starttimeEnd, string endtimeBegin, string endtimeEnd)
        {
            List<FlightInfo> flightList;
            try
            {
                OrclDBManager orclDB_in = new OrclDBManager("gateDatain");
                string flightSql = "select * from V_FLIGHT where STARTTIME >= :starttimeBegin and STARTTIME <= :starttimeEnd";
                Dictionary<string, object> paramDic = new Dictionary<string, object>();
                paramDic.Add("starttimeBegin", Convert.ToDateTime("2015/08/13 19:20:00"));
                paramDic.Add("starttimeEnd", Convert.ToDateTime("2015/08/13 22:20:00"));
                DataTable a = orclDB_in.GetDataTable(flightSql, paramDic);

                for (int i = 0; i != a.Rows; i++)
                {

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {

                Console.Read();
            }
            return flightList;
        }
        static void Main(string[] args)
        {
            
          
            

        }
    }
}
