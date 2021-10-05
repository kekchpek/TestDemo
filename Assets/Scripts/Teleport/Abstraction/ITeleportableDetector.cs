using System;

namespace TestDemo.Teleport
{
    public interface ITeleportableDetector
    {
        event Action<ITeleportable> OnEnter;
        event Action<ITeleportable> OnExit;
    }
}
