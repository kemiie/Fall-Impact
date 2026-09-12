using System;

namespace DialoguePack.Runtime
{
    public interface IQuestEventBus
    {
        void RaiseQuestEvent(string eventId, string questId = null, string payload = null);

        int SubscribeQuestEvent(string eventId, Action<QuestEventData> callback);

        void UnsubscribeQuestEvent(int subscriptionId);

        event Action<QuestEventData> QuestEventRaised;
    }

    public interface IQuestService
    {
        bool HasQuest(string questId);

        QuestState GetState(string questId);

        int GetStage(string questId);

        int GetProgress(string questId);

        bool HasFlag(string questId, string flagId);

        bool TryGetSnapshot(string questId, out QuestSnapshot snapshot);

        bool OfferQuest(string questId, string journalEntry = null);

        bool CompleteQuest(string questId);

        bool FailQuest(string questId);

        bool SetStage(string questId, int stage);

        bool AdvanceStage(string questId, int amount = 1);

        bool SetProgress(string questId, int progress);

        bool AddProgress(string questId, int amount);

        bool SetFlag(string questId, string flagId, bool value);

        event Action<QuestChangedEvent> QuestChanged;
    }
}
