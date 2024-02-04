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
using ElectionMamagementApplication.UtilsView;

namespace ElectionMamagementApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class PartiesController : ControllerBase
    {
        private readonly ElectionManagementDataBaseContext _context;
        private readonly IMapper _mapper;

        public PartiesController(ElectionManagementDataBaseContext context, IMapper mapper)
        {
            _context = context;
            this._mapper = mapper;
        }
        [HttpPost("LogInParty")]
        public IActionResult LogInParty(LoginView logIn)
        {
            var partyId=0;
            string email = logIn.Email;
            try
            {
                var party = _context.Parties.Where(v => v.Email.Equals(email)).ToList();

                foreach (var item in party)
                {
                    if (!item.Password.Equals(logIn.Password))
                    {
                       
                        return StatusCode(404,"Wrong Password!!!!");
                    }
                }
       var   partyid = _context.Parties.Where(p=>p.Email==email).Select(p=>p.PartyId).ToList();
         foreach(var item in partyid)
                {
                    partyId = item;
                }
            }
           
            catch (Exception)
            {
                NotFound();
            }
            return new ObjectResult(new { Id = partyId }) { StatusCode=StatusCodes.Status202Accepted};

        }

        // GET: api/Parties
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Party>>> GetParties()
        {
          if (_context.Parties == null)
          {
              return NotFound();
          }
            return await _context.Parties.ToListAsync();
        }

        // GET: api/Parties/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Party>> GetParty(int id)
        {
          if (_context.Parties == null)
          {
              return NotFound();
          }
            var party = await _context.Parties.FindAsync(id);

            if (party == null)
            {
                return NotFound();
            }

            return party;
        }

        // PUT: api/Parties/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutParty(int id, Party party)
        {
            if (id != party.PartyId)
            {
                return BadRequest();
            }

            _context.Entry(party).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PartyExists(id))
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

        // POST: api/Parties
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Party>> PostParty(Party party)
        {
          if (_context.Parties == null)
          {
              return Problem("Entity set 'ElectionManagementDataBaseContext.Parties'  is null.");
          }
        var p= _context.Parties.Where(p => p.Email.Equals(party.Email));
            foreach(var i in p)
                if (i != null){
                    return StatusCode(406, "email already registered!!!");
                }
            _context.Parties.Add(party);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetParty", new { id = party.PartyId }, party);
        }

        // DELETE: api/Parties/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParty(int id)
        {
            if (_context.Parties == null)
            {
                return NotFound();
            }
            var party = await _context.Parties.FindAsync(id);
            if (party == null)
            {
                return NotFound();
            }

            _context.Parties.Remove(party);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("GetResultByPartyId/{id}")]
        public async Task<ActionResult<List<Candidate>>> GetResultByPartyId(int id)
        {
            if (_context.Parties == null)
            {
                return new ObjectResult(new { msg = "Result not declare yet!!!" }) { StatusCode = StatusCodes.Status405MethodNotAllowed };
            }
            try
            {
                var cndList = await _context.Parties.Where(p => p.PartyId == id).Select(p => p.Candidates).ToListAsync();
                if (cndList.Count() == 0)
                {
                    return new ObjectResult(new { msg = "Party has no winning candidates yet!!!" }) { StatusCode = StatusCodes.Status405MethodNotAllowed };
                }
                List<Candidate> candidates = new List<Candidate>();
                {

                }
                foreach (var cnd in cndList)
                {
                    foreach (var candidate in cnd)
                    {
                        var validcnd = _context.ElectionsResults.AsParallel().AsOrdered().Where(e => e.ConstituencyId == candidate.Id).Select(election => new { election.Candidate.FirstName, election.Candidate.LastName, election.Candidate.Gender, election.Election.ElectionName, election.Constituency.ConstituencyName, election.PercentageVotes, election.TotalVotes });
                        candidates.Add(candidate);
                    }
                   
                }
                return candidates;
            }catch (Exception)
            {
                return new ObjectResult(new { msg = "Not Found!!!" }) { StatusCode = StatusCodes.Status404NotFound };
            }
            
        }
        private bool PartyExists(int id)
        {
            return (_context.Parties?.Any(e => e.PartyId == id)).GetValueOrDefault();
        }
    }
}
