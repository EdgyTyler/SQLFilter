using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLFilter.Models;

namespace SQLFilter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfanityFiltersController : ControllerBase
    {
        private readonly SqlWordsContext _context;

        public ProfanityFiltersController(SqlWordsContext context)
        {
            _context = context;
        }

        //POST: api/ProfanityFilters
        //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("SentenceToFix/{SentenceToFix}")]
        public ActionResult<string> PostProfanityFilterSentence(string SentenceToFix)
        {

            List<ProfanityFilter> lstBadWords = _context.ProfanityFilters.ToList();

            foreach (ProfanityFilter WordEntry in lstBadWords)
            {
                SentenceToFix = SentenceToFix.Replace(WordEntry.BadWord, (String.Concat(Enumerable.Repeat("*", WordEntry.BadWord.Length))));
            }

            return SentenceToFix;
        }

        // GET: api/ProfanityFilters
        [HttpGet("GetAllProhibitedWords")]
        public async Task<ActionResult<IEnumerable<ProfanityFilter>>> GetProfanityFilters()
        {
            if (_context.ProfanityFilters == null)
            {
                return NotFound();
            }
            return await _context.ProfanityFilters.ToListAsync();
        }

        // GET: api/ProfanityFilters/5
        [HttpGet("GetProhibitedWordById/{id}")]
        public async Task<ActionResult<ProfanityFilter>> GetProfanityFilter(int id)
        {
            if (_context.ProfanityFilters == null)
            {
                return NotFound();
            }
            var profanityFilter = await _context.ProfanityFilters.FindAsync(id);

            if (profanityFilter == null)
            {
                return NotFound();
            }

            return profanityFilter;
        }

        // PUT: api/ProfanityFilters/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("EditExistingProhibitedWord")]
        public async Task<IActionResult> PutProfanityFilter(ProfanityFilter profanityFilter)
        {
     
            _context.Entry(profanityFilter).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfanityFilterExists(profanityFilter.Id))
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

        // POST: api/ProfanityFilters
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("AddNewProhibitedWord/{NewWord}")]
        public async Task<ActionResult<ProfanityFilter>> PostProfanityFilter(string NewWord)
        {
            ProfanityFilter AlteredProFilter = new ProfanityFilter();
            List<int> lstIds = _context.ProfanityFilters.Select(c => c.Id).ToList();
            lstIds.Sort();
            int LastId = lstIds.Last();
            int id = LastId + 1;
            AlteredProFilter.Id = id;
            AlteredProFilter.BadWord = NewWord;

            if (_context.ProfanityFilters == null)
          {
              return Problem("Entity set 'SqlWordsContext.ProfanityFilters'  is null.");
          }
            _context.ProfanityFilters.Add(AlteredProFilter);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProfanityFilterExists(AlteredProFilter.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetProfanityFilter", new { id = AlteredProFilter.Id }, AlteredProFilter);
        }

        // DELETE: api/ProfanityFilters/5
        [HttpDelete("DeleteProhibitedWordById/{id}")]
        public async Task<IActionResult> DeleteProfanityFilter(int id)
        {
            if (_context.ProfanityFilters == null)
            {
                return NotFound();
            }
            var profanityFilter = await _context.ProfanityFilters.FindAsync(id);
            if (profanityFilter == null)
            {
                return NotFound();
            }

            _context.ProfanityFilters.Remove(profanityFilter);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/ProfanityFilters/5
        [HttpDelete("DeleteProhibitedWord/{Word}")]
        public async Task<IActionResult> DeleteProfanityFilterWord(string Word)
        {
            List<ProfanityFilter> lstBadWords = _context.ProfanityFilters.ToList();
            int id = (from a in lstBadWords where a.BadWord == Word select a.Id).FirstOrDefault();

            if (_context.ProfanityFilters == null)
            {
                return NotFound();
            }
            var profanityFilter = await _context.ProfanityFilters.FindAsync(id);
            if (profanityFilter == null)
            {
                return NotFound();
            }

            _context.ProfanityFilters.Remove(profanityFilter);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProfanityFilterExists(int id)
        {
            return (_context.ProfanityFilters?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
