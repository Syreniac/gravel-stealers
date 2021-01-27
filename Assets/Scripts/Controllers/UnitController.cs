using Assets.Scripts.Behaviours;
using Assets.Scripts.ControllerConfigs;
using Assets.Scripts.Selections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class UnitController : InteractableEntity
    {
        private UnitControllerConfig controllerConfig;

        public override void DoUpdate()
        {
            controllerConfig.Update();
        }

        public void Start()
        {
            this.addInteractor(new SelectEntityInteractor(Select));
            this.addBehaviour(new ExampleBehaviour());
            controllerConfig = new UnitControllerConfig(this);
        }

        bool isSelected = false;
        Shader shader = null;

        public void Select()
        {
            if (!isSelected)
            {
                Material mat = this.gameObject.GetComponent<Material>();
                Shader shader = mat.shader;
                mat.shader = Shader.Find("Outlined/Silhouetted Diffuse");
                isSelected = true;
            } else
            {
                Material mat = this.gameObject.GetComponent<Material>();
                mat.shader = this.shader;
                isSelected = false;
            }
            
        }

    }
}
