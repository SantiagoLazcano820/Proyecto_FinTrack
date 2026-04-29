using FinTrack.Core.DTOs;
using FinTrack.Core.Entities;
using FinTrack.Core.Exceptions;
using FinTrack.Core.Helpers;
using FinTrack.Core.Interfaces;
using FinTrack.Core.QueryFilters;
using FinTrack.Services.Interfaces;
using System.Diagnostics;
using System.Net;

namespace FinTrack.Services.Services
{
    public class TransactionService : ITransactionService
    {
        //public readonly IBaseRepository<Transaction> _transactionRepository;
        //public readonly IBaseRepository<User> _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        //public TransactionService(IBaseRepository<Transaction> transactionRepository, IBaseRepository<User> userRepository)
        //{
        //    _transactionRepository = transactionRepository;
        //    _userRepository = userRepository;
        //}

        public TransactionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync(TransactionQueryFilter filters)
        {
            var transactions = await _unitOfWork.TransactionRepository.GetAll();

            if (filters != null)
            {
                if (filters.UserId != null)
                {
                    transactions = transactions.Where(x => x.UserId == filters.UserId);
                }
                if (filters.CategoryId != null)
                {
                    transactions = transactions.Where(x => x.CategoryId == filters.CategoryId);
                }
                if (!string.IsNullOrEmpty(filters.Type))
                {
                    transactions = transactions.Where(x => x.Type == filters.Type);
                }
                
                if (!string.IsNullOrEmpty(filters.Date))
                {
                    string fechaAux = Procesos.ParseFechaFlexible(filters.Date);
                    if (fechaAux != null)
                    {
                        var fechaComparar = DateTime.ParseExact(fechaAux, "dd/MM/yyyy", null);
                        transactions = transactions.Where(x => x.Date.Date == fechaComparar.Date);
                    }
                }
                if (!string.IsNullOrEmpty(filters.Description))
                {
                    transactions = transactions.Where(x => x.Description.ToLower().Contains(filters.Description.ToLower()));
                }
            }
            return transactions;
        }

        public async Task<Transaction> GetTransactionByIdAsync(int id)
        {
            return await _unitOfWork.TransactionRepository.GetById(id);
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionsDapperAsync()
        {
            return await _unitOfWork.TransactionRepository.GetAllTransactionsDapperAsync();
        }

        public async Task<Transaction> GetTransactionByIdDapperAsync(int id)
        {
            return await _unitOfWork.TransactionRepository.GetTransactionByIdDapperAsync(id);
        }

        public async Task InsertTransaction(Transaction transaction)
        {
            var user = await _unitOfWork.UserRepository.GetById(transaction.UserId);
            if (user == null)
            {
                throw new BusinessException("El usuario no existe", HttpStatusCode.BadRequest);
            }

            if (ContainsForbiddenContent(transaction.Description))
            {
                throw new BusinessException("La descripción contiene palabras no permitidas.", HttpStatusCode.UnprocessableEntity);
            }
            
            if (transaction.Date > DateTime.Now.AddDays(30))
            {
                throw new BusinessException("No se permite registrar transacciones con más de 30 días a futuro.", HttpStatusCode.BadRequest);
            }

            var userTransactions = await _unitOfWork.TransactionRepository.GetTransactionsByUserIdDapperAsync(transaction.UserId);

            if (userTransactions.Count() < 5)
            {
                var lastTransaction = userTransactions.OrderByDescending(x => x.Date).FirstOrDefault();

                if (lastTransaction != null)
                {
                    var minutesSinceLast = (DateTime.Now - lastTransaction.Date).TotalMinutes;
                    if (minutesSinceLast < 1)
                    {
                        throw new BusinessException("Usuarios nuevos deben esperar 1 minuto entre registros.", HttpStatusCode.TooManyRequests);
                    }
                }
            }

            await _unitOfWork.TransactionRepository.Insert(transaction);
            await _unitOfWork.SaveChangesAsync();
        }

        public void UpdateTransaction(Transaction transaction)
        {
            var existing = _unitOfWork.TransactionRepository.GetById(transaction.Id).Result;

            if (existing == null)
            {
                throw new BusinessException("La transacción no existe para ser editada", HttpStatusCode.BadRequest);
            }
            if (existing.UserId != transaction.UserId)
            {
                throw new BusinessException("No tienes permiso para editar esta transacción.", HttpStatusCode.Forbidden);
            }
            if (existing.Date < DateTime.Now.AddDays(-60))
            {
                throw new BusinessException("No se pueden editar transacciones con más de 60 días de antigüedad.", HttpStatusCode.BadRequest);
            }
            if (!existing.Type.Equals(transaction.Type, StringComparison.OrdinalIgnoreCase))
            {
                throw new BusinessException("No se permite cambiar el tipo de transacción.", HttpStatusCode.BadRequest);
            }
            if (string.IsNullOrWhiteSpace(transaction.Description))
            {
                transaction.Description = $"(Editado el {DateTime.Now:dd/MM/yyyy})";
            }
            if (ContainsForbiddenContent(transaction.Description))
            {
                throw new BusinessException("La nueva descripción contiene palabras no permitidas.", HttpStatusCode.UnprocessableEntity);
            }
            var category = _unitOfWork.CategoryRepository.GetById(transaction.CategoryId).Result;
            if (category == null || category.UserId != transaction.UserId)
            {
                throw new BusinessException("La categoría seleccionada no es válida o no te pertenece.", HttpStatusCode.BadRequest);
            }

            _unitOfWork.TransactionRepository.Update(transaction);
            _unitOfWork.SaveChanges();
        }

        public async Task<bool> DeleteTransaction(int id)
        {
            var existing = await _unitOfWork.TransactionRepository.GetById(id);
            if (existing == null)
            {
                throw new BusinessException("La transacción no existe.", HttpStatusCode.NotFound);
            }
            if (existing.Date < DateTime.Now.AddDays(-60))
            {
                throw new BusinessException("No se pueden eliminar transacciones con más de 60 días de antigüedad.", HttpStatusCode.BadRequest);
            }
            if (existing.Type.Equals("Income", StringComparison.OrdinalIgnoreCase))
            {
                decimal saldoActual = await _unitOfWork.TransactionRepository.GetTotalBalanceByUserId(existing.UserId);

                if (saldoActual - existing.Amount < 0)
                {
                    throw new BusinessException("No puedes eliminar este ingreso porque tu saldo quedaría en negativo.", HttpStatusCode.BadRequest);
                }
            }

            await _unitOfWork.TransactionRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<MonthlyBalanceDto> GetMonthlyBalance(int userId, int month, int year)
        {
            DateTime fechaConsulta = new DateTime(year, month, 1);
            DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            if (fechaConsulta > fechaActual)
            {
                return new MonthlyBalanceDto
                {
                    Message = "No hay datos proyectados para meses futuros.",
                    TotalIncomes = 0,
                    TotalExpenses = 0,
                    IsDeficit = false
                };
            }
            if (fechaConsulta < fechaActual.AddMonths(-24))
            {
                throw new BusinessException("Solo se permite consultar balances de hasta 24 meses atrás.", HttpStatusCode.BadRequest);
            }

            var balance = await _unitOfWork.TransactionRepository.GetMonthlyBalanceDapperAsync(userId, month, year);

            if (balance != null)
            {
                balance.IsDeficit = balance.TotalExpenses > balance.TotalIncomes;
            }

            return balance ?? new MonthlyBalanceDto();
        }

        public readonly string[] ForbiddenWords = 
            { 
            "odio", 
            "estafa", 
            "violencia", 
            "ilegal", 
            "alcohol", 
            "drogas", 
            "discriminacion",
            "amenazas"
        };

        public bool ContainsForbiddenContent(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) 
                return false;

            foreach (var word in ForbiddenWords)
            {
                if (text.Contains(word, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}