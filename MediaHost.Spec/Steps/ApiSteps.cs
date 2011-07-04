using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using MediaHost.Domain.Repository;
using Moq;
using NUnit.Framework;
using StructureMap;
using TechTalk.SpecFlow;
using MediaHost.Controllers;
using MediaHost.Domain.Models;
using MediaHost.Helpers;

namespace MediaHost.Spec.Steps
{
    [Binding]
    public class ApiSteps : BaseStep
    {
        private Entity _entity;
        private Group _group;
        private Playlist _playlist;
        private MediaFile _file;

        private readonly ApiController _apiController;
        private readonly Mock<IDbRepository> _mockIDbRepository;
        

        public ApiSteps()
        {
            _mockIDbRepository = new Mock<IDbRepository>();
            _apiController = new ApiController(_mockIDbRepository.Object);
            _apiController.RunValidationForTest = true;

        }

        #region Private Methods
        private void LoadObject<T>(ContentResult contentResult, ref T obj)
        {
            if (contentResult.ContentType == "application/json")
            {
                obj = contentResult.Content.FromJsonToObj<T>();
            }
            else
            {
                ErrorMessages.Clear();

                ErrorMessages.AddRange(contentResult.Content.Replace("\r\n","\n").Split('\n'));
            }
        }
        #endregion

        #region Wildcards
        [Given(@"I have an empty (.*)")]
        public void GivenIHaveAnEmptyEntity(string objectName)
        {
            switch (objectName)
            {
                case "entity": _entity = new Entity(); break;
                case "group": _group = new Group(); break;
                case "playlist": _playlist = new Playlist(); break;
                case "file": _file = new MediaFile(); break;
                default: Assert.Fail("ObjectName Not found: " + objectName); break;
            }
        }

        [Given(@"I have a valid (.*)")]
        public void GivenIHaveAValidEntity(string objectName)
        {
            switch (objectName)
            {
                case "entity": _entity = new Entity{Name = "EntityName"}; break;
                case "group": _group = new Group {Name = "GroupName", EntityId = 1}; break;
                case "playlist": _playlist = new Playlist { Name = "PlaylistName", EntityId = 1 }; break;
                default: Assert.Fail("ObjectName Not found: " + objectName); break;
            }
        }

        [Then(@"the (.*) should be added to the db and return with an Id")]
        public void ThenTheEntityShouldBeAddedToTheDbAndReturnWithAnId(string objectName)
        {
            switch (objectName)
            {
                case "entity": Assert.Greater(_entity.Id, 0); break;
                case "group": Assert.Greater(_group.Id, 0); break;
                case "playlist": Assert.Greater(_playlist.Id, 0); break;
                default: Assert.Fail("ObjectName Not found: " + objectName); break;
            }
        }
        #endregion

        #region Entity

        [Then(@"I call AddEntity")]
        public void ThenICallAddEntity()
        {
            _mockIDbRepository.Setup(m => m.InsertEntity(_entity)).Returns(new Entity{Id = 1, Name = _entity.Name});
            
            var contentResult = _apiController.AddEntity(_entity);
            
            LoadObject<Entity>(contentResult, ref _entity);
        }

        #endregion

        #region Group

        [Then(@"I call AddGroup")]
        public void ThenICallAddGroup()
        {
            _mockIDbRepository.Setup(m => m.InsertGroup(_group)).Returns(new Group { Id = 1, EntityId = _group.EntityId, Name = _group.Name });

            var contentResult = _apiController.AddGroup(_group);

            LoadObject<Group>(contentResult, ref _group);
        }

        #endregion

        #region Playlist

        [Then(@"I call AddPlaylist")]
        public void ThenICallAddPlaylist()
        {
            _mockIDbRepository.Setup(m => m.InsertPlaylist(_playlist)).Returns(new Playlist { Id = 1, EntityId = _playlist.EntityId, Name = _playlist.Name });

            var contentResult = _apiController.AddPlaylist(_playlist);

            LoadObject<Playlist>(contentResult, ref _playlist);
        }

        #endregion

        #region MediaFile

        [Then(@"I call AddFile")]
        public void ThenICallAddFile()
        {
            _mockIDbRepository.Setup(m => m.InsertFile(_file)).Returns(new MediaFile { Id = 1, EntityId = _file.EntityId, Name = _file.Name });

            var contentResult = _apiController.AddFile(_file, null);

            LoadObject<MediaFile>(contentResult, ref _file);
        }

        #endregion
    }
}
