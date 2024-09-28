using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB_253504_LIANHA.API.Data;
using WEB_253504_LIANHA.Domain.Entities;
using WEB_253504_LIANHA.Domain.Models;
using WEB_253504_LIANHA.API.Services.CategoryService;

namespace WEB_253504_LIANHA.API.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class AutomobileCategoriesController : ControllerBase
    {
        private readonly IAutomobileCategoryService _categoryService;

        public AutomobileCategoriesController(IAutomobileCategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: api/AutomobileCategories
        [HttpGet]
        public async Task<ActionResult<ResponseData<List<AutomobileCategory>>>> GetAutomobileCategories()
        {
            return await _categoryService.GetAutomobileCategoryListAsync();
        }

        // GET: api/AutomobileCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AutomobileCategory>> GetAutomobileCategory(int id)
        {
            var automobileCategory = await _categoryService.GetAutomobileCategoryAsync(id);

            if (automobileCategory.Data == null)
            {
                return NotFound();
            }

            return automobileCategory.Data;
        }

        // PUT: api/AutomobileCategories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAutomobileCategory(int id, AutomobileCategory automobileCategory)
        {
            if (id != automobileCategory.Id)
            {
                return BadRequest();
            }

            //_context.Entry(automobileCategory).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!AutomobileCategoryExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            return NoContent();
        }

        // POST: api/AutomobileCategories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AutomobileCategory>> PostAutomobileCategory(AutomobileCategory automobileCategory)
        {
            //_context.AutomobileCategories.Add(automobileCategory);
            //await _context.SaveChangesAsync();

            return CreatedAtAction("GetAutomobileCategory", new { id = automobileCategory.Id }, automobileCategory);
        }

        // DELETE: api/AutomobileCategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAutomobileCategory(int id)
        {
            //var automobileCategory = await _context.AutomobileCategories.FindAsync(id);
            //if (automobileCategory == null)
            //{
            //    return NotFound();
            //}

            //_context.AutomobileCategories.Remove(automobileCategory);
            //await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AutomobileCategoryExists(int id)
        {
            return false;
        }
    }
}
