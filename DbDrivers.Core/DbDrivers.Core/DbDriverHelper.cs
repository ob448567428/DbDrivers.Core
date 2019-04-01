using DbDrivers.Core.Drivers;
using DbDrivers.Core.Factories;
using System;
using System.Reflection;

namespace DbDrivers.Core
{
    /// <summary>
    /// 数据库驱动入口
    /// </summary>
    public static class DbDriverHelper
    {
        private static string _namespace = "DbDrivers";
        private static string _namespaceFactory = "DbDrivers.Factories.";
        private static string _sqlServerFactoryStr = "SqlServerDriverFactory";
        private static string _mySqlFactoryStr = "MySqlDriverFactory";
        private static string _oracleSqlFactoryStr = "OracleDriverFactory";
        /// <summary>
        /// 获取数据库驱动
        /// </summary>
        /// <param name="typeEnum">数据库驱动类型</param>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="timeout">数据库超时时间</param>
        /// <returns></returns>
        public static DbDriver GetDbDriver(DbDriverTypeEnum typeEnum, string connectionString, int timeout)
        {
            DbDriver dbDriver = null;
            var factoryNamespace = _namespaceFactory;
            if (DbDriverTypeEnum.SqlServer == typeEnum)
            {
                factoryNamespace += _sqlServerFactoryStr;
            }
            else if (DbDriverTypeEnum.MySql == typeEnum)
            {
                factoryNamespace += _mySqlFactoryStr;
            }
            else if (DbDriverTypeEnum.Oracle == typeEnum)
            {
                factoryNamespace += _oracleSqlFactoryStr;
            }
            var dbFactory = (DbDriverFactory)Assembly.Load(_namespace).CreateInstance(factoryNamespace);
            if (dbFactory != null)
            {
                dbDriver = dbFactory.GetDbDriver(connectionString, timeout);
            }
            return dbDriver;
        }
    }
}
