using UnityEngine;

namespace DialoguePack.Runtime
{
    public abstract class StringIdAsset : ScriptableObject
    {
        [Tooltip("Stable ID used at runtime. Leave empty to fall back to asset name.")]
        public string Id;

        public string ResolvedId => string.IsNullOrWhiteSpace(Id) ? name : Id;
    }

    [CreateAssetMenu(menuName = "Dialogue Pack/IDs/Quest ID")]
    public sealed class QuestIdAsset : StringIdAsset { }

    [CreateAssetMenu(menuName = "Dialogue Pack/IDs/Flag ID")]
    public sealed class FlagIdAsset : StringIdAsset { }

    [CreateAssetMenu(menuName = "Dialogue Pack/IDs/Event ID")]
    public sealed class EventIdAsset : StringIdAsset { }

    public static class IdUtility
    {
        public static string Resolve(string textId, StringIdAsset asset)
        {
            if (asset != null)
                return asset.ResolvedId;

            return string.IsNullOrWhiteSpace(textId) ? string.Empty : textId.Trim();
        }
    }
}
