using System;
using System.Collections.Generic;
using Emmit;
using Microsoft.AspNet.SignalR;

namespace Emmit
{
    public class EmitterFactory:IEmitterFactory
    {
        public TEmitter Create<TEmitter>() where TEmitter : Emitter
        {
            return Create<TEmitter>((EmitterModel)null);
        }

        public TEmitter Create<TEmitter>(EmitterModel model) where TEmitter : Emitter
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<TEmitter>();
            IEmitterContext emitterContext = new EmitterContext(context);
            var emitter = (TEmitter)Activator.CreateInstance(typeof(TEmitter), new[] { model });
            emitter.EmitterContext = emitterContext;
            return emitter;
        }

        public TEmitter Create<TEmitter>(IDictionary<string,object> data) where TEmitter : Emitter
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<TEmitter>();
            IEmitterContext emitterContext = new EmitterContext(context);
            var emitter = (TEmitter)Activator.CreateInstance(typeof(TEmitter), new[] { data });
            emitter.EmitterContext = emitterContext;
            return emitter;
        }


        public TEmitter Create<TEmitter>(Func<TEmitter> factory) where TEmitter : Emitter
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<TEmitter>();
            IEmitterContext emitterContext = new EmitterContext(context);
            var emitter = (TEmitter)factory.DynamicInvoke();
            emitter.EmitterContext = emitter.EmitterContext ?? emitterContext;
            return emitter;
        }

        public TEmitter Create<TEmitter>(Func<IEmitterContext, TEmitter> factory) where TEmitter : Emitter
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<TEmitter>();
            IEmitterContext emitterContext = new EmitterContext(context);
            var emitter = (TEmitter)factory.DynamicInvoke(emitterContext);
            emitter.EmitterContext = emitter.EmitterContext ?? emitterContext;
            return emitter;
        }

    }
}