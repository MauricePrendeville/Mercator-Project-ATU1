using Microsoft.AspNetCore.Mvc.Rendering;
using VendorProcessManagerV1.Models;

namespace VendorProcessManagerV1.ViewModels
{
    public class VendorCandidateIndexViewModel
    {
        public List<VendorCandidate> VendorCandidates { get; set; } = new();
        public string? CurrentSort { get; set; }
        public string VendorNameSort { get; set; }
        public string CategorySort { get; set; }
        public string DateSort { get; set; }
        public string OwnerSort { get; set; }
        public string TermsSort { get; set; }
    }
}
