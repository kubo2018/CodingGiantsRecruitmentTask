
namespace CodingGiantsRecruitmentTask.Application.Dtos
{
    public class ChatMessageRatingDto
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public Guid ChatMessageId { get; set; }
        public int ChatMessageRatingTypeId { get; set; }

        public ChatMessageRatingTypeDto? ChatMessageRatingType { get; set; }
    }
}
