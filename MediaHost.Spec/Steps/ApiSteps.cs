using System.Web;
using System.Web.Mvc;
using MediaHost.Domain.Repository;
using MediaHost.Domain.Repository.Dapper.MySql;
using MediaHost.Domain.Storage;
using Moq;
using MySql.Data.MySqlClient;
using NUnit.Framework;
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
        private HttpPostedFileBase _postedFile;

        private readonly Mock<IDbRepository> _mockIDbRepository;
        private readonly Mock<HttpPostedFileBase> _mockHttpPost;
        private readonly Mock<IStorage> _mockStorage;


        public ApiSteps()
        {
            _mockIDbRepository = new Mock<IDbRepository>();
            
            _mockHttpPost = new Mock<HttpPostedFileBase>();
            _mockHttpPost.SetupGet(s => s.ContentLength).Returns(1);

            _mockStorage = new Mock<IStorage>();
            _mockStorage.Setup(s => s.StoreFile(_mockHttpPost.Object.InputStream)).Returns("/something/file.vid");

            _apiController = new ApiController(_mockIDbRepository.Object, _mockStorage.Object);
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
                case "file": _file = new MediaFile();
                             _postedFile = null;
                             break;
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
                case "file": _file = new MediaFile { Name = "FileName", EntityId = 1, RelativeFilePath = "/something/file.vid"};
                            _postedFile = _mockHttpPost.Object;
                            break;
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
                case "file": Assert.Greater(_file.Id, 0); break;
                default: Assert.Fail("ObjectName Not found: " + objectName); break;
            }
        }
        #endregion

        #region Entity

        [Then(@"I call AddEntity")]
        public void ThenICallAddEntity()
        {
            _mockIDbRepository.Setup(m => m.Insert(_entity)).Returns(new Entity { Id = 1, Name = _entity.Name });
            var contentResult = _apiController.AddEntity(_entity);

            LoadObject<Entity>(contentResult, ref _entity);
        }

        #endregion

        #region Group

        [Then(@"I call AddGroup")]
        public void ThenICallAddGroup()
        {
            _mockIDbRepository.Setup(m => m.Insert(_group)).Returns(new Group { Id = 1, EntityId = _group.EntityId, Name = _group.Name });

            var contentResult = _apiController.AddGroup(_group);

            LoadObject<Group>(contentResult, ref _group);
        }

        #endregion

        #region Playlist

        [Then(@"I call AddPlaylist")]
        public void ThenICallAddPlaylist()
        {
            _mockIDbRepository.Setup(m => m.Insert(_playlist)).Returns(new Playlist { Id = 1, EntityId = _playlist.EntityId, Name = _playlist.Name });

            var contentResult = _apiController.AddPlaylist(_playlist);

            LoadObject<Playlist>(contentResult, ref _playlist);
        }

        #endregion

        #region MediaFile

        [Then(@"I call AddFile")]
        public void ThenICallAddFile()
        {
            _mockIDbRepository.Setup(m => m.Insert(_file)).Returns(new MediaFile { Id = 1, EntityId = _file.EntityId, Name = _file.Name, RelativeFilePath = "/something/file.vid" });

            var contentResult = _apiController.AddFile(_file, _postedFile);

            LoadObject<MediaFile>(contentResult, ref _file);
        }

        [Then(@"the file should be added to the IO system")]
        public void ThenTheFileShouldBeAddedToTheIOSystem()
        {
            Assert.IsNotNullOrEmpty(_file.RelativeFilePath);
        }


        #endregion

        [Given(@"I have an empty")]
        public void GivenIHaveAnEmpty()
        {
            ScenarioContext.Current.Pending();
        }


    }
}
