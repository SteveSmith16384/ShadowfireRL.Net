using RoguelikeFramework.components;
using RoguelikeFramework.models;
using System.Collections.Generic;
using System.Drawing;

namespace RoguelikeFramework.systems {

    public class CheckMapVisibilitySystem : AbstractSystem {

        private MapData map_data;
        public bool ReCheckVisibility = true;

        public CheckMapVisibilitySystem(BasicEcs ecs, MapData _mapData) : base(ecs, false) {
            this.map_data = _mapData;
        }


        public void process(IEnumerable<AbstractEntity> playersUnits) {
            if (!this.ReCheckVisibility) {
                return;
            }
            this.ReCheckVisibility = false;
            // Set all to hidden initially
            for (int y = 0; y < this.map_data.getHeight(); y++) {
                for (int x = 0; x < this.map_data.getWidth(); x++) {
                    foreach (AbstractEntity sq in this.map_data.map[x, y]) {
                        MapsquareData msdc = (MapsquareData)sq.GetComponent(nameof(MapsquareData));
                        if (msdc != null) {
                            msdc.visible = false;
                            break;
                        }
                    }
                }
            }

            // loop through each unit and see if they can see it
            foreach (var unit in playersUnits) {
                var pos = (PositionComponent)unit.GetComponent(nameof(PositionComponent));
                for (int y = 0; y < this.map_data.getHeight(); y++) {
                    for (int x = 0; x < this.map_data.getWidth(); x++) {
                        foreach (AbstractEntity sq in this.map_data.map[x, y]) {
                            MapsquareData msdc = (MapsquareData)sq.GetComponent(nameof(MapsquareData));
                            if (msdc != null) {
                                if (msdc.visible == false) { // Otherwise we know we've already checked it
                                    this.CheckAndMarkVisibility(pos.x, pos.y, x, y);
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }


        private void CheckAndMarkVisibility(int x1, int y1, int x2, int y2) {
            List<Point> line = Misc.GetLine(x1, y1, x2, y2, true);
            foreach (var p in line) {
                foreach (AbstractEntity sq in this.map_data.map[p.X, p.Y]) {
                    MapsquareData msdc = (MapsquareData)sq.GetComponent(nameof(MapsquareData)); // todo - better way of selecting map component
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


        public bool CanSee(int x1, int y1, int x2, int y2) {
            List<Point> line = Misc.GetLine(x1, y1, x2, y2, true);
            foreach (var p in line) {
                MapsquareData msdc = (MapsquareData)this.map_data.GetSingleComponent(p.X, p.Y, nameof(MapsquareData));
                if (msdc.blocksView) {
                    return false;
                }
            }
            return true;
        }
    }

}