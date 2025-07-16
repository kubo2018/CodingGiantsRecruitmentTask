using CodingGiantsRecruitmentTask.Application.Dtos;
using CodingGiantsRecruitmentTask.Application.Queries.ChatMessages.Messages;
using CodingGiantsRecruitmentTask.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CodingGiantsRecruitmentTask.Application.Queries.ChatMessages.Handlers
{
    internal class GetChatMessageListHandler : IRequestHandler<GetChatMessageListMessage, List<ChatMessageDto>>
    {
        private readonly IChatMessageRepository _chatRepository;

        public GetChatMessageListHandler(IChatMessageRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public async Task<List<ChatMessageDto>> Handle(GetChatMessageListMessage message, CancellationToken cancellationToken)
        {
            return await _chatRepository
                .GetChatMessagesQuery(cancellationToken)
                .Select(y => new ChatMessageDto
                {
                    Id = y.Id,
                    Text = y.Text,
                    IsFromBot = y.IsFromBot,
                    CreationDate = y.CreationDate,
                    ChatMessageRating = y.ChatMessageRating != null ? new ChatMessageRatingDto
                    {
                        Id = y.ChatMessageRating.Id,
                        ChatMessageId = y.ChatMessageRating.ChatMessageId,
                        ChatMessageRatingTypeId = y.ChatMessageRating.ChatMessageRatingTypeId,
                        ChatMessageRatingType = y.ChatMessageRating.ChatMessageRatingType != null ? new ChatMessageRatingTypeDto
                        {
                            Id = y.ChatMessageRating.ChatMessageRatingType.Id,
                            Name = y.ChatMessageRating.ChatMessageRatingType.Name,
                            Icon = y.ChatMessageRating.ChatMessageRatingType.Icon
                        }: null
                    } : null
                })
                .ToListAsync();
        }
    }
}
