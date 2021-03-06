﻿using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
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

        public IEnumerable<T> GetAll<T>(out int recordCount, string where = "", dynamic param = null, int page = 0, int pagesize = 0, string orderBy = "", bool isOrderByAsc = true) where T : class, IActiveRecord
        {
            IEnumerable<T> retval;
            using (_conn = new MySqlConnection(ConnectionString))
            {
                _conn.Open();
                retval = _conn.GetAll<T>(out recordCount, where: where, whereParam: param as object, page: page, pagesize: pagesize, orderBy: orderBy, isOrderByAsc: isOrderByAsc);
            }

            return retval;
        }

        public T Insert<T>(T record) where T : class, IActiveRecord
        {
            using (_conn = new MySqlConnection(ConnectionString))
            {
                _conn.Open();
                long id = (long)_conn.Insert(record);

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

            var sql = @"select * from playlist where Id = @playlistid;
                       select * from mediafile where Id in (select MediaFileId from playlist_mediafile where PlaylistId = @playlistid);";

            using (_conn = new MySqlConnection(ConnectionString))
            {
                _conn.Open();
                using (var multi = _conn.QueryMultiple(sql, new { playlistid = id }))
                {
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

        public MediaFile GetMediaFile(long id)
        {
            MediaFile retval = null;

            var sql =
            @"select * from mediafile where Id = @mediafileid;";

            using (_conn = new MySqlConnection(ConnectionString))
            {
                _conn.Open();
                using (var multi = _conn.QueryMultiple(sql, new { mediafileid = id }))
                {
                    retval = multi.Read<MediaFile>().Single();
                }
            }

            return retval;
        }



        public IEnumerable<Playlist> GetPlaylists_ByPlaylistType(long entityId, int type)
        {
            IEnumerable<Playlist> retval;

            using (_conn = new MySqlConnection(ConnectionString))
            {
                _conn.Open();

                var sql = @"select * from playlist where EntityId = @id and PlaylistType=@playlistType";
                retval = _conn.Query<Playlist>(sql, new { id = entityId, playlistType = type });
            }

            return retval;
        }

        public IEnumerable<Playlist> GetPlaylists_ByName(long entityId, string name)
        {
            IEnumerable<Playlist> retval;

            using (_conn = new MySqlConnection(ConnectionString))
            {
                _conn.Open();

                var sql = string.Format(@"select * from playlist where EntityId = {0} and name like '%{1}%'", entityId, name);
                retval = _conn.Query<Playlist>(sql);//, new { id = entityId, name = name });
            }

            return retval;
        }

        public Playlist GetPlaylistFiles(long playlistId, Pager pager, string search = "")
        {
            string whereCondition = string.Empty;
            string countSql = string.Empty;
            SqlMapperExtensions.BuildPagedQuery(ref whereCondition, ref countSql, page: pager.PageIndex, pagesize: pager.PageSize, orderBy: pager.OrderBy, isOrderByAsc: pager.IsAsc);

            whereCondition = (string.IsNullOrEmpty(search) ? string.Empty : " and  mf.name like '%" + search + "%'") + whereCondition;

            Playlist retval = null;

            string sql = string.Format(@"select * from playlist where Id = @playlistid;
                       select * from mediafile mf where Id in (select MediaFileId from playlist_mediafile where PlaylistId = @playlistid) {0};", whereCondition);

            using (_conn = new MySqlConnection(ConnectionString))
            {
                _conn.Open();
                using (var multi = _conn.QueryMultiple(sql, new { playlistid = playlistId }))
                {
                    retval = multi.Read<Playlist>().Single();
                    retval.Files = multi.Read<MediaFile>().ToList();
                }
            }

            return retval;
        }

        public int GetTotalPlaylistFiles(long playlistId, string search)
        {
            int total = 0;

            string whereCondition = string.IsNullOrEmpty(search) ? "" : "";

            string sql = string.Format(@"SELECT * FROM mediafile m
                                         inner join playlist_mediafile pm on m.id=pm.mediafileid
                                         where pm.PlaylistId={0}", playlistId);

            if (!string.IsNullOrEmpty(search))
            {
                sql += string.Format(" and m.name like '%{0}%'", search);
            }

            using (_conn = new MySqlConnection(ConnectionString))
            {
                _conn.Open();
                total = _conn.Query(sql).ToList().Count();
            }

            return total;
        }
    }
}