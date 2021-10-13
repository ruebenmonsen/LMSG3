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

        private ILiteratureRepository literatureRepository;
        private ICourseRepository courseRepository;
        private IRepository<Module> moduleRepository;
        private IRepository<Activity> activityRepository;
        private ILiteratureAuthorRepository literatureAuthorRepository;

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
        public ILiteratureAuthorRepository LiteratureAuthorRepository
        {
            get
            {
                if (this.LiteratureAuthorRepository == null)
                    this.literatureAuthorRepository = new LiteratureAuthorRepository(context, logger);

                return literatureAuthorRepository;
            }
        }
        public ICourseRepository CourseRepository
        {
            get
            {
                if (this.CourseRepository == null)
                    this.courseRepository = new CourseRepository(context, logger);

                return courseRepository;
            }
        }
        public IRepository<Module> ModuleRepository
        {
            get
            {
                if (this.ModuleRepository == null)
                    this.moduleRepository = new ModuleRepository(context, logger);

                return moduleRepository;
            }
        }
        public IRepository<Activity> ActivityRepository
        {
            get
            {
                if (this.ActivityRepository == null)
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
    }
}
