using DevSpot.Data;
using DevSpot.Models;
using Microsoft.EntityFrameworkCore;

namespace DevSpot.Repositories
{
    public class JobPostingRepository : IRepository<JobPosting>
    {
        private readonly ApplicationDbContext _context;
        public JobPostingRepository(ApplicationDbContext dbContext)
        {

            _context = dbContext;
        }

        public async Task AddAysnc(JobPosting jopPosting)
        {

            await _context.JobPostings.AddAsync(jopPosting);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {

            var jobPosting = await _context.JobPostings.FindAsync(id);

            if (jobPosting == null)
            {
                throw new KeyNotFoundException();
            }
            _context.JobPostings.Remove(jobPosting);
                await _context.SaveChangesAsync();
            
        }

        public async Task<IEnumerable<JobPosting>> GetAllAsync()
        {
            return await _context.JobPostings.ToListAsync();

        }



        public async Task<JobPosting> GetByIdAsync(int id)
        {
            var jobPosting = await _context.JobPostings.FindAsync(id);

            if (jobPosting == null)
            {
                throw new KeyNotFoundException();
            }
            return jobPosting;
        }

        public async Task UpdateAsync(JobPosting entity)
        {
             _context.JobPostings.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
