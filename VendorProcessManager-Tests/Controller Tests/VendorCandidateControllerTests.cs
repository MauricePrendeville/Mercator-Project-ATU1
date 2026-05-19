using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using VendorProcessManagerV1.Data;
using VendorProcessManagerV1.Models;
using VendorProcessManagerV1.Services;

namespace VendorProcessManager_Tests.Controller_Tests
{
    public class VendorCandidateControllerTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly ProcessTaskService _service;
        private readonly Mock<UserManager<AppUser>> _userManagerMock;

        public VendorCandidateControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString())
               .Options;

            _context = new ApplicationDbContext(options);

            _userManagerMock = new Mock<UserManager<AppUser>>(
               Mock.Of<IUserStore<AppUser>>(),
               null, null, null, null,
               null, null, null, null);

            //_service = new ProcessTaskService(_context, _userManagerMock.Object);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
