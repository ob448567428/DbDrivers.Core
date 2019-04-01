using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Transactions;

namespace DbDrivers.Core.Drivers
{
    public sealed class SqlServerDriver : DbDriver
    {
        public SqlServerDriver(string connectionString, int timeout) : base(connectionString, timeout)
        {

        }
        /// <summary>
        /// 执行数据库查询（select）sql语句
        /// </summary>
        /// <param name="cmdType">数据库命令类型（枚举）</param>
        /// <param name="cmdText">数据库命令文本</param>
        /// <param name="cmdParms">传入数据库参数</param>
        /// <returns>查询的结果集</returns>
        public override DataTable ExecuteTable(CommandType cmdType, string cmdText, params DbParameter[] cmdParms)
        {
            var dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandTimeout = this.Timeout;
                    PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                    using (var adapter = new SqlDataAdapter())
                    {
                        adapter.SelectCommand = cmd;
                        adapter.Fill(dt);
                        cmd.Parameters.Clear();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return dt;
        }
        /// <summary>
        /// 执行数据库操作（delete, update,insert）sql语句
        /// </summary>
        /// <param name="cmdType">数据库命令类型（枚举）</param>
        /// <param name="cmdText">数据库命令文本</param>
        /// <param name="cmdParms">传入数据库参数</param>
        /// <returns>操作数据集的第一行第一列</returns>
        /// </summary>
        public override object ExecuteScalar(CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {
            object val = null;
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandTimeout = this.Timeout;
                    PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                    val = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return val;
        }
        /// <summary>
        /// 执行数据库操作（delete, update,insert）sql语句
        /// </summary>
        /// <param name="cmdType">数据库命令类型（枚举）</param>
        /// <param name="cmdText">数据库命令文本</param>
        /// <param name="cmdParms">传入数据库参数</param>
        /// <returns>影响的行数</returns>
        public override int ExecuteNonQuery(CommandType cmdType, string cmdText, params DbParameter[] cmdParms)
        {
            int val;
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandTimeout = this.Timeout;
                    PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                    val = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return val;
        }
        /// <summary>
        /// 使用事务（Transaction）执行数据库操作（delete, update,insert）sql语句
        /// </summary>
        /// <param name="cmdType">数据库命令类型（枚举）</param>
        /// <param name="cmdText">数据库命令文本</param>
        /// <param name="cmdParms">传入数据库参数</param>
        /// <returns>影响的行数</returns>
        public override int ExecuteNonQueryWithTransaction(CommandType cmdType, string cmdText, params DbParameter[] cmdParms)
        {
            int val;
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandTimeout = this.Timeout;
                        PrepareCommand(cmd, conn, trans, cmdType, cmdText, cmdParms);
                        val = cmd.ExecuteNonQuery();
                        trans.Commit();
                        cmd.Parameters.Clear();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw ex;
                    }
                }
            }
            return val;
        }
        /// <summary>
        /// 使用事务（TransactionScope）执行数据库操作（delete, update,insert）sql语句
        /// </summary>
        /// <param name="cmdType">数据库命令类型（枚举）</param>
        /// <param name="cmdText">数据库命令文本</param>
        /// <param name="cmdParms">传入数据库参数</param>
        /// <returns>影响的行数</returns>
        /// </summary>
        public override int ExecuteNonQueryWithTransactionScope(CommandType cmdType, string cmdText, params DbParameter[] cmdParms)
        {
            using (var scope = new TransactionScope())
            {
                int val = 0;
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    try
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandTimeout = this.Timeout;
                        PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                        val = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                scope.Complete();
                return val;
            }
        }
    }
}
