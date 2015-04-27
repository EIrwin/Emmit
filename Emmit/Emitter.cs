using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNet.SignalR;

namespace Emmit
{
    public class Emitter:Hub,IEmitter
    {
        public IEmitterContext EmitterContext { get; set; }
        protected IDictionary<string, object> Model { get; set; }
        public Emitter(IEmitterContext emitterContext)
        {
            EmitterContext = emitterContext;
            Model = new Dictionary<string, object>();
        }
        public Emitter(IEmitterContext emitterContext,EmitterModel model)
        {
            EmitterContext = emitterContext;
            Model = model;
        }
        public Emitter(IDictionary<string, object> model)
        {
            Model = model;
        }

        public void Emit(string methodName, object data)
        {
            if (EmitterContext != null)
            {
                Emit(methodName, data, EmitterContext.Clients.All);
            }
        }

        public void Emit(string methodName,object data,dynamic proxy)
        {
            if (EmitterContext != null)
            {
                proxy.Invoke(methodName, new { data });
            }
        }
    }
}