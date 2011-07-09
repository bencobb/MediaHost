using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;


using MediaHost.Domain.Models;
using MySql.Data.MySqlClient;


namespace MediaHost.Domain.Repository.Dapper.MySql
{
    public class DbRepository : IDbRepository
    {
        private IDbConnection _conn;
 
        private static string ConnectionString
        {
            get 
            { 
                return ConfigurationManager.ConnectionStrings["MySql"].ConnectionString;
            }
        }

        public DbRepository(IDbConnection conn)
        {
            _conn = conn;
        }

        public T Find<T>(long id) where T : class, IActiveRecord
        {
            T retval;
            using (_conn = new MySqlConnection(ConnectionString))
            {
                _conn.Open();
                retval = _conn.Get<T>(id);
            }

            return retval;
        }

        public T Insert<T>(T record) where T : class, IActiveRecord
        {
            using (_conn = new MySqlConnection(ConnectionString))
            {
                _conn.Open();
                long id = _conn.Insert(record);

                record.Id = id;
            }

            return record;
        }

        public bool Update<T>(T record) where T : class, IActiveRecord
        {
            bool retval = false;

            using (_conn = new MySqlConnection(ConnectionString))
            {
                _conn.Open();
                retval = _conn.Update(record);
            }

            return retval;
        }
    }
}
