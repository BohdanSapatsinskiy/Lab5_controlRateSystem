using Microsoft.AspNetCore.Mvc;
using controlRateSystem.Data.Models;
using controlRateSystem.Data.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace controlRateSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly IRatesRepository _repository;

        public CurrencyController(IRatesRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var currencies = _repository.Currencies
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    c.Code,
                    c.PriceInDollar,
                    c.CategoryId
                })
                .ToList();

            return Ok(currencies);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var currency = _repository.Currencies
                .Where(c => c.Id == id)
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    c.Code,
                    c.PriceInDollar,
                    c.CategoryId
                })
                .FirstOrDefault();

            if (currency == null)
                return NotFound();

            return Ok(currency);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create([FromBody] Currency currency)
        {
            if (currency.PriceInDollar <= 0)
            {
                return BadRequest("Price must be a positive value.");
            }

            var categoryExists = _repository.Categories.Any(c => c.Id == currency.CategoryId);
            if (!categoryExists)
            {
                return BadRequest("Category not found.");
            }

            _repository.CreateCurrency(currency);

            return CreatedAtAction(nameof(Get), new { id = currency.Id }, currency);
        }

        [HttpPut("{id}")]
        [Authorize]
        public IActionResult Update(int id, [FromBody] Currency currency)
        {
            if (id != currency.Id)
                return BadRequest("Currency ID mismatch.");

            if (currency.PriceInDollar <= 0)
                return BadRequest("Price must be a positive value.");

            var categoryExists = _repository.Categories.Any(c => c.Id == currency.CategoryId);
            if (!categoryExists)
                return BadRequest("Category not found.");

            _repository.SaveCurrency(currency);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            var currency = _repository.Currencies.FirstOrDefault(c => c.Id == id);
            if (currency == null)
                return NotFound();

            _repository.DeleteCurrency(currency);

            return NoContent();
        }
    }
}
