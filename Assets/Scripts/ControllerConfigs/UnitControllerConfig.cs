using Assets.Scripts.Behaviours;
using Assets.Scripts.Controls;
using Assets.Scripts.Mouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.ControllerConfigs
{
    public class UnitControllerConfig
    {
        private List<IControlAction> controls = new List<IControlAction>();
        private List<EntityBehaviour> behaviours = new List<EntityBehaviour>();
        private BehaviourEntity unit;

        public UnitControllerConfig(BehaviourEntity unit)
        {
            this.unit = unit;
            controls.Add(new MouseControl());
        }

        public void Update()
        {
            foreach (IControlAction control in controls)
            {
                control.Check();
            }

            foreach (EntityBehaviour behaviour in behaviours)
            {
                behaviour.Act(unit.gameObject);
            }
        }
    }
}
