using System;
using UnityEngine;

namespace TestDemo.Teleport
{
    public interface IPortal
    {
        bool Enabled { get; set; }
        event Action<ITeleportable> OnTeleported;
        IPortal PairedPortal { get; }
        void Teleport(ITeleportable teleportable);
        Vector3 Postion { get; }
    }
}
