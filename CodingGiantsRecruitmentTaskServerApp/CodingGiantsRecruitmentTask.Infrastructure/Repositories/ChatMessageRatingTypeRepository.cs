using CodingGiantsRecruitmentTask.Domain.Entities;
using CodingGiantsRecruitmentTask.Domain.Interfaces.Repositories;

namespace CodingGiantsRecruitmentTask.Infrastructure.Repositories
{
    public class ChatMessageRatingTypeRepository : IChatMessageRatingTypeRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ChatMessageRatingTypeRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<ChatMessageRatingType> GetAllChatMessageRatingTypesQuery(CancellationToken cancellationToken)
        {
            return _dbContext.ChatMessageRatingTypes
                .OrderBy(z => z.Id);
        }
    }
}
