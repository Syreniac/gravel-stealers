using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Behaviours
{
    //Reverses the control flow of gameobjects from Unity
    //Instead of tying a script directly to a gameobject, this allows for one script to be tied to many gameObjects
    //Also allows for many behaviours to be added and removed, each of which will be called during the Update thread

    public abstract class BehaviourEntity : MonoBehaviour
    {
        private List<EntityBehaviour> behaviours = new List<EntityBehaviour>();

        private List<GameObject> alternates = new List<GameObject>();

        void Update()
        {
            foreach (EntityBehaviour behaviour in behaviours)
            {
                if (alternates.Count == 0)
                {
                    behaviour.Act(gameObject);
                } else
                {
                    foreach(GameObject alternate in alternates){
                        behaviour.Act(alternate);
                    }
                }
            }
            this.DoUpdate();
        }

        public abstract void DoUpdate();

        public void addBehaviour(EntityBehaviour behaviour)
        {
            this.behaviours.Add(behaviour);
        }

        public void removeBehaviour(EntityBehaviour behaviour)
        {
            this.behaviours.Remove(behaviour);
        }

        public void setGameObject(GameObject go)
        {
            this.alternates.Clear();
            this.alternates.Add(go);
        }

        public void setGameObjects(List<GameObject> gameObjects)
        {
            this.alternates = gameObjects;
        }

        public void addGameObject(GameObject go)
        {
            this.alternates.Add(go);
        }
    }
}
