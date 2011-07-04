using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaHost.Domain.Models;

namespace MediaHost.Domain.Repository.Dapper
{
    public class DbRepository : IDbRepository
    {
        public Entity InsertEntity(Entity entity)
        {
            throw new NotImplementedException();
        }

        public Group InsertGroup(Group group)
        {
            throw new NotImplementedException();
        }

        public Playlist InsertPlaylist(Playlist playlist)
        {
            throw new NotImplementedException();
        }

        public MediaFile InsertFile(MediaFile file)
        {
            throw new NotImplementedException();
        }
    }
}
