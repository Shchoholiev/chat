namespace Chat.API.SignalR
{
    public interface IChatHub
    {
        Task SendMessage(string message);
    }
}
