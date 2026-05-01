using Microsoft.EntityFrameworkCore;
using VendorProcessManagerV1.Data;
using VendorProcessManagerV1.Models;


namespace VendorProcessManagerV1.Services
{
    public interface IProcessInstanceService
    {
        Task<ProcessInstance> StartInstanceAsync(
            Guid templateId,
            Guid vendorCandidateId,
            string createdById);
    }
}
