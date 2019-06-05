using RoguelikeFramework.components;
using RoguelikeFramework.models;
using System.Collections.Generic;

namespace RoguelikeFramework.systems {
    public class CheckVisibilitySystem {//: AbstractSystem {

        private MapData map_data;
        private IEnumerable<AbstractEntity> playersUnits;

        public CheckVisibilitySystem(MapData _mapData, IEnumerable<AbstractEntity> _playersUnits) {
            this.map_data = _mapData;
            this.playersUnits = _playersUnits;
        }


        public void process() {
            //if (this.map_data.map != null) {
            // Set all to hidden initially
            for (int y = 0; y < this.map_data.getHeight(); y++) {
                for (int x = 0; x < this.map_data.getWidth(); x++) {
                    foreach (AbstractEntity sq in this.map_data.map[x, y]) {
                        MapsquareData msdc = (MapsquareData)sq.getComponent(nameof(MapsquareData));
                        if (msdc != null) {
                            msdc.visible = false;
                            break;
                        }
                    }
                }
            }

            // loop through each unit and see if they can see it
            foreach (var unit in this.playersUnits) {
                var pos = (PositionComponent)unit.getComponent(nameof(PositionComponent));
                for (int y = 0; y < this.map_data.getHeight(); y++) {
                    for (int x = 0; x < this.map_data.getWidth(); x++) {
                        foreach (AbstractEntity sq in this.map_data.map[x, y]) {
                            MapsquareData msdc = (MapsquareData)sq.getComponent(nameof(MapsquareData));
                            if (msdc != null) {
                                if (msdc.visible == false) { // Otherwise we know we've already checked it
                                    this.CheckVisibility(pos.x, pos.y, x, y);
                                }
                                break;
                            }
                        }
                    }
                }
            }
            //}
        }


        private void CheckVisibility(int x1, int y1, int x2, int y2) {
            List<Point> line = Misc.line(x1, y1, x2, y2);
            foreach (var p in line) { 
                foreach (AbstractEntity sq in this.map_data.map[p.x, p.y]) {
                    MapsquareData msdc = (MapsquareData)sq.getComponent(nameof(MapsquareData)); // todo - better way of selecting map component
                    if (msdc != null) {
                        msdc.visible = true; // Even walls get seen
                        msdc.seen = true;
                        if (msdc.blocksView) {
                            return;
                        }
                        break; // Found the mapsquare component, so move to next square
                    }
                }
            }
        }

    }

}