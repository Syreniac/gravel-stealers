using Assets.Scripts.Behaviours;
using Assets.Scripts.ControllerConfigs;
using Assets.Scripts.Selections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public class UnitController : InteractableEntity
    {
        private UnitControllerConfig controllerConfig;

        public void Start()
        {
            this.addBehaviour(new ExampleBehaviour());
            controllerConfig = new UnitControllerConfig(this);
        }

        public void Update()
        {
            controllerConfig.Update();
        }
    }
}
