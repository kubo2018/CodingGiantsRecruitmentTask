using CodingGiantsRecruitmentTask.Domain.Entities;
using CodingGiantsRecruitmentTask.Domain.Interfaces.Repositories;
using CodingGiantsRecruitmentTask.Domain.Interfaces.Services;
using Microsoft.AspNetCore.SignalR;
using System.Text;

namespace CodingGiantsRecruitmentTask.Infrastructure.Services
{
    public class ChatGeneratorService : IChatGeneratorService
    {
        private readonly IChatMessageRepository _chatMessageRepository;
        private static readonly TimeSpan _saveInterval = TimeSpan.FromSeconds(2);
        private readonly string[] _sentences =
        [
            "Lorem ipsum dolor sit amet.",
            "Consectetur adipiscing elit.",
            "Sed do eiusmod tempor incididunt ut labore.",
            "Ut enim ad minim veniam, quis nostrud.",
            "Duis aute irure dolor in reprehenderit.",
            "Excepteur sint occaecat cupidatat non proident.",
            "Sunt in culpa qui officia deserunt mollit anim id est laborum.",
            "Pellentesque habitant morbi tristique senectus et netus.",
            "Curabitur pretium tincidunt lacus.",
            "Nulla gravida orci a odio.",
            "Nullam varius, turpis et commodo pharetra, est eros bibendum elit."
        ];

        public ChatGeneratorService(IChatMessageRepository chatMessageRepository)
        {
            _chatMessageRepository = chatMessageRepository;
        }

        public async Task GenerateInRealTime(string connectionId, string userMessageText, Guid botMessageId, IClientProxy client, CancellationToken cancellationToken)
        {
            var chatMessage = await _chatMessageRepository.GetChatMessageByIdAsync(botMessageId, cancellationToken);
            if (chatMessage == null)
                throw new InvalidOperationException("Chat message not found.");

            var random = new Random(userMessageText.GetHashCode());
            var generatedText = GenerateVariableLengthLoremIpsumText(random, userMessageText);
            var lastSaveTime = DateTime.Now;
            var stringBuilder = new StringBuilder();

            try
            {
                foreach (var ch in generatedText)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    stringBuilder.Append(ch);
                    var partialText = stringBuilder.ToString();

                    await client.SendAsync("ReceiveMessageFragment", new { FragmentText = partialText });

                    if ((DateTime.Now - lastSaveTime) >= _saveInterval)
                    {
                        await SaveChatMessageAsync(chatMessage, partialText, cancellationToken);
                        lastSaveTime = DateTime.Now;
                    }

                    var delay = ch switch
                    {
                        '.' => random.Next(200, 300),
                        ',' => random.Next(100, 150),
                        ' ' => random.Next(40, 70),
                        '\n' => random.Next(250, 350),
                        _ => random.Next(15, 30)
                    };
                    await Task.Delay(delay, cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                await SaveChatMessageAsync(chatMessage, stringBuilder.ToString(), CancellationToken.None);
                throw;
            }

            await SaveChatMessageAsync(chatMessage, stringBuilder.ToString(), cancellationToken);
        }

        private string GenerateVariableLengthLoremIpsumText(Random random, string userMessageText)
        {
            var stringBuilder = new StringBuilder();

            int paragraphCount;
            int minSentences;
            int maxSentences;

            switch (random.Next(1, 4))
            {
                case 1: // Krótka: 1-2 zdania
                    paragraphCount = 1;
                    minSentences = 1;
                    maxSentences = 2;
                    break;
                case 2: // Średnia: 1-3 akapity po 3-5 zdań
                    paragraphCount = random.Next(1, 4);
                    minSentences = 3;
                    maxSentences = 5;
                    break;
                case 3: // Długa: 3-6 akapitów po 5-8 zdań
                default:
                    paragraphCount = random.Next(3, 7);
                    minSentences = 5;
                    maxSentences = 8;
                    break;
            }

            for (int p = 0; p < paragraphCount; p++)
            {
                int sentenceCount = random.Next(minSentences, maxSentences + 1);
                for (int s = 0; s < sentenceCount; s++)
                {
                    stringBuilder.Append($"{_sentences[random.Next(_sentences.Length)]} ");
                }
                stringBuilder.AppendLine("\n");
            }

            return stringBuilder.ToString().Trim();
        }

        private async Task SaveChatMessageAsync(ChatMessage chatMessage, string text, CancellationToken cancellationToken)
        {
            chatMessage.Text = text;
            _chatMessageRepository.UpdateChatMessage(chatMessage, cancellationToken);
            await _chatMessageRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
