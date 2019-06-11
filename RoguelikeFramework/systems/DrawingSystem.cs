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
        private bool debug_show_all;
        private RLCell invisible = new RLCell(RLColor.Black, RLColor.Black, ' ');

        public DrawingSystem(DefaultRLView _view, IDataForView _viewData, bool _debug_show_all) {
            this.view = _view;
            this.viewData = _viewData;
            this.debug_show_all = _debug_show_all;
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
                        if (msdc.visible || this.debug_show_all) {
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
            int yPos = 1;
            this.view.crewListConsole.Print(0, yPos, "CREW LIST", RLColor.White);
            int idx = 1;
            foreach (AbstractEntity e in this.viewData.GetUnits()) {
                RLColor c = RLColor.White;
                if (e == this.viewData.GetCurrentUnit()) {
                    c = RLColor.Yellow; // Highlight selected unit
                }
                MobDataComponent mdc = (MobDataComponent)e.GetComponent(nameof(MobDataComponent));
                this.view.crewListConsole.Print(0, yPos, $"{idx}: {e.name} ({mdc.actionPoints} APs)", c);
                yPos++;
                idx++;
            }

            // Draw unit stats or menu selection
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
            var log = this.viewData.GetLog();
            int pos = 1;
            foreach (var line in log) {
                this.view.messageConsole.Print(1, pos++, line, RLColor.White);
            }

        }

    }

}