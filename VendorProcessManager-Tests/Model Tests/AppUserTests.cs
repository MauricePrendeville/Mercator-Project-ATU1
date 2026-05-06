using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VendorProcessManagerV1.Data;
using VendorProcessManagerV1.Models;

namespace VendorProcessManager_Tests.Model_Tests
{
    
    public class AppUserTests : IDisposable
    {
        private readonly ApplicationDbContext _context; 

        public AppUserTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options; 

            _context = new ApplicationDbContext(options);
        }

        //Dispose to clean up after tests
        public void Dispose()
        {
            _context.Dispose();
        }

        private List<ValidationResult> ValidateModel(object model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model);
            Validator.TryValidateObject(model, context, results, true);
            return results; 
        }
        
        
        [Fact]
        public void AppUser_WithValidData_PassesValidation()
        {
            var user = new AppUser
            {
                FirstName = "Rickety",
                LastName = "Cricket",
                Email = "cricket@hobomail.com",
                UserName = "cricket@hobomail.com"
            };

            var errors = ValidateModel(user);
            Xunit.Assert.Empty(errors);

        }

        [Fact]
        public void AppUser_MissingFirstName_FailsValidation()
        {
            var user = new AppUser
            {
                FirstName = "",
                LastName = "Cricket",
                Email = "cricket@hobomail.com",
                UserName = "cricket@hobomail.com"
            }; 

            var errors = ValidateModel(user);
            Xunit.Assert.Contains(errors, e => e.MemberNames.Contains("FirstName"));
        }

    }
}
