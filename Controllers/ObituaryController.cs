
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assignment1.Models;
using Assignment1.Data;

public class ObituaryController : Controller
{
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

    // GET: OBITUARYS/Details/5
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

    // GET: OBITUARYS/Create
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

    // GET: OBITUARYS/Edit/5
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

    // GET: OBITUARYS/Delete/5
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

    private bool ObituaryExists(int? id)
    {
        return _context.Obituaries.Any(e => e.Id == id);
    }
}
