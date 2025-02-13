using DevSpot.Constants;
using DevSpot.Models;
using DevSpot.Repositories;
using DevSpot.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace DevSpot.Controllers
{
    [Authorize]
    public class JobPostingsController : Controller
    {

        private readonly IRepository<JobPosting> _repository;
        private readonly UserManager<IdentityUser> _userManager;
        public JobPostingsController(IRepository<JobPosting> repository, UserManager<IdentityUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var result = await _repository.GetAllAsync();
            if (User.IsInRole(Roles.Employeer))
            {
                var userId = _userManager.GetUserId(User);
                var filterdJobPosting = result.Where(jb => jb.UserId == userId);

                return View(filterdJobPosting);
            }
            return View(result);
        }
        [Authorize(Roles = "Admin,Employeer")]
        public IActionResult Create()
        {
            return View();
        }



        [HttpPost]
        [Authorize(Roles = "Admin,Employeer")]
        public async Task<IActionResult> Create(JobPostingViewModel jobPostingVm)
        {
            if (ModelState.IsValid)
            {

                var jobPosting = new JobPosting
                {
                    Title = jobPostingVm.Title,
                    Description = jobPostingVm.Description,
                    Company = jobPostingVm.Company,
                    Location = jobPostingVm.Location,
                    UserId = _userManager.GetUserId(User)

                };
                await _repository.AddAysnc(jobPosting);
                return RedirectToAction(nameof(Index));
            }
            return View(jobPostingVm);
        }
        [HttpDelete]
        [Authorize(Roles = "Admin,Employeer")]
        public async Task<IActionResult> Delete(int id)
        {

            var jobPosting = await _repository.GetByIdAsync(id);
            if (jobPosting == null)
            {
                return NotFound();
            }
            var userId = _userManager.GetUserId(User);
            if (User.IsInRole(Roles.Admin) == false && userId != jobPosting.UserId)
            {
                return Forbid();
            }
            await _repository.DeleteAsync(id);
            return Ok();
        }
    }
}
