using RLNET;
using RoguelikeFramework.components;
using RoguelikeFramework.models;
using RoguelikeFramework.view;
using ShadowfireRL.view;

namespace ShadowfireRL.systems {

    public class DrawingSystem {

        private DefaultRLView view;
        private IDataForView viewData;

        public DrawingSystem(DefaultRLView _view, IDataForView _viewData) {
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
                            this.view._mapConsole.Set(x, y, tc);
                        }
                    }
                }
            }

            // Draw line
            var line2 = this.viewData.GetLine();
            if (line2 != null) {
                foreach (var point in line2) {
                    this.view._mapConsole.SetBackColor(point.Item1, point.Item2, RLColor.Gray);
                }
            }

            // Draw log
            var log = this.viewData.GetLog();
            int pos = 1;
            foreach (var line in log) {
                this.view._messageConsole.Print(1, pos++, line, RLColor.White);
            }

            // Draw hover text
            //todo this.view.._messageConsole.Print(1, pos++, line, RLColor.White);

        }

    }

}