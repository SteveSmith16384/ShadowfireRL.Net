using RLNET;
using RoguelikeFramework.components;
using RoguelikeFramework.models;
using RoguelikeFramework.view;
using System.Collections.Generic;
using System.Linq;

namespace RoguelikeFramework.systems {

    public class DrawingSystem {

        private DefaultRLView view;
        private IDataForView viewData;

        private RLCell invisible = new RLCell(RLColor.Black, RLColor.Gray, 'I');

        public DrawingSystem(DefaultRLView _view, IDataForView _viewData) {
            this.view = _view;
            this.viewData = _viewData;
        }


        public void process() {
            // Draw map
            MapData map_data = this.viewData.GetMapData();
            if (map_data.map != null) {
                this.view.mapConsole.Clear();
                for (int y = 0; y < map_data.getHeight(); y++) {
                    for (int x = 0; x < map_data.getWidth(); x++) {
                        var entities = map_data.map[x, y];
                        var mapEnt = entities.Single(ent => ent.components.ContainsKey(nameof(MapsquareData)));
                        MapsquareData msdc = (MapsquareData)mapEnt.getComponent(nameof(MapsquareData));
                        if (msdc.visible) { 
                            // Only draw stuff if mapsquare visible
                            foreach (AbstractEntity sq in entities) {
                                GraphicComponent gc = (GraphicComponent)sq.getComponent(nameof(GraphicComponent));
                                RLCell tc = gc.getChar();
                                this.view.mapConsole.Set(x, y, tc);
                            }
                        } else if (msdc.seen) {
                            this.view.mapConsole.Set(x, y, msdc.seen_ch);
                            //break;
                        } else {
                            this.view.mapConsole.Set(x, y, this.invisible);
                        }
                    }
                }
            }

            // Draw line
            var line2 = this.viewData.GetLine();
            if (line2 != null) {
                foreach (var point in line2) {
                    this.view.mapConsole.SetBackColor(point.x, point.y, RLColor.Gray);
                }
            }

            // Draw hover text
            this.view.mapConsole.Print(1, DefaultRLView._mapHeight - 1, this.viewData.GetHoverText(), RLColor.White);

            // Draw crew
            int yPos = 0;
            foreach (AbstractEntity e in this.viewData.GetUnits().Values) {
                RLColor c = RLColor.White;
                if (e == this.viewData.GetCurrentUnit()) {
                    c = RLColor.Yellow;
                }
                this.view.crewListConsole.Print(0, yPos, yPos + 1 + ": " + e.name, c);
                yPos++;
            }

            // Draw unit stats or menu selection
            yPos = 0;
            Dictionary<int, AbstractEntity> items = this.viewData.GetItemSelectionList();
            if (items != null) {
            } else { 
            AbstractEntity currentUnit = this.viewData.GetCurrentUnit();
                if (currentUnit != null) {
                    foreach (var s in this.viewData.GetStatsFor(currentUnit)) {
                        this.view.statConsole.Print(0, yPos, s, RLColor.White);
                        yPos++;
                    }
                }
            }

            // Draw log
            var log = this.viewData.GetLog();
            int pos = 1;
            foreach (var line in log) {
                this.view.messageConsole.Print(1, pos++, line, RLColor.White);
            }

        }

    }

}