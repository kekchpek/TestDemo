using NSubstitute;
using NUnit.Framework;
using System;
using TestDemo.Teleport;
using UnityEngine;

namespace TestDemo.Tests
{
    public class PortalTests
    {

        [Test]
        public void TestInitialization()
        {
            // Arrange
            Portal portal = new GameObject().AddComponent<Portal>();
            IPortalAnimator portalAnimator = Substitute.For<IPortalAnimator>();
            ITeleportableDetector teleportableDetector = Substitute.For<ITeleportableDetector>();
            IPortal pairedPortal = Substitute.For<IPortal>();

            // Act
            portal.Initialize(portalAnimator, teleportableDetector, pairedPortal);

            // Assert
            Assert.IsFalse(portal.Enabled);
            Assert.AreEqual(pairedPortal, portal.PairedPortal);
            
        }

        [Test]
        public void TestEnable()
        {
            // Arrange
            Portal portal = new GameObject().AddComponent<Portal>();
            IPortalAnimator portalAnimator = Substitute.For<IPortalAnimator>();
            ITeleportableDetector teleportableDetector = Substitute.For<ITeleportableDetector>();
            IPortal pairedPortal = Substitute.For<IPortal>();
            pairedPortal.Enabled = false;

            // Act
            portal.Initialize(portalAnimator, teleportableDetector, pairedPortal);
            portal.Enabled = true;

            // Assert
            Assert.IsTrue(pairedPortal.Enabled);

        }

        [Test]
        public void TestDisable()
        {
            // Arrange
            Portal portal = new GameObject().AddComponent<Portal>();
            IPortalAnimator portalAnimator = Substitute.For<IPortalAnimator>();
            ITeleportableDetector teleportableDetector = Substitute.For<ITeleportableDetector>();
            IPortal pairedPortal = Substitute.For<IPortal>();
            pairedPortal.Enabled = true;

            // Act
            portal.Initialize(portalAnimator, teleportableDetector, pairedPortal);
            portal.Enabled = true;
            portal.Enabled = false;

            // Assert
            Assert.IsFalse(pairedPortal.Enabled);

        }

        [Test]
        public void TestDisableCycle()
        {
            // Arrange
            Portal portal = new GameObject().AddComponent<Portal>();
            IPortalAnimator portalAnimator = Substitute.For<IPortalAnimator>();
            ITeleportableDetector teleportableDetector = Substitute.For<ITeleportableDetector>();
            IPortal pairedPortal = Substitute.For<IPortal>();
            bool enabledWasSet = false;
            pairedPortal.Enabled = Arg.Do<bool>(x =>
            {
                enabledWasSet = true;
            });

            // Act
            portal.Initialize(portalAnimator, teleportableDetector, pairedPortal);
            portal.Enabled = false;

            // Assert
            Assert.IsFalse(enabledWasSet);
        }

        [Test]
        public void TestTeleportationOff()
        {
            // Arrange
            Portal portal = new GameObject().AddComponent<Portal>();
            IPortalAnimator portalAnimator = Substitute.For<IPortalAnimator>();
            ITeleportableDetector teleportableDetector = Substitute.For<ITeleportableDetector>();
            IPortal pairedPortal = Substitute.For<IPortal>();
            ITeleportable teleportable = Substitute.For<ITeleportable>();
            Vector3? teleportationPosition = null;
            teleportable.TeleportTo(Arg.Do<Vector3>(x =>
            {
                teleportationPosition = x;
            }));

            // Act
            portal.Initialize(portalAnimator, teleportableDetector, pairedPortal);
            teleportableDetector.OnEnter += Raise.Event<Action<ITeleportable>>(teleportable);

            // Assert
            Assert.IsNull(teleportationPosition);
        }

        [Test]
        public void TestTeleportationOn()
        {
            // Arrange
            Portal portal = new GameObject().AddComponent<Portal>();
            IPortalAnimator portalAnimator = Substitute.For<IPortalAnimator>();
            ITeleportableDetector teleportableDetector = Substitute.For<ITeleportableDetector>();
            IPortal pairedPortal = Substitute.For<IPortal>();
            pairedPortal.Postion.Returns(new Vector3(34f, 1111f, 1.2232f));
            ITeleportable teleportable = Substitute.For<ITeleportable>();
            Vector3? teleportationPosition = null;
            teleportable.TeleportTo(Arg.Do<Vector3>(x =>
            {
                teleportationPosition = x;
            }));

            // Act
            portal.Initialize(portalAnimator, teleportableDetector, pairedPortal);
            portal.Enabled = true;
            teleportableDetector.OnEnter += Raise.Event<Action<ITeleportable>>(teleportable);

            // Assert
            Assert.AreEqual(pairedPortal.Postion, teleportationPosition);
        }

