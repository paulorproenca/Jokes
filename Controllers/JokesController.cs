using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Jokes.Models;

namespace Jokes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JokesController : ControllerBase
    {
        private readonly JokesContext _context;

        public JokesController(JokesContext context)
        {
            _context = context;
        }

        // GET: api/Jokes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Joke>>> GetJokes()
        {
            if (_context.Jokes == null)
            {
                return NotFound();
            }

            if(Request.Query.ContainsKey("search"))
                return await _context.Jokes.Where(t => (t.Text ?? "").Contains(Request.Query["search"].First() ?? "")).
                                            Include(CAT => CAT.Category).ToListAsync();

            return await _context.Jokes.Include(CAT => CAT.Category).ToListAsync();  
        }

        // GET: api/Jokes/random
        // obtem uma piada aleatoria e mostra apenas a categoria e o texto
        // By Rafael Moreira <rafa4codin@gmail.com>
        [HttpGet("random")]
        public async Task<ActionResult<Random_DTO>> GetRandomJoke()
        {
            if (!_context.Jokes.Any())
            {
                return NotFound();
            }

            var jokes = await _context.Jokes.Include(j => j.Category).ToListAsync();

            Random rnd = new Random();
            int i = rnd.Next(jokes.Count);

            var joke = jokes[i];

            var category = "";
            if (joke.Category != null) category = joke.Category.Name;

            var jokeRandom = new Random_DTO
            {
                Category = category,
                Text = joke.Text
            };

            return jokeRandom;
        }

        // GET: api/Jokes/Category/5
        [HttpGet("Category/{id}")]
        public async Task<ActionResult<IEnumerable<Joke>>> GetJokesByCat(long id)
        {
            if (_context.Jokes == null)
            {
              return NotFound();
            }
                        
            return await _context.Jokes.Where(c => c.Category != null && c.Category.Id==id).Include(CAT => CAT.Category).ToListAsync();
        }

        // GET: api/Jokes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Joke>> GetJoke(long id)
        {
          if (_context.Jokes == null)
          {
              return NotFound();
          }
           // var joke = await _context.Jokes.FindAsync(id);
            var joke = await _context.Jokes.Where(j => j.Id==id ).
                                    Include(CAT => CAT.Category).FirstAsync();
            if (joke == null)
            {
                return NotFound();
            }

            return joke;
        }

        // GET: api/Jokes/contains/{word}
        [HttpGet("contains/{word}")]
        public async Task<ActionResult<IEnumerable<Joke>>> GetJokesByWord(string word)
        {           
            return await _context.Jokes.Where(t => (t.Text ?? "").Contains(word)).
                                    Include(CAT => CAT.Category).ToListAsync();
        }

        // PUT: api/Jokes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJoke(long id, Joke joke)
        {
            if (id != joke.Id)
            {
                return BadRequest();
            }

            _context.Entry(joke).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JokeExists(id))
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

        // POST: api/Jokes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Joke_DTO>> PostJoke(Joke_DTO dto)
        {
            if (_context.Jokes == null)
            {
                return Problem("Entity set 'JokesContext.Jokes'  is null.");
            }

            var theNewJoke = new Joke {
                            Text = dto.Text,
                            Category = _context.Categories.Find(dto.CategoryId)
            };

            _context.Jokes.Add(theNewJoke);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetJoke", new { id = theNewJoke.Id }, theNewJoke);
        }

        // DELETE: api/Jokes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJoke(long id)
        {
            if (_context.Jokes == null)
            {
                return NotFound();
            }
            var joke = await _context.Jokes.FindAsync(id);
            if (joke == null)
            {
                return NotFound();
            }

            _context.Jokes.Remove(joke);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool JokeExists(long id)
        {
            return (_context.Jokes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
