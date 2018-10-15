using Assets.Scripts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Mouse
{
    public class MouseClickEvent : CancellableEvent
    {

        private Vector3 mouseVector;
        private GameObject target;

        public MouseClickEvent(Vector3 mouseVector, GameObject target)
        {
            this.mouseVector = mouseVector;
            this.target = target;
        }

        public GameObject GetTarget()
        {
            return target;
        }

    }
}
