//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using SimpleGameWebAPI.Hubs.Clients;
//using SimpleGameWebAPI.Data.Models;
//using SimpleGameWebAPI.Data.Dto;
//using Microsoft.AspNetCore.SignalR;

//namespace SimpleGameWebAPI.Hubs
//{
//    public class GameHub : Hub<IGameSesssionClient>
//    {
//        //public async Task SendMessage(ChatMessage message)
//        //{
//        //    await Clients.All.ReceiveMessage(message);
//        //}

//        // Task UpdateGame(SessionHubDto game, Guid id);
//    //    public async Task UpdaeteField(SessionHubDto data, string connectionId)
//    //    => await Clients.Group;//.SendAsync("updategame", data);
//    //        Clients.Client(connectionId).SendAsync("updategame", data);

//    //    public async Task AddToGroup(string groupName)
//    //=> await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

//    //    public async Task RemoveFromGroup(string groupName)
//    //        => await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
//    //    public string GetConnectionId() => Context.ConnectionId;
//    }
//}
