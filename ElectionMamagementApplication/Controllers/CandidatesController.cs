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
using ElectionMamagementApplication.UtilsView;

namespace ElectionMamagementApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class CandidatesController : ControllerBase
    {
        private readonly ElectionManagementDataBaseContext _context;
        //automapper
        private readonly IMapper mapper;

        public CandidatesController(ElectionManagementDataBaseContext context,IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        [HttpPost("LogInCandidate")]
        public  IActionResult LogInCandidate(LoginView logIn)
        {
            string email = logIn.Email;
            try
            {
                var candidate = _context.Candidates.Where(v => v.Email.Equals(email));
                Console.WriteLine(candidate);
                foreach (var item in candidate)
                {
                    Console.WriteLine(item);
                    if (!item.Password.Equals(logIn.Password))
                    {
                        return StatusCode(404, "Wrong Password!!!");

                    }

                    /*     var candidate1 = await _context.Candidates.FindAsync(item.Id);*/
                    return new ObjectResult(new { id = item.Id }) { StatusCode = StatusCodes.Status202Accepted };
                }
                return StatusCode(404, "Email already registered, Email must be unique!!!");

            }
            catch (Exception)
            {
                return StatusCode(404, "Bad Request!!!"); ;
            }
        }



        // GET: api/Candidates
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Candidate>>> GetCandidates()
        {
          if (_context.Candidates == null)
          {
              return NotFound();
          }
            return await _context.Candidates.ToListAsync();
        }

        // GET: api/Candidates/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CandidateView>> GetCandidate(int id)
        {
          if (_context.Candidates == null)
          {
              return NotFound();
          }
            var candidate = await _context.Candidates.FindAsync(id);

            if (candidate == null)
            {
                return NotFound();
            }

            var cndView = mapper.Map<CandidateView>(candidate);

            return cndView;
        }

        // PUT: api/Candidates/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCandidate(int id, Candidate candidate)
        {
          //  var candidate = mapper.Map<Candidate>(candidateview);
            if (id != candidate.Id)
            {
                return BadRequest();
            }

            var cnd = _context.Candidates.Where(c=>c.Id!=candidate.Id).Where(c => c.Email.Equals(candidate.Email));
            foreach(var i in cnd)
            if (i != null)
            {
                return StatusCode(406, "email alredy registered!!!");
            }
            if (candidate.PartyId != 0)
            {

                var cnd2 = _context.Candidates.Where(c=>c.Id!=candidate.Id).Where(c => c.ConstituencyId == candidate.ConstituencyId).Where(c => c.PartyId == candidate.PartyId);
                foreach(var i in cnd2)
                if (i != null)
                {
                    return StatusCode(406, "party already registered in constituency!!!");
                }

            }

            _context.Entry(candidate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CandidateExists(id))
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

        // POST: api/Candidates
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Candidate>> PostCandidate(CandidateView candidateview)
        {
            try
            {
                if (_context.Candidates == null)
                {
                    return Problem("Entity set 'ElectionManagementDataBaseContext.Candidates'  is null.");
                }
                var candidate = mapper.Map<Candidate>(candidateview);

                var cnd = _context.Candidates.Where(c => c.Email.Equals(candidate.Email));
                foreach(var i in cnd)
                {
                    if (i.Email.Equals(candidate.Email))
                    {
                        return StatusCode(406, "email alredy registered!!!");
                    }
                }
                var electionId = candidate.ElectionId;
                if(_context.Elections.Where(e=>e.ElectionId == electionId).Where(e => e.ElectionStatus.Equals("UP_COMING")).Count() == 0)
                {
                    new ObjectResult(new { msg = "Candidate can not register, election is not upcoming!!!" }) { StatusCode = StatusCodes.Status405MethodNotAllowed };
                
            }
                if (candidate.PartyId != 0)
                {

                    var cnd2 = _context.Candidates.Where(c => c.ConstituencyId == candidate.ConstituencyId).Where(c => c.PartyId == candidate.PartyId);
                   foreach(var i in cnd2)
                    if (i != null)
                    {
                        return StatusCode(406, "party already registered in constituency!!!");
                    }

                }

                _context.Candidates.Add(candidate);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetCandidate", new { id = candidate.Id }, candidate);
            }catch (Exception)
            {
                return NoContent();
            }
        }

        // DELETE: api/Candidates/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCandidate(int id)
        {
            if (_context.Candidates == null)
            {
                return NotFound();
            }

            var candidate = await _context.Candidates.FindAsync(id);
        
            if (candidate == null)
            {
                return NotFound();
            }
            if (candidate.Election.ElectionStatus.Equals("UP_COMING"))
            {
                _context.Candidates.Remove(candidate);
                await _context.SaveChangesAsync();
                return new ObjectResult(new { msg = "Candidate remove successfully!!!" }) { StatusCode = StatusCodes.Status200OK};
            }

            return new ObjectResult(new { msg="Candidate can not remove, election is ongoing or completed!!!" }) { StatusCode=StatusCodes.Status405MethodNotAllowed};
        }

        //GET : api/Candidates/GetCandidateByPartyId/5
        [HttpGet("GetCandidateByPartyId/{id}")]
        public IActionResult GetCandidateByPartyId(int id)
        {
            if (_context.Candidates == null)
            {
                return NotFound();
            }
            try
            {
                var cndlist = _context.Candidates.Where(c=>c.PartyId == id).Select(candidate => new {candidate,candidate.Election.ElectionName ,candidate.Party.PartyName, candidate.Constituency.ConstituencyName}).ToList();
              
                
                if (cndlist.Count > 0)
                {
                    return Ok(cndlist);
                }
                return new ObjectResult(new {msg = "Party has no Candidates yet!!!!" }) { StatusCode=StatusCodes.Status404NotFound};
            }catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("GetCandidatesListForVoting/{electionId}/{voterId}")]
        public   IActionResult GetCandidatesListForVoting(int electionId, int voterId)
        {
           
                if (_context.Candidates == null)
                {
                    return NotFound();
                }
                try
                {
                var contsId = _context.Voters.AsParallel().AsOrdered().Where(v => v.Id == voterId).Select(v => v.ConstituencyId).FirstOrDefault();
                    var cndlist = _context.Candidates.AsParallel().AsOrdered().Where(c => c.ElectionId == electionId&&c.ConstituencyId==contsId).Select(candidate => new { candidate, candidate.Election.ElectionName, candidate.Party.PartyName, candidate.Constituency.ConstituencyName }).ToList();

                        return Ok(cndlist);
                    
                   
                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }
            private bool CandidateExists(int id)
        {
            return (_context.Candidates?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
