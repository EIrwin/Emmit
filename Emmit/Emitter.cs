using Microsoft.AspNet.SignalR;

namespace Emmit
{
    public class Emitter:Hub,IEmitter
    {
        public IHubContext EmitterContext { get; set; }

        public Emitter(){/*Do not remove*/}
        public Emitter(IHubContext emitterContext)
        {
            EmitterContext = emitterContext;
        }
    }
}