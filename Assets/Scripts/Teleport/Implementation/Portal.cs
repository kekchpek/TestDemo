using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace TestDemo.Teleport
{
    public class Portal : MonoBehaviour, IPortal
    {

        private bool _enabled = false;

        private IPortalAnimator _portalAnimator;
        private ITeleportableDetector _teleportableDetector;
        private IPortal _pairedPortal;

        private readonly ISet<ITeleportable> _teleportedObjects = new HashSet<ITeleportable>();
        public void Initialize(IPortalAnimator portalAnimator, ITeleportableDetector teleportableDetector, IPortal pairedPortal)
        {
            _portalAnimator = portalAnimator;
            _teleportableDetector = teleportableDetector ?? throw new ArgumentNullException(nameof(teleportableDetector));
            _pairedPortal = pairedPortal ?? throw new ArgumentNullException(nameof(pairedPortal));

            _teleportableDetector.OnEnter += Teleport;
            _teleportableDetector.OnExit += UnmarkTeleported;
            _pairedPortal.OnTeleported += MarkTeleported;
        }

        public bool Enabled
        {
            get => _enabled;
            set
            {
                if (value == _enabled)
                    return;
                _enabled = value;
                if (_enabled)
                    _portalAnimator?.AnimateEnable();
                else
                    _portalAnimator?.AnimateDisable();
                PairedPortal.Enabled = value;
            }
        }

        public IPortal PairedPortal => _pairedPortal;

        public Vector3 Postion => transform.position;

        public event Action<ITeleportable> OnTeleported;

        public void Teleport(ITeleportable teleportable)
        {
            if (!_enabled)
                return;
            if (_teleportedObjects.Contains(teleportable))
                return;
            _portalAnimator?.AnimateTeleport();
            OnTeleported?.Invoke(teleportable);
            teleportable.TeleportTo(PairedPortal.Postion);
        }

        private void OnDestroy()
        {
            _teleportableDetector.OnEnter -= Teleport;
            _teleportableDetector.OnExit -= UnmarkTeleported;
            _pairedPortal.OnTeleported -= MarkTeleported;
        }

        private void UnmarkTeleported(ITeleportable teleportable)
        {
            if (_teleportedObjects.Contains(teleportable))
            {
                _teleportedObjects.Remove(teleportable);
            }
        }

        private void MarkTeleported(ITeleportable teleportable)
        {
            Assert.IsFalse(_teleportedObjects.Contains(teleportable));
            _teleportedObjects.Add(teleportable);
        }
    }
}
