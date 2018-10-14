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
        private MonoBehaviour entity;

        private Vector3 mouseVector;

        public MouseClickEvent(MonoBehaviour entity, Vector3 mouseVector)
        {
            this.entity = entity;
            this.mouseVector = mouseVector;
        }

    }
}
