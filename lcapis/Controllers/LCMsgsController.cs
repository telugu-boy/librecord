using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lcapis.Models;
using Microsoft.AspNetCore.Authorization;
using lcapis.Helpers;
using lcapis.Entities;

namespace lcapis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LCMsgsController : ControllerBase
    {
        private readonly LCProdDbContext _context;

        public LCMsgsController(LCProdDbContext context)
        {
            _context = context;
        }

        // GET: api/LCMsgs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LCMsg>>> GetLCMsgs()
        {
            return await _context.LCMsgs.ToListAsync();
        }

        // GET: api/LCMsgs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LCMsg>> GetLCMsg(long? id)
        {
            var lCMsg = await _context.LCMsgs.FindAsync(id);

            if (lCMsg == null)
            {
                return NotFound();
            }

            return lCMsg;
        }

        // PUT: api/LCMsgs/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLCMsg(long? id, LCMsg lCMsg)
        {
            if (id != lCMsg.Id)
            {
                return BadRequest();
            }

            _context.Entry(lCMsg).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LCMsgExists(id))
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

        // POST: api/LCMsgs
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<LCMsg>> PostLCMsg(LCMsg lCMsg)
        {

            if (lCMsg.Username == null) return BadRequest();
            if (lCMsg.Content == null) return BadRequest();
            if (lCMsg.Id != null) return BadRequest();

            lCMsg.Id = Utils.SnowflakeGeneratorSingleton.Instance.CreateId();

            _context.LCMsgs.Add(lCMsg);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLCMsg", new { id = lCMsg.Id }, lCMsg);
        }

        // DELETE: api/LCMsgs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<LCMsg>> DeleteLCMsg(long? id)
        {
            var lCMsg = await _context.LCMsgs.FindAsync(id);
            if (lCMsg == null)
            {
                return NotFound();
            }

            _context.LCMsgs.Remove(lCMsg);
            await _context.SaveChangesAsync();

            return lCMsg;
        }

        private bool LCMsgExists(long? id)
        {
            return _context.LCMsgs.Any(e => e.Id == id);
        }
    }
}
