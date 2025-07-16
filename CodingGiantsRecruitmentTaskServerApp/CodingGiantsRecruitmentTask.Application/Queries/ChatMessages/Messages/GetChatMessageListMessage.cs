using CodingGiantsRecruitmentTask.Application.Dtos;
using MediatR;

namespace CodingGiantsRecruitmentTask.Application.Queries.ChatMessages.Messages
{
    public class GetChatMessageListMessage : IRequest<List<ChatMessageDto>>
    {
    }
}
