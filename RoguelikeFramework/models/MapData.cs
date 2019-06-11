using RoguelikeFramework.components;
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


        // Returns the component with the highest z-order
        public AbstractEntity GetComponentToDraw(int x, int y) {
            var entities = this.map[x, y];
            if (entities.Count == 1) {
                return entities[0];
            }

            Dictionary<GraphicComponent, AbstractEntity> gcs = new Dictionary<GraphicComponent, AbstractEntity>();
            foreach (var e in entities) {
                GraphicComponent gc = (GraphicComponent)e.GetComponent(nameof(GraphicComponent));
                if (gc != null) {
                    gcs.Add(gc, e);
                }
            }
            return gcs.OrderByDescending(e2 => e2.Key.zOrder).First().Value;
            //return gcs.First().Value;

            /*entities.OrderBy(e => (GraphicComponent)(e.GetComponent(nameof(GraphicComponent))).zOrder);

            foreach (var e in entities) {
                var mapEnts = entities.Where(ent => ent.GetComponents().ContainsKey(nameof(GraphicComponent)));
                return mapEnts.OrderBy(x.)
                //return mapEnt.getComponent(nameof(component));
            }*/
        }

    }

}
