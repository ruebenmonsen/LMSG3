using AutoMapper;
using LMSG3.Core.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSG3.Api.Controllers
{
    [Route("api/LiteratureSubjects")]
    [ApiController]
    public class LiteratureSubjectsController : ControllerBase
    {
        private readonly ApiDbContext context;
        private readonly IMapper mapper;

        public LiteratureSubjectsController(ApiDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<ActionResult<IEnumerable<Subject>>> GetLiteratureSubjects()
        {
            var subjects = await context.LiteratureSubjects.ToListAsync();

            return Ok(subjects);
        }
    }
}
