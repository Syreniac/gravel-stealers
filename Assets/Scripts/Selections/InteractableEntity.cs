using Assets.Scripts.Behaviours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Selections
{
    public abstract class InteractableEntity : BehaviourEntity
    {
        private List<EntityInteractor> interactors = new List<EntityInteractor>();

        void OnDisable()
        {
            foreach (EntityInteractor interactor in interactors){
                InteractorRegistry.Unregister(interactor, gameObject);
            }
        }

        public void addInteractor(EntityInteractor interactor)
        {
            this.interactors.Add(interactor);
            InteractorRegistry.Register(interactor, gameObject);
        }

        public void removeInteractor(EntityInteractor interactor)
        {
            this.interactors.Remove(interactor);
            InteractorRegistry.Unregister(interactor, gameObject);
        }
    }
}
