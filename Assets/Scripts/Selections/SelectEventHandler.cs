using Assets.Scripts.Mouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Selections
{
    public class SelectEventHandler
    {
        private MouseClickEventListener listener;

        public SelectEventHandler()
        {
            this.listener = new MouseClickEventListener(SelectEvent);
        }

        public void SelectEvent(MouseClickEvent clickEvent)
        {
            GameObject selectedObject = clickEvent.GetTarget();
            if (InteractorRegistry.Contains(selectedObject)){
                InteractorRegistry.interact(typeof(SelectEntityInteractor), selectedObject);
            }
        }

    }
}
