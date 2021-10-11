using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMSG3.Core.Configuration;
using LMSG3.Core.Repositories;
using LMSG3.Data.Repositories;
using LMSG3.Core.Models.Entities;

namespace LMSG3.Data.Configuration
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger logger;

        private IGenericRepository<Course> courseRepository;
        private IGenericRepository<Module> moduleRepository;
        private IGenericRepository<Activity> activityRepository;
        private ILiteratureRepository literatureRepository;

        public UnitOfWork(ApplicationDbContext context, ILoggerFactory loggerFactory)
        {
            this.context = context;
            this.logger = loggerFactory.CreateLogger("logs");
        }
        public ILiteratureRepository LiteratureRepository
        {
            get
            {
                if (this.LiteratureRepository == null)
                    this.literatureRepository = new LiteratureRepository(context, logger);

                return literatureRepository;
            }
        }

        public GenericRepository<Course> CourseRepository
        {
            get
            {
                if (this.CourseRepository == null)
                    this.courseRepository = new GenericRepository<Course>(context);

                return courseRepository as GenericRepository<Course>;
            }
        }
        public GenericRepository<Module> ModuleRepository
        {
            get
            {
                if (this.ModuleRepository == null)
                    this.moduleRepository = new GenericRepository<Module>(context);

                return moduleRepository as GenericRepository<Module>;
            }
        }
        public GenericRepository<Activity> ActivityRepository
        {
            get
            {
                if (this.ActivityRepository == null)
                    this.activityRepository = new GenericRepository<Activity>(context);

                return activityRepository as GenericRepository<Activity>;
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
    }
}
