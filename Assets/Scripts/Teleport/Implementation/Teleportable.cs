using UnityEngine;

namespace TestDemo.Teleport
{
    public class Teleportable : MonoBehaviour, ITeleportable
    {
        public void TeleportTo(Vector3 teleportPosition)
        {
            transform.position = teleportPosition;
        }
    }
}
