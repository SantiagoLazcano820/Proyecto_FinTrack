using AutoMapper;
using FinTrack.API.Responses;
using FinTrack.Core.DTOs;
using FinTrack.Core.Entities;
using FinTrack.Services.Interfaces;
using FinTrack.Services.Validators;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        //private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;
        private readonly CrearTransactionDtoValidator _crearValidator;
        private readonly ActualizarTransactionDtoValidator _actualizarValidator;

        public TransactionController(
            IMapper mapper,
            //ITransactionRepository transactionRepository,
            ITransactionService transactionService,
            CrearTransactionDtoValidator crearValidator,
            ActualizarTransactionDtoValidator actualizarValidator)
        {
            //_transactionRepository = transactionRepository;
            _transactionService = transactionService;
            _mapper = mapper;
            _crearValidator = crearValidator;
            _actualizarValidator = actualizarValidator;
        }

        #region Con Dto Mapper
        [HttpGet("dto/mapper/")]
        public async Task<IActionResult> GetTransactionsDtoMapper()
        {
            var transactions = await _transactionService.GetTransactionsAsync();
            var transactionsDto = _mapper.Map<IEnumerable<TransactionDto>>(transactions);
            var response = new ApiResponse<IEnumerable<TransactionDto>>(transactionsDto);
            return Ok(response);
        }

        [HttpGet("dto/mapper/{id}")]
        public async Task<IActionResult> GetTransactionByIdDtoMapper(int id)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(id);
            if (transaction == null)
                return NotFound("Transacción no encontrada.");

            var transactionDto = _mapper.Map<TransactionDto>(transaction);
            var response = new ApiResponse<TransactionDto>(transactionDto);
            return Ok(response);
        }

        [HttpPost("dto/mapper/")]
        public async Task<IActionResult> RegisterTransactionDtoMapper(TransactionDto transactionDto)
        {
            var validationResult = await _crearValidator.ValidateAsync(transactionDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    message = "Error de validación",
                    errors = validationResult.Errors.Select(e => new { field = e.PropertyName, error = e.ErrorMessage })
                });
            }

            try
            {
                var transaction = _mapper.Map<Transaction>(transactionDto);
                await _transactionService.InsertTransaction(transaction);
                var response = new ApiResponse<Transaction>(transaction);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al registrar", error = ex.Message });
            }
        }

        [HttpPut("dto/mapper/{id}")]
        public async Task<IActionResult> UpdateTransactionDtoMapper(int id, [FromBody] TransactionDto transactionDto)
        {
            if (id != transactionDto.Id)
                return BadRequest("El ID no coincide.");

            var validationResult = await _actualizarValidator.ValidateAsync(transactionDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    message = "Error de validación",
                    errors = validationResult.Errors.Select(e => new { field = e.PropertyName, error = e.ErrorMessage })
                });
            }
            var transaction = await _transactionService.GetTransactionByIdAsync(id);
            if (transaction == null)
                return NotFound("Transacción no encontrada.");

            try
            {
                _mapper.Map(transactionDto, transaction);
                await _transactionService.UpdateTransaction(transaction);
                var response = new ApiResponse<Transaction>(transaction);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al actualizar", error = ex.Message });
            }
        }

        [HttpDelete("dto/mapper/{id}")]
        public async Task<IActionResult> DeleteTransactionDtoMapper(int id)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(id);
            if (transaction == null)
                return NotFound("Transacción no encontrada.");

            try
            {
                await _transactionService.DeleteTransaction(id);
                var response = new ApiResponse<bool>(true);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al eliminar", error = ex.Message });
            }
        }
        #endregion
    }
}