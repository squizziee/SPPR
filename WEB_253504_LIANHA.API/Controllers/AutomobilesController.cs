﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB_253504_LIANHA.Domain.Entities;
using WEB_253504_LIANHA.Domain.Models;
using WEB_253504_LIANHA.API.Services;
using WEB_253504_LIANHA.API.Services.ProductService;
using Microsoft.AspNetCore.Authorization;

namespace WEB_253504_LIANHA.API.Controllers
{
    [Route("api/automobiles")]
    [ApiController]
    public class AutomobilesController : ControllerBase
    {
        private readonly IAutomobileService _automobileService;

        public AutomobilesController(IAutomobileService automobileService)
        {
            _automobileService = automobileService;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseData<ListModel<Automobile>>>> GetAutomobiles()
        {
            return (await _automobileService.GetAutomobileListAsync());
        }

        // GET: api/Automobiles/categories
        [Authorize]
        [HttpGet("categories/{category}")]
        public async Task<ActionResult<ResponseData<ListModel<Automobile>>>> GetAutomobiles(string? category, int pageNo = 0, int pageSize = 3)
        {
            if (category == "all") {
                category = null;
            }
            return (await _automobileService.GetAutomobileListAsync(category, pageNo, pageSize));
        }

        // GET: api/Automobiles/5
        // [Authorize(Roles = "POWER_USER")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseData<Automobile>>> GetAutomobile(int id)
        {
            var automobile = (await _automobileService.GetAutomobileByIdAsync(id)).Data!;
            return ResponseData<Automobile>.Success(automobile);
        }

        // PUT: api/Automobiles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "POWER_USER")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAutomobile(int id, Automobile automobile)
        {
            await _automobileService.UpdateAutomobileAsync(id, automobile, null);

            return NoContent();
        }

        // POST: api/Automobiles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "POWER_USER")]
        [HttpPost]
        public async Task<ActionResult<Automobile>> PostAutomobile(Automobile automobile)
        {
            await _automobileService.CreateAutomobileAsync(automobile);

            return CreatedAtAction("GetAutomobile", new { id = automobile.Id }, automobile);
        }

        // DELETE: api/Automobiles/5
        [Authorize(Roles = "POWER_USER")]
        [HttpDelete]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAutomobile(int id)
        {
            await _automobileService.DeleteAutomobileAsync(id);

            return NoContent();
        }
    }
}
