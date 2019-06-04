using RLNET;
using ShadowfireRL.components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowfireRL.systems
{
    public class DrawingSystem : AbstractSystem
    {

        private IGameView view;
        private MapData map_data;

        public DrawingSystem(IGameView _view, MapData _map_data)
        {
            this.view = _view;
            this.map_data = _map_data;
        }


        public void process()
        {
            // Draw map
            for (int y = 0; y < this.map_data.getHeight(); y++)
            {
                for (int x = 0; x < this.map_data.getWidth(); x++)
                {
                    foreach (AbstractEntity sq in this.map_data.map[x, y])
                    {
                        GraphicComponent gc = (GraphicComponent)sq.getComponent("GraphicComponent");
                        if (gc.is_visible)
                        {
                            RLCell tc = gc.getChar();
                            this.view.drawCharacter(x, y, tc);
                        }
                    }
                }
            }
        }

    }

}