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

namespace ElectionMamagementApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class ElectionsController : ControllerBase
    {
        private readonly ElectionManagementDataBaseContext _context;

        private readonly IMapper _mapper;

     
        public ElectionsController(ElectionManagementDataBaseContext context, IMapper mapper)
        {
            _context = context;
            this._mapper = mapper;
           
        }

        // GET: api/Elections
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Election>>> GetElections()
        {
          if (_context.Elections == null)
          {
              return NotFound();
          }
            return  await _context.Elections.OrderByDescending(e=>e.ElectionStatus).ToListAsync();
        }
        [HttpGet("UpComingElectionsList")]
        public async Task<ActionResult<IEnumerable<Election>>> UpComingElectionsList()
        {
            if (_context.Elections == null)
            {
                return NotFound();
            }
         var list = await  _context.Elections.Where(e=>e.ElectionStatus.Equals("UP_COMING")).ToListAsync();
            if(list.Count == 0)
            {
                return NotFound("No one Election Upcoming!!!");

            }
            return list;
        }
        [HttpGet("CompletedElectionsList")]
        public async Task<ActionResult<IEnumerable<Election>>> CompletedElectionsList()
        {
            if (_context.Elections == null)
            {
                return NotFound();
            }

            var list = await _context.Elections.Where(e => e.ElectionStatus.Equals("COMPLETED")).ToListAsync();
            if (list.Count == 0)
            {
                return new ObjectResult(new { msg = "No one Election COMPLETED!!!" }) { StatusCode = StatusCodes.Status404NotFound };

            }
            return list;
        }

        [HttpGet("OnGoingElectionsList")]
        public async Task<ActionResult<IEnumerable<Election>>> OnGoingElectionsList()
        {
            if (_context.Elections == null)
            {
                return NotFound();
            }

            var list = await _context.Elections.Where(e => e.ElectionStatus.Equals("ON_COMING")).ToListAsync();
            if (list.Count == 0)
            {
                return new ObjectResult(new { msg = "No one Election ONGOING!!!" }) { StatusCode = StatusCodes.Status404NotFound };

            }
            return list;
        }

        // GET: api/Elections/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Election>> GetElection(int id)
        {
          if (_context.Elections == null)
          {
              return NotFound();
          }
            var election = await _context.Elections.FindAsync(id);

            if (election == null)
            {
                return NotFound();
            }

            return election;
        }

        // PUT: api/Elections/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutElection(int id, Election election)
        {
            if (id != election.ElectionId)
            {
                return BadRequest();
            }
       var olde =  await _context.Elections.Where(e => e.ElectionId == id).Select(e=>e.ElectionStatus).FirstOrDefaultAsync();
            if (olde.Equals("COMPLETED"))
            {
                return Unauthorized("Election has already declared, now it's status can not be changed!!!");
            }
            else if (olde.Equals("ON_GOING") && election.Equals("UP_COMING"))
            {
                return Unauthorized("Election has already started, now it's status can only be assign to COMPLETED!!!");
            }
            else if (olde.Equals("UP_COMING") && election.Equals("COMPLETED"))
            {
                return Unauthorized("Election has already started, now it's status can only be assign to COMPLETED!!!");
            }
          
          
            _context.Entry(election).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                if (election.ElectionStatus.Equals("COMPLETED"))
                {
                    
              return    RedirectToAction("CreateResultList", "ElectionsResults", new { election.ElectionId });
                }
               
      
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ElectionExists(id))
                {
                    return  NotFound("Server Side error!!!");
                }
                else
                {
                    throw;
                }
            }

            return Ok("Election updated!!!");
        }

        // POST: api/Elections
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Election>> PostElection(Election election)
        {
          if (_context.Elections == null)
          {
              return Problem("Entity set 'ElectionManagementDataBaseContext.Elections'  is null.");
          }
            _context.Elections.Add(election);
            await _context.SaveChangesAsync();

            return Ok("Election successfully registered as UP_COMING");
        }

        // DELETE: api/Elections/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteElection(int id)
        {
            if (_context.Elections == null)
            {
                return NotFound();
            }
            var election = await _context.Elections.FindAsync(id);
            if (election == null)
            {
                return NotFound();
            }

            _context.Elections.Remove(election);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ElectionExists(int id)
        {
            return (_context.Elections?.Any(e => e.ElectionId == id)).GetValueOrDefault();
        }
    }
}
