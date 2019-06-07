using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeFramework.components {
    public class ExplodesWhenTimerExpiresComponent : AbstractComponent {

        public int range, power;

        public ExplodesWhenTimerExpiresComponent(int _range, int _power) {
            this.range = _range;
            this.power = _power;
        }


    }
}
