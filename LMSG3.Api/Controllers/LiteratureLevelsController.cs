using AutoMapper;
using LMSG3.Core.Models.Dtos;
using LMSG3.Core.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSG3.Api.Controllers
{
    [Route("api/literatureLevels")]
    [ApiController]
    public class LiteratureLevelsController : ControllerBase
    {
        private readonly ApiDbContext context;
        private readonly IMapper mapper;

        public LiteratureLevelsController(ApiDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        
        public async Task<ActionResult<IEnumerable<LiteratureLevel>>> GetLiteratureAuthors()
            {
            var levels = await context.literatureLevels.ToListAsync();
           
            return Ok(levels);
        }
    }
}
