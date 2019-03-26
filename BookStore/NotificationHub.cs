using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookStore.Domain.Application;
using Microsoft.AspNet.SignalR;

namespace BookStore
{
    public class NotificationHub : Hub<IBookStoreAppClient>
    {
        public void Hello()
        {
            Clients.All.OnNotify("hello");
        }
    }
}