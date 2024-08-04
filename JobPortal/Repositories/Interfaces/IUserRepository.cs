﻿using JobPortal.Models;

namespace JobPortal.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByEmailAsync(string email); 
    }
}
