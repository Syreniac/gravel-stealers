using Assets.Scripts.Behaviours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Selections
{
    public class InteractableEntity : BehaviourEntity
    {
        private List<EntityInteractor> interactors = new List<EntityInteractor>();

        void OnDisable()
        {
            foreach (EntityInteractor interactor in interactors){
                InteractorRegistry.Unregister(interactor, gameObject);
            }
        }

        void addInteractor(EntityInteractor interactor)
        {
            this.interactors.Add(interactor);
            InteractorRegistry.Register(interactor, gameObject);
        }

        void removeInteractor(EntityInteractor interactor)
        {
            this.interactors.Remove(interactor);
            InteractorRegistry.Unregister(interactor, gameObject);
        }
    }
}
