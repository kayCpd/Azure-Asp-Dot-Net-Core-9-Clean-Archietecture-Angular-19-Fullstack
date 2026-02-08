using LSC.SmartCertify.Functions.Entities;
using LSC.SmartCertify.Functions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Text.Json;

namespace LSC.SmartCertify.Functions
{
    public class ProfileFunction
    {
        private readonly ILogger<ProfileFunction> _logger;
        private readonly IConfiguration configuration;

        public ProfileFunction(ILogger<ProfileFunction> logger, IConfiguration configuration)
        {
            _logger = logger;
            this.configuration = configuration;
        }

        [Function("UpdateProfile")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "UpdateProfile")]
        HttpRequest req, ExecutionContext context)
        {
            _logger.LogInformation("C# HTTP trigger function processed UpdateProfile request.");

            var ProfileResponse = new Profile();

            try
            {
                //Read connection string value from our local settings from project.
                string connectionString = configuration.GetConnectionString("DbContext");

                if (string.IsNullOrEmpty(connectionString))
                {
                    _logger.LogError("The connection string is null or empty.");
                    throw new InvalidOperationException("The connection string has not been initialized.");
                }

                _logger.LogInformation($"DbContext: {connectionString}");

                var optionsBuilder = new DbContextOptionsBuilder<SmartCertifyContext>();
                optionsBuilder.UseSqlServer(connectionString);

                var learnSmartDbContext = new SmartCertifyContext(optionsBuilder.Options);

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                if (string.IsNullOrEmpty(requestBody))
                {
                    return new BadRequestObjectResult("Invalid request body. Please provide a valid Profile. Body cannot be empty");
                }

                //we will parse our request body to this model
                Profile? profile = JsonSerializer.Deserialize<Profile>(requestBody);

                if (profile == null)
                {
                    return new BadRequestObjectResult("Invalid request body. Please provide a valid Profile.");
                }

                string adObjId = profile.AdObjId;

                if (string.IsNullOrEmpty(adObjId))
                {
                    return new BadRequestObjectResult("Please provide AdObjId in the request body.");
                }

                // Check if Profile with given AdObjId exists
                var existingUserProfile = await learnSmartDbContext.UserProfiles.Include(d => d.UserRoles)
                    .FirstOrDefaultAsync(u => u.AdObjId == adObjId);

                var smartApp = await learnSmartDbContext.SmartApps.FirstOrDefaultAsync(f => f.AppName == "SmartCertify");

                var role = await learnSmartDbContext.Roles.FirstOrDefaultAsync(f => f.RoleName == "Customer"); // Default Role assignment
                var adminRole = await learnSmartDbContext.Roles.FirstOrDefaultAsync(f => f.RoleName == "Admin");

                if (existingUserProfile == null)
                {
                    // If not exists, create a new Profile
                    existingUserProfile = new UserProfile
                    {
                        AdObjId = adObjId,
                        DisplayName = profile.DisplayName,
                        FirstName = profile.FirstName,
                        LastName = profile.LastName,
                        Email = profile.Email
                    };

                    //assign roles for the first time, if we need to change we can do via admin dashboard from UI to manage user's role
                    if (role != null && smartApp != null)
                    {
                        existingUserProfile.UserRoles = new List<UserRole>() {
                            new UserRole() { SmartAppId = smartApp.SmartAppId, RoleId = role.RoleId}
                        };
                        if (existingUserProfile.Email.Equals("learnsmartcoding@gmail.com"))// you can configure this from ap settings as well
                        {
                            existingUserProfile.UserRoles.Add(new UserRole()
                            {
                                SmartAppId = smartApp.SmartAppId,
                                RoleId = adminRole.RoleId
                            });
                        }
                    }

                    learnSmartDbContext.UserProfiles.Add(existingUserProfile);
                }
                else
                {
                    // If exists, update the existing Profile
                    // You can update other properties here if needed
                    existingUserProfile.DisplayName = profile.DisplayName;
                    existingUserProfile.FirstName = profile.FirstName;
                    existingUserProfile.LastName = profile.LastName;
                    existingUserProfile.Email = profile.Email;
                }

                await learnSmartDbContext.SaveChangesAsync();

                //get user's roles here
                var userRoles = await learnSmartDbContext.UserRoles.Include(i => i.Role)
                    .Where(u => u.UserId == existingUserProfile.UserId).Select(s => s.Role.RoleName).ToListAsync();

                ProfileResponse = new Profile()
                {
                    UserId = existingUserProfile.UserId,
                    AdObjId = existingUserProfile.AdObjId,
                    DisplayName = existingUserProfile.DisplayName,
                    Email = existingUserProfile.Email,
                    FirstName = existingUserProfile.FirstName,
                    LastName = existingUserProfile.LastName,
                    Roles = userRoles == null ? new List<string>() : userRoles
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }


            return new OkObjectResult(ProfileResponse);
        }
    }
}
