using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    public class Rotations
    {
        public enum TurnDirection
        {
            Clockwise,
            Anticlockwise
        }

        public static void turn(Transform transform, TurnDirection d, float degrees, TimeSpan span)
        {
            if (d == TurnDirection.Anticlockwise)
            {
                degrees *= -1;
            }
            float time = (float)span.TotalSeconds;
            float rotate = degrees * Time.deltaTime * time;
            transform.Rotate(0, rotate, 0);
        }

        public enum RollDirection
        {
            FORWARD,
            BACKWARD,
            LEFT,
            RIGHT
        }

        

        public static void roll(Transform transform, RollDirection d, float degrees, TimeSpan span)
        {
            if (d == RollDirection.BACKWARD || d == RollDirection.RIGHT)
            {
                degrees *= -1;
            }
            float time = (float)span.TotalSeconds;
            float rotate = degrees * Time.deltaTime * time;
            if (d == RollDirection.FORWARD || d == RollDirection.BACKWARD) {
                transform.Rotate(rotate, 0, 0);
            } else
            {
                transform.Rotate(0, 0, rotate);
            }
        }

    }
}
