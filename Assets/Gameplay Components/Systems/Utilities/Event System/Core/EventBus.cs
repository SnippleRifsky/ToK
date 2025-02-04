using System;
using System.Collections.Generic;
using UnityEngine;

public static class EventBus
{
    private static readonly Dictionary<Type, List<Delegate>> _eventHandlers = new();

    public static void Subscribe<T>(Action<T> action)
    {
        var eventType = typeof(T);

        if (!_eventHandlers.ContainsKey(eventType))
        {
            _eventHandlers[eventType] = new List<Delegate>();
        }

        _eventHandlers[eventType].Add(action);
    }

    public static void Unsubscribe<T>(Action<T> action)
    {
        var handlers = CheckAndGetEventHandlers(typeof(T));
        if (handlers == null) return;

        handlers.Remove(action);

        if (handlers.Count == 0)
        {
            _eventHandlers.Remove(typeof(T));
        }
    }

    public static void Publish<T>(T eventData)
    {
        var handlers = CheckAndGetEventHandlers(typeof(T));
        if (handlers == null) return;

        foreach (var handler in new List<Delegate>(handlers)) // Prevent invalid enumeration during modification
        {
            try
            {
                ((Action<T>)handler)?.Invoke(eventData);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error dispatching event {typeof(T).Name}: {e}");
            }
        }
    }

    public static void Clear()
    {
        _eventHandlers.Clear();
    }

    private static List<Delegate> CheckAndGetEventHandlers(Type eventType)
    {
        return _eventHandlers.ContainsKey(eventType) ? _eventHandlers[eventType] : null;
    }
}