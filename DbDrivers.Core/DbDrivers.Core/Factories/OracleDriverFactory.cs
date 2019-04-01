using DbDrivers.Core.Drivers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbDrivers.Core.Factories
{
    public sealed class OracleDriverFactory : DbDriverFactory
    {
        public override DbDriver GetDbDriver(string connectionString, int timeout)
        {
            return new OracleDriver(connectionString, timeout);
        }
    }
}
