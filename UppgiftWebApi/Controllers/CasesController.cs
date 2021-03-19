using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UppgiftWebApi.Data;
using UppgiftWebApi.Entities;

namespace UppgiftWebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CasesController : ControllerBase
    {
        private readonly SqlDbContext _context;

        public CasesController(SqlDbContext context)
        {
            _context = context;
        }

        // GET: api/Cases
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Case>>> GetCases()
        {
            return await _context.Cases.Include(c => c.Customer).ToListAsync();
        }
     
        // GET: api/Cases/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Case>> GetCase(int id)
        {
            var @case = await _context.Cases.FindAsync(id);

            if (@case == null)
            {
                return NotFound();
            }

            return @case;
        }

        // GET: api/Cases/status
        [HttpGet("status")]
        public async Task<ActionResult<IEnumerable<Case>>> GetCaseStatus(string status)
        {
            var @case = await _context.Cases.Where(x => x.CaseStatus == status).ToListAsync();

            if(@case == null)
            {
                return NotFound();
            }
            return @case;
        }

        [HttpGet("date")]
        public async Task<ActionResult<IEnumerable<Case>>> GetCaseDate(DateTime date)
        {
            var @case = await _context.Cases.OrderBy(x => x.CaseDate <= date).ToListAsync();

            if (@case == null)
            {
                return NotFound();
            }
            return @case;
        }

        [HttpGet("customer")]
        public async Task<ActionResult<IEnumerable<Case>>> GetCaseCustomer(int id)
        {
            var @case = await _context.Cases.Where(x => x.CustomerId == id).ToListAsync();

            if (@case == null)
            {
                return NotFound();
            }
            return @case;
        }


        // PUT: api/Cases/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCase(int id, Case @case)
        {
            if (id != @case.Id)
            {
                return BadRequest();
            }

            _context.Entry(@case).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CaseExists(id))
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

        // POST: api/Cases
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Case>> PostCase(Case @case)
        {
            _context.Cases.Add(@case);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCase", new { id = @case.Id }, @case);
        }

        // DELETE: api/Cases/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCase(int id)
        {
            var @case = await _context.Cases.FindAsync(id);
            if (@case == null)
            {
                return NotFound();
            }

            _context.Cases.Remove(@case);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CaseExists(int id)
        {
            return _context.Cases.Any(e => e.Id == id);
        }
    }
}
