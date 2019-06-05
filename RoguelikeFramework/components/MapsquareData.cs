using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeFramework.components {

    public class MapsquareData : AbstractComponent {

        public bool visible, seen, traversable;
        public float traverseCost;

        public MapsquareData() {
            this.visible = false;
            this.seen = false;
            this.traversable = true;
            this.traverseCost = 1f;
        }
    }
}
