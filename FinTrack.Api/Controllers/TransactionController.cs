using AutoMapper;
using FinTrack.API.Responses;
using FinTrack.Core.CustomEntities;
using FinTrack.Core.DTOs;
using FinTrack.Core.Entities;
using FinTrack.Core.Exceptions;
using FinTrack.Core.QueryFilters;
using FinTrack.Services.Interfaces;
using FinTrack.Services.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FinTrack.Api.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;
        private readonly CrearTransactionDtoValidator _crearValidator;
        private readonly ActualizarTransactionDtoValidator _actualizarValidator;

        public TransactionController(
            IMapper mapper,
            ITransactionService transactionService,
            CrearTransactionDtoValidator crearValidator,
            ActualizarTransactionDtoValidator actualizarValidator)
        {
            _transactionService = transactionService;
            _mapper = mapper;
            _crearValidator = crearValidator;
            _actualizarValidator = actualizarValidator;
        }

        #region Con Dto Mapper
        /// <summary>
        /// Recupera una lista paginada de transacciones como objetos de transferencia de datos (DTO) según los filtros especificados.
        /// </summary>
        /// <remarks>Este método utiliza un mapeador para convertir las transacciones recuperadas en DTO, que luego se 
        /// devuelven junto con la información de paginación. Si se produce un error durante el proceso, se devuelve un 
        /// código de estado 500 con los detalles del error.<see cref="ApiResponse{T}"/></remarks>
        /// <param name="filters">Los filtros que se aplicarán al recuperar transacciones, como la paginación y los criterios de búsqueda.</param>
        /// <returns>Un <see cref="IActionResult"/> que contiene un <see cref="ApiResponse{T}"/> con una colección de objetos <see cref="TransactionDto"/> 
        /// y detalles de paginación.</returns>
        /// <response code="200">Retorna la lista de [TransactionDto]</response>
        /// <response code="400">Petición incorrecta o filtros inválidos</response>
        /// <response code="500">Error interno del servidor</response>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<TransactionDto>>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("dto/mapper/")]
        public async Task<IActionResult> GetTransactionsDtoMapper([FromQuery] TransactionQueryFilter filters)
        {
            var transactions = await _transactionService.GetAllTransactionsAsync(filters);
            var transactionsDto = _mapper.Map<IEnumerable<TransactionDto>>(transactions.Pagination);

            var pagination = new Pagination
            {
                TotalCount = transactions.Pagination.TotalCount,
                PageSize = transactions.Pagination.PageSize,
                CurrentPage = transactions.Pagination.CurrentPage,
                TotalPages = transactions.Pagination.TotalPages,
                HasNextPage = transactions.Pagination.HasNextPage,
                HasPreviousPage = transactions.Pagination.HasPreviousPage
            };

            var response = new ApiResponse<IEnumerable<TransactionDto>>(transactionsDto)
            {
                Pagination = pagination,
                Messages = transactions.Messages
            };

            return StatusCode((int)transactions.StatusCode, response);
        }

        /// <summary>
        /// Recupera el detalle de una transacción específica por su identificador único.
        /// </summary>
        /// <remarks>Busca en la base de datos la transacción solicitada. Si no existe, arroja una excepción de negocio 404.</remarks>
        /// <param name="id">Identificador único de la transacción.</param>
        /// <returns>Un <see cref="ApiResponse{T}"/> con el objeto <see cref="TransactionDto"/> encontrado.</returns>
        /// <response code="200">Retorna la transacción solicitada</response>
        /// <response code="404">Transacción no encontrada</response>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<TransactionDto>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
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

        /// <summary>
        /// Recupera una lista paginada de transacciones utilizando Dapper para optimizar el rendimiento.
        /// </summary>
        /// <remarks>Ideal para consultas masivas de lectura directa. Devuelve la estructura paginada estándar.</remarks>
        /// <param name="filters">Filtros de búsqueda y paginación.</param>
        /// <returns>Colección paginada de <see cref="TransactionDto"/>.</returns>
        /// <response code="200">Retorna la lista de transacciones obtenida con Dapper</response>
        /// <response code="500">Error interno del servidor</response>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<TransactionDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("dto/mapper/dapper/")]
        public async Task<IActionResult> GetTransactionsDtoMapperDapper([FromQuery] TransactionQueryFilter filters)
        {
            var transactions = await _transactionService.GetAllTransactionsDapperAsync(filters);
            var transactionsDto = _mapper.Map<IEnumerable<TransactionDto>>(transactions.Pagination);

            var pagination = new Pagination
            {
                TotalCount = transactions.Pagination.TotalCount,
                PageSize = transactions.Pagination.PageSize,
                CurrentPage = transactions.Pagination.CurrentPage,
                TotalPages = transactions.Pagination.TotalPages,
                HasNextPage = transactions.Pagination.HasNextPage,
                HasPreviousPage = transactions.Pagination.HasPreviousPage
            };

            var response = new ApiResponse<IEnumerable<TransactionDto>>(transactionsDto)
            {
                Pagination = pagination,
                Messages = transactions.Messages
            };

            return StatusCode((int)transactions.StatusCode, response);
        }

        /// <summary>
        /// Registra una nueva transacción financiera en el sistema.
        /// </summary>
        /// <remarks>Valida los campos obligatorios a través del FluentValidator antes de procesar el guardado de datos.</remarks>
        /// <param name="transactionDto">Objeto que contiene los datos de la transacción a crear.</param>
        /// <returns>La transacción creada con su ID asignado.</returns>
        /// <response code="200">Transacción registrada exitosamente</response>
        /// <response code="400">Error de validación en los datos de entrada</response>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<Transaction>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpPost("dto/mapper/")]
        public async Task<IActionResult> InsertTransactionDtoMapper(TransactionDto transactionDto)
        {
            var validationResult = await _crearValidator.ValidateAsync(transactionDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var transaction = _mapper.Map<Transaction>(transactionDto);
            await _transactionService.InsertTransaction(transaction);
            var response = new ApiResponse<Transaction>(transaction);
            return Ok(response);
        }

        /// <summary>
        /// Actualiza por completo una transacción existente.
        /// </summary>
        /// <remarks>Verifica la coherencia del ID de la URL y del cuerpo, valida reglas de negocio y actualiza el registro en la base de datos.</remarks>
        /// <param name="id">ID de la transacción a modificar.</param>
        /// <param name="transactionDto">Datos modificados del objeto.</param>
        /// <returns>El objeto de la transacción con los cambios aplicados.</returns>
        /// <response code="200">Modificación realizada correctamente</response>
        /// <response code="400">El ID especificado no coincide</response>
        /// <response code="404">La transacción a editar no existe</response>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<Transaction>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
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

            _mapper.Map(transactionDto, transaction);
            _transactionService.UpdateTransaction(transaction);
            var response = new ApiResponse<Transaction>(transaction);
            return Ok(response);
        }

        /// <summary>
        /// Elimina una transacción del sistema de forma permanente.
        /// </summary>
        /// <param name="id">ID de la transacción a dar de baja.</param>
        /// <returns>Un valor booleano indicando el éxito de la operación.</returns>
        /// <response code="200">Transacción eliminada con éxito</response>
        /// <response code="404">La transacción no fue encontrada</response>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpDelete("dto/mapper/{id}")]
        public async Task<IActionResult> DeleteTransactionDtoMapper(int id)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(id);
            if (transaction == null)
                throw new BusinessException("Transacción no encontrada para eliminar.", HttpStatusCode.NotFound);

            await _transactionService.DeleteTransaction(id);
            var response = new ApiResponse<bool>(true);
            return Ok(response);
        }

        /// <summary>
        /// Obtiene el resumen del balance mensual (ingresos y egresos) de un usuario mediante Dapper.
        /// </summary>
        /// <param name="userId">ID del usuario consultante.</param>
        /// <param name="month">Número del mes a calcular (1-12).</param>
        /// <param name="year">Año de la consulta financiera.</param>
        /// <returns>Un objeto de tipo <see cref="MonthlyBalanceDto"/> con los totales de ingresos y egresos.</returns>
        /// <response code="200">Balance calculado correctamente</response>
        /// <response code="404">No hay datos financieros para el periodo indicado</response>
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<MonthlyBalanceDto>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [HttpGet("balance/dapper")]
        public async Task<IActionResult> GetMonthlyBalance(int userId, int month, int year)
        {
            var balance = await _transactionService.GetMonthlyBalance(userId, month, year);
            if (balance == null)
                throw new BusinessException("No se encontró información de balance para el periodo especificado.", HttpStatusCode.NotFound);

            var response = new ApiResponse<MonthlyBalanceDto>(balance);
            return Ok(response);
        }
        #endregion
    }
}