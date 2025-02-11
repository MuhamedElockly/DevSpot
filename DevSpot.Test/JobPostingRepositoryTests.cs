using DevSpot.Data;
using DevSpot.Models;
using DevSpot.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DevSpot.Test
{
    public class JobPostingRepositoryTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public JobPostingRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase("JobPostingDb")
                    .Options;

        }
        private ApplicationDbContext CreateDbContext() => new ApplicationDbContext(_options);

        [Fact]
        public async Task AddAsync_ShouldAddPosting()
        {
            var db = CreateDbContext();
            var repository = new JobPostingRepository(db);
            var jobPosting = new JobPosting
            {
                Title = "Test Title",
                Description = "Test Desc",
                Company = "Test Company",
                PostedDate = DateTime.Now,
                Location = "Test Location",
                UserId = "Test UserId"

            };
            repository.AddAysnc(jobPosting);

            var result = db.JobPostings.SingleOrDefault(x => x.Title == "Test Title");
            Assert.NotNull(result);
            Assert.Equal(result.Title, "Test Title");
        }

        [Fact]
        public async Task GetById_ShouldReturnJopPosting()
        {
            var db = CreateDbContext();
            var repository = new JobPostingRepository(db);
            var jobPosting = new JobPosting
            {
                Title = "Test Title",
                Description = "Test Desc",
                Company = "Test Company",
                PostedDate = DateTime.Now,
                Location = "Test Location",
                UserId = "Test UserId"
            };
            await db.JobPostings.AddAsync(jobPosting);
            await db.SaveChangesAsync();


            var result = await repository.GetByIdAsync(jobPosting.Id);
            Assert.Equal("Test Title", result.Title);
        }
        [Fact]
        public async Task GetByIdAsync_ShouldThorwNotFoundKeyException()
        {
            var db = CreateDbContext();
            var repository = new JobPostingRepository(db);
            await Assert.ThrowsAnyAsync<KeyNotFoundException>(() => repository.GetByIdAsync(998));
        }
        [Fact]
        public async Task GetAllAsync_ShouldGetAllJobPosting()
        {
            var db = CreateDbContext();
            var repository = new JobPostingRepository(db);
            var jobPosting1 = new JobPosting
            {
                Title = "Test1 Title",
                Description = "Test1 Desc",
                Company = "Test1 Company",
                PostedDate = DateTime.Now,
                Location = "Test1 Location",
                UserId = "Test1 UserId"
            };
            var jobPosting2 = new JobPosting
            {
                Title = "Test2 Title",
                Description = "Test2 Desc",
                Company = "Test2 Company",
                PostedDate = DateTime.Now,
                Location = "Test2 Location",
                UserId = "Test2 UserId"
            };
            await db.JobPostings.AddRangeAsync(jobPosting1, jobPosting2);
            await db.SaveChangesAsync();
            var result = await repository.GetAllAsync();
            Assert.NotNull(result);
            Assert.True(result.Count() >= 2);
        }
        [Fact]
        public async Task Update_ShouldUpadateJopPosting()
        {
            var db = CreateDbContext();
            var repository = new JobPostingRepository(db);
            var jobPosting = new JobPosting
            {
                Title = "Test Title",
                Description = "Test Desc",
                Company = "Test Company",
                PostedDate = DateTime.Now,
                Location = "Test Location",
                UserId = "Test UserId"
            };
            await repository.AddAysnc(jobPosting);
            await db.SaveChangesAsync();
            jobPosting.Title = "Updated Titile";
            db.JobPostings.Update(jobPosting);
            var result = await repository.GetByIdAsync(jobPosting.Id);
            Assert.NotNull(result);
            Assert.Equal(result.Title, "Updated Titile");

        }
        [Fact]
        public async Task Delete_ShouldDeleteJobPosting()
        {
            var db = CreateDbContext();
            var repository = new JobPostingRepository(db);
            var jobPosting = new JobPosting
            {
                Title = "Test Title",
                Description = "Test Desc",
                Company = "Test Company",
                PostedDate = DateTime.Now,
                Location = "Test Location",
                UserId = "Test UserId"
            };
            await db.JobPostings.AddAsync(jobPosting);
            await db.SaveChangesAsync();
            await repository.DeleteAsync(jobPosting.Id);
            var result = db.JobPostings.Find(jobPosting.Id);
            Assert.Null(result);

        }
    }
}