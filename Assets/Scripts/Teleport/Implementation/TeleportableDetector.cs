using System;
using UnityEngine;

namespace TestDemo.Teleport
{
    public class TeleportableDetector : MonoBehaviour, ITeleportableDetector
    {
        public event Action<ITeleportable> OnEnter;
        public event Action<ITeleportable> OnExit;

        private void OnTriggerEnter(Collider other)
        {
            var teleportable = other.GetComponent<ITeleportable>();
            if (teleportable != null)
                OnEnter?.Invoke(teleportable);
        }

        private void OnTriggerExit(Collider other)
        {

            var teleportable = other.GetComponent<ITeleportable>();
            if (teleportable != null)
                OnExit?.Invoke(teleportable);
        }

    }
}
