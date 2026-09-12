using System;
using UnityEngine;

namespace DialoguePack.Runtime
{
    public enum DialogueEndResult
    {
        Completed,
        Failed,
        Cancelled
    }

    public enum DialogueSignalKind
    {
        Generic,
        LockInput,
        LockCamera,
        PlayOneShot,
        TriggerAnimation
    }

    [Serializable]
    public struct DialogueContext
    {
        public string SpeakerId;
        public GameObject Target;
        public bool LockInput;
        public bool LockCamera;
        public string ConversationId;

        [Tooltip("Optional blackboard quest service reference. Must implement IQuestService.")]
        public UnityEngine.Object QuestServiceOverride;
    }

    [Serializable]
    public struct DialogueLine
    {
        public string SpeakerId;
        [TextArea]
        public string Text;
        public string Tags;
        public string AudioKey;

        public DialogueLine(string speakerId, string text, string tags, string audioKey)
        {
            SpeakerId = speakerId;
            Text = text;
            Tags = tags;
            AudioKey = audioKey;
        }
    }

    [Serializable]
    public struct DialogueChoiceView
    {
        public string ChoiceId;
        public string Text;
        public bool Enabled;

        public DialogueChoiceView(string choiceId, string text, bool enabled)
        {
            ChoiceId = choiceId;
            Text = text;
            Enabled = enabled;
        }
    }

    [Serializable]
    public struct DialogueSignal
    {
        public DialogueSignalKind Kind;
        public string EventId;
        public string Payload;

        public DialogueSignal(DialogueSignalKind kind, string eventId, string payload)
        {
            Kind = kind;
            EventId = eventId;
            Payload = payload;
        }
    }

    [Serializable]
    public struct DialogueCheckpoint
    {
        public string CheckpointId;
        public string JumpLabel;
        public string SpeakerId;
        public string ConversationId;
        public string TreeName;
        public long CreatedAtUnixMs;

        public DialogueCheckpoint(
            string checkpointId,
            string jumpLabel,
            string speakerId,
            string conversationId,
            string treeName,
            long createdAtUnixMs)
        {
            CheckpointId = checkpointId;
            JumpLabel = jumpLabel;
            SpeakerId = speakerId;
            ConversationId = conversationId;
            TreeName = treeName;
            CreatedAtUnixMs = createdAtUnixMs;
        }
    }
}
