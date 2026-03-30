using FinTrack.Core.Entities;
using FinTrack.Core.Interfaces;
using FinTrack.Services.Interfaces;

namespace FinTrack.Services.Services
{
    public class TransactionService : ITransactionService
    {
        public readonly ITransactionRepository _transactionRepository;
        public readonly IUserRepository _userRepository;

        public TransactionService(ITransactionRepository transactionRepository, IUserRepository userRepository)
        {
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsAsync()
        {
            return await _transactionRepository.GetTransactionsAsync();
        }

        public async Task<Transaction> GetTransactionByIdAsync(int id)
        {
            return await _transactionRepository.GetTransactionByIdAsync(id);
        }

        public async Task InsertTransaction(Transaction transaction)
        {
            var user = await _userRepository.GetUserByIdAsync(transaction.UserId);
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

            await _transactionRepository.InsertTransactionAsync(transaction);
        }

        public async Task UpdateTransaction(Transaction transaction)
        {
            var existing = await _transactionRepository.GetTransactionByIdAsync(transaction.Id);
            if (existing == null) throw new Exception("La transacción no existe para ser editada.");
            await _transactionRepository.UpdateTransactionAsync(transaction);
        }

        public async Task<bool> DeleteTransaction(int id)
        {
            await _transactionRepository.DeleteTransactionAsync(id);
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