using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Emmit
{
    public class EmitterContext:IEmitterContext
    {
        public IHubConnectionContext<dynamic> Clients { get; private set; }
        public IGroupManager Groups { get; private set; }

        public EmitterContext(IHubContext hubContext)
        {
            if (hubContext != null)
            {
                Clients = hubContext.Clients;
                Groups = hubContext.Groups;
            }
        }
    }
}