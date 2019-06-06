using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeFramework.components {
    public class ItemCanShootComponent : AbstractComponent {

        public float range, power;

        public ItemCanShootComponent(float r, float p) {
            this.range = r;
            this.power = p;
        }
    }
}
