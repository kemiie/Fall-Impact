using System;
using UnityEngine;

namespace DialoguePack.Runtime
{
    [DisallowMultipleComponent]
    public sealed class SimpleQuestServiceComponent : MonoBehaviour, IQuestService, IQuestEventBus
    {
        [Header("Persistence")]
        [Tooltip("If true, quest state is loaded from PlayerPrefs on Awake and saved on change.")]
        public bool EnablePersistence;

        [Tooltip("PlayerPrefs key used by the demo service.")]
        public string PersistenceKey = "DialoguePack.SimpleQuestService";

        [Tooltip("If true, this component creates itself as a global singleton when first used.")]
        public bool DontDestroy = true;

        private readonly SimpleQuestService _service = new();

        public event Action<QuestChangedEvent> QuestChanged
        {
            add => _service.QuestChanged += value;
            remove => _service.QuestChanged -= value;
        }

        public event Action<QuestEventData> QuestEventRaised
        {
            add => _service.QuestEventRaised += value;
            remove => _service.QuestEventRaised -= value;
        }

        private void Awake()
        {
            _service.EnablePersistence = EnablePersistence;
            _service.PersistenceKey = string.IsNullOrWhiteSpace(PersistenceKey)
                ? "DialoguePack.SimpleQuestService"
                : PersistenceKey;

            if (EnablePersistence)
                _service.LoadFromPlayerPrefs();

            if (DontDestroy)
                DontDestroyOnLoad(gameObject);
        }

        private void OnApplicationQuit()
        {
            if (EnablePersistence)
                _service.SaveToPlayerPrefs();
        }

        public bool HasQuest(string questId) => _service.HasQuest(questId);
        public QuestState GetState(string questId) => _service.GetState(questId);
        public int GetStage(string questId) => _service.GetStage(questId);
        public int GetProgress(string questId) => _service.GetProgress(questId);
        public bool HasFlag(string questId, string flagId) => _service.HasFlag(questId, flagId);
        public bool TryGetSnapshot(string questId, out QuestSnapshot snapshot) => _service.TryGetSnapshot(questId, out snapshot);
        public bool OfferQuest(string questId, string journalEntry = null) => _service.OfferQuest(questId, journalEntry);
        public bool CompleteQuest(string questId) => _service.CompleteQuest(questId);
        public bool FailQuest(string questId) => _service.FailQuest(questId);
        public bool SetStage(string questId, int stage) => _service.SetStage(questId, stage);
        public bool AdvanceStage(string questId, int amount = 1) => _service.AdvanceStage(questId, amount);
        public bool SetProgress(string questId, int progress) => _service.SetProgress(questId, progress);
        public bool AddProgress(string questId, int amount) => _service.AddProgress(questId, amount);
        public bool SetFlag(string questId, string flagId, bool value) => _service.SetFlag(questId, flagId, value);
        public void RaiseQuestEvent(string eventId, string questId = null, string payload = null) => _service.RaiseQuestEvent(eventId, questId, payload);
        public int SubscribeQuestEvent(string eventId, Action<QuestEventData> callback) => _service.SubscribeQuestEvent(eventId, callback);
        public void UnsubscribeQuestEvent(int subscriptionId) => _service.UnsubscribeQuestEvent(subscriptionId);
    }
}
