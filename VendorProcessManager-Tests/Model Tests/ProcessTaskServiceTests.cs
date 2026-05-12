using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using VendorProcessManagerV1.Data;
using VendorProcessManagerV1.Models;
using VendorProcessManagerV1.Services;

namespace VendorProcessManager_Tests.Model_Tests
{
    public class ProcessTaskServiceTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly ProcessTaskService _service;
        private readonly UserManager<AppUser> _userManager;

        public ProcessTaskServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            //_userManager = userManager;
            _service = new ProcessTaskService(_context, _userManager);
        }

        public void Dispose()
        {
            _context.Dispose();            
        }

        private ProcessInstance MakeInstance(Action<ProcessInstance>? configure = null)
        {
            var instance = new ProcessInstance
            {
                Id = Guid.NewGuid(),
                Status = ProcessInstanceStatus.InProgress,
                InstanceName = "Test Instance",
                InitiatedById = "test-user-id",
                ProcessTemplateId = Guid.NewGuid(),
                VendorCandidateId = Guid.NewGuid(),
                CreatedDate = DateTime.Now
            };

            configure?.Invoke(instance);
            return instance; 
        }

        private ProcessTask MakeTask(
            Guid instanceId, 
            int sortOrder=1, 
            Action<ProcessTask>? configure = null)
        {
            var task = new ProcessTask
            {
                Id                      = Guid.NewGuid(),
                ProcessInstanceId       = instanceId,
                ProcessTemplateTaskId   = Guid.NewGuid(),
                SortOrder               = sortOrder,
                ProcessTaskStatus       = ProcessTaskStatus.NotStarted,
                IsActive                = true,
                Title                   = "Test task",
                CreatorId               = "test-user-id",
                OwnerId                 = "test-user-id"                
            };

            configure?.Invoke(task);
            return task; 
        }

        private AppUser MakeUser (string team = "Legal", 
            Action<AppUser>? configure = null)
        {
            var user = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "test@voltron.com",
                NormalizedUserName = "TEST@VOLTRON.COM",
                Email = "test@voltron.com",
                NormalizedEmail = "TEST@VOLTRON.COM",
                FirstName = "Test",
                LastName = "User",
                Team = team, 
                EmailConfirmed = true, 
                SecurityStamp = Guid.NewGuid().ToString(), 
                ConcurrencyStamp = Guid.NewGuid().ToString(), 
                CreatedDate = DateTime.Now, 
                UpdatedDate = DateTime.Now
            };

            configure?.Invoke(user);
            return user; 
        }


        [Fact]
        public async Task CanStartTask_WhenFirstTask_AlwaysReturnsTrue()
        {   //Arrange
            var instance = MakeInstance();
            var task = MakeTask(instance.Id, sortOrder: 1); 
                        
            _context.ProcessInstances.Add(instance); 
            _context.ProcessTasks.Add(task); 
            await _context.SaveChangesAsync();
            
            //Act
            var result = await _service.CanStartTaskAsync(task.Id);

            //Assert
            Xunit.Assert.True(result); 
        }

        [Fact]
        public async Task CanStartTask_WhenPredecessorNotComplete_ReturnsFalse()
        {   //Arrange
            var instance = MakeInstance();

            var task1 = MakeTask(instance.Id, sortOrder: 1, configure: t =>
            {
                t.ProcessTaskStatus = ProcessTaskStatus.InProgress;
                t.IsActive = true;
            });

            var task2 = MakeTask(instance.Id, sortOrder: 2, configure: t =>
            {
                t.ProcessTaskStatus = ProcessTaskStatus.NotStarted;
                t.IsActive = false;
            });

            _context.ProcessInstances.Add(instance);
            _context.ProcessTasks.AddRange(task1, task2);
            await _context.SaveChangesAsync();

            //Act
            var result = await _service.CanStartTaskAsync(task2.Id);

            //Assert 
            Xunit.Assert.False(result);
        }

        [Fact]
        public async Task CanStartTask_WhenPredecessorComplete_ReturnsTrue()
        {   //arrange
            var instance = MakeInstance();

            var task1 = MakeTask(instance.Id, sortOrder: 1, configure: t =>
                t.ProcessTaskStatus = ProcessTaskStatus.Completed);
            var task2 = MakeTask(instance.Id, sortOrder: 2, configure: t =>
            {
                t.ProcessTaskStatus = ProcessTaskStatus.NotStarted;
                t.IsActive = false;
            });

            _context.ProcessInstances.Add(instance);
            _context.ProcessTasks.AddRange(task1, task2);
            await _context.SaveChangesAsync();
            //act
            var result = await _service.CanStartTaskAsync(task2.Id);
            //assert
            Xunit.Assert.True(result);
        }

        [Fact]
        public async Task CanApproveTask_WhenUserTeamDoesNotMatch_ReturnsFalse()
        {
            var user = MakeUser(team: "R&D");
            var instance = MakeInstance();

            var task = MakeTask(instance.Id, configure: t =>
            {
                t.ApprovalRequired = true;
                t.ApproveStatus = ApproveStatus.Pending;
                t.ApproverTeam = "Legal";
                t.ProcessTaskStatus = ProcessTaskStatus.InProgress;
            });

            _context.Users.Add(user);
            _context.ProcessInstances.Add(instance);
            _context.ProcessTasks.Add(task);
            await _context.SaveChangesAsync();

            //act
            var result = await _service.CanApproveTaskAsync(task.Id, user.Id);

            //Assert
            Xunit.Assert.False(result);

        }

        [Fact]
        public async Task CanApproveTask_WhenUserTeamDoesMatch_ReturnsTrue()
        {
            var user = MakeUser(team: "Legal");
            var instance = MakeInstance();

            var task = MakeTask(instance.Id, configure: t =>
            {
                t.ApprovalRequired = true;
                t.ApproveStatus = ApproveStatus.Pending;
                t.ApproverTeam = "Legal";
                t.ProcessTaskStatus = ProcessTaskStatus.InProgress;
            });

            _context.Users.Add(user);
            _context.ProcessInstances.Add(instance);
            _context.ProcessTasks.Add(task);
            await _context.SaveChangesAsync();

            //act
            var result = await _service.CanApproveTaskAsync(task.Id, user.Id);

            //Assert
            Xunit.Assert.True(result);
        }
        
        [Theory]
        [InlineData(ProcessTaskStatus.Completed)]
        [InlineData(ProcessTaskStatus.Approved)]
        [InlineData(ProcessTaskStatus.Skipped)]
        public async Task CanStartTask_WhenAllPrecessorsTerminal_ReturnsTrue(
            ProcessTaskStatus predecessorStatus)
        {
            var instance = MakeInstance();
            
            var task1 = MakeTask(instance.Id, sortOrder: 1, configure: t =>
                t.ProcessTaskStatus = predecessorStatus);

            var task2 = MakeTask(instance.Id, sortOrder: 2, configure: t =>
            {
                t.ProcessTaskStatus = ProcessTaskStatus.NotStarted;
                t.IsActive = false;
            });

            _context.ProcessInstances.Add(instance);
            _context.ProcessTasks.AddRange(task1, task2);
            await _context.SaveChangesAsync();

            var result = await _service.CanStartTaskAsync(task2.Id);

            Xunit.Assert.True(result);
        }
    }
}
