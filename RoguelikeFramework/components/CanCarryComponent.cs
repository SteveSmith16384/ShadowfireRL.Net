using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeFramework.components {

    public class CanCarryComponent : AbstractComponent {

        public List<AbstractEntity> items = new List<AbstractEntity>();
        public float maxWeight;

        public CanCarryComponent(float max) {
            this.maxWeight = max;
        }

    }

}
