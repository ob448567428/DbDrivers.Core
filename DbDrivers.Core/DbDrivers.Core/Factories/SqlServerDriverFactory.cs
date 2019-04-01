using DbDrivers.Core.Drivers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbDrivers.Core.Factories
{
    public sealed class SqlServerDriverFactory : DbDriverFactory
    {
        public override DbDriver GetDbDriver(string connectionString, int timeout)
        {
            return new SqlServerDriver(connectionString, timeout);
        }
    }
}
