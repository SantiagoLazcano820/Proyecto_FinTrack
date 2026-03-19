using FinTrack.Core.DTOs;
using FinTrack.Core.Entities;
using FinTrack.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionController(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        [HttpGet("dto")]
        public async Task<IActionResult> GetTransactionsDto()
        {
            var transactions = await _transactionRepository.GetTransactionsAsync();
            var transactionsDto = transactions.Select(t => new TransactionDto
            {
                Id = t.Id,
                UserId = t.UserId,
                CategoryId = t.CategoryId,
                Amount = t.Amount,
                Type = t.Type,
                Date = t.Date.ToString("dd-MM-yyyy")
            });
            return Ok(transactionsDto);
        }

        [HttpPost("dto")]
        public async Task<IActionResult> RegisterTransactionDto(TransactionDto transactionDto)
        {
            var transaction = new Transaction
            {
                UserId = transactionDto.UserId,
                CategoryId = transactionDto.CategoryId,
                Amount = transactionDto.Amount,
                Type = transactionDto.Type,
                Date = DateTime.Now,
                Description = "Registro desde API"
            };

            await _transactionRepository.InsertTransactionAsync(transaction);
            transactionDto.Id = transaction.Id;
            return Ok(transactionDto);
        }

        [HttpPut("dto/{id}")]
        public async Task<IActionResult> UpdateTransactionDto(int id, [FromBody] TransactionDto dto)
        {
            if (id != dto.Id) return BadRequest("ID no coincide");

            var transaction = await _transactionRepository.GetTransactionByIdAsync(id);
            if (transaction == null) return NotFound();

            transaction.Amount = dto.Amount;
            transaction.CategoryId = dto.CategoryId;
            transaction.Type = dto.Type;

            await _transactionRepository.UpdateTransactionAsync(transaction);
            return NoContent();
        }

        [HttpDelete("dto/{id}")]
        public async Task<IActionResult> DeleteTransactionDto(int id)
        {
            await _transactionRepository.DeleteTransactionAsync(id);
            return NoContent();
        }
    }
}