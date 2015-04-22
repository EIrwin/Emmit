using System;
using System.Collections.Generic;

namespace Emmit
{
    public interface IEmitterFactory
    {
        TEmitter Create<TEmitter>() where TEmitter : Emitter,new();
        TEmitter Create<TEmitter>(EmitterModel model) where TEmitter : Emitter, new();
        TEmitter Create<TEmitter>(IDictionary<string, object> model) where TEmitter : Emitter, new();
        TEmitter Create<TEmitter>(Func<IEmitterContext, TEmitter> factory) where TEmitter : Emitter, new();
    }
}