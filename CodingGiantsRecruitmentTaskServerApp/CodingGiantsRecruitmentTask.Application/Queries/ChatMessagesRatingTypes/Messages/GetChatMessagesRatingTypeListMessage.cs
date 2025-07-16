using CodingGiantsRecruitmentTask.Application.Dtos;
using MediatR;

namespace CodingGiantsRecruitmentTask.Application.Queries.ChatMessagesRatingTypes.Messages
{
    public class GetChatMessagesRatingTypeListMessage : IRequest<List<ChatMessageRatingTypeDto>>
    {
    }
}
