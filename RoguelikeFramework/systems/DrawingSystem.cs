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
        private RLCell invisible = new RLCell(RLColor.Black, RLColor.Black, ' ');

        public DrawingSystem(DefaultRLView _view, IDataForView _viewData) {
            this.view = _view;
            this.viewData = _viewData;
        }


        public void Process(List<AbstractEffect> effects) {
            // Draw map
            MapData map_data = this.viewData.GetMapData();
            if (map_data.map != null) {
                this.view.mapConsole.Clear();
                for (int y = 0; y < map_data.getHeight(); y++) {
                    for (int x = 0; x < map_data.getWidth(); x++) {
                        var entities = map_data.map[x, y];
                        var mapEnt = entities.Single(ent => ent.GetComponents().ContainsKey(nameof(MapsquareData)));
                        MapsquareData msdc = (MapsquareData)mapEnt.GetComponent(nameof(MapsquareData));
                        if (msdc.visible || Settings.DEBUG_DRAW_ALL) {
                            // Only draw stuff if mapsquare visible
                            AbstractEntity sq = map_data.GetComponentToDraw(x, y);
                            GraphicComponent gc = (GraphicComponent)sq.GetComponent(nameof(GraphicComponent));
                            RLCell tc = gc.getVisibleChar();
                            this.view.mapConsole.Set(x, y, tc);
                            if (sq == this.viewData.GetCurrentUnit()) {
                                this.view.mapConsole.SetBackColor(x, y, RLColor.Yellow);
                            }
                        } else if (msdc.seen) {
                            AbstractEntity sq = map_data.GetComponentToDraw(x, y);
                            GraphicComponent gc = (GraphicComponent)sq.GetComponent(nameof(GraphicComponent));
                            this.view.mapConsole.Set(x, y, gc.getSeenChar());
                        } else {
                            this.view.mapConsole.Set(x, y, this.invisible);
                        }
                    }
                }
            }

            // Draw effects
            foreach (var effect in effects) {
                effect.draw(this.view.mapConsole);
            }

            // Draw line
            var line2 = this.viewData.GetLine();
            if (line2 != null) {
                foreach (var point in line2) {
                    this.view.mapConsole.SetBackColor(point.X, point.Y, RLColor.Gray);
                }
            }

            // Draw hover text
            this.view.mapConsole.Print(1, DefaultRLView._mapHeight - 1, this.viewData.GetHoverText(), RLColor.White);

            // Draw crew
            this.view.crewListConsole.Clear();
            int yPos = 0;
            this.view.crewListConsole.Print(0, yPos, "CREW LIST", RLColor.White);
            yPos++;
            int idx = 1;
            foreach (AbstractEntity e in this.viewData.GetUnits()) {
                RLColor c = RLColor.White;
                MobDataComponent mdc = (MobDataComponent)e.GetComponent(nameof(MobDataComponent));
                if (e == this.viewData.GetCurrentUnit()) {
                    c = RLColor.Yellow; // Highlight selected unit
                } else if (mdc.actionPoints <= 0) {
                    c = RLColor.Gray;
                }
                MovementDataComponent move = (MovementDataComponent)e.GetComponent(nameof(MovementDataComponent));
                string routeIndicator = move.route == null || move.route.Count == 0 ? "" : "(R)";
                this.view.crewListConsole.Print(0, yPos, $"{idx}: {e.name} {routeIndicator} ({mdc.actionPoints} APs)", c);
                yPos++;
                idx++;
            }

            // Draw unit stats or menu selection
            this.view.statConsole.Clear();
            yPos = 0;
            Dictionary<int, AbstractEntity> items = this.viewData.GetItemSelectionList();
            if (items != null && items.Count > 0) {
                foreach (var idx2 in items.Keys) {
                    this.view.statConsole.Print(0, yPos, $"{idx2} : {items[idx2].name}", RLColor.White);
                    yPos++;
                }
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
            this.view.logConsole.Clear();
            var log = this.viewData.GetLog();
            int pos = 1;
            foreach (var line in log) {
                this.view.logConsole.Print(1, pos++, line, RLColor.White);
            }
        }

    }

}
