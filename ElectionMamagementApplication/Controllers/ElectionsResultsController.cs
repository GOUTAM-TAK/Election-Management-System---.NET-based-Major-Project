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

namespace ElectionMamagementApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class ElectionsResultsController : ControllerBase
    {
        private readonly ElectionManagementDataBaseContext _context;

        private readonly IMapper _mapper;

        public ElectionsResultsController(ElectionManagementDataBaseContext context, IMapper mapper)
        {
            _context = context;
            this._mapper = mapper;
        }

        // GET: api/ElectionsResults/5
        [HttpGet("GetElectionsResultsByElectionId/{electionId}")]
        public async Task<ActionResult<IEnumerable<ElectionsResult>>> GetElectionsResultsByElectionId(int electionId)
        {
          if (_context.ElectionsResults == null)
          {
              return NotFound();
          }
            try
            {
                var list = await _context.ElectionsResults.Where(e => e.ElectionId == electionId)
                    .Select(electionsResult => new { electionsResult, electionsResult.Candidate.FirstName,electionsResult
                    .Candidate.LastName,electionsResult.Candidate.Gender,electionsResult.Candidate.Party.PartyName,electionsResult
                    .Candidate.Email, electionsResult.Election.ElectionName, electionsResult.Constituency.ConstituencyName }).ToListAsync();
                return Ok(list);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

       

        // GET: api/ElectionsResults/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ElectionsResult>> GetElectionsResult(int id)
        {
          if (_context.ElectionsResults == null)
          {
              return NotFound();
          }
            var electionsResult = await _context.ElectionsResults.FindAsync(id);

            if (electionsResult == null)
            {
                return NotFound();
            }

            return electionsResult;
        }

        [HttpGet("GetElectionsResultsByPartyId{id}")]
        public async Task<ActionResult<IEnumerable<ElectionsResult>>> GetElectionsResultsByPartyId(int id)
        {
            var list =  await  _context.ElectionsResults.Where(e=>e.Candidate.PartyId== id).Select(electionsResult => new {
                electionsResult,
                electionsResult.Candidate.FirstName, 
                electionsResult
                    .Candidate.LastName,
                electionsResult.Candidate.Gender,
                electionsResult.Candidate.Party.PartyName,
                electionsResult
                    .Candidate.Email,
                electionsResult.Election.ElectionName,
                electionsResult.Constituency.ConstituencyName
            }).ToListAsync();
            return Ok(list);
        }
        [HttpGet("GetElectionsResultsByCandidateId/{id}")]
        public async Task<IActionResult> GetElectionsResultsByCandidateId(int id)
        {
            var list =await _context.ElectionsResults.Where(e=>e.CandidateId==id).ToListAsync();
            return Ok(list);
        }

        [HttpGet("GetElectionsResultsByConstituencyId/{id}")]
        public async Task<ActionResult<IEnumerable<ElectionsResult>>> GetElectionsResultsByConstituencyId(int id)
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

               
               var cnd1 = await _context.ElectionsResults.Where(cnd => cnd.ConstituencyId == id)
                     .Select(electionsResult => new {
                         electionsResult,
                         electionsResult.Candidate.FirstName,
                         electionsResult
                    .Candidate.LastName,
                         electionsResult.Candidate.Gender,
                         electionsResult.Candidate.Party.PartyName,
                         electionsResult
                    .Candidate.Email,
                         electionsResult.Election.ElectionName,
                         electionsResult.Constituency.ConstituencyName
                     }).ToListAsync();

                if (cnd1.Count() == 0)
                {
                    return StatusCode(404,"Constituency Result not declared yet!!!!");
                }
                return Ok(cnd1);
            }
            catch (Exception)
            {
                return StatusCode(406, "not acceptable");

            }
        }
        [HttpPut("CreateResultList/{ElectionId}")]
        public async Task<ActionResult> CreateResultList(int ElectionId)
        {
          
            var constituencyList = await _context.Constituencies.Select(c => c.ConstituencyId).ToListAsync();
            foreach (var i in constituencyList) {
          var winCandidatesList = await _context.Candidates.Where(c => c.ElectionId == ElectionId && c.ConstituencyId == i).ToListAsync();
                Candidate cndi=null;
                int cnt=0,idd=0;
                foreach (var item in winCandidatesList)
                { var cnt1 = await _context.Votes.Where(v => v.CandidateId == item.Id).CountAsync();
                    if (cnt1 > cnt)
                    {
                        cnt=cnt1;
                        idd = item.Id;
                        cndi = item;
                    }
                }
                ElectionsResult electionsResult = new ElectionsResult();
                electionsResult.ElectionId = ElectionId;
                electionsResult.ConstituencyId = i;
                electionsResult.CandidateId = idd;
                electionsResult.TotalVotes =await  _context.Votes.Where(v=>v.CandidateId==idd&&v.ElectionId==ElectionId).CountAsync();
                var votes =await _context.Votes.Where(v => v.ElectionId == ElectionId && v.Candidate.ConstituencyId == cndi.ConstituencyId).CountAsync();
                electionsResult.PercentageVotes = (electionsResult.TotalVotes/votes)*100;
                //  var cndd= winCandidatesList.Where(c => c.Id == idd).FirstOrDefault();
                // Console.WriteLine(cndd.FirstName+" "+cndd.Votes+" "+cndd.ConstituencyId);
                if (_context.ElectionsResults == null)
                {
                    return Problem("Entity set 'ElectionManagementDataBaseContext.ElectionsResults'  is null.");
                }
                _context.ElectionsResults.Add(electionsResult);
              
                }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
                {
                    return BadRequest();
                }
                return Ok("Successfully Election Conducted, Election Result is ready now!!!");
        }

        // PUT: api/ElectionsResults/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutElectionsResult(int id, ElectionResultViewOut electionsResultviewOut)
        {
        var electionsResult= _mapper.Map<ElectionsResult>(electionsResultviewOut);
            if (id != electionsResult.ResultId)
            {
                return BadRequest();
            }
            var cnd =  _context.Candidates.Where(c=>c.Id==electionsResult.CandidateId).Select(c=>c.ConstituencyId).ToList();
            foreach(var i in cnd)
            if (i!=electionsResult.ConstituencyId)
            {
                return StatusCode(406, "Candidate not match with Constituency!!!");
            }

        /*    var constId = _context.Candidates.Where(cnd => cnd.Id == electionsResult.CandidateId).Where(cnd => cnd.ConstituencyId == electionsResult.ConstituencyId);
            foreach (var i in constId)
                if (i == null)
                {
                    return StatusCode(406, "Candidate not match with Constituency!!!");
                }*/
          /*  var e2 = _context.ElectionsResults.Where(e => e.ElectionId == electionsResult.ElectionId).Where(e => e.ConstituencyId == electionsResult.ConstituencyId);
            foreach (var i in e2)
                if (i != null)
                {
                    return StatusCode(406, "result already declared of given constituency!!!");

                }*/
                    _context.Entry(electionsResult).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ElectionsResultExists(id))
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

        // POST: api/ElectionsResults
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ElectionsResult>> PostElectionsResult(ElectionResultView electionsResultView)
        {
         var electionsResult =  _mapper.Map<ElectionsResult>(electionsResultView);
            var e2 = _context.ElectionsResults.Where(e => e.ElectionId == electionsResult.ElectionId).Where(e => e.ConstituencyId == electionsResult.ConstituencyId);
            foreach (var i in e2)
                if (i != null)
                {
                    return StatusCode(406, "result already declared of given constituency!!!");
                }

         var cnd =  await _context.Candidates.FindAsync(electionsResult.CandidateId);
           // var constId =     _context.Candidates.Where(cnd=>cnd.Id==electionsResult.CandidateId).Where(cnd=>cnd.ConstituencyId==electionsResult.ConstituencyId).ToList();
      
            if(cnd.ConstituencyId != electionsResult.ConstituencyId)
            {
                return StatusCode(406,"Candidate not match with Constituency!!!");
            }
        
         
          if (_context.ElectionsResults == null)
          {
              return Problem("Entity set 'ElectionManagementDataBaseContext.ElectionsResults'  is null.");
          }
            _context.ElectionsResults.Add(electionsResult);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ElectionsResultExists(electionsResult.ResultId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetElectionsResult", new { id = electionsResult.ResultId }, electionsResult);
        }

        // DELETE: api/ElectionsResults/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteElectionsResult(int id)
        {
            if (_context.ElectionsResults == null)
            {
                return NotFound();
            }
            var electionsResult = await _context.ElectionsResults.FindAsync(id);
            if (electionsResult == null)
            {
                return NotFound();
            }

            _context.ElectionsResults.Remove(electionsResult);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ElectionsResultExists(int id)
        {
            return (_context.ElectionsResults?.Any(e => e.ResultId == id)).GetValueOrDefault();
        }
    }
}
