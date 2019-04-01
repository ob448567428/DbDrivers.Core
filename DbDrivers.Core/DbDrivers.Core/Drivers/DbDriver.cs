using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace DbDrivers.Core.Drivers
{
    /// <summary>
    /// 数据库执行驱动
    /// </summary>
    public abstract class DbDriver
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        private string _connectionString = "";
        /// <summary>
        /// 默认超时时间
        /// </summary>
        private int _timeout = 60;
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString
        {
            get { return _connectionString; }
        }
        /// <summary>
        /// 默认超时时间
        /// </summary>
        public int Timeout
        {
            get { return _timeout; }
        }
        /// <summary>
        /// 初始化驱动
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="timeout">超时时间</param>
        public DbDriver(string connectionString, int timeout)
        {
            this._connectionString = connectionString;
            this._timeout = timeout;
        }
        /// <summary>
        /// 参数执行准备
        /// </summary>
        /// <param name="cmd">数据库操作命令对象</param>
        /// <param name="conn">数据库链接对象</param>
        /// <param name="trans">数据库事务对象</param>
        /// <param name="cmdType">数据库命令类型（枚举）</param>
        /// <param name="cmdText">数据库命令文本</param>
        /// <param name="cmdParms">传入数据库参数</param>
        protected void PrepareCommand(DbCommand cmd, DbConnection conn, DbTransaction trans, CommandType cmdType, string cmdText, DbParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = cmdType;
            if (cmdParms != null)
            {
                foreach (DbParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }
        /// <summary>
        /// 执行数据库查询（select）sql语句
        /// </summary>
        /// <param name="cmdType">数据库命令类型（枚举）</param>
        /// <param name="cmdText">数据库命令文本</param>
        /// <param name="cmdParms">传入数据库参数</param>
        /// <returns>查询的结果集</returns>
        public abstract DataTable ExecuteTable(CommandType cmdType, string cmdText, params DbParameter[] cmdParms);
        /// <summary>
        /// 执行数据库查询（select）sql语句
        /// </summary>
        /// <param name="cmdType">数据库命令类型（枚举）</param>
        /// <param name="cmdText">数据库命令文本</param>
        /// <param name="cmdParms">传入数据库参数</param>
        /// <returns>返回查询结果集的第一行第一列</returns>
        /// </summary>
        public abstract object ExecuteScalar(CommandType cmdType, string cmdText, params DbParameter[] cmdParms);
        /// <summary>
        /// 执行数据库操作（delete, update,insert）sql语句
        /// </summary>
        /// <param name="cmdType">数据库命令类型（枚举）</param>
        /// <param name="cmdText">数据库命令文本</param>
        /// <param name="cmdParms">传入数据库参数</param>
        /// <returns>影响的行数</returns>
        public abstract int ExecuteNonQuery(CommandType cmdType, string cmdText, params DbParameter[] cmdParms);
        /// <summary>
        /// 使用事务（Transaction）执行数据库操作（delete, update,insert）sql语句
        /// </summary>
        /// <param name="cmdType">数据库命令类型（枚举）</param>
        /// <param name="cmdText">数据库命令文本</param>
        /// <param name="cmdParms">传入数据库参数</param>
        /// <returns>影响的行数</returns>
        public abstract int ExecuteNonQueryWithTransaction(CommandType cmdType, string cmdText, params DbParameter[] cmdParms);
        /// <summary>
        /// 使用事务（TransactionScope）执行数据库操作（delete, update,insert）sql语句
        /// </summary>
        /// <param name="cmdType">数据库命令类型（枚举）</param>
        /// <param name="cmdText">数据库命令文本</param>
        /// <param name="cmdParms">传入数据库参数</param>
        /// <returns>影响的行数</returns>
        /// </summary>
        public abstract int ExecuteNonQueryWithTransactionScope(CommandType cmdType, string cmdText, params DbParameter[] cmdParms);
    }
}
