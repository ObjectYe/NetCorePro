using Microsoft.AspNetCore.SignalR;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCore_pc.core
{
    public class ChatHub : Hub
    {
        //SendMsg用于前端调用
        public Task SendMsg(string name, string message)
        {
            //在客户端实现此处的Show方法
            return Clients.All.SendAsync("addMessageToList", name + ":" + message);
        }
    }
}
