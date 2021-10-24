using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSG3.Api.Controllers
{
    [Route("api/LiteratureTypes")]
    [ApiController]
    public class LiteratureTypesController : ControllerBase
    {
        private readonly ApiDbContext context;
        private readonly IMapper mapper;
        public LiteratureTypesController(ApiDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LiteratureTypesController>>> GetLiteratureTypes()
        {
            var types = await context.literatureTypes.ToListAsync();

            return Ok(types);
        }
    }
}
