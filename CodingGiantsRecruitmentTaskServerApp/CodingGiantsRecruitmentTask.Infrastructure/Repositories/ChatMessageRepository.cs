using CodingGiantsRecruitmentTask.Domain.Entities;
using CodingGiantsRecruitmentTask.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CodingGiantsRecruitmentTask.Infrastructure.Repositories
{
    public class ChatMessageRepository : IChatMessageRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ChatMessageRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<ChatMessage> GetChatMessagesQuery(CancellationToken cancellationToken)
        {
            return _dbContext.ChatMessages
                .Include(x => x.ChatMessageRating)
                .ThenInclude(x => x != null ? x.ChatMessageRatingType : null)
                .OrderBy(z => z.CreationDate);
        }

        public async Task AddChatMessageAsync(ChatMessage chatMessage, CancellationToken cancellationToken)
        {
            await _dbContext.ChatMessages.AddAsync(chatMessage, cancellationToken);
        }

        public void UpdateChatMessage(ChatMessage chatMessage, CancellationToken cancellationToken)
        {
            _dbContext.ChatMessages.Update(chatMessage);
        }

        public async Task<ChatMessage?> GetChatMessageByIdAsync(Guid chatMessageId, CancellationToken cancellationToken)
        {
            return await _dbContext.ChatMessages
                .Include(x => x.ChatMessageRating)
                .FirstOrDefaultAsync(y => y.Id == chatMessageId, cancellationToken);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
