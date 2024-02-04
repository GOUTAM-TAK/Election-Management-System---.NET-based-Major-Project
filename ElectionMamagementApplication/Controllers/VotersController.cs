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
using ElectionMamagementApplication.ModelView;
using System.ComponentModel.DataAnnotations;
using ElectionMamagementApplication.UtilsView;

namespace ElectionMamagementApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class VotersController : ControllerBase
    {
        private readonly ElectionManagementDataBaseContext _context;
        private readonly IMapper _mapper;

        public VotersController(ElectionManagementDataBaseContext context, IMapper mapper)
        {
            _context = context;
            this._mapper = mapper;
        }

        [HttpPost("LogInVoter")]
        public  IActionResult LogInVoter(LoginView logIn)
        {
            string email = logIn.Email;
            try
            {
                var voter = _context.Voters.Where(v => v.Email.Equals(email)).ToList();

                foreach (var item in voter) {
                    if (item.Password.Equals(logIn.Password))
                    {
                        //get constituency name
                        return new ObjectResult(new { id = item.Id }) { StatusCode = StatusCodes.Status202Accepted };
                    }
                    else
                    {
                        return StatusCode(404, "Wrong Password");
                    }
                }
                return StatusCode(404, "Account Not Found!!!");

            } catch (Exception)
            {
                NotFound();
            }
            return NoContent();
        }
     
      
    

        // GET: api/Voters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Voter>>> GetVoters()
        {
          if (_context.Voters == null)
          {
              return NotFound();
          }
            return await _context.Voters.ToListAsync();
        }

        // GET: api/Voters/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Voter>> GetVoter(int id)
        {
          if (_context.Voters == null)
          {
              return NotFound();
          }
            var voter = await _context.Voters.FindAsync(id);

            if (voter == null)
            {
                return NotFound();
            }

            return voter;
        }

        // PUT: api/Voters/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVoter(int id, VoterViewOut voterviewout)
        {
            
            var voter = _mapper.Map<Voter>(voterviewout);
          /*  if (id != voter.Id)
            {
                return BadRequest();
            }*/
          voter.Id = id;
            _context.Entry(voter).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VoterExists(id))
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

        // POST: api/Voters
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Voter>> PostVoter(VoterView voterview)
        {
          if (_context.Voters == null)
          {
              return Problem("Entity set 'ElectionManagementDataBaseContext.Voters'  is null.");
          }
          var voter = _mapper.Map<Voter>(voterview);
            _context.Voters.Add(voter);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVoter", new { id = voter.Id }, voter);
        }

        // DELETE: api/Voters/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVoter(int id)
        {
            if (_context.Voters == null)
            {
                return NotFound();
            }
            var voter = await _context.Voters.FindAsync(id);
            if (voter == null)
            {
                return NotFound();
            }

            _context.Voters.Remove(voter);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VoterExists(int id)
        {
            return (_context.Voters?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
