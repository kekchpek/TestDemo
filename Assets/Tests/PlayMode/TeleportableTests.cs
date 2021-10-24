using NUnit.Framework;
using System.Collections;
using TestDemo.Teleport;
using UnityEngine;
using UnityEngine.TestTools;

namespace TestDemo.Tests
{
    public class TeleportableTests
    {
        [UnityTest]
        public IEnumerator TestTeleport()
        {
            // Arrange
            var gameObj = new GameObject();
            var teleportable = gameObj.AddComponent<Teleportable>();
            Vector3 teleportPosition = new Vector3(23f, 1.11111f, 343.191f);
            yield return new WaitForFixedUpdate();

            // Act
            teleportable.TeleportTo(teleportPosition);
            yield return new WaitForFixedUpdate();

            // Assert
            Assert.AreEqual(teleportPosition, gameObj.transform.position);
        }
    }
}
