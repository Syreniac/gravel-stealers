using Assets.Scripts.Behaviours;
using Assets.Scripts.Controls;
using Assets.Scripts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Mouse
{
    public class MouseControl : IControlAction
    {
        public Texture2D cursorTexture = Resources.Load<Texture2D>("pointer");
        public CursorMode cursorMode = CursorMode.Auto;
        public Vector2 hotSpot = Vector2.zero;
        public bool cursor = false;
        Vector3 renderPosition = Vector3.zero;

        private MonoBehaviour entity;


        public MouseControl(MonoBehaviour entity)
        {
            this.entity = entity;
        }



        public void Check()
        {
            if (!cursor)
            {
                Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
                cursor = true;
            }
            float distance = 0f;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane hPlane = new Plane(Vector3.up, new Vector3(0, entity.transform.position.y, 0));
            if (hPlane.Raycast(ray, out distance))
            {
                renderPosition = ray.GetPoint(distance);
                renderPosition = new Vector3(renderPosition.x, renderPosition.y + 0.1f, renderPosition.z);
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                EventRegistry.BroadcastEvent<MouseClickEvent>(new MouseClickEvent(entity, renderPosition));
            }

        }



        void OnMouseExit()
        {
            Cursor.SetCursor(null, Vector2.zero, cursorMode);
        }
    }
}
