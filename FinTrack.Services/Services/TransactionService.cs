using FinTrack.Core.Interfaces;
using AutoMapper;
using FinTrack.Core.Entities;
using FinTrack.Core.DTOs;

namespace FinTrack.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly ICategoryRepository _categoryRepository;

        public readonly string[] ForbiddenWords =
        {
            "violencia", "odio", "grosería", "discriminación", "pornografía"
        };

        public TransactionService(ITransactionRepository transactionRepository, IMapper mapper, IUserRepository userRepository, ICategoryRepository categoryRepository)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<TransactionDto>> GetTransactionsAsync()
        {
            var transactions = await _transactionRepository.GetTransactionsAsync();
            return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
        }

        public async Task<TransactionDto> RegisterTransactionAsync(TransactionDto transactionDto)
        {
            // Verificar que la descripción no contenga palabras prohibidas
            if (ContainsForbiddenContent(transactionDto.Description))
            {
                throw new Exception("El contenido no es permitido");
            }

            // Business Rule: User must exist and be active
            var user = await _userRepository.GetUserByIdAsync(transactionDto.UserId);
            if (user == null)
            {
                throw new Exception("El usuario no existe");
            }
            if (!user.IsActive)
            {
                throw new Exception("El usuario está inactivo");
            }

            // Business Rule: Category must exist and be active
            var category = await _categoryRepository.GetCategoryByIdAsync(transactionDto.CategoryId);
            if (category == null)
            {
                throw new Exception("La categoría no existe");
            }
            if (!category.IsActive)
            {
                throw new Exception("La categoría está inactiva");
            }

            var transaction = _mapper.Map<Transaction>(transactionDto);
            // Usamos la fecha enviada en el DTO
            await _transactionRepository.InsertTransactionAsync(transaction);
            
            transactionDto.Id = transaction.Id;
            return transactionDto;
        }

        public bool ContainsForbiddenContent(string? text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;
            foreach (var word in ForbiddenWords)
            {
                if (text.Contains(word, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        public async Task<bool> UpdateTransactionAsync(int id, TransactionDto dto)
        {
            var transaction = await _transactionRepository.GetTransactionByIdAsync(id);
            if (transaction == null) return false;

            if (ContainsForbiddenContent(dto.Description))
            {
                throw new Exception("El contenido no es permitido");
            }

            _mapper.Map(dto, transaction);

            await _transactionRepository.UpdateTransactionAsync(transaction);
            return true;
        }

        public async Task<decimal> GetUserBalanceAsync(int userId)
        {
            var transactions = await _transactionRepository.GetTransactionsByUserIdAsync(userId);
            decimal balance = 0;
            foreach (var t in transactions)
            {
                if (t.Type == "Income") balance += t.Amount;
                else if (t.Type == "Expense") balance -= t.Amount;
            }
            return balance;
        }

        public async Task<bool> DeleteTransactionAsync(int id)
        {
            var transaction = await _transactionRepository.GetTransactionByIdAsync(id);
            if (transaction == null) return false;

            await _transactionRepository.DeleteTransactionAsync(id);
            return true;
        }
    }
}
