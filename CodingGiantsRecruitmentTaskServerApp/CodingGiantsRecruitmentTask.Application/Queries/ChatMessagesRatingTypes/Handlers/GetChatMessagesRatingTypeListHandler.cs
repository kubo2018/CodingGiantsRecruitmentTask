using CodingGiantsRecruitmentTask.Application.Dtos;
using CodingGiantsRecruitmentTask.Application.Queries.ChatMessagesRatingTypes.Messages;
using CodingGiantsRecruitmentTask.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CodingGiantsRecruitmentTask.Application.Queries.ChatMessagesRatingTypes.Handlers
{
    internal class GetChatMessagesRatingTypeListHandler : IRequestHandler<GetChatMessagesRatingTypeListMessage, List<ChatMessageRatingTypeDto>>
    {
        private readonly IChatMessageRatingTypeRepository _chatMessageRatingTypeRepository;

        public GetChatMessagesRatingTypeListHandler(IChatMessageRatingTypeRepository chatMessageRatingTypeRepository)
        {
            _chatMessageRatingTypeRepository = chatMessageRatingTypeRepository;
        }

        public async Task<List<ChatMessageRatingTypeDto>> Handle(GetChatMessagesRatingTypeListMessage message, CancellationToken cancellationToken)
        {
            return await _chatMessageRatingTypeRepository
                .GetAllChatMessageRatingTypesQuery(cancellationToken)
                .Select(y => new ChatMessageRatingTypeDto
                {
                    Id = y.Id,
                    Name = y.Name,
                    Icon = y.Icon
                })
                .ToListAsync();
        }
    }
}
