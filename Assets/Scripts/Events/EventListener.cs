using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Events
{
    public interface EventListener
    {

    }

    public abstract class EventListener<T> : EventListener where T : IEvent
    {
        public abstract void OnEvent(T eventData);

        public EventListener()
        {
            EventRegistry.Listen<T>(this);
        }

        public void Unregister()
        {
            EventRegistry.UnListen<T>(this);
        }

    }
}
