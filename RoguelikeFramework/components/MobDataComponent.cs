using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeFramework.components {

    public class MobDataComponent : AbstractComponent {

        public int side;

        public MobDataComponent(int _side) {
            this.side = _side;
        }
    }
}
