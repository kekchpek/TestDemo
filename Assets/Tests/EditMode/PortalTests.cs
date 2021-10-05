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
            // Arange
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
            // Arange
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
            // Arange
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
            // Arange
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
            // Arange
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
            // Arange
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
            Assert.Fail("The test is not implemented");
        }

        [Test]
        public void TestAnimatorDisable()
        {
            Assert.Fail("The test is not implemented");
        }

        [Test]
        public void TestAnimatorTeleportation()
        {
            Assert.Fail("The test is not implemented");
        }
        
        // Event should be called when the object have been teleported
        [Test]
        public void TestTeleportEvent()
        {
            Assert.Fail("The test is not implemented");
        }

        // When the object is teleported it should not be teleported back till it leaves portal
        [Test]
        public void TestTeleportCycle()
        {
            Assert.Fail("The test is not implemented");
        }

        // When the object was teleported, left portal and enter again it should be teleported
        [Test]
        public void TestTeleportBack()
        {
            Assert.Fail("The test is not implemented");
        }
    }
}
