namespace Emmit
{
    public interface IEmitterFactory
    {
        TEmitter Create<TEmitter>() where TEmitter : Emitter,new();
    }
}