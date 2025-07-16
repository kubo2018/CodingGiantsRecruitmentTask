using CodingGiantsRecruitmentTask.Application.Dtos;
using MediatR;

namespace CodingGiantsRecruitmentTask.Application.Commands.ChatMessages.Messages
{
    public class CreateChatMessageMessage : IRequest<Guid>
    {
        public ChatMessageDto ChatMessage { get; private set; } = null!;

        public CreateChatMessageMessage(ChatMessageDto chatMessage)
        {
            ChatMessage = chatMessage ?? throw new ArgumentNullException(nameof(chatMessage), "Chat message cannot be null.");
        }
    }
}
