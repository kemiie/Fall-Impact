using UnityEngine;

namespace DialoguePack.Runtime
{
    [DisallowMultipleComponent]
    public sealed class GameSignalProvider : MonoBehaviour, IGameSignalBus
    {
        [Tooltip("Optional explicit signal bus implementation.")]
        public MonoBehaviour SignalBusBehaviour;

        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetSharedState() { Active = null; }

        public static GameSignalProvider Active { get; private set; }

        private IGameSignalBus ResolvedBus
        {
            get
            {
                if (SignalBusBehaviour is IGameSignalBus typed)
                    return typed;

                if (TryGetComponent<IGameSignalBus>(out var local) && !ReferenceEquals(local, this))
                    return local;

                return GameSignalBus.Instance;
            }
        }

        private void Awake()
        {
            Active = this;
        }

        private void OnDestroy()
        {
            if (Active == this)
                Active = null;
        }

        public void RaiseSignal(string eventId, string payload = null)
        {
            ResolvedBus.RaiseSignal(eventId, payload);
        }

        public int Subscribe(string eventId, System.Action<string, string> callback)
        {
            return ResolvedBus.Subscribe(eventId, callback);
        }

        public void Unsubscribe(int subscriptionId)
        {
            ResolvedBus.Unsubscribe(subscriptionId);
        }
    }
}
