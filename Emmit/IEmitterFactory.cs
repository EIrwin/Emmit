using System;
using System.Collections.Generic;
using Microsoft.AspNet.SignalR;

namespace Emmit
{
    public interface IEmitterFactory
    {
        TEmitter Create<TEmitter>() where TEmitter : Emitter;
        TEmitter Create<TEmitter>(EmitterModel model) where TEmitter : Emitter;
        TEmitter Create<TEmitter>(IDictionary<string, object> model) where TEmitter : Emitter;
        TEmitter Create<TEmitter>(Func<TEmitter> factory) where TEmitter : Emitter;
        TEmitter Create<TEmitter>(Func<IEmitterContext, TEmitter> factory) where TEmitter : Emitter;
    }
}