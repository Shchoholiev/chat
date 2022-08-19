namespace Chat.Application.Models.Dtos
{
    public class MessageDto
    {
        public string Text { get; set; }

        public int RepliedTo { get; set; }

        public int RoomId { get; set; }
    }
}
