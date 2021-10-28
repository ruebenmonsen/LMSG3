using AutoMapper;
using LMSG3.Api.Service.Repositories;
using LMSG3.Api.Services.Repositories;
using LMSG3.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace LMSG3.Api.Configuration
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApiDbContext _context;
        private readonly IMapper mapper;
        private readonly ILogger _logger;

        

        public Services.Repositories.ILiteratureRepository LiteratureRepository { get; private set; }

        public ILiteratureAuthorRepository LiteratureAuthorRepository { get; private set; }

        

        public UnitOfWork(ApiDbContext context, ILoggerFactory loggerFactory, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
            _logger = loggerFactory.CreateLogger("logs");

            LiteratureRepository = new LitertureRepository(context, _logger, mapper);
            LiteratureAuthorRepository = new LiteratureAuthorRepository(context, _logger, mapper);
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
