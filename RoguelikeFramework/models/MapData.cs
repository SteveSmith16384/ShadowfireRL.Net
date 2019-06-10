using System;
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


        public AbstractComponent GetSingleComponent(int x, int y, string componentName) {
            var entities = this.map[x, y];
            foreach (var e in entities) {
                //var mapEnt = entities.Single(ent => ent.GetComponents().ContainsKey(nameof(component)));
                //return mapEnt.getComponent(nameof(component));
                var component = e.GetComponent(componentName);
                if (component != null) {
                    return component;
                }
            }
            throw new Exception($"Component {componentName} not found");
        }

    }

}
