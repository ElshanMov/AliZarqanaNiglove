using Core.Entities;
using Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> CreateAsync(CreateUser model);
        Task<List<AppUser>> GetAll();
        Task UpdatePasswordAsync(string userId, string newPassword);
        Task AssignRoleToUserAsnyc(string userId, string[] roles);
        Task<string[]> GetRolesToUserAsync(string userIdOrName);
    }
}
