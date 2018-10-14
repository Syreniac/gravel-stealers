using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Selections
{
    public class InteractorRegistry
    {
        public class EntityRecord : Dictionary<Type, List<EntityInteractor>> {
            private int id;

            public EntityRecord(int id)
            {
                this.id = id;
            }

            public int getId()
            {
                return this.id;
            }
        }
        
        private static Dictionary<int, EntityRecord> entities = new Dictionary<int, EntityRecord>();

        public static void Register<T>(T entityInteractor, GameObject gameObject) where T : EntityInteractor
        {
            int id = gameObject.GetInstanceID();
            if (!entities.ContainsKey(id)){
                EntityRecord record = new EntityRecord(id);
                List<EntityInteractor> interactors = new List<EntityInteractor>();
                interactors.Add(entityInteractor);
                record.Add(typeof(T), interactors);
                entities.Add(gameObject.GetInstanceID(), record);
            } else
            {
                EntityRecord record = entities[id];
                if (record.ContainsKey(typeof(T))){
                    record[typeof(T)].Add(entityInteractor);
                } else
                {
                    List<EntityInteractor> interactors = new List<EntityInteractor>();
                    interactors.Add(entityInteractor);
                    record.Add(typeof(T), interactors);
                }
                entities.Add(gameObject.GetInstanceID(), record);
            }
            
        }

        public static void Unregister<T>(T entityInteractor, GameObject gameObject) where T : EntityInteractor
        {
            int id = gameObject.GetInstanceID();
            if (entities.ContainsKey(id))
            {
                EntityRecord record = entities[id];
                record[typeof(T)].Remove(entityInteractor);
                if(record.Keys.Count == 0)
                {
                    entities.Remove(id);
                }
            }
        }

    }
}
