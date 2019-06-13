using RoguelikeFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlienRL.components {

    public class ImpregnatedComponent : AbstractComponent {

        public int turnsUntilBurst;

        public ImpregnatedComponent() {
            this.turnsUntilBurst = Misc.random.Next(5, 15);
        }

    }

}
