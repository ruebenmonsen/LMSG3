using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LMSG3.Core.Models.Entities;
using LMSG3.Data;
using LMSG3.Core.Models.Dtos;
using AutoMapper;
using LMSG3.Api.ResourceParameters;
using LMSG3.Api.Configuration;


namespace LMSG3.Api.Controllers
{

    [Route("api/literatures")]
    [ApiController]
    public class LiteraturesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public LiteraturesController(ApplicationDbContext context, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _context = context;
            uow = unitOfWork;
            this.mapper = mapper;
        }

        // GET: api/Literatures
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Literature>>> GetLiteratures(bool includeAllInfo) //bool includeAuthor, bool includeSubject, bool includeLevel, bool includeType
        //{
        //    var literatures = await uow.LiteratureRepository.GetAsync(includeAllInfo);

        //    return Ok(mapper.Map<IEnumerable<LiteratureDto>>(literatures)); //await _context.Literatures.ToListAsync();
        //}

        // GET: api/Literatures/5
        //[HttpGet("{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Literature>> GetLiterature(int id, bool includeAllInfos)
        {
            var literature = await uow.LiteratureRepository.GetAsync(id, includeAllInfos);//await _context.Literatures.FindAsync(id);

            if (literature == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<LiteratureDto>(literature));
        }


        [HttpGet()]
        [HttpHead]    //authorsResourceParameters.
        public async Task<ActionResult<IEnumerable<Literature>>> GetLiteratures([FromQuery] LiteraturesResourceParameters literatureResourceParameters)
        {
            var literature = await uow.LiteratureRepository.FindAsync(literatureResourceParameters);

            if (literature == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<IEnumerable<LiteratureDto>>(literature));
        }

        // PUT: api/Literatures/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{Id}")]
        public async Task<IActionResult> PutLiterature(int id, Literature literature)
        {
            if (id != literature.Id)
            {
                return BadRequest();
            }

            _context.Entry(literature).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LiteratureExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Literatures
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Literature>> PostLiterature(Literature literature)
        {
            _context.Literatures.Add(literature);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLiterature", new { id = literature.Id }, literature);
        }

        // DELETE: api/Literatures/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLiterature(int id)
        {
            var literature = await _context.Literatures.FindAsync(id);
            if (literature == null)
            {
                return NotFound();
            }

            _context.Literatures.Remove(literature);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LiteratureExists(int id)
        {
            return _context.Literatures.Any(e => e.Id == id);
        }
    }
}
