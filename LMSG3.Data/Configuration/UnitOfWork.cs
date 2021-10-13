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
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        

        //public ILiteratureRepository LiteratureRepository { get; private set; }

        //public ILiteratureAuthorRepository LiteratureAuthorRepository { get; private set; }

        

        public UnitOfWork(ApplicationDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("logs");

           // LiteratureRepository = new LitertureRepository(context, _logger);
            //LiteratureAuthorRepository = new LiteratureAuthorRepository(context, _logger);
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
