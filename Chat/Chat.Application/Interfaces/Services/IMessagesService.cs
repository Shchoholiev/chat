using Chat.Application.Models.Dtos;
using Chat.Application.Paging;

namespace Chat.Application.Interfaces.Services
{
    public interface IMessagesService
    {
        Task<IEnumerable<MessageDto>> GetPageAsync(PageParameters pageParameters, int roomId, 
            CancellationToken cancellationToken);

        Task SendAsync(MessageDto messageDTO, CancellationToken cancellationToken);

        Task EditAsync(int id, MessageDto messageDTO, CancellationToken cancellationToken);

        Task HideForSenderAsync(int id, CancellationToken cancellationToken);

        Task DeleteAsync(int id, CancellationToken cancellationToken);

        Task ReplyInPersonAsync(string email, MessageDto messageDTO, CancellationToken cancellationToken);
    }
}
