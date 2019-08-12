using Microsoft.AspNetCore.SignalR;
using SuperM.Chat.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperM.Chat.SignarR
{
    public class ChatHub : Hub
    {
        public List<TestUser> testUsers;
        public ChatHub() {
            if (Online.OnLineUsers == null)
                Online.OnLineUsers = new List<TestUser>();
            testUsers = Online.OnLineUsers;
        }
        #region method
        /// <summary>
        /// LoginIn
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task LoginIn(string userId, string password)
        {
            var user = testUsers.Find(t => t.userId == userId);
            if (user == null)
            {
                var token = Guid.NewGuid().ToString();
                testUsers.Add(new TestUser { userId = userId, token = token, clientId = Context.ConnectionId });
                await Clients.Client(Context.ConnectionId).SendAsync("LoginIn", token);
            }
            else
            {
                await Clients.Client(Context.ConnectionId).SendAsync("LoginIn", user.token);
            }
            await Groups.AddToGroupAsync(Context.ConnectionId,"1");
        }
        /// <summary>
        /// LoginOut
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task LoginOut(string token)
        {
            var user = testUsers.Find(t => t.token == token);
            if (user == null)
            {
                testUsers.Remove(user);
                await Clients.Client(Context.ConnectionId).SendAsync("LoginOut", "success");
            }
            else
            {
                await Clients.Client(Context.ConnectionId).SendAsync("SendSingle", "error: unauthorized");
            }

        }
        /// <summary>
        /// SendSingle
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task SendSingle(string token, string userId, string message)
        {
            var user = testUsers.Find(t => t.token == token);
            if(user==null)
                await Clients.Client(Context.ConnectionId).SendAsync("SendSingle", "error: unauthorized");
            var sendToUser = testUsers.Find(t => t.userId == userId);
            await Clients.Client(sendToUser?.clientId).SendAsync("SendSingle", message);
        }

        /// <summary>
        /// Send
        /// </summary>
        /// <param name="token"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public async Task Send(string token, string message, string groupId = "1")
        {
            var user = testUsers.Find(t => t.token == token);
            if (user == null)
                await Clients.Client(Context.ConnectionId).SendAsync("SendSingle", "error: unauthorized");
            await Clients.Group(groupId).SendAsync("SendSingle", message);
        }

        #endregion



    }
}
