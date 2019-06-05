using RLNET;
using RoguelikeFramework.components;
using RoguelikeFramework.models;
using RoguelikeFramework.view;
using ShadowfireRL.view;

namespace ShadowfireRL.systems {

    public class DrawingSystem {

        private ShadowfireRLView view;
        private IDataForView viewData;

        public DrawingSystem(ShadowfireRLView _view, IDataForView _viewData) {
            this.view = _view;
            this.viewData = _viewData;
        }


        public void process() {
            // Draw map
            MapData map_data = this.viewData.GetMapData();
            for (int y = 0; y < map_data.getHeight(); y++) {
                for (int x = 0; x < map_data.getWidth(); x++) {
                    foreach (AbstractEntity sq in map_data.map[x, y]) {
                        GraphicComponent gc = (GraphicComponent)sq.getComponent(nameof(GraphicComponent));
                        if (gc.is_visible) {
                            RLCell tc = gc.getChar();
                            this.view.mapConsole.Set(x, y, tc);
                        }
                    }
                }
            }

            // Draw hover text
            this.view.mapConsole.Print(1, ShadowfireRLView._mapHeight-1, this.viewData.GetHoverText(), RLColor.White);

            // Draw line
            var line2 = this.viewData.GetLine();
            if (line2 != null) {
                foreach (var point in line2) {
                    this.view.mapConsole.SetBackColor(point.x, point.y, RLColor.Gray);
                }
            }

            // Draw crew
            int yPos = 0;
            foreach (AbstractEntity e in this.viewData.GetUnits().Values) {
                this.view.crewListConsole.Print(0, yPos, (yPos+1) + ": " + e.name, RLColor.White);
                yPos++;
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