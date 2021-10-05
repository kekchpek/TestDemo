using System;
using UnityEngine;

namespace TestDemo.Teleport
{
    public class Portal : MonoBehaviour, IPortal
    {

        private bool _enabled = false;

        private IPortalAnimator _portalAnimator;
        private ITeleportableDetector _teleportableDetector;
        private IPortal _pairedPortal;
        public void Initialize(IPortalAnimator portalAnimator, ITeleportableDetector teleportableDetector, IPortal pairedPortal)
        {
            _portalAnimator = portalAnimator;
            _teleportableDetector = teleportableDetector ?? throw new ArgumentNullException(nameof(teleportableDetector));
            _pairedPortal = pairedPortal ?? throw new ArgumentNullException(nameof(pairedPortal));

            _teleportableDetector.OnEnter += Teleport;
        }

        public bool Enabled
        {
            get => _enabled;
            set
            {
                if (value == _enabled)
                    return;
                _enabled = value;
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
            teleportable.TeleportTo(PairedPortal.Postion);
        }

        private void OnDestroy()
        {
            _teleportableDetector.OnEnter -= Teleport;
        }
    }
}
