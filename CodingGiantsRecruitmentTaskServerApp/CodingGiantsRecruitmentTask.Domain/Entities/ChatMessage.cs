
namespace CodingGiantsRecruitmentTask.Domain.Entities
{
    public class ChatMessage
    {
        public Guid Id { get; set; }
        public string Text { get; set; } = null!;
        public bool IsFromBot { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual ChatMessageRating? ChatMessageRating { get; set; }
    }
}
