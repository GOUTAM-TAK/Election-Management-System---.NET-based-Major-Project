using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElectionMamagementApplication.ModelView;
using ElectionMamagementApplication.Models;
using Microsoft.AspNetCore.Cors;
using AutoMapper;
using ElectionMamagementApplication.UtilsView;

namespace ElectionMamagementApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class AdminsController : ControllerBase
    {
        private readonly ElectionManagementDataBaseContext _context;
        private static Admin admin = new Admin { Id=1001 ,Name = "Admin", Description = "Election Commision Office",Email = "admin@gmail.com", Password= "Admin@123" };

         static AdminsController()
        {

        }
        public AdminsController(ElectionManagementDataBaseContext context)
        {
            _context = context;
           /* admin.Id = 1001;
            admin.Name = "Admin";
            admin.Description = "Election Commision Office";
            admin.Email = "admin@gmail.com";
            admin.Password = "Admin@123";*/
        }

        // GET: api/Admins/LogInAdmin
        [HttpPost("LogInAdmin")]
        public ActionResult LogInAdmin(LoginView loginView)
        {
            if (loginView.Email.Equals(admin.Email))
            {
                if (loginView.Password.Equals(admin.Password))
                {
                    return new ObjectResult(new { id = admin.Id }) { StatusCode = StatusCodes.Status202Accepted };
                }
                else
                {
                    return StatusCode(404, "Incorrect Admin Password!!!");
                }
            }
            else
            {
                return StatusCode(404, "Incorrect Admin Email!!!");
            }


        }

       
        
       /* // GET: api/Admins
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Admin>>> GetAdmin()
        {
          if (_context.Admin == null)
          {
              return NotFound();
          }
            return admin;
        }*/

        // GET: api/Admins/5
        [HttpGet("{id}")]
        public IActionResult GetAdmin(int id)
        {
            if (id == admin.Id)
            {

                return Ok(admin) ;
            }
            return StatusCode(StatusCodes.Status401Unauthorized, "UnAuthorized only Admin can access!!!");
        }

        // PUT: api/Admins/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public  IActionResult UpdateAdmin(int id, Admin admin1)
        {
            if (id != admin.Id)
                return new ObjectResult(new { msg ="UnAuthorised Access!!!!,Only Admin can access." }) {StatusCode=StatusCodes.Status401Unauthorized};
            if (id != admin1.Id)
            {
                return BadRequest();
            }

          //  _context.Entry(admin).State = EntityState.Modified;

            try
            {
                admin.Name = admin1.Name;
                admin.Description = admin1.Description;
                admin.Email = admin1.Email;
                admin.Password = admin1.Password;
              
            }
            catch (Exception)
            {
               
                    throw;
               
            }

            return Ok("Admin updated successfully!!!");
        }

        // POST: api/Admins
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
       /* [HttpPost]
        public async Task<ActionResult<Admin>> PostAdmin(Admin admin)
        {
          if (_context.Admin == null)
          {
              return Problem("Entity set 'ElectionManagementDataBaseContext.Admin'  is null.");
          }
            _context.Admin.Add(admin);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAdmin", new { id = admin.Id }, admin);
        }*/

        // DELETE: api/Admins/5
       /* [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            if (_context.Admin == null)
            {
                return NotFound();
            }
            var admin = await _context.Admin.FindAsync(id);
            if (admin == null)
            {
                return NotFound();
            }

            _context.Admin.Remove(admin);
            await _context.SaveChangesAsync();

            return NoContent();
        }*/

      /*  private bool AdminExists(int id)
        {
            return (_context.Admin?.Any(e => e.Id == id)).GetValueOrDefault();
        }*/
    }
}
