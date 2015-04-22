using System;
using System.Collections.Generic;
using Emmit;
using Microsoft.AspNet.SignalR;

namespace Emmit
{
    public class EmitterFactory:IEmitterFactory
    {
        public TEmitter Create<TEmitter>() where TEmitter : Emitter, new()
        {
            return Create<TEmitter>((EmitterModel)null);
        }

        public TEmitter Create<TEmitter>(EmitterModel model) where TEmitter : Emitter, new()
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<TEmitter>();
            var emitter = (TEmitter)Activator.CreateInstance(typeof(TEmitter), new[] { model });
            emitter.EmitterContext = (IEmitterContext)context;
            return emitter;
        }

        public TEmitter Create<TEmitter>(IDictionary<string,object> data) where TEmitter : Emitter, new()
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<TEmitter>();
            var emitter = (TEmitter)Activator.CreateInstance(typeof(TEmitter), new[] { data });
            emitter.EmitterContext = (IEmitterContext)context;
            return emitter;
        }

        public TEmitter Create<TEmitter>(Func<IEmitterContext,TEmitter> factory)where TEmitter: Emitter,new()
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<TEmitter>();
            var emitter = (TEmitter) factory.DynamicInvoke(context);
            emitter.EmitterContext = (IEmitterContext) (emitter.EmitterContext ?? context);
            return (TEmitter) factory.DynamicInvoke(context);
        }
    }
}