using System.Collections.Generic;

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


    }

}
