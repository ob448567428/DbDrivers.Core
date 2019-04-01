using DbDrivers.Core.Drivers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DbDrivers.Core.Factories
{
    public abstract class DbDriverFactory
    {
        public abstract DbDriver GetDbDriver(string connectionString, int timeout);
    }
}
