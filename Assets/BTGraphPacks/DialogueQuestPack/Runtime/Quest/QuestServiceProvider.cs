using UnityEngine;

namespace DialoguePack.Runtime
{
    public interface IQuestServiceProvider
    {
        IQuestService QuestService { get; }
        IQuestEventBus QuestEventBus { get; }
    }

    [DisallowMultipleComponent]
    public sealed class QuestServiceProvider : MonoBehaviour, IQuestServiceProvider
    {
        [Tooltip("Optional explicit quest service source. Must implement IQuestService (and ideally IQuestEventBus).")]
        public MonoBehaviour ServiceBehaviour;

        [Tooltip("Fallback: auto-create the demo SimpleQuestService if nothing is wired in scene/blackboard.")]
        public bool AutoCreateDemoService = true;

        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetSharedState() { _active = null; }

        private static QuestServiceProvider _active;
        public static QuestServiceProvider Active
        {
            get
            {
                if (_active == null)
                    _active = FindFirstObjectByType<QuestServiceProvider>();
                return _active;
            }
        }

        public IQuestService QuestService
        {
            get
            {
                if (ServiceBehaviour is IQuestService typed)
                    return typed;

                if (TryGetComponent<IQuestService>(out var local))
                    return local;

                return null;
            }
        }

        public IQuestEventBus QuestEventBus
        {
            get
            {
                if (ServiceBehaviour is IQuestEventBus typed)
                    return typed;

                if (TryGetComponent<IQuestEventBus>(out var local))
                    return local;

                return null;
            }
        }

        private void Awake()
        {
            _active = this;
        }

        private void OnDestroy()
        {
            if (_active == this)
                _active = null;
        }
    }
}
