using CodingGiantsRecruitmentTask.Domain.Entities;

namespace CodingGiantsRecruitmentTask.Domain.Interfaces.Repositories
{
    public interface IChatMessageRepository
    {
        IQueryable<ChatMessage> GetChatMessagesQuery(CancellationToken cancellationToken);
        Task<ChatMessage?> GetChatMessageByIdAsync(Guid chatMessageId, CancellationToken cancellationToken);
        Task AddChatMessageAsync(ChatMessage chatMessage, CancellationToken cancellationToken);
        void UpdateChatMessage(ChatMessage chatMessage, CancellationToken cancellationToken);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
