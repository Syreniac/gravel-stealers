using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Events
{
    public class EventRegistry
    {
        public static Dictionary<Type, List<EventListener>> eventListeners = new Dictionary<Type, List<EventListener>>();

        public static T BroadcastEvent<T>(T eventData) where T : IEvent
        {
            if (eventListeners.ContainsKey(eventData.GetType()))
            {
                foreach (EventListener<T> listener in eventListeners[typeof(T)])
                {
                    listener.OnEvent(eventData);
                }
            }
            return eventData;
        }

        public static void Listen<T>(EventListener<T> listener) where T : IEvent
        {
            if (!eventListeners.ContainsKey(typeof(T)))
            {
                eventListeners.Add(typeof(T), new List<EventListener>());

            }
            eventListeners[typeof(T)].Add(listener);
        }

        public static void UnListen<T>(EventListener<T> listener) where T : IEvent
        {
            if (!eventListeners.ContainsKey(typeof(T)))
            {
                eventListeners[typeof(T)].Remove(listener);
            }
        }
    }
}
