using Microsoft.AspNetCore.SignalR;

namespace CodingGiantsRecruitmentTask.Domain.Interfaces.Services
{
    public interface IChatGeneratorService
    {
        Task GenerateInRealTime(string connectionId, string userMessageText, Guid botMessageId, IClientProxy client, CancellationToken cancellationToken);
    }
}
