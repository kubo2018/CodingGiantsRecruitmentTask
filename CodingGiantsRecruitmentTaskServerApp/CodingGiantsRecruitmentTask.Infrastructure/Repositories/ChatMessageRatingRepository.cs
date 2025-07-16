using CodingGiantsRecruitmentTask.Domain.Entities;
using CodingGiantsRecruitmentTask.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CodingGiantsRecruitmentTask.Infrastructure.Repositories
{
    public class ChatMessageRatingRepository : IChatMessageRatingRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ChatMessageRatingRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ChatMessageRating?> GetChatMessageRaitingByIdAsync(Guid chatMessageRatingId, CancellationToken cancellationToken)
        {
            return await _dbContext.ChatMessageRatings.SingleOrDefaultAsync(y => y.Id == chatMessageRatingId, cancellationToken);
        }

        public async Task AddChatMessageRatingAsync(ChatMessageRating chatMessageRating, CancellationToken cancellationToken)
        {
            await _dbContext.ChatMessageRatings.AddAsync(chatMessageRating, cancellationToken);
        }

        public void UpdateChatMessageRating(ChatMessageRating chatMessageRating, CancellationToken cancellationToken)
        {
            _dbContext.ChatMessageRatings.Update(chatMessageRating);
        }

        public async Task RemoveChatMessageRatingAsync(Guid chatMessageRatingId, CancellationToken cancellationToken)
        {
            var chatMessageRating = await GetChatMessageRaitingByIdAsync(chatMessageRatingId, cancellationToken);
            if (chatMessageRating != null)
                _dbContext.ChatMessageRatings.Remove(chatMessageRating);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
