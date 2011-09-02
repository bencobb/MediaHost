using System.Configuration;
using System.Data;
using MediaHost.Domain.Repository;
using MediaHost.Domain.Storage;
using MySql.Data.MySqlClient;
using StructureMap.Configuration.DSL;
using StructureMap.Pipeline;
using MediaHost.Domain.Cache;

namespace MediaHost.Domain.IOC
{
    public class MainRegistry : Registry
    {
        public MainRegistry()
        {
            For<IDbRepository>().Use<Repository.Dapper.MySql.DbRepository>();
            For<IDbConnection>()
                .Use<MySqlConnection>().SetValue(typeof(string), ConfigurationManager.ConnectionStrings["MySql"].ConnectionString, CannotFindProperty.ThrowException);
            For<IStorage>().Use<StorageS3>();
        }
    }
}