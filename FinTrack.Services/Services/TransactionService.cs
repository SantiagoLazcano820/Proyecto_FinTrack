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

            var userTransactions = await _unitOfWork.TransactionRepository.GetTransactionsByUserIdAsync(transaction.UserId);

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
            var existing = _unitOfWork.TransactionRepository.GetById(transaction.Id);
            if (existing == null)
            {
                throw new BusinessException("El transaccion no existe para ser editada", HttpStatusCode.BadRequest);
            }
            _unitOfWork.TransactionRepository.Update(transaction);
            _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> DeleteTransaction(int id)
        {
            var existing = await _unitOfWork.TransactionRepository.GetById(id);
            if (existing == null)
            {
                return false;
            }
            await _unitOfWork.TransactionRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
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