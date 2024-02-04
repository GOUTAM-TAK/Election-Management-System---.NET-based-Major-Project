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
using NuGet.Protocol.Plugins;
using ElectionMamagementApplication.ModelView;

namespace ElectionMamagementApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class VotesController : ControllerBase
    {
        private readonly ElectionManagementDataBaseContext _context;

        private readonly IMapper _mapper;

        public VotesController(ElectionManagementDataBaseContext context, IMapper mapper)
        {
            _context = context;
            this._mapper = mapper;
        }

        //Post : api/Votes/VoteLogIn
        [HttpPost("VoteLogIn")]
        public async Task<IActionResult> VoteLogIn(LoginView logIn) 
        {
       var i =_context.Elections.Where(e => e.ElectionStatus.Equals("ON_GOING")).ToList();
            var election = new Election();
          foreach (var e in i)
            {
                if (e == null)
                    return StatusCode(404, "Election not started yet!!!!");
               election = e;
            }
            string email = logIn.Email;
            try
            {
                var voter = _context.Voters.Where(v => v.Email.Equals(email)).ToList();

                foreach (var item in voter)
                {
                    if (item.Password.Equals(logIn.Password))
                    {
                        if (item.IsVoted.Equals(false))
                        {
                            //get constituency name
                      var consti =await _context.Constituencies.FindAsync(item.ConstituencyId);

                      
                            return new ObjectResult(new { id = item.Id , ConstituencyId = consti.ConstituencyId ,ConstituencyName = consti.ConstituencyName , ElectionId = election.ElectionId, ElectionName = election.ElectionName }) { StatusCode = StatusCodes.Status202Accepted };
                        }
                        else
                        {
                            return StatusCode(401, "Already Voted!!!!");
                        }
                        }
                    else
                    {
                        return StatusCode(404, "Wrong Password");
                    }
                }
                return StatusCode(404, "Account Not Found!!!");

            }
            catch (Exception)
            {
                NotFound();
            }
            return NoContent();
        }

   

        // GET: api/Votes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vote>>> GetVotes()
        {
          if (_context.Votes == null)
          {
              return NotFound();
          }
            return await _context.Votes.ToListAsync();
        }

        // GET: api/Votes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Vote>> GetVote(int id)
        {
          if (_context.Votes == null)
          {
              return NotFound();
          }
            var vote = await _context.Votes.FindAsync(id);

            if (vote == null)
            {
                return NotFound();
            }

            return vote;
        }

        // PUT: api/Votes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVote(int id, Vote vote)
        {
            if (id != vote.VoteId)
            {
                return BadRequest();
            }

            _context.Entry(vote).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VoteExists(id))
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

        // POST: api/Votes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Vote>> PostVote(VoteView voteView)
        {
          if (_context.Votes == null)
          {
              return Problem("Entity set 'ElectionManagementDataBaseContext.Votes'  is null.");
          }
        var vote = _mapper.Map<Vote>(voteView);
            var cnd = await _context.Candidates.Where(c => c.Id == vote.CandidateId).CountAsync();
            if (cnd != 1)
                return new ObjectResult(new { msg = "Candidate not found!!!" }) { StatusCode = StatusCodes.Status404NotFound };
            var votercnt =   await  _context.Votes.Where(v=>v.VoterId==vote.VoterId&&v.CandidateId==vote.CandidateId&&v.ElectionId==vote.ElectionId).CountAsync();
            if (votercnt != 0)
            {
                return new ObjectResult(new { msg = "Voter alredy cast vote!!!" }) { StatusCode = StatusCodes.Status406NotAcceptable };
            }
           
     
            _context.Votes.Add(vote);
            try
            {
                
          /* var voter = await  _context.Voters.FindAsync(vote.VoterId);
                voter.IsVoted = true;*/
                
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (VoteExists(vote.VoteId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(200,"Succussfully Cast Vote!!!");
        }

        // DELETE: api/Votes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVote(int id)
        {
            if (_context.Votes == null)
            {
                return NotFound();
            }
            var vote = await _context.Votes.FindAsync(id);
            if (vote == null)
            {
                return NotFound();
            }

            _context.Votes.Remove(vote);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VoteExists(int id)
        {
            return (_context.Votes?.Any(e => e.VoteId == id)).GetValueOrDefault();
        }
    }
}
