using CodingGiantsRecruitmentTask.Domain.Entities;

namespace CodingGiantsRecruitmentTask.Domain.Interfaces.Repositories
{
    public interface IChatMessageRatingRepository
    {
        Task<ChatMessageRating?> GetChatMessageRaitingByIdAsync(Guid chatMessageRatingId, CancellationToken cancellationToken);
        Task AddChatMessageRatingAsync(ChatMessageRating chatMessageRating, CancellationToken cancellationToken);
        void UpdateChatMessageRating(ChatMessageRating chatMessageRating, CancellationToken cancellationToken);
        Task RemoveChatMessageRatingAsync(Guid chatMessageRatingId, CancellationToken cancellationToken);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
