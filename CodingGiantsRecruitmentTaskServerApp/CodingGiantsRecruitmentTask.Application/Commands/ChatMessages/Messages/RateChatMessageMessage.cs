using MediatR;

namespace CodingGiantsRecruitmentTask.Application.Commands.ChatMessages.Messages
{
    public class RateChatMessageMessage : IRequest<bool>
    {
        public Guid ChatMessageId { get; private set; }
        public int? ChatMessageRatingTypeId { get; private set; }

        public RateChatMessageMessage(Guid chatMessageId, int? chatMessageRatingTypeId)
        {
            ChatMessageId = chatMessageId;
            ChatMessageRatingTypeId = chatMessageRatingTypeId;
        }
    }
}
