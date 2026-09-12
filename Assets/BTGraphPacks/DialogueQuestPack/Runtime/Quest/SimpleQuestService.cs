using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialoguePack.Runtime
{
    [Serializable]
    public sealed class SimpleQuestService : IQuestService, IQuestEventBus
    {
        [Serializable]
        private sealed class QuestRuntimeData
        {
            public string QuestId;
            public QuestState State = QuestState.Inactive;
            public int Stage;
            public int Progress;
            public List<string> Flags = new();

            public bool HasFlag(string flagId)
            {
                if (string.IsNullOrWhiteSpace(flagId))
                    return false;

                for (var i = 0; i < Flags.Count; i++)
                {
                    if (string.Equals(Flags[i], flagId, StringComparison.Ordinal))
                        return true;
                }

                return false;
            }

            public bool SetFlag(string flagId, bool value)
            {
                if (string.IsNullOrWhiteSpace(flagId))
                    return false;

                var index = Flags.FindIndex(flag => string.Equals(flag, flagId, StringComparison.Ordinal));
                if (value)
                {
                    if (index >= 0)
                        return false;

                    Flags.Add(flagId);
                    return true;
                }

                if (index < 0)
                    return false;

                Flags.RemoveAt(index);
                return true;
            }

            public QuestSnapshot ToSnapshot()
            {
                return new QuestSnapshot
                {
                    QuestId = QuestId,
                    State = State,
                    Stage = Stage,
                    Progress = Progress
                };
            }
        }

        [Serializable]
        private sealed class PersistedQuestState
        {
            public List<QuestRuntimeData> Quests = new();
        }

        private sealed class QuestSubscription
        {
            public int Id;
            public string EventId;
            public Action<QuestEventData> Callback;
        }

        private readonly Dictionary<string, QuestRuntimeData> _quests = new(StringComparer.Ordinal);
        private readonly Dictionary<int, QuestSubscription> _subscriptions = new();
        private readonly List<int> _raiseEventBuffer = new();
        private int _nextSubscriptionId = 1;

        public event Action<QuestChangedEvent> QuestChanged;
        public event Action<QuestEventData> QuestEventRaised;

        public bool EnablePersistence { get; set; }
        public string PersistenceKey { get; set; } = "DialoguePack.SimpleQuestService";

        public bool HasQuest(string questId)
        {
            return _quests.ContainsKey(NormalizeId(questId));
        }

        public QuestState GetState(string questId)
        {
            return TryGetData(questId, out var data) ? data.State : QuestState.Inactive;
        }

        public int GetStage(string questId)
        {
            return TryGetData(questId, out var data) ? data.Stage : 0;
        }

        public int GetProgress(string questId)
        {
            return TryGetData(questId, out var data) ? data.Progress : 0;
        }

        public bool HasFlag(string questId, string flagId)
        {
            return TryGetData(questId, out var data) && data.HasFlag(flagId);
        }

        public bool TryGetSnapshot(string questId, out QuestSnapshot snapshot)
        {
            if (!TryGetData(questId, out var data))
            {
                snapshot = default;
                return false;
            }

            snapshot = data.ToSnapshot();
            return true;
        }

        public bool OfferQuest(string questId, string journalEntry = null)
        {
            var data = GetOrCreateData(questId);
            if (data == null)
                return false;

            // Offer should only activate quests that have not started yet.
            if (data.State != QuestState.Inactive)
                return false;

            data.State = QuestState.Active;
            if (data.Stage == 0)
                data.Stage = 1;

            NotifyQuestChanged(data);

            if (!string.IsNullOrWhiteSpace(journalEntry))
                RaiseQuestEvent("quest.journal", data.QuestId, journalEntry);

            RaiseQuestEvent("quest.offered", data.QuestId, journalEntry);
            PersistIfEnabled();
            return true;
        }

        public bool CompleteQuest(string questId)
        {
            var data = GetOrCreateData(questId);
            if (data == null || data.State == QuestState.Completed)
                return false;

            data.State = QuestState.Completed;
            NotifyQuestChanged(data);
            RaiseQuestEvent("quest.completed", data.QuestId, null);
            PersistIfEnabled();
            return true;
        }

        public bool FailQuest(string questId)
        {
            var data = GetOrCreateData(questId);
            if (data == null || data.State == QuestState.Failed)
                return false;

            data.State = QuestState.Failed;
            NotifyQuestChanged(data);
            RaiseQuestEvent("quest.failed", data.QuestId, null);
            PersistIfEnabled();
            return true;
        }

        public bool SetStage(string questId, int stage)
        {
            var data = GetOrCreateData(questId);
            if (data == null || data.Stage == stage)
                return false;

            data.Stage = stage;
            if (data.State == QuestState.Inactive)
                data.State = QuestState.Active;

            NotifyQuestChanged(data);
            RaiseQuestEvent("quest.stage.changed", data.QuestId, stage.ToString());
            PersistIfEnabled();
            return true;
        }

        public bool AdvanceStage(string questId, int amount = 1)
        {
            if (amount == 0)
                return false;

            var data = GetOrCreateData(questId);
            if (data == null)
                return false;

            data.Stage += amount;
            if (data.Stage < 0)
                data.Stage = 0;

            if (data.State == QuestState.Inactive)
                data.State = QuestState.Active;

            NotifyQuestChanged(data);
            RaiseQuestEvent("quest.stage.changed", data.QuestId, data.Stage.ToString());
            PersistIfEnabled();
            return true;
        }

        public bool SetProgress(string questId, int progress)
        {
            var data = GetOrCreateData(questId);
            if (data == null || data.Progress == progress)
                return false;

            data.Progress = progress;
            if (data.State == QuestState.Inactive)
                data.State = QuestState.Active;

            NotifyQuestChanged(data);
            RaiseQuestEvent("quest.progress.changed", data.QuestId, progress.ToString());
            PersistIfEnabled();
            return true;
        }

        public bool AddProgress(string questId, int amount)
        {
            if (amount == 0)
                return false;

            var data = GetOrCreateData(questId);
            if (data == null)
                return false;

            data.Progress += amount;
            if (data.State == QuestState.Inactive)
                data.State = QuestState.Active;

            NotifyQuestChanged(data);
            RaiseQuestEvent("quest.progress.changed", data.QuestId, data.Progress.ToString());
            PersistIfEnabled();
            return true;
        }

        public bool SetFlag(string questId, string flagId, bool value)
        {
            var data = GetOrCreateData(questId);
            if (data == null)
                return false;

            if (!data.SetFlag(flagId, value))
                return false;

            NotifyQuestChanged(data);
            RaiseQuestEvent(value ? "quest.flag.set" : "quest.flag.cleared", data.QuestId, flagId);
            PersistIfEnabled();
            return true;
        }

        public void RaiseQuestEvent(string eventId, string questId = null, string payload = null)
        {
            var normalizedEventId = NormalizeId(eventId);
            if (string.IsNullOrWhiteSpace(normalizedEventId))
                return;

            var eventData = new QuestEventData(normalizedEventId, NormalizeId(questId), payload ?? string.Empty);
            QuestEventRaised?.Invoke(eventData);

            if (_subscriptions.Count == 0)
                return;

            _raiseEventBuffer.Clear();
            foreach (var pair in _subscriptions)
                _raiseEventBuffer.Add(pair.Key);

            for (var i = 0; i < _raiseEventBuffer.Count; i++)
            {
                if (!_subscriptions.TryGetValue(_raiseEventBuffer[i], out var subscription))
                    continue;

                if (subscription == null || subscription.Callback == null)
                    continue;

                if (!string.IsNullOrEmpty(subscription.EventId)
                    && !string.Equals(subscription.EventId, normalizedEventId, StringComparison.Ordinal))
                {
                    continue;
                }

                subscription.Callback.Invoke(eventData);
            }
        }

        public int SubscribeQuestEvent(string eventId, Action<QuestEventData> callback)
        {
            if (callback == null)
                return -1;

            var id = _nextSubscriptionId++;
            _subscriptions[id] = new QuestSubscription
            {
                Id = id,
                EventId = NormalizeId(eventId),
                Callback = callback
            };
            return id;
        }

        public void UnsubscribeQuestEvent(int subscriptionId)
        {
            if (subscriptionId <= 0)
                return;

            _subscriptions.Remove(subscriptionId);
        }

        public void LoadFromPlayerPrefs()
        {
            if (string.IsNullOrWhiteSpace(PersistenceKey) || !PlayerPrefs.HasKey(PersistenceKey))
                return;

            var json = PlayerPrefs.GetString(PersistenceKey, string.Empty);
            if (string.IsNullOrWhiteSpace(json))
                return;

            try
            {
                var persisted = JsonUtility.FromJson<PersistedQuestState>(json);
                _quests.Clear();
                if (persisted?.Quests != null)
                {
                    for (var i = 0; i < persisted.Quests.Count; i++)
                    {
                        var data = persisted.Quests[i];
                        if (data == null || string.IsNullOrWhiteSpace(data.QuestId))
                            continue;

                        data.QuestId = NormalizeId(data.QuestId);
                        _quests[data.QuestId] = data;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"SimpleQuestService failed to load persistence: {ex.Message}");
            }
        }

        public void SaveToPlayerPrefs()
        {
            if (string.IsNullOrWhiteSpace(PersistenceKey))
                return;

            var persisted = new PersistedQuestState
            {
                Quests = new List<QuestRuntimeData>(_quests.Values)
            };

            var json = JsonUtility.ToJson(persisted);
            PlayerPrefs.SetString(PersistenceKey, json);
            PlayerPrefs.Save();
        }

        public void ClearAll()
        {
            _quests.Clear();
        }

        private void PersistIfEnabled()
        {
            if (EnablePersistence)
                SaveToPlayerPrefs();
        }

        private bool TryGetData(string questId, out QuestRuntimeData data)
        {
            return _quests.TryGetValue(NormalizeId(questId), out data);
        }

        private QuestRuntimeData GetOrCreateData(string questId)
        {
            var normalizedId = NormalizeId(questId);
            if (string.IsNullOrWhiteSpace(normalizedId))
                return null;

            if (_quests.TryGetValue(normalizedId, out var existing))
                return existing;

            var data = new QuestRuntimeData
            {
                QuestId = normalizedId,
                State = QuestState.Inactive,
                Stage = 0,
                Progress = 0
            };
            _quests[normalizedId] = data;
            return data;
        }

        private void NotifyQuestChanged(QuestRuntimeData data)
        {
            if (data == null)
                return;

            QuestChanged?.Invoke(new QuestChangedEvent(
                data.QuestId,
                data.State,
                data.Stage,
                data.Progress));
        }

        private static string NormalizeId(string id)
        {
            return string.IsNullOrWhiteSpace(id) ? string.Empty : id.Trim();
        }
    }
}
