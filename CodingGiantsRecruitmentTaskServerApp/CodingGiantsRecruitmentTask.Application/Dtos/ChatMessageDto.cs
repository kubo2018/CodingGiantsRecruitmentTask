
namespace CodingGiantsRecruitmentTask.Application.Dtos
{
    public class ChatMessageDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; } = null!;
        public bool IsFromBot { get; set; }
        public DateTime CreationDate { get; set; }

        public ChatMessageRatingDto? ChatMessageRating { get; set; }
    }
}
