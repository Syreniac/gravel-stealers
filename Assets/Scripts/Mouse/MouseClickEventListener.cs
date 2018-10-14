using Assets.Scripts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Mouse
{
    public class MouseClickEventListener : EventListener<MouseClickEvent>
    {
        private Action<MouseClickEvent> function;

        public MouseClickEventListener(Action<MouseClickEvent> function){
            this.function = function;
            }

        public override void OnEvent(MouseClickEvent eventData)
        {
            function(eventData);
        }
    }
}
