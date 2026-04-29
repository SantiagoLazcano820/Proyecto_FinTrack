using AutoMapper;
using FinTrack.API.Responses;
using FinTrack.Core.DTOs;
using FinTrack.Core.Entities;
using FinTrack.Core.Exceptions;
using FinTrack.Core.QueryFilters;
using FinTrack.Services.Interfaces;
using FinTrack.Services.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
        public async Task<IActionResult> GetTransactionsDtoMapper([FromQuery] TransactionQueryFilter filters)
        {
            var transactions = await _transactionService.GetAllTransactionsAsync(filters);
            var transactionsDto = _mapper.Map<IEnumerable<TransactionDto>>(transactions);
            var response = new ApiResponse<IEnumerable<TransactionDto>>(transactionsDto);
            return Ok(response);
        }

        [HttpGet("dto/mapper/{id}")]
        public async Task<IActionResult> GetTransactionByIdDtoMapper(int id)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(id);
            if (transaction == null)
                throw new BusinessException("Transacción no encontrada.", HttpStatusCode.NotFound);

            var transactionDto = _mapper.Map<TransactionDto>(transaction);
            var response = new ApiResponse<TransactionDto>(transactionDto);
            return Ok(response);
        }

        [HttpGet("dto/mapper/dapper/")]
        public async Task<IActionResult> GetTransactionsDtoMapperDapper(int userId)
        {
            var transactions = await _transactionService.GetAllTransactionsDapperAsync();
            var transactionsDto = _mapper.Map<IEnumerable<TransactionDto>>(transactions);
            var response = new ApiResponse<IEnumerable<TransactionDto>>(transactionsDto);
            return Ok(response);
        }

        [HttpGet("dto/mapper/{id}")]
        public async Task<IActionResult> GetTransactionByIdDtoMappeDapperr(int id)
        {
            var transaction = await _transactionService.GetTransactionByIdDapperAsync(id);
            if (transaction == null)
                throw new BusinessException("Transacción no encontrada.", HttpStatusCode.NotFound);

            var transactionDto = _mapper.Map<TransactionDto>(transaction);
            var response = new ApiResponse<TransactionDto>(transactionDto);
            return Ok(response);
        }

        [HttpPost("dto/mapper/")]
        public async Task<IActionResult> InsertTransactionDtoMapper(TransactionDto transactionDto)
        {
            var validationResult = await _crearValidator.ValidateAsync(transactionDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            try
            {
                var transaction = _mapper.Map<Transaction>(transactionDto);
                await _transactionService.InsertTransaction(transaction);
                var response = new ApiResponse<Transaction>(transaction);
                return Ok(response);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Error crítico al registrar la transacción.", ex);
            }
        }

        [HttpPut("dto/mapper/{id}")]
        public async Task<IActionResult> UpdateTransactionDtoMapper(int id, [FromBody] TransactionDto transactionDto)
        {
            if (id != transactionDto.Id)
                throw new BusinessException("El ID de la transacción no coincide.", HttpStatusCode.BadRequest);

            var validationResult = await _actualizarValidator.ValidateAsync(transactionDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            var transaction = await _transactionService.GetTransactionByIdAsync(id);
            if (transaction == null)
                throw new BusinessException("La transacción no existe para ser editada.", HttpStatusCode.NotFound);

            try
            {
                _mapper.Map(transactionDto, transaction);
                _transactionService.UpdateTransaction(transaction);
                var response = new ApiResponse<Transaction>(transaction);
                return Ok(response);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Error inesperado al actualizar la transacción.", ex);
            }
        }

        [HttpDelete("dto/mapper/{id}")]
        public async Task<IActionResult> DeleteTransactionDtoMapper(int id)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(id);
            if (transaction == null)
                throw new BusinessException("Transacción no encontrada para eliminar.", HttpStatusCode.NotFound);

            try
            {
                await _transactionService.DeleteTransaction(id);
                var response = new ApiResponse<bool>(true);
                return Ok(response);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Error crítico al intentar eliminar la transacción.", ex);
            }
        }

        [HttpGet("balance/dapper")]
        public async Task<IActionResult> GetMonthlyBalance(int userId, int month, int year)
        {
            var balance = await _transactionService.GetMonthlyBalance(userId, month, year);

            if (balance == null)
            {
                throw new BusinessException("No se encontró información de balance para el periodo especificado.", HttpStatusCode.NotFound);
            }

            var response = new ApiResponse<MonthlyBalanceDto>(balance);

            return Ok(response);
        }
        #endregion
    }
}