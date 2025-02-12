using DevSpot.Models;
using DevSpot.Repositories;
using DevSpot.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace DevSpot.Controllers
{
    public class JobPostingsController : Controller
    {

        private readonly IRepository<JobPosting> _repository;
        private readonly UserManager<IdentityUser> _userManager;
        public JobPostingsController(IRepository<JobPosting> repository, UserManager<IdentityUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _repository.GetAllAsync();


            return View(result);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(JobPostingViewModel jobPostingVm)
        {
            if (ModelState.IsValid)
            {
               // jobPosting.UserId = _userManager.GetUserId(User);
               // await _repository.AddAysnc(jobPosting);
            }
         return   RedirectToAction(nameof(Index));
        }
    }
}
