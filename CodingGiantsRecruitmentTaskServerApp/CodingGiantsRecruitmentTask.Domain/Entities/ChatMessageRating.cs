namespace CodingGiantsRecruitmentTask.Domain.Entities
{
    public class ChatMessageRating
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public Guid ChatMessageId { get; set; }
        public int ChatMessageRatingTypeId { get; set; }

        public virtual ChatMessage ChatMessage { get; set; } = null!;
        public virtual ChatMessageRatingType ChatMessageRatingType { get; set; } = null!;
    }
}
