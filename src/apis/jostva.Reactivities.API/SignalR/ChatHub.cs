using jostva.Reactivities.application.Comments;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Threading.Tasks;

namespace jostva.Reactivities.API.SignalR
{
    public class ChatHub : Hub
    {
        private readonly IMediator mediator;


        public ChatHub(IMediator mediator)
        {
            this.mediator = mediator;
        }


        private string GetUsername()
        {
            //  TODO: revisar porque no trae username..
            return Context.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        }


        public async Task SendComment(Create.Command command)
        {
            command.Username = GetUsername();
            CommentDto comment = await mediator.Send(command);
            await Clients.Group(command.ActivityId.ToString()).SendAsync("ReceiveComment", comment);
        }


        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            string useraname = GetUsername();
            await Clients.Group(groupName).SendAsync("Send", $"{useraname} has joined the gorup");
        }


        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            string useraname = GetUsername();
            await Clients.Group(groupName).SendAsync("Send", $"{useraname} has left the gorup");
        }
    }
}