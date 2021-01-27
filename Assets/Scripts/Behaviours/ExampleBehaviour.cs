using Assets.Scripts.Utils;
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
            Rotations.turn(entity.transform, Rotations.TurnDirection.Anticlockwise, 180, TimeSpan.FromSeconds(1.0));
            //Rotations.roll(entity.transform, Rotations.RollDirection.LEFT, 180, TimeSpan.FromSeconds(1.0));
            //Rotations.roll(entity.transform, Rotations.RollDirection.FORWARD, 180, TimeSpan.FromSeconds(1.0));
        }
    }
}
