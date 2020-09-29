using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using JoggingTracker.Models;
using JoggingTracker.Db;
using JoggingTracker.Db.Repository;
using StructureMap;
using AutoMapper;
using AutoMapper.Configuration;

namespace JoggingTracker
{
    public class SimpleUserStore<T> : IUserStore<T, string> where T : ApplicationUser {
        private readonly IUserRepository _userRepository;

        public SimpleUserStore(IUserRepository userRepository) {
            this._userRepository = userRepository;
        }

        public Task CreateAsync(T user) {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(T user) {
            throw new NotImplementedException();
        }

        public void Dispose() {

        }

        public Task<T> FindByIdAsync(string userId) {
            var task = Task<T>.Run(() => {
                var user = this._userRepository.getById(Convert.ToInt32(userId));
                return Map(user);
            });

            return task;
        }

        public Task<T> FindByNameAsync(string userName) {
            var task = Task<T>.Run(() => {
                var user = this._userRepository.getByUserName(userName);
                return Map(user);
            });

            return task;
        }

        public Task UpdateAsync(T user) {
            throw new NotImplementedException();
        }

        private T Map(User user) {
            var applicationUser = Mapper.Map<T>(user);
            applicationUser.RolesName = user.UserRole.RolesName;
            return applicationUser;
        }
    }

    public interface ISimpleUserManager {
        Task<ApplicationUser> FindAsync(string userName, string password);
        Task SignInAsync(ApplicationUser user, bool isPersistent);
        void SignOut();
    }

    public class SimpleUserManager : UserManager<ApplicationUser, string>, ISimpleUserManager {
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationManager _authenticationManager;

        public SimpleUserManager(IUserRepository userRepository, IAuthenticationManager authenticationManager)
            : base(new SimpleUserStore<ApplicationUser>(userRepository)) {
            _userRepository = userRepository;
            _authenticationManager = authenticationManager;
        }

        public override Task<ApplicationUser> FindAsync(string UserName, string password) {
            var task = Task<User>.Run(() => {
                var user = _userRepository.getByUserName(UserName);
                if (user == null) {
                    return null;
                }

                var hashPassword = this.HashPassword(password);
                if (!hashPassword.Equals(user.Password)) {
                    return null;
                }

                Mapper.Initialize(cfg => {
                    cfg.CreateMap<User, ApplicationUser>().ForMember(dest => dest.Name, opt => opt.Ignore());
                });

                var applicationUser = Mapper.Instance.Map<ApplicationUser>(user);
                applicationUser.RolesName = user.UserRole.RolesName;

                return applicationUser;
            });

            return task;
        }

        public async Task SignInAsync(ApplicationUser user, bool isPersistent) {
            SignOut();

            var identity = await base.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            identity.AddClaim(new Claim(ClaimTypes.Role, user.RolesName));
            _authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        public void SignOut() {
            _authenticationManager.SignOut();
        }
    }

    public static class SimpleUserManagerExtensions {
        public static async Task<SignInStatus> PasswordSignInAsync(this ISimpleUserManager manager,
            string UserName, String Password, bool isPersistent) {
            if (manager == null) {
                throw new ArgumentNullException("manager");
            }

            var user = manager.FindAsync(UserName, Password).Result;
            if (user == null) {
                return SignInStatus.Failure;
            }

            await manager.SignInAsync(user, isPersistent);
            return SignInStatus.Success;
        }

        public static string HashPassword(this ISimpleUserManager manager, string Password) {
            SHA512Managed alg = new SHA512Managed();
            return Convert.ToBase64String(alg.ComputeHash(Encoding.UTF8.GetBytes(string.Format("{0}{1}", Password, ApplicationConstants.PasswordSalt))));
        }
    }
}