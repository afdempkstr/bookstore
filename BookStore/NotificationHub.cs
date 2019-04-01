using BookStore.Application;
using BookStore.Domain.Application;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace BookStore
{
    public class NotificationHub : Hub<IBookStoreAppClient>
    {
        private IBookStoreApp _app;

        public NotificationHub(IBookStoreApp app)
        {
            _app = app;
        }

        public void Hello()
        {
            Clients.All.OnNotify("hello");
        }

        public override async Task OnConnected()
        {
            _app.GetPublisher(1);
            await base.OnConnected();
        }
    }
}