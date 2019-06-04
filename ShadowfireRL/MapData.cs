using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowfireRL
{
    public class MapData
    {

        public List<AbstractEntity>[,] map;

        public MapData()
        {
        }


        public void createMap(EntityFactory factory, int w, int h)
        {
            this.map = new List<AbstractEntity>[w, h];
            for (int y = 0; y < this.getHeight(); y++)
            {
                for (int x = 0; x < this.getWidth(); x++)
                {
                    this.map[x, y] = new List<AbstractEntity>();
                    //if (Main.RND.nextFloat() > .1f)
                    {
                        factory.createFloorMapSquare(x, y);
                    }
                    /*else
                    {
                        factory.createWallMapSquare(x, y);
                    }*/
                }
            }
        }


        public int getWidth()
        {
            return this.map.GetLength(0);
        }


        public int getHeight()
        {
            return this.map.GetLength(1);
        }


    }

}
