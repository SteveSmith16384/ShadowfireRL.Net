using RLNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowfireRL.components
{
    class GraphicComponent : AbstractComponent
    {
        private RLCell ch;
        public bool is_visible = true;

        public GraphicComponent(int _ch, RLColor foreground, RLColor background)
        {
            this.ch = new RLCell(background, foreground, _ch);
        }


        public RLCell getChar()
        {
            return this.ch;
        }
    }
}
