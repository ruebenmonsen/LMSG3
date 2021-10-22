﻿using System;
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
using LMSG3.Api.Services;

namespace LMSG3.Api.Controllers
{

    [Route("api/literatures")]
    [ApiController]
    public class LiteraturesController : ControllerBase
    {
        private readonly ApiDbContext _context;
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public LiteraturesController(ApiDbContext context, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _context = context;
            uow = unitOfWork;
            this.mapper = mapper;
        }


        //[HttpGet("{id}")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Literature>> GetLiterature(int id, bool includeAllInfo)
        {
            var literature = await uow.LiteratureRepository.GetAsync(id, includeAllInfo);//await _context.Literatures.FindAsync(id);

            if (literature == null)
            {
                return NotFound();
            }
            //var somId = literature.LiteraLevelId;
            var literaDto = mapper.Map<LiteratureDto>(literature);
            
            literaDto.LevelName = ModelsJoinHelper.GetLevelName(literature.LiteraLevelId, _context);//levelNames;//levelNames.First(e => e.LiteraLevelId == somId).Name.ToString();
            literaDto.LiteraTypeName = ModelsJoinHelper.GetTypeName(literature.LiteraTypeId, _context);
            literaDto.SubjectName = ModelsJoinHelper.GetSubjectName(literature.SubId, _context);
            return Ok(literaDto);
        }

       
        
        [HttpGet()]
        [HttpHead]    //authorsResourceParameters.
        public async Task<ActionResult<IEnumerable<Literature>>> GetLiteratures([FromQuery] LiteraturesResourceParameters literatureResourceParameters)
        {
            var literaDto = await uow.LiteratureRepository.FindAsync(literatureResourceParameters);

            if (literaDto == null)
            {
                return NotFound();
            }
           // var literaDto = mapper.Map<IEnumerable<LiteratureDto>>(literature);
            //if (!string.IsNullOrWhiteSpace(literatureResourceParameters.titleStr) 
            //    || !string.IsNullOrWhiteSpace(literatureResourceParameters.subjectStr)
            //    || !string.IsNullOrWhiteSpace(literatureResourceParameters.discriptionStr))
            //{
                
                //literaDto = literaDto.Where(l => l.Title.ToLower()
                //                       .Contains(literatureResourceParameters.titleStr.ToLower())
                //                       || l.SubjectName.ToLower().Contains(literatureResourceParameters.subjectStr.ToLower())
                //                       ||l.Description.ToLower().Contains(literatureResourceParameters.discriptionStr.ToLower()));

            //}
            //if (ModelState.IsValid && literatureResourceParameters.levelFilter > 0)
            //{
            //    literaDto = literaDto.Where(l=>l.LiteraLevelId.Equals(literatureResourceParameters.levelFilter));
            //}
                
            // literaDto.LevelName = GetLevelName(literature.LiteraLevelId)
            

            return Ok(literaDto);
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
        public ActionResult<Literature> CreateLiterature2(Literature literature)
        {
            LiteraturesResourceParameters literatureResourceParameters = new LiteraturesResourceParameters();
            literatureResourceParameters.searchString = literature.Title;


            if (uow.LiteratureRepository.LiteratureExist(literatureResourceParameters) == true)
            {
                ModelState.AddModelError("Title", "Title is in use");
                return BadRequest(ModelState);
            }
            var authorEntity = mapper.Map<Literature>(literature);
            uow.LiteratureRepository.AddLiterature(literature);
           
            if (uow.LiteratureRepository.CompleteAsync())
            {
                return CreatedAtAction("GetLiterature", new { id = literature.Id }, literature);
            }
            else
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            
        }

        

        // DELETE: api/Literatures/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLiterature(int id)
        {
            var literature = await uow.LiteratureRepository.GetAsync(id, false);//await _context.Literatures.FindAsync(id);
            if (literature == null)
            {
                return NotFound();
            }


            uow.LiteratureRepository.DeliteLiterature(literature);
            //await _context.SaveChangesAsync();
            uow.LiteratureRepository.CompleteAsync();
            return NoContent();
        }

        private bool LiteratureExists(int id)
        {
            return _context.Literatures.Any(e => e.Id == id);
        }
    }
}