using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Selections
{
    public class SelectEntityInteractor : EntityInteractor
    {
        private Action function;

        public SelectEntityInteractor(Action function)
        {
            this.function = function;
        }

        public void Interact()
        {
            function();
        }
    }
}
