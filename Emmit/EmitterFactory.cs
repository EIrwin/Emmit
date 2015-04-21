using Microsoft.AspNet.SignalR;

namespace Emmit
{
    public class EmitterFactory:IEmitterFactory
    {
        public TEmitter Create<TEmitter>() where TEmitter : Emitter, new()
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<TEmitter>();
            var hub = new TEmitter();
            hub.EmitterContext = context;
            return hub;
        }
    }
}