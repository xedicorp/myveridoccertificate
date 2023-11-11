using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;

namespace App.Dal.AppDb
{
    public class SqlHelper : IDisposable
    {
        private readonly SqlCommand oCmd;
        private readonly int vConTimeOut = 1000;
        private readonly string _connectionString;

        public SqlHelper(string pSqlString, string connectionString)
        {
            _connectionString = connectionString + ";Connect Timeout= " + vConTimeOut + "; pooling='true'; Max Pool Size=200;";

            oCmd = new SqlCommand
            {
                CommandText = pSqlString,
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = vConTimeOut
            };
        }

        public SqlParameter this[Int32 id]
        {
            get
            {
                return oCmd.Parameters[id];
            }
        }

        public SqlParameter this[string name]
        {
            get
            {
                return oCmd.Parameters[name];
            }
        }

        public object? GetParameterValue(Int16 pId, ref string pMsg)
        {
            try
            {
                return (object)this[pId].Value;
            }
            catch (Exception ex)
            {
                pMsg = ex.Message;
                return null;
            }
        }

        public object? GetParameterValue(string pName, ref string pMsg)
        {
            try
            {
                return (object)this[pName].Value;
            }
            catch (Exception ex)
            {
                pMsg = ex.Message;
                return null;
            }
        }

        public DataTable GetDataTable(ref string pMsg)
        {
            DataTable dt = new();

            try
            {
                SqlDataAdapter da = new();
                using SqlConnection con = new(_connectionString);
                oCmd.Connection = con;
                da.SelectCommand = oCmd;
                con.Open();
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                pMsg = ex.Message;
            }

            return dt;
        }

        public DataTable GetDataTable(SqlParameter[] pParamList, ref string pMsg)
        {
            DataTable dt = new();
            try
            {
                SqlDataAdapter da = new();
                foreach (SqlParameter vParam in pParamList)
                {
                    if (vParam != null)
                        oCmd.Parameters.Add(vParam);
                    else
                    {
                        pMsg = "Parameter list must not contain null.";
                        return dt;
                    }
                }
                using SqlConnection con = new(_connectionString);
                oCmd.Connection = con;
                da.SelectCommand = oCmd;
                con.Open();
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                pMsg = ex.Message;
            }

            return dt;
        }

        public DataSet GetDataSet(ref string pMsg)
        {
            DataSet ds = new();
            try
            {
                SqlDataAdapter da = new();
                using SqlConnection con = new(_connectionString);
                oCmd.Connection = con;
                da.SelectCommand = oCmd;
                con.Open();
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                pMsg = ex.Message;
            }

            return ds;
        }

        public DataSet GetDataSet(SqlParameter[] pParamList, ref string pMsg)
        {
            DataSet ds = new();
            try
            {
                SqlDataAdapter da = new();
                foreach (SqlParameter vParam in pParamList)
                {
                    if (vParam != null)
                        oCmd.Parameters.Add(vParam);
                    else
                    {
                        pMsg = "Parameter list must not contain null.";
                        return ds;
                    }
                }

                using SqlConnection con = new(_connectionString);
                oCmd.Connection = con;
                da.SelectCommand = oCmd;
                con.Open();
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                pMsg = ex.Message;
            }

            return ds;
        }

        public object ExecuteScaler(SqlParameter[] pParamList, ref string pMsg)
        {
            object oRet = new();
            try
            {
                foreach (SqlParameter vParam in pParamList)
                {
                    if (vParam != null)
                        oCmd.Parameters.Add(vParam);
                    else
                    {
                        pMsg = "Parameter list must not contain null.";
                        return oRet;
                    }
                }
                using SqlConnection con = new(_connectionString);
                oCmd.Connection = con;
                con.Open();
                oRet = oCmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                pMsg = ex.Message;
            }
            return oRet;
        }

        public void ExecuteNonQuery(SqlParameter[] pParamList, ref string pMsg)
        {
            try
            {
                foreach (SqlParameter vParam in pParamList)
                {
                    if (vParam != null)
                        oCmd.Parameters.Add(vParam);
                    else
                    {
                        pMsg = "Parameter list must not contain null.";
                        return;
                    }
                }
                using SqlConnection con = new(_connectionString);
                oCmd.Connection = con;
                con.Open();
                oCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                pMsg = ex.Message;
            }
        }

        public void Dispose() => oCmd.Dispose();
    }
}
