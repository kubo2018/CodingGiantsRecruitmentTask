using CodingGiantsRecruitmentTask.Application.Commands.ChatMessages.Messages;
using CodingGiantsRecruitmentTask.Domain.Entities;
using CodingGiantsRecruitmentTask.Domain.Interfaces.Repositories;
using MediatR;
using Serilog;

namespace CodingGiantsRecruitmentTask.Application.Commands.ChatMessages.Handlers
{
    internal class RateChatMessageHandler : IRequestHandler<RateChatMessageMessage, bool>
    {
        private readonly IChatMessageRepository _chatMessageRepository;
        private readonly IChatMessageRatingRepository _chatMessageRatingRepository;
        private readonly ILogger _logger;

        public RateChatMessageHandler(IChatMessageRepository chatMessageRepository, IChatMessageRatingRepository chatMessageRatingRepository, ILogger logger)
        {
            _chatMessageRepository = chatMessageRepository;
            _chatMessageRatingRepository = chatMessageRatingRepository;
            _logger = logger.ForContext<RateChatMessageHandler>();
        }

        public async Task<bool> Handle(RateChatMessageMessage message, CancellationToken cancellationToken)
        {
            var chatMessage = await _chatMessageRepository.GetChatMessageByIdAsync(message.ChatMessageId, cancellationToken);
            if (chatMessage == null)
            {
                _logger.Warning($"Chat message with Id: {message.ChatMessageId} not found for rating update.");
                return false;
            }

            if (chatMessage.ChatMessageRating != null)
            {
                if (message.ChatMessageRatingTypeId.HasValue)
                {
                    chatMessage.ChatMessageRating.ChatMessageRatingTypeId = message.ChatMessageRatingTypeId.Value;
                    _chatMessageRatingRepository.UpdateChatMessageRating(chatMessage.ChatMessageRating, cancellationToken);

                    await _chatMessageRatingRepository.SaveChangesAsync(cancellationToken);
                    _logger.Debug($"Chat message with Id: {chatMessage.Id} rating updated to type Id: {message.ChatMessageRatingTypeId.Value}.");
                }
                else
                {
                    await _chatMessageRatingRepository.RemoveChatMessageRatingAsync(chatMessage.ChatMessageRating.Id, cancellationToken);
                    _logger.Debug($"Chat message with Id: {chatMessage.Id} rating removed.");
                }

                await _chatMessageRepository.SaveChangesAsync(cancellationToken);
                return true;
            }
            else if (message.ChatMessageRatingTypeId.HasValue)
            {
                await _chatMessageRatingRepository.AddChatMessageRatingAsync(new ChatMessageRating
                {
                    CreationDate = DateTime.Now,
                    ChatMessageId = chatMessage.Id,
                    ChatMessageRatingTypeId = message.ChatMessageRatingTypeId.Value
                }, cancellationToken);
                await _chatMessageRepository.SaveChangesAsync(cancellationToken);

                _logger.Debug($"Chat message with Id: {chatMessage.Id} rated with type Id: {message.ChatMessageRatingTypeId.Value}.");
                return true;
            }
            else
                return false;
        }
    }
}
