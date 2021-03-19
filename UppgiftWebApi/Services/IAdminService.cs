using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharedLibrary;

namespace UppgiftWebApi.Services
{
    public interface IAdminService
    {
        Task<bool> CreateAdminAsync(Register model);

        Task<LogInResponse> LogInAsync(string email, string password);

        Task<IEnumerable<AdminResponse>> GetAdminAsync();

        bool ValidateAccessRights(RequestAdmin requestAdmin);
    }
}
