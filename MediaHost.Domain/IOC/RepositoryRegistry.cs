using MediaHost.Domain.Repository;
using StructureMap.Configuration.DSL;

namespace MediaHost.Domain.IOC
{
    public class RepositoryRegistry : Registry
    {
        public RepositoryRegistry()
        {
            For<IDbRepository>().Use<Repository.Dapper.DbRepository>();    
        }
    }
}