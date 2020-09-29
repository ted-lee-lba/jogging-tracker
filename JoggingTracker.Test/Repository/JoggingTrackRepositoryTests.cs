using Microsoft.VisualStudio.TestTools.UnitTesting;
using JoggingTracker.Db.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoggingTracker.Db.Repository.Tests {
    [TestClass()]
    public class JoggingTrackRepositoryTests {
        private IJoggingTrackRepository _joggingTrackRepo;
        private JT_DBEntities _dbContext;
        public JoggingTrackRepositoryTests() {
            _dbContext = new JT_DBEntities();
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Null_Parameter_Constructor() {
            _joggingTrackRepo = new JoggingTrackRepository(null);
        }

        [TestMethod()]
        public void Should_Not_Throw_Exception() {
            _joggingTrackRepo = new JoggingTrackRepository(_dbContext);
            var _tracks = _joggingTrackRepo.getAll();
            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void Should_Get_Only_User_Track_With_ADMINISTRATOR_ROLE() {
            _joggingTrackRepo = new JoggingTrackRepository(_dbContext);
            var _tracks = _joggingTrackRepo.getAll(new string[] { "USER", "MANAGER" });
            Assert.IsFalse(_tracks.Where(c=>c.User.UserRole.RolesName != "ADMINISTRATOR").Any());
        }

        [TestMethod()]
        public void Should_Get_Only_User_Track_With_MANAGER_ROLE() {
            _joggingTrackRepo = new JoggingTrackRepository(_dbContext);
            var _tracks = _joggingTrackRepo.getAll(new string[] { "USER", "ADMINISTRATOR" });
            Assert.IsFalse(_tracks.Where(c => c.User.UserRole.RolesName != "MANAGER").Any());
        }

        [TestMethod()]
        public void Should_Get_Only_User_Track_With_USER_ROLE() {
            _joggingTrackRepo = new JoggingTrackRepository(_dbContext);
            var _tracks = _joggingTrackRepo.getAll(new string[] { "MANAGER", "ADMINISTRATOR" });
            Assert.IsFalse(_tracks.Where(c => c.User.UserRole.RolesName != "USER").Any());
        }

        [TestMethod()]
        public void Get_Track_By_User_Id() {
            _joggingTrackRepo = new JoggingTrackRepository(_dbContext);
            var _tracks = _joggingTrackRepo.getByUserId(1);
            Assert.IsTrue(_tracks != null);
        }

        [TestMethod()]
        public void Get_Track_By_Track_Id() {
            _joggingTrackRepo = new JoggingTrackRepository(_dbContext);
            var _tracks = _joggingTrackRepo.Get(11);
            Assert.IsTrue(_tracks != null);
        }

        [TestMethod()]
        public void Get_Track_By_Filter() {
            var fromDateTime = new DateTime(DateTime.Now.Year, 1, 1);
            _joggingTrackRepo = new JoggingTrackRepository(_dbContext);
            var _tracks = _joggingTrackRepo.getByFilter(1, fromDateTime, fromDateTime.AddYears(1).AddDays(-1));
            Assert.IsTrue(_tracks.Count() != 0);
        }
    }
}