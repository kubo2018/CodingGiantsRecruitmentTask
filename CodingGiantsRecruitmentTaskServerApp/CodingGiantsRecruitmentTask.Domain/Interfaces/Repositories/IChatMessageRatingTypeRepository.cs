using CodingGiantsRecruitmentTask.Domain.Entities;

namespace CodingGiantsRecruitmentTask.Domain.Interfaces.Repositories
{
    public interface IChatMessageRatingTypeRepository
    {
        IQueryable<ChatMessageRatingType> GetAllChatMessageRatingTypesQuery(CancellationToken cancellationToken);
    }
}
