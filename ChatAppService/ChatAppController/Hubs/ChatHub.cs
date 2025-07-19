using ChatAppController.Data;
using ChatAppController.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChatAppController.Hubs
{
    public class ChatHub : Hub
    {

        private readonly ShareDb _shareDb;

        public ChatHub(ShareDb shareDb)
        {
            _shareDb = shareDb;
        }

        public async Task JoinChat(UserConnection userConnection)
        {
            // send all user of app --> use All
            await Clients.All.SendAsync(method: "ReceiveMessage", arg1: "admin", arg2: $"{userConnection.UserName} has joined");
        }

        public async Task JoinChatRoom(UserConnection userConnection)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.ChatRoom);

            _shareDb.connections[Context.ConnectionId] = userConnection;
            //chat room sau nay co the su dung ID trong thuc te
            await Clients.Group(userConnection.ChatRoom).SendAsync("JoinSpecChatRoom", "admin", arg2: $"{userConnection.UserName} has joined {userConnection.ChatRoom}");
        }

        public async Task SendMessage(string msg)
        {
            if(_shareDb.connections.TryGetValue(Context.ConnectionId, out UserConnection conn))
            {
                await Clients.Group(conn.ChatRoom)
                    .SendAsync(method: "RecievedSpecMessage", conn.UserName, msg);
            }
        }
    }
}
