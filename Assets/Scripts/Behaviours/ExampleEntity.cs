using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Behaviours
{
    public class ExampleEntity : BehaviourEntity
    {
        public override void DoUpdate()
        {
        }

        // Use this for initialization
        void Start()
        {
            this.addBehaviour(new ExampleBehaviour());
        }

    }
}
