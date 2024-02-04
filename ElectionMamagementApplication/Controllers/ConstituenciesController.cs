using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElectionMamagementApplication.Models;
using Microsoft.AspNetCore.Cors;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ElectionMamagementApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class ConstituenciesController : ControllerBase
    {
        private readonly ElectionManagementDataBaseContext _context;
        private readonly IMapper _mapper;
        public ConstituenciesController(ElectionManagementDataBaseContext context, IMapper mapper)
        {
            _context = context;
            this._mapper = mapper;
        }

        // GET: api/Constituencies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Constituency>>> GetConstituencies()
        {
          if (_context.Constituencies == null)
          {
              return NotFound();
          }
            return await _context.Constituencies.ToListAsync();
        }

        // GET: api/Constituencies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Constituency>> GetConstituency(int id)
        {
          if (_context.Constituencies == null)
          {
              return NotFound();
          }
            var constituency = await _context.Constituencies.FindAsync(id);

            if (constituency == null)
            {
                return NotFound();
            }

            return constituency;
        }
        [HttpGet("GetCandidateByConstituencyId")]
        public async Task<ActionResult<IEnumerable<Candidate>>> GetCandidatesByConstituencyId(int id)
        {
            if (_context.Candidates == null)
            {
                return NotFound();
            }
            try
            {
                var constituency = await _context.Constituencies.FindAsync(id);
                if (constituency == null)
                {
                    return NotFound();
                }

                var cnd1 = _context.Candidates.Where(cnd => cnd.ConstituencyId == id).Select(cnd => new { cnd.Id, cnd.FirstName, cnd.LastName, cnd.Gender, cnd.Party.PartyName });
                /*  foreach(var item in cnd1) {
                         var a = item.FirstName;
                     }*/

                return Ok(cnd1);
            }catch(Exception) {
            return StatusCode(406,"not acceptable");
  
            }
        }

        [HttpGet("GetVotersByConstituencyId")]
        public async Task<ActionResult<IEnumerable<Candidate>>> GetVotersByConstituencyId(int id)
        {
            if (_context.Voters == null)
            {
                return NotFound();
            }
            try
            {
                var constituency = await _context.Constituencies.FindAsync(id);
                if (constituency == null)
                {
                    return NotFound();
                }

                var cnd1 = _context.Voters.Where(cnd => cnd.ConstituencyId == id).Select(cnd => new { cnd.Id, cnd.FirstName, cnd.LastName, cnd.Gender, cnd.DateOfBirth });
                /*  foreach(var item in cnd1) {
                         var a = item.FirstName;
                     }*/

                return Ok(cnd1);
            }
            catch (Exception)
            {
                return StatusCode(406, "not acceptable");

            }
        }

       /* [HttpGet("GetVotersByConstituencyId")]
        public async Task<ActionResult<IEnumerable<Candidate>>> GetResultsByConstituencyId(int id)
        {
            if (_context.ElectionsResults == null)
            {
                return NotFound();
            }
            try
            {
                var constituency = await _context.Constituencies.FindAsync(id);
                if (constituency == null)
                {
                    return NotFound();
                }

                var cnd1 = _context.Constituencies.Where(cnd => cnd.ConstituencyId == id).Select(cnd => new { cnd.Election.ElectionName,cnd.Candidate.FirstName,cnd.Candidate });
                *//*  foreach(var item in cnd1) {
                         var a = item.FirstName;
                     }*//* 

                return Ok(cnd1);
            }
            catch (Exception)
            {
                return StatusCode(406, "not acceptable");

            }
        }*/

        // PUT: api/Constituencies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConstituency(int id, Constituency constituency)
        {
            if (id != constituency.ConstituencyId)
            {
                return BadRequest();
            }

            var con = _context.Constituencies.Where(c=>c.ConstituencyId!=constituency.ConstituencyId).Where(c => c.ConstituencyName.Equals(constituency.ConstituencyName));
            foreach (var i in con)
            {
                if (i != null)
                {
                    return StatusCode(406, "Constituency name should be unique!!!");
                }
            }

            _context.Entry(constituency).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConstituencyExists(id))
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

        // POST: api/Constituencies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Constituency>> PostConstituency(Constituency constituency)
        {
          if (_context.Constituencies == null)
          {
              return Problem("Entity set 'ElectionManagementDataBaseContext.Constituencies'  is null.");
          }

      var con =_context.Constituencies.Where(c => c.ConstituencyName.Equals(constituency.ConstituencyName));
            foreach(var i in con)
            {
                if (i != null)
                {
                    return StatusCode(406, "Constituency name should be unique!!!");
                }
            }
            _context.Constituencies.Add(constituency);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConstituency", new { id = constituency.ConstituencyId }, constituency);
        }

        // DELETE: api/Constituencies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConstituency(int id)
        {
            if (_context.Constituencies == null)
            {
                return NotFound();
            }
            var constituency = await _context.Constituencies.FindAsync(id);
            if (constituency == null)
            {
                return NotFound();
            }

            _context.Constituencies.Remove(constituency);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ConstituencyExists(int id)
        {
            return (_context.Constituencies?.Any(e => e.ConstituencyId == id)).GetValueOrDefault();
        }
    }
}
