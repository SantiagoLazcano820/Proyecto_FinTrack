using FinTrack.Core.Entities;
using FinTrack.Core.Interfaces;
using FinTrack.Services.Interfaces;

namespace FinTrack.Services.Services
{
    public class TransactionService : ITransactionService
    {
        public readonly IBaseRepository<Transaction> _transactionRepository;
        public readonly IBaseRepository<User> _userRepository;

        public TransactionService(IBaseRepository<Transaction> transactionRepository, IBaseRepository<User> userRepository)
        {
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
        {
            return await _transactionRepository.GetAll();
        }

        public async Task<Transaction> GetTransactionByIdAsync(int id)
        {
            return await _transactionRepository.GetById(id);
        }

        public async Task InsertTransaction(Transaction transaction)
        {
            var user = await _userRepository.GetById(transaction.UserId);
            if (user == null)
            {
                throw new Exception("El usuario no existe");
            }

            if (ContainsForbiddenContent(transaction.Description))
            {
                throw new Exception("El contenido de la descripción no es permitido");
            }
            
            if (transaction.Date > DateTime.Now.AddDays(30))
            {
                throw new Exception("No se permite registrar transacciones con más de 30 días a futuro");
            }

            await _transactionRepository.Insert(transaction);
        }

        public async Task UpdateTransaction(Transaction transaction)
        {
            var existing = await _transactionRepository.GetById(transaction.Id);
            if (existing == null) throw new Exception("La transacción no existe para ser editada.");
            _transactionRepository.Update(transaction);
        }

        public async Task<bool> DeleteTransaction(int id)
        {
            await _transactionRepository.Delete(id);
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