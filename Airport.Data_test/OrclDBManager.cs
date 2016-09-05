using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using System.Xml;
using System.Configuration;
using log4net;

namespace Airport.Gate.Data.Dao
{
    public class OrclDBManager : IDisposable
    {
        private ILog log = log4net.LogManager.GetLogger("OrclDBManager");

        private OracleConnection _connection = new OracleConnection();
        public OracleConnection Connection
        {
            get
            {
                return _connection;
            }
        }
        public OrclDBManager()
        {
            _connection.ConnectionString = ConfigurationManager.ConnectionStrings["gateContection"].ToString();
        }

        public OrclDBManager(string ConnectionString)
        {
            _connection.ConnectionString = ConfigurationManager.ConnectionStrings[ConnectionString].ToString();
        }

        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql)
        {
            try
            {
                using (OracleCommand orclCommand = new OracleCommand())
                {
                    _connection.Open();
                    orclCommand.Connection = _connection;
                    orclCommand.CommandText = sql;

                    return orclCommand.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                log.Error("Oracle执行出错。", e);
            }
            return 0;
        }

        /// <summary>
        /// 执行多个sql（带事务）
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteNonQueryTransaction(string[] sqlArr)
        {
            int ret = 0;
            _connection.Open();
            using (OracleCommand orclCommand = new OracleCommand())
            {
                OracleTransaction transaction = null;                
                try
                {
                    orclCommand.Connection = _connection;                   
                    transaction = _connection.BeginTransaction();
                    foreach (string sql in sqlArr)
                    {
                        orclCommand.CommandText = sql;
                        orclCommand.Transaction = transaction;
                        ret = orclCommand.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    _connection.Close();
                }
                catch (Exception e)
                {
                    log.Error("Oracle执行出错。", e);
                    if (transaction !=null) transaction.Rollback();
                }
            }

            return ret;
        }
        /// <summary>
        /// 查询单个值
        /// </summary>
        /// <param name="sql">查询sql</param>
        /// <param name="parameterDic">参数键值对（参数名，参数值）</param>
        /// <returns>返回查询到的第一个值</returns>
        public object ExecuteScalar(string sql, Dictionary<string, object> parameterDic)
        {
            using (OracleCommand orclCommand = new OracleCommand())
            {
                _connection.Open();
                orclCommand.Connection = _connection;
                orclCommand.CommandText = sql;
                orclCommand.Parameters.AddRange(GetParameters(parameterDic));

                return orclCommand.ExecuteScalar();
            }
        }

        private OracleParameter[] GetParameters(Dictionary<string, object> parameterDic)
        {
            List<OracleParameter> paramList = new List<OracleParameter>();
            if (parameterDic != null)
            {
                foreach (KeyValuePair<string, object> keyValue in parameterDic)
                {
                    OracleParameter oraParameter = new OracleParameter(keyValue.Key, keyValue.Value); ;
                    if (keyValue.Value is DateTime)
                    {
                        oraParameter = new OracleParameter(keyValue.Key, OracleType.DateTime);
                        oraParameter.Value = keyValue.Value;
                    }
                    else if (keyValue.Value is int)
                    {
                        oraParameter = new OracleParameter(keyValue.Key, OracleType.Int32);
                        oraParameter.Value = keyValue.Value;
                    }
                    else if (keyValue.Value is float)
                    {
                        oraParameter = new OracleParameter(keyValue.Key, OracleType.Float);
                        oraParameter.Value = keyValue.Value;
                    }
                    else if (keyValue.Value is Double)
                    {
                        oraParameter = new OracleParameter(keyValue.Key, OracleType.Double);
                        oraParameter.Value = keyValue.Value;
                    }
                    else
                    {
                        oraParameter = new OracleParameter(keyValue.Key, keyValue.Value);
                    }

                    paramList.Add(oraParameter);
                }
            }

            return paramList.ToArray();
        }
        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="sql">查询sql，可以带占位符</param>
        /// <param name="parameterDic">参数键值对（参数名，参数值）</param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(string sql, Dictionary<string, object> parameterDic)
        {
            DataTable dt = new DataTable();
            try
            {
                using (OracleCommand orclCommand = new OracleCommand())
                {
                    //_connection.Open();
                    orclCommand.Connection = _connection;
                    orclCommand.CommandText = sql;
                    orclCommand.Parameters.AddRange(GetParameters(parameterDic));

                    OracleDataAdapter oracleDataAdapter = new OracleDataAdapter(orclCommand);
                    oracleDataAdapter.Fill(dt);
                }
            }
            catch (Exception e)
            {
                log.Error("Oracle查询出错。", e);
            }
            return dt;
        }
        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="sql">查询sql，可以带占位符</param>
        /// <param name="parameterDic">参数键值对（参数名，参数值）</param>
        /// <returns>DataSet</returns>
        public DataSet GetDataSet(string sql, Dictionary<string, object> parameterDic)
        {
            DataSet ds = new DataSet();
            try
            {
                using (OracleCommand orclCommand = new OracleCommand())
                {
                    _connection.Open();
                    orclCommand.Connection = _connection;
                    orclCommand.CommandText = sql;
                    orclCommand.Parameters.AddRange(GetParameters(parameterDic));

                    OracleDataAdapter oracleDataAdapter = new OracleDataAdapter(orclCommand);
                    oracleDataAdapter.Fill(ds);
                }
            }
            catch (Exception e)
            {
                log.Error("Oracle查询出错。", e);
            }
            return ds;
        }


        public IDataReader GetDataReader(string sql, Dictionary<string, object> parameterDic)
        {
            IDataReader reader = null;
            try
            {
                using (OracleCommand orclCommand = new OracleCommand())
                {
                    _connection.Open();
                    orclCommand.Connection = _connection;
                    orclCommand.CommandText = sql;
                    orclCommand.Parameters.AddRange(GetParameters(parameterDic));
                    reader = orclCommand.ExecuteReader();

                    return reader;
                }
            }
            catch (Exception e)
            {
                log.Error("Oracle查询出错。", e);
            }
            return null;
        }

        #region IDisposable Members

        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
            GC.SuppressFinalize(_connection);
        }

        #endregion
    }
}
