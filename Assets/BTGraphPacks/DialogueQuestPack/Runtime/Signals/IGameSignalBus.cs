using System;

namespace DialoguePack.Runtime
{
    public interface IGameSignalBus
    {
        void RaiseSignal(string eventId, string payload = null);
        int Subscribe(string eventId, Action<string, string> callback);
        void Unsubscribe(int subscriptionId);
    }
}
