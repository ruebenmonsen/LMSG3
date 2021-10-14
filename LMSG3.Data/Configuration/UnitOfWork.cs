using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMSG3.Core.Configuration;



namespace LMSG3.Data.Configuration
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private ApplicationDbContext context;
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
                if (this.literatureRepository == null)
                    this.literatureRepository = new LiteratureRepository(context, logger);

        //public ILiteratureRepository LiteratureRepository { get; private set; }

        //public ILiteratureAuthorRepository LiteratureAuthorRepository { get; private set; }

        

                return moduleRepository;
            }
        }
        public IRepository<Activity> ActivityRepository
        {
            get
            {
                if (this.activityRepository == null)
                    this.activityRepository = new ActivityRepository(context, logger);

           // LiteratureRepository = new LitertureRepository(context, _logger);
            //LiteratureAuthorRepository = new LiteratureAuthorRepository(context, _logger);
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
