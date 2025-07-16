using CodingGiantsRecruitmentTask.Application.Commands.ChatMessages.Messages;
using CodingGiantsRecruitmentTask.Application.Queries.ChatMessages.Messages;
using CodingGiantsRecruitmentTask.Application.Queries.ChatMessagesRatingTypes.Messages;
using CodingGiantsRecruitmentTask.Web.Hubs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CodingGiantsRecruitmentTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChatController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            var chatMessages = await _mediator.Send(new GetChatMessageListMessage());
            return Ok(chatMessages);
        }

        [HttpGet("messageRatingTypes")]
        public async Task<IActionResult> GetMessageRatingTypes()
        {
            var chatMessages = await _mediator.Send(new GetChatMessagesRatingTypeListMessage());
            return Ok(chatMessages);
        }

        [HttpPost("rate")]
        public async Task<IActionResult> RateMessage([FromBody] RateChatMessageMessage message)
        {
            if (await _mediator.Send(message))
                return Ok();
            else
                return NotFound();
        }

        [HttpDelete("cancelGenerating/{messageId}")]
        public IActionResult CancelGeneratingMessage(Guid messageId)
        {
            if (ChatHub.CancelGenerating(messageId))
                return Ok();
            else
                return NotFound();
        }
    }
}
