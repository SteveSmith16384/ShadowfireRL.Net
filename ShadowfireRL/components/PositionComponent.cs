using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowfireRL.components
{
    public class PositionComponent : AbstractComponent
    {


    public int x, y;
    public bool blocks_movement;

    public PositionComponent(AbstractEntity e, MapData map_data, int _x, int _y, bool _blocks_movement)
    {
            this.x = _x;
            this.y = _y;
            this.blocks_movement = _blocks_movement;

        map_data.map[this.x, this.y].Add(e);
    }

}

}
