using CodingGiantsRecruitmentTask.Application.Commands.ChatMessages.Messages;
using CodingGiantsRecruitmentTask.Domain.Entities;
using CodingGiantsRecruitmentTask.Domain.Interfaces.Repositories;
using Mapster;
using MediatR;
using Serilog;

namespace CodingGiantsRecruitmentTask.Application.Commands.ChatMessages.Handlers
{
    internal class CreateChatMessageHandler : IRequestHandler<CreateChatMessageMessage, Guid>
    {
        private readonly IChatMessageRepository _chatMessageRepository;
        private readonly ILogger _logger;

        public CreateChatMessageHandler(IChatMessageRepository chatMessageRepository, ILogger logger)
        {
            _chatMessageRepository = chatMessageRepository;
            _logger = logger.ForContext<CreateChatMessageHandler>();
        }

        public async Task<Guid> Handle(CreateChatMessageMessage message, CancellationToken cancellationToken)
        {
            var chatMessage = message.ChatMessage.Adapt<ChatMessage>();

            await _chatMessageRepository.AddChatMessageAsync(chatMessage, cancellationToken);
            await _chatMessageRepository.SaveChangesAsync(cancellationToken);

            _logger.Debug($"Chat message from {(chatMessage.IsFromBot ? "bot" : "user")} created with Id: {chatMessage.Id}");

            return chatMessage.Id;
        }
    }
}
