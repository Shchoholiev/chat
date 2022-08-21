using Chat.Application.Models.Dtos;
using Chat.Application.Paging;

namespace Chat.Application.Interfaces.Services
{
    public interface IMessagesService
    {
        Task<PagedList<MessageDto>> GetPageAsync(PageParameters pageParameters, int roomId,
            string userEmail, CancellationToken cancellationToken);

        Task SendAsync(MessageCreateDto messageDTO, string userEmail, CancellationToken cancellationToken);

        Task EditAsync(int id, MessageCreateDto messageDTO, CancellationToken cancellationToken);

        Task HideForSenderAsync(int id, CancellationToken cancellationToken);

        Task DeleteAsync(int id, CancellationToken cancellationToken);

        Task ReplyInPersonAsync(string email, string senderEmail, MessageCreateDto messageDTO,
            CancellationToken cancellationToken);
    }
}
