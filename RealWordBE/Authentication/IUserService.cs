﻿using RealWord.DB.Entities;
using System.Threading.Tasks;

namespace RealWordBE.Authentication
{
    public interface IUserService
    {
        Task<string> RegisterAsync(User user);
        Task<User> AuthenticateUser(string email ,string password);
        public Task<string> CreateJwtToken(User user);


    }
}