using Microsoft.AspNetCore.Identity;
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
                Email = "test@voltron.com",
                FirstName = "Test",
                LastName = "User",
                Team = team
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
        {
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
        
        
    }
}
