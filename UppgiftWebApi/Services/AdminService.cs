using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UppgiftWebApi.Data;
using UppgiftWebApi.Entities;
using SharedLibrary;

namespace UppgiftWebApi.Services
{
    public class AdminService : IAdminService
    {
        private readonly SqlDbContext _context; 
        private IConfiguration _configuration { get; }
        public AdminService(SqlDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        public async Task<bool> CreateAdminAsync(Register model)
        {
            if(!_context.Administrators.Any(admin => admin.Email == model.Email))
            {
                try
                {
                    var admin = new Administrator()
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email
                    };

                    admin.CreatePasswordHash(model.Password);
                    _context.Administrators.Add(admin);

                    await _context.SaveChangesAsync();
                    return true;
                }
                catch { }
            }
            return false;
            
        }

        public async Task<IEnumerable<AdminResponse>> GetAdminAsync()
        {
            var admins = new List<AdminResponse>();

            foreach (var admin in await _context.Administrators.ToListAsync())
            {
                admins.Add(new AdminResponse
                {
                    FirstName = admin.FirstName,
                    LastName = admin.FirstName,
                    Email = admin.Email

                });
            }
            return admins;
        }


        public async Task<LogInResponse> LogInAsync(string email, string password)
        {
            try
            {
                var admin = await _context.Administrators.FirstOrDefaultAsync(admin => admin.Email == email);

                var token = _context.SessionTokens.FirstOrDefault(t => t.AdminId == admin.Id);
                if (token != null)
                {
                    _context.SessionTokens.Remove(token);
                    _context.SaveChanges();
                }

                if (admin != null)
                {
                    try
                    {
                        if (admin.ValidatePasswordHash(password))
                        {
                            var tokenHandler = new JwtSecurityTokenHandler();
                            var _secretKey = Encoding.UTF8.GetBytes(_configuration.GetSection("SecretKey").Value);

                            var tokenDescriptor = new SecurityTokenDescriptor
                            {
                                Subject = new ClaimsIdentity(new Claim[] { new Claim("AdminId", admin.Id.ToString()) }),
                                Expires = DateTime.Now.AddHours(1),
                                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_secretKey), SecurityAlgorithms.HmacSha512Signature)
                            };

                            var _accessToken = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

                            _context.SessionTokens.Add(new SessionToken { AdminId = admin.Id, AccessToken = _accessToken });
                            await _context.SaveChangesAsync();

                            return new LogInResponse
                            {
                                Succeeded = true,
                                Result = new LogInResponseResult
                                {
                                    Id = admin.Id,
                                    Email = admin.Email,
                                    AccessToken = _accessToken
                                }
                            };
                        }
                    }
                    catch { }

                }
            }
            catch { }
            return new LogInResponse { Succeeded = false };
        }

        public bool ValidateAccessRights(RequestAdmin requestAdmin)
        {
            if (_context.SessionTokens.Any(x => x.AdminId == requestAdmin.AdminId && x.AccessToken == requestAdmin.AccessToken))
                return true;

            return false;
        }
    }
}