        [Test]
        public void TestAnimatorEnable()
        {
            // Arange
            Portal portal = new GameObject().AddComponent<Portal>();
            IPortalAnimator portalAnimator = Substitute.For<IPortalAnimator>();
            bool animated = false;
            portalAnimator.When(x => x.AnimateEnable()).Do(x =>
            {
                animated = true;
            });
            ITeleportableDetector teleportableDetector = Substitute.For<ITeleportableDetector>();
            IPortal pairedPortal = Substitute.For<IPortal>();

            // Act
            portal.Initialize(portalAnimator, teleportableDetector, pairedPortal);
            portal.Enabled = true;

            // Assert
            Assert.IsTrue(animated);
        }

        [Test]
        public void TestAnimatorDisable()
        {
            // Arange
            Portal portal = new GameObject().AddComponent<Portal>();
            IPortalAnimator portalAnimator = Substitute.For<IPortalAnimator>();
            bool animated = false;
            portalAnimator.When(x => x.AnimateDisable()).Do(x =>
            {
                animated = true;
            });
            ITeleportableDetector teleportableDetector = Substitute.For<ITeleportableDetector>();
            IPortal pairedPortal = Substitute.For<IPortal>();

            // Act
            portal.Initialize(portalAnimator, teleportableDetector, pairedPortal);
            portal.Enabled = true;
            portal.Enabled = false;

            // Assert
            Assert.IsTrue(animated);
        }

        [Test]
        public void TestAnimatorTeleportation()
        {
            // Arange
            Portal portal = new GameObject().AddComponent<Portal>();
            IPortalAnimator portalAnimator = Substitute.For<IPortalAnimator>();
            bool animated = false;
            portalAnimator.When(x => x.AnimateTeleport()).Do(x =>
            {
                animated = true;
            });
            ITeleportableDetector teleportableDetector = Substitute.For<ITeleportableDetector>();
            IPortal pairedPortal = Substitute.For<IPortal>();

            // Act
            portal.Initialize(portalAnimator, teleportableDetector, pairedPortal);
            portal.Enabled = true;
            portal.Teleport(Substitute.For<ITeleportable>());

            // Assert
            Assert.IsTrue(animated);
        }
        
        // Event should be called when the object have been teleported
        [Test]
        public void TestTeleportEvent()
        {
            // Arange
            Portal portal = new GameObject().AddComponent<Portal>();
            IPortalAnimator portalAnimator = Substitute.For<IPortalAnimator>();
            ITeleportableDetector teleportableDetector = Substitute.For<ITeleportableDetector>();
            IPortal pairedPortal = Substitute.For<IPortal>();
            ITeleportable teleportable = Substitute.For<ITeleportable>();
            ITeleportable eventTeleportable = null;
            portal.OnTeleported += x => eventTeleportable = x;

            // Act
            portal.Initialize(portalAnimator, teleportableDetector, pairedPortal);
            portal.Enabled = true;
            portal.Teleport(teleportable);

            // Assert
            Assert.AreEqual(teleportable, eventTeleportable);
        }

        // When the object is teleported it should not be teleported back till it leaves portal
        [Test]
        public void TestTeleportCycle()
        {
            // Arange
            Portal portal = new GameObject().AddComponent<Portal>();
            IPortalAnimator portalAnimator = Substitute.For<IPortalAnimator>();
            ITeleportableDetector teleportableDetector = Substitute.For<ITeleportableDetector>();
            IPortal pairedPortal = Substitute.For<IPortal>();
            ITeleportable teleportable = Substitute.For<ITeleportable>();
            bool isTeleportCalled = false;
            teleportable.When(x => x.TeleportTo(Arg.Any<Vector3>())).Do(x => isTeleportCalled = true);

            // Act
            portal.Initialize(portalAnimator, teleportableDetector, pairedPortal);
            portal.Enabled = true;
            pairedPortal.OnTeleported += Raise.Event<Action<ITeleportable>>(teleportable);
            teleportableDetector.OnEnter += Raise.Event<Action<ITeleportable>>(teleportable);

            // Assert
            Assert.IsFalse(isTeleportCalled);
            
        }

        // When the object was teleported, left portal and enter again it should be teleported
        [Test]
        public void TestTeleportBack()
        {
            // Arange
            Portal portal = new GameObject().AddComponent<Portal>();
            IPortalAnimator portalAnimator = Substitute.For<IPortalAnimator>();
            ITeleportableDetector teleportableDetector = Substitute.For<ITeleportableDetector>();
            IPortal pairedPortal = Substitute.For<IPortal>();
            ITeleportable teleportable = Substitute.For<ITeleportable>();
            int teleortedCalledCount = 0;
            teleportable.When(x => x.TeleportTo(Arg.Any<Vector3>())).Do(x => teleortedCalledCount++);

            // Act
            portal.Initialize(portalAnimator, teleportableDetector, pairedPortal);
            portal.Enabled = true;
            pairedPortal.OnTeleported += Raise.Event<Action<ITeleportable>>(teleportable);
            teleportableDetector.OnEnter += Raise.Event<Action<ITeleportable>>(teleportable);
            teleportableDetector.OnExit += Raise.Event<Action<ITeleportable>>(teleportable);
            teleportableDetector.OnEnter += Raise.Event<Action<ITeleportable>>(teleportable);

            // Assert
            Assert.AreEqual(1, teleortedCalledCount);
        }
    }
}
