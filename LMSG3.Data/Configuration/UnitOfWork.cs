using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMSG3.Core.Configuration;
using LMSG3.Core.Repositories;
using LMSG3.Core.Models.Entities;
using LMSG3.Data.Repositories;
using Microsoft.AspNetCore.Identity;

namespace LMSG3.Data.Configuration
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger logger;

        public IStudentRepository StudentRepository { get; private set; }

        private ICourseRepository courseRepository;
        private IRepository<Module> moduleRepository;
        private IRepository<Activity> activityRepository;
       
        public UnitOfWork(ApplicationDbContext context, ILoggerFactory loggerFactory)
        {
            this.context = context;
            this.logger = loggerFactory.CreateLogger("logs");
            StudentRepository = new StudentRepository(context);
        }
        public ICourseRepository CourseRepository
        {
            get
            {
                if (this.courseRepository == null)
                    this.courseRepository = new CourseRepository(context, logger);

                return courseRepository;
            }
        }
        public IRepository<Module> ModuleRepository
        {
            get
            {
                if (this.moduleRepository == null)
                    this.moduleRepository = new ModuleRepository(context, logger);

                return moduleRepository;
            }
        }
        public IRepository<Activity> ActivityRepository
        {
            get
            {
                if (this.activityRepository == null)
                    this.activityRepository = new ActivityRepository(context, logger);

                return activityRepository;
            }
        }

       
        public async Task CompleteAsync()
        {
            await context.SaveChangesAsync();
        }
        public void Dispose()
        {
            context.Dispose();
            GC.SuppressFinalize(this);
        }
        public async Task<bool> CompleteAsyncCheck()
        {
            return (await context.SaveChangesAsync()) >= 0;
        }
    }
}
