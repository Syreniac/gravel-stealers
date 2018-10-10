using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Behaviours
{
    //Example rotate behaviour, 
    public class ExampleBehaviour : EntityBehaviour
    {
        public void Act(GameObject entity)
        {
            entity.transform.Rotate(0, -300 * Time.deltaTime * 1.0f, 0);
        }
    }
}
