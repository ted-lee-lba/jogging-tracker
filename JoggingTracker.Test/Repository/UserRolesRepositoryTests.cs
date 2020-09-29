using Microsoft.VisualStudio.TestTools.UnitTesting;
using JoggingTracker.Db.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoggingTracker.Db.Repository.Tests {
    [TestClass()]
    public class UserRolesRepositoryTests {
        private IUserRolesRepository _userRolesRepo;
        private IUnitOfWork _unitOfWork;
        private JT_DBEntities _dbContext;

        public UserRolesRepositoryTests() {
            _dbContext = new JT_DBEntities();
            _unitOfWork = new UnitOfWork();
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Null_Parameter_Constructor() {
            _userRolesRepo = new UserRolesRepository(null);
        }

        [TestMethod()]
        public void Should_Get_Only_ADMINISTRATOR_ROLE() {
            var _roles = _unitOfWork.UserRolesRepository.getAllRoles(new string[] { "USER", "MANAGER" });
            Assert.IsFalse(_roles.Where(c => c.RolesName != "ADMINISTRATOR").Any());
        }

        [TestMethod()]
        public void Should_Get_Only_MANAGER_ROLE() {
            var _users = _unitOfWork.UserRepository.getAllUser(new string[] { "USER", "ADMINISTRATOR" });
            Assert.IsFalse(_users.Where(c => c.UserRole.RolesName != "MANAGER").Any());
        }

        [TestMethod()]
        public void Should_Get_Only_USER_ROLE() {
            var _users = _unitOfWork.UserRepository.getAllUser(new string[] { "MANAGER", "ADMINISTRATOR" });
            Assert.IsFalse(_users.Where(c => c.UserRole.RolesName != "USER").Any());
        }
    }
}