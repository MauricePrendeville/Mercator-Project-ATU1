using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using VendorProcessManagerV1.Models;
using VendorProcessManagerV1.DTO;

namespace VendorProcessManagerV1.ViewModels
{
    public class DetailsProcessInstanceViewModel
    {
        public ProcessInstance ProcessInstance { get; set; }
        public List<GanttDTO> GanttTasks { get; set; }
    }
}
