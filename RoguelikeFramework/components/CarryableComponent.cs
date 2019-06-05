using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeFramework.components {

    public class CarryableComponent : AbstractComponent {

        public float weight;

        public CarryableComponent(float w) {
            this.weight = w;
        }
    }
}
