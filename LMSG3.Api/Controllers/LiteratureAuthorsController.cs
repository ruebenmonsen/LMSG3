using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LMSG3.Core.Models.Entities;
using LMSG3.Data;
using LMSG3.Core.Configuration;
using AutoMapper;
using LMSG3.Core.Models.Dtos;

namespace LMSG3.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LiteratureAuthorsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public LiteratureAuthorsController(ApplicationDbContext context, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _context = context;
            uow = unitOfWork;
            this.mapper = mapper;
        }

        // GET: api/LiteratureAuthors 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LiteratureAuthor>>> GetLiteratureAuthors(bool includeAllInfo)
        {
            var authors = await uow.LiteratureAuthorRepository.GetAsync(includeAllInfo);
            return Ok(mapper.Map<IEnumerable<LiteratureAuthorDto>>(authors));
            //return Ok(authors);
            //return await _context.LiteratureAuthors.ToListAsync();
        }

        // GET: api/LiteratureAuthors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LiteratureAuthor>> GetLiteratureAuthor(int id, bool includeAllInfo)
        {
            var literatureAuthor = await uow.LiteratureAuthorRepository.GetAsync(id, includeAllInfo);

            if (literatureAuthor == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<LiteratureAuthorDto>(literatureAuthor));
        }

        [HttpGet("GetByName")]
        public async Task<ActionResult<IEnumerable<LiteratureAuthor>>> GetLiteratureAuthor(string searchStr)
        {
            var author = await uow.LiteratureAuthorRepository.FindAsync(searchStr.ToLower());

            if (author == null)
            {
                return NotFound();
            }

            return author.ToList();
        }

        // PUT: api/LiteratureAuthors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLiteratureAuthor(int id, LiteratureAuthor literatureAuthor)
        {
            if (id != literatureAuthor.Id)
            {
                return BadRequest();
            }

            _context.Entry(literatureAuthor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LiteratureAuthorExists(id))
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

        // POST: api/LiteratureAuthors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LiteratureAuthor>> PostLiteratureAuthor(LiteratureAuthor literatureAuthor)
        {
            _context.LiteratureAuthors.Add(literatureAuthor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLiteratureAuthor", new { id = literatureAuthor.Id }, literatureAuthor);
        }

        // DELETE: api/LiteratureAuthors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLiteratureAuthor(int id)
        {
            var literatureAuthor = await _context.LiteratureAuthors.FindAsync(id);
            if (literatureAuthor == null)
            {
                return NotFound();
            }

            _context.LiteratureAuthors.Remove(literatureAuthor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LiteratureAuthorExists(int id)
        {
            return _context.LiteratureAuthors.Any(e => e.Id == id);
        }
    }
}
