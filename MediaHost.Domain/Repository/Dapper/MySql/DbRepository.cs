using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Transactions;

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

        public IEnumerable<T> GetAll<T>() where T : class, IActiveRecord
        {
            IEnumerable<T> retval;
            using (_conn = new MySqlConnection(ConnectionString))
            {
                _conn.Open();
                retval = _conn.GetAll<T>();
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

        public bool Remove<T>(T obj) where T : class, IActiveRecord
        {
            bool retval = false;

            using (_conn = new MySqlConnection(ConnectionString))
            {
                _conn.Open();
                retval = _conn.Delete(obj);
            }

            return retval;
        }

        public Playlist GetPlaylist(long id)
        {
            Playlist retval = null;

            var sql = 
            @"select * from playlist where Id = @playlistid
            select * from mediafile where Id in (select MediaFileId from playlist_mediafile where PlaylistId = @playlistid)";

            using (_conn = new MySqlConnection(ConnectionString))
            {
                using (var multi = _conn.QueryMultiple(sql, new { playlistid = id }))
                {
                    _conn.Open();

                    retval = multi.Read<Playlist>().Single();
                    retval.Files = multi.Read<MediaFile>().ToList();
                }
            }

            return retval;
        }

        public IEnumerable<Playlist> GetPlaylists_ByEntity(long entityId, bool includeFiles)
        {
            IEnumerable<Playlist> retval;

            using (_conn = new MySqlConnection(ConnectionString))
            {
                _conn.Open();

                

                if (includeFiles)
                {
                    var sql = @"select * from playlist where EntityId = @id ;
                             select a.*, b.PlaylistId from mediafile a join playlist_mediafile b on a.Id = b.MediaFileId where a.EntityId = @id ;";

                    retval = _conn.QueryMultiple(sql, new { id = entityId }).Map<Playlist, MediaFile, long>(
                        a => a.Id, b => b.PlaylistId, (c, bees) => c.Files = bees);
                }
                else
                {
                    var sql = @"select * from playlist where EntityId = @id";
                    retval = _conn.Query<Playlist>(sql, new { id = entityId });
                }

                
            }

            return retval; 
        }
    }
}
