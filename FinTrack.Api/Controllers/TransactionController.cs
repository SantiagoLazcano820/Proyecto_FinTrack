using FinTrack.Core.DTOs;
using FinTrack.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using FinTrack.Api.Responses;
using FluentValidation;
using FinTrack.Core.Validations;

namespace FinTrack.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly CrearTransactionValidator _crearValidator;
        private readonly ActualizarTransactionValidator _actualizarValidator;

        public TransactionController(ITransactionService transactionService, CrearTransactionValidator crearValidator, ActualizarTransactionValidator actualizarValidator)
        {
            _transactionService = transactionService;
            _crearValidator = crearValidator;
            _actualizarValidator = actualizarValidator;
        }

        [HttpGet("dto")]
        public async Task<IActionResult> GetTransactionsDto()
        {
            var transactionsDto = await _transactionService.GetTransactionsAsync();
            var response = new ApiResponse<IEnumerable<TransactionDto>>(transactionsDto);
            return Ok(response);
        }

        [HttpGet("dto/balance/{userId}")]
        public async Task<IActionResult> GetUserBalance(int userId)
        {
            try
            {
                var balance = await _transactionService.GetUserBalanceAsync(userId);
                var response = new ApiResponse<decimal>(balance);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al calcular el balance", error = ex.Message });
            }
        }

        [HttpPost("dto")]
        public async Task<IActionResult> RegisterTransactionDto([FromBody] TransactionDto transactionDto)
        {
            var validationResult = await _crearValidator.ValidateAsync(transactionDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    message = "Error de validación",
                    errors = validationResult.Errors.Select(e => new
                    {
                        field = e.PropertyName,
                        error = e.ErrorMessage
                    })
                });
            }

            try
            {
                var createdTransactionDto = await _transactionService.RegisterTransactionAsync(transactionDto);
                var response = new ApiResponse<TransactionDto>(createdTransactionDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al registrar la transacción", error = ex.Message });
            }
        }

        [HttpPut("dto/{id}")]
        public async Task<IActionResult> UpdateTransactionDto(int id, [FromBody] TransactionDto dto)
        {
            if (id != dto.Id) 
                return BadRequest("El ID de la transacción no coincide.");

            var validationResult = await _actualizarValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    message = "Error de validación",
                    errors = validationResult.Errors.Select(e => new
                    {
                        field = e.PropertyName,
                        error = e.ErrorMessage
                    })
                });
            }

            try
            {
                var updated = await _transactionService.UpdateTransactionAsync(id, dto);
                if (!updated) 
                    return NotFound("Transacción no encontrada.");

                return Ok(new ApiResponse<bool>(true));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al actualizar la transacción", error = ex.Message });
            }
        }

        [HttpDelete("dto/{id}")]
        public async Task<IActionResult> DeleteTransactionDto(int id)
        {
            var deleted = await _transactionService.DeleteTransactionAsync(id);
            if (!deleted) 
                return NotFound("Transacción no encontrada.");

            return Ok(new ApiResponse<bool>(true));
        }
    }
}