using GLCompanyAPI.Models;
using GLCompanyAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GLCompanyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CompanyController(ICompanyService service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await service.GetAllAsync());



        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var company = await service.GetByIdAsync(id);
            return company == null ? NotFound() : Ok(company);
        }
        
        
        [HttpGet("{isin}")]
        public async Task<IActionResult> GetByIsin(string isin)
        {
            var company = await service.GetByIsinAsync(isin);
            return company == null ? NotFound() : Ok(company);
        }

        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Company company)
        {
            if (!await service.AddAsync(company)) return BadRequest("ISIN must be unique");
            return CreatedAtAction(nameof(GetById), new { id = company.Id }, company);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Company company)
        {
            if (id != company.Id) return BadRequest("Mismatched ID");
            return await service.UpdateAsync(company) ? NoContent() : NotFound();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) => await service.DeleteAsync(id) ? NoContent() : NotFound();
    }
}