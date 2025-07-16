
namespace CodingGiantsRecruitmentTask.Domain.Entities
{
    public class ChatMessageRatingType
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Icon { get; set; } = null!;

        public ICollection<ChatMessageRating> ChatMessageRatings { get; set; } = new List<ChatMessageRating>();
    }
}
