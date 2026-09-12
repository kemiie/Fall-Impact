using System;

namespace DialoguePack.Runtime
{
    public enum QuestState
    {
        Inactive = 0,
        Active = 1,
        Completed = 2,
        Failed = 3
    }

    public enum QuestCompareMode
    {
        Equal,
        NotEqual,
        Greater,
        GreaterOrEqual,
        Less,
        LessOrEqual
    }

    public enum TimeoutResult
    {
        Failure,
        Success
    }

    [Serializable]
    public struct QuestSnapshot
    {
        public string QuestId;
        public QuestState State;
        public int Stage;
        public int Progress;
    }

    [Serializable]
    public struct QuestEventData
    {
        public string EventId;
        public string QuestId;
        public string Payload;

        public QuestEventData(string eventId, string questId, string payload)
        {
            EventId = eventId;
            QuestId = questId;
            Payload = payload;
        }
    }

    [Serializable]
    public struct QuestChangedEvent
    {
        public string QuestId;
        public QuestState State;
        public int Stage;
        public int Progress;

        public QuestChangedEvent(string questId, QuestState state, int stage, int progress)
        {
            QuestId = questId;
            State = state;
            Stage = stage;
            Progress = progress;
        }
    }
}
