using System;
using System.Collections.Generic;

namespace DialoguePack.Runtime
{
    public sealed class GameSignalBus : IGameSignalBus
    {
        private sealed class Subscription
        {
            public int Id;
            public string EventId;
            public Action<string, string> Callback;
        }

        private readonly Dictionary<int, Subscription> _subscriptions = new();
        private readonly List<int> _raiseBuffer = new();
        private int _nextId = 1;

        public static GameSignalBus Instance { get; } = new();

        public void RaiseSignal(string eventId, string payload = null)
        {
            if (string.IsNullOrWhiteSpace(eventId))
                return;

            if (_subscriptions.Count == 0)
                return;

            _raiseBuffer.Clear();
            foreach (var pair in _subscriptions)
                _raiseBuffer.Add(pair.Key);

            for (var i = 0; i < _raiseBuffer.Count; i++)
            {
                if (!_subscriptions.TryGetValue(_raiseBuffer[i], out var sub))
                    continue;

                if (sub?.Callback == null)
                    continue;

                if (!string.IsNullOrEmpty(sub.EventId)
                    && !string.Equals(sub.EventId, eventId, StringComparison.Ordinal))
                    continue;

                sub.Callback(eventId, payload ?? string.Empty);
            }
        }

        public int Subscribe(string eventId, Action<string, string> callback)
        {
            if (callback == null)
                return -1;

            var id = _nextId++;
            _subscriptions[id] = new Subscription
            {
                Id = id,
                EventId = string.IsNullOrWhiteSpace(eventId) ? string.Empty : eventId.Trim(),
                Callback = callback
            };
            return id;
        }

        public void Unsubscribe(int subscriptionId)
        {
            if (subscriptionId <= 0)
                return;

            _subscriptions.Remove(subscriptionId);
        }
    }
}
