using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace TestDemo.Teleport
{
    public class TeleportableDetectorTests
    {
        private class TeleportableMock : MonoBehaviour, ITeleportable
        {
            public void TeleportTo(Vector3 vector3)
            {
                throw new System.NotImplementedException();
            }
        }


        [UnityTest]
        public IEnumerator TestEnter()
        {
            // Arrage
            var detectorObj = new GameObject();
            var triggerCollider = detectorObj.AddComponent<SphereCollider>();
            triggerCollider.isTrigger = true;
            triggerCollider.radius = 1f;
            var detectorRigidbody = detectorObj.AddComponent<Rigidbody>();
            detectorRigidbody.isKinematic = true;
            var detector = detectorObj.AddComponent<TeleportableDetector>();
            ITeleportable detectedObj = null;
            detector.OnEnter += x => detectedObj = x;

            var teleportableObj = new GameObject();
            var teleportableCollider = teleportableObj.AddComponent<SphereCollider>();
            teleportableCollider.radius = 1f;
            teleportableObj.AddComponent<Rigidbody>();
            var teleportable = teleportableObj.AddComponent<TeleportableMock>();
            yield return new WaitForFixedUpdate();

            // Act
            teleportableObj.transform.position = detectorObj.transform.position;
            yield return new WaitForFixedUpdate();

            // Assert
            Assert.AreEqual(teleportable, detectedObj);
        }

        [UnityTest]
        public IEnumerator TestExit()
        {
            // Arrage
            var detectorObj = new GameObject();
            var triggerCollider = detectorObj.AddComponent<SphereCollider>();
            triggerCollider.isTrigger = true;
            triggerCollider.radius = 1f;
            var detectorRigidbody = detectorObj.AddComponent<Rigidbody>();
            detectorRigidbody.isKinematic = true;
            var detector = detectorObj.AddComponent<TeleportableDetector>();
            ITeleportable detectedObj = null;
            detector.OnExit += x => detectedObj = x;

            var teleportableObj = new GameObject();
            var teleportableCollider = teleportableObj.AddComponent<SphereCollider>();
            teleportableCollider.radius = 1f;
            teleportableObj.AddComponent<Rigidbody>();
            var teleportable = teleportableObj.AddComponent<TeleportableMock>();
            yield return new WaitForFixedUpdate();

            // Act
            teleportableObj.transform.position = detectorObj.transform.position;
            yield return new WaitForFixedUpdate();
            teleportableObj.transform.position = Vector3.up * 10000f;
            yield return new WaitForFixedUpdate();

            // Assert
            Assert.AreEqual(teleportable, detectedObj);
        }
    }
}
