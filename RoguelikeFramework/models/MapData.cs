using System.Collections.Generic;
using System.Linq;

namespace RoguelikeFramework.models {

    public class MapData {

        public List<AbstractEntity>[,] map;

        public MapData() {
        }


        public int getWidth() {
            return this.map.GetLength(0);
        }


        public int getHeight() {
            return this.map.GetLength(1);
        }


        public AbstractComponent GetSingleComponent(int x, int y, string component) {
            var entities = this.map[x, y];
            var mapEnt = entities.Single(ent => ent.GetComponents().ContainsKey(nameof(component)));
            return mapEnt.getComponent(nameof(component));
        }

    }

}
