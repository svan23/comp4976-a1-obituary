
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assignment1.Models;
using Assignment1.Data;
using Microsoft.AspNetCore.Authorization;


public class ObituaryController : Controller
{
    private const string BearerScheme = "Identity.Bearer";
    private readonly ApplicationDbContext _context;

    public ObituaryController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: OBITUARYS
    public async Task<IActionResult> Index()
    {
        return View(await _context.Obituaries.ToListAsync());
    }


    // GET: api/obituary/all (JSON API endpoint)
    [Authorize(AuthenticationSchemes = BearerScheme)]
    [HttpGet("api/obituary/all")]
    public async Task<ActionResult<IEnumerable<Obituary>>> GetObituaries()
    {
        return await _context.Obituaries.ToListAsync();
    }


    // GET: OBITUARYS/Details/5
    [Authorize]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var obituary = await _context.Obituaries
            .FirstOrDefaultAsync(m => m.Id == id);
        if (obituary == null)
        {
            return NotFound();
        }

        return View(obituary);
    }


    // GET: api/obituary/Details/5
    [Authorize(AuthenticationSchemes = BearerScheme)]
    [HttpGet("api/obituary/details/{id}")]
    public async Task<ActionResult<Obituary>> GetObituaryDetails(int id)
    {
        var obituary = await _context.Obituaries
            .FirstOrDefaultAsync(m => m.Id == id);
        if (obituary == null)
        {
            return NotFound();
        }

        return obituary;
    }





    // GET: OBITUARYS/Create
    [Authorize]
    public IActionResult Create()
    {
        return View();
    }

    // POST: OBITUARYS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Obituary obituary)
    {
        if (ModelState.IsValid)
        {
            _context.Add(obituary);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(obituary);
    }


    [Authorize(AuthenticationSchemes = BearerScheme)]
    [HttpPost("/api/obituary")]
    public async Task<ActionResult<Obituary>> CreateObituary([FromBody] Obituary obituary)
    {
        if (ModelState.IsValid)
        {
            _context.Add(obituary);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetObituaryDetails), new { id = obituary.Id }, obituary);
        }
        return BadRequest(ModelState);
    }




    // GET: OBITUARYS/Edit/5
    [Authorize]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var obituary = await _context.Obituaries.FindAsync(id);
        if (obituary == null)
        {
            return NotFound();
        }
        return View(obituary);
    }


    // POST: OBITUARYS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, Obituary obituary)
    {
        if (id != obituary.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(obituary);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ObituaryExists(obituary.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(obituary);
    }


    // PUT: api/obituary/{id}
    [Authorize(AuthenticationSchemes = BearerScheme)]
    [HttpPut("/api/obituary/{id}")]
    public async Task<IActionResult> UpdateObituary(int id, [FromBody] Obituary obituary)
    {
        if (id != obituary.Id)
        {
            return BadRequest("ID mismatch");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _context.Entry(obituary).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ObituaryExists(obituary.Id))
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


    // DELETE: api/obituary/{id}
    [Authorize(AuthenticationSchemes = BearerScheme)]
    [HttpDelete("/api/obituary/{id}")]
    public async Task<IActionResult> DeleteObituary(int id)
    {
        var obituary = await _context.Obituaries.FindAsync(id);
        if (obituary == null)
        {
            return NotFound();
        }

        _context.Obituaries.Remove(obituary);
        await _context.SaveChangesAsync();

        return NoContent();
    }



    // GET: OBITUARYS/Delete/5
    [Authorize]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var obituary = await _context.Obituaries
            .FirstOrDefaultAsync(m => m.Id == id);
        if (obituary == null)
        {
            return NotFound();
        }

        return View(obituary);
    }

    // POST: OBITUARYS/Delete/5
    [Authorize]
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var obituary = await _context.Obituaries.FindAsync(id);
        if (obituary != null)
        {
            _context.Obituaries.Remove(obituary);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }



    [HttpGet]
    [Authorize]
    [Route("Obituary/Search")]

    public async Task<IActionResult> Search(string? name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return RedirectToAction(nameof(Index));
        }

        var obituaries = await _context.Obituaries.Where(obit => obit.FullName.ToLower().Contains(name.ToLower())).ToListAsync();

        return View(obituaries);
    }



    private bool ObituaryExists(int? id)
    {
        return _context.Obituaries.Any(e => e.Id == id);
    }
}
