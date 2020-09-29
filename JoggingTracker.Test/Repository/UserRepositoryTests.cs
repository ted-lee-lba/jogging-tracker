using Microsoft.VisualStudio.TestTools.UnitTesting;
using JoggingTracker.Db.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoggingTracker.Db.Repository.Tests {
    [TestClass()]
    public class UserRepositoryTests {
        private IUserRepository _userRepo;
        private IUnitOfWork _unitOfWork;
        private JT_DBEntities _dbContext;
        public UserRepositoryTests() {
            _dbContext = new JT_DBEntities();
            _unitOfWork = new UnitOfWork();
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Null_Parameter_Constructor() {
            _userRepo = new UserRepository(null);
        }

        [TestMethod()]
        public void Get_User_By_Id() {
            _unitOfWork = new UnitOfWork();
            var _user = _unitOfWork.UserRepository.Get(1);
            Assert.IsTrue(_user != null);
        }

        [TestMethod()]
        public void Get_User_By_User_Name() {
            _unitOfWork = new UnitOfWork();
            var _user = _unitOfWork.UserRepository.getByUserName("ADMIN");
            Assert.IsTrue(_user != null);
        }

        [TestMethod()]
        public void Should_Get_Only_User_With_ADMINISTRATOR_ROLE() {
            _unitOfWork = new UnitOfWork();
            var _users = _unitOfWork.UserRepository.getAllUser(new string[] { "USER", "MANAGER" });
            Assert.IsFalse(_users.Where(c => c.UserRole.RolesName != "ADMINISTRATOR").Any());
        }

        [TestMethod()]
        public void Should_Get_Only_User_With_MANAGER_ROLE() {
            _unitOfWork = new UnitOfWork();
            var _users = _unitOfWork.UserRepository.getAllUser(new string[] { "USER", "ADMINISTRATOR" });
            Assert.IsFalse(_users.Where(c => c.UserRole.RolesName != "MANAGER").Any());
        }

        [TestMethod()]
        public void Should_Get_Only_User_With_USER_ROLE() {
            _unitOfWork = new UnitOfWork();
            var _users = _unitOfWork.UserRepository.getAllUser(new string[] { "MANAGER", "ADMINISTRATOR" });
            Assert.IsFalse(_users.Where(c => c.UserRole.RolesName != "USER").Any());
        }

        [TestMethod()]
        public void Should_Able_To_Add_Dummy_User() {
            var _user = CreateDummyUser();
            _unitOfWork.UserRepository.Save(_user);
            _unitOfWork.SaveChanges();
            Assert.IsTrue(true);
        }

        [TestMethod()]
        [ExpectedException(typeof(System.Data.Entity.Infrastructure.DbUpdateException))]
        public void Should_Not_Able_To_Delete() {
            _unitOfWork = new UnitOfWork();
            var _user = _unitOfWork.UserRepository.Get(1);
            _unitOfWork.UserRepository.Delete(_user);
            _unitOfWork.SaveChanges();
        }

        [TestMethod()]
        public void Should_Able_To_Delete() {
            var _user = _unitOfWork.UserRepository.getByUserName("DUMMY.002");
            _unitOfWork.UserRepository.Delete(_user);
            _unitOfWork.SaveChanges();
            Assert.IsTrue(true);
        }

        private User CreateDummyUser() {
            var _user = _unitOfWork.UserRepository.Get(1);
            
            var _newUser = new User {
                UserName = "DUMMY.002",
                FirstName = _user.FirstName,
                LastName = _user.LastName,
                Email = _user.Email,
                IsActive = _user.IsActive,
                UserRoles_Id = _user.UserRoles_Id,
                CreatedBy = _user.CreatedBy,
                CreatedDate = _user.CreatedDate,
                UpdatedBy = _user.UpdatedBy,
                UpdatedDate = _user.UpdatedDate
            };
            return _newUser;
        }
    }
}