using CodingGiantsRecruitmentTask.Application.Commands.ChatMessages.Messages;
using CodingGiantsRecruitmentTask.Application.Dtos;
using CodingGiantsRecruitmentTask.Domain.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using SignalRSwaggerGen.Attributes;
using System.Collections.Concurrent;

namespace CodingGiantsRecruitmentTask.Web.Hubs
{
    [SignalRHub]
    public class ChatHub : Hub
    {
        private static readonly ConcurrentDictionary<Guid, CancellationTokenSource> _cancellationTokens = new();

        private readonly IChatGeneratorService _chatGeneratorService;
        private readonly IMediator _mediator;
        private readonly Serilog.ILogger _logger;

        public ChatHub(IChatGeneratorService chatGeneratorService, IMediator mediator, Serilog.ILogger logger)
        {
            _chatGeneratorService = chatGeneratorService;
            _mediator = mediator;
            _logger = logger.ForContext<ChatHub>();
        }

        public async Task StartGenerating(string userMessageText)
        {
            var userMessageId = await _mediator.Send(new CreateChatMessageMessage(new ChatMessageDto
            {
                Text = userMessageText,
                IsFromBot = false,
                CreationDate = DateTime.Now
            }));
            var botMessageId = await _mediator.Send(new CreateChatMessageMessage(new ChatMessageDto
            {
                Text = string.Empty,
                IsFromBot = true,
                CreationDate = DateTime.Now
            }));

            await Clients.Caller.SendAsync("ReceiveMessageId", new { Id = botMessageId });

            var cancellationTokenSource = new CancellationTokenSource();
            _cancellationTokens[botMessageId] = cancellationTokenSource;

            try
            {
                _logger.Debug($"Starting real-time chat message generation for user message Id: {userMessageId}, bot message Id: {botMessageId}.");
                await _chatGeneratorService.GenerateInRealTime(Context.ConnectionId, userMessageText, botMessageId, Clients.Caller, cancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
                _logger.Debug($"Real-time chat message generation cancelled by user for bot message Id: {botMessageId}.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error while generating in real-time chat message for user message Id: {userMessageId}.");
                await Clients.Caller.SendAsync("Error", new { Message = "Error while generating in real-time chat message." });
            }
            finally
            {
                _cancellationTokens.TryRemove(botMessageId, out _);
            }
        }

        public static bool CancelGenerating(Guid messageId)
        {
            if (_cancellationTokens.TryGetValue(messageId, out var cancellationTokenSource))
            {
                cancellationTokenSource.Cancel();
                return true;
            }
            else
                return false;
        }
    }
}
