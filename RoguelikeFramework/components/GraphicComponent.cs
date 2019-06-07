using RLNET;

namespace RoguelikeFramework.components {

    public class GraphicComponent : AbstractComponent {

        private RLCell ch_visible, ch_seen;

        public GraphicComponent(int _ch_visible, RLColor foreground, RLColor background, int ch_seen) {
            this.ch_visible = new RLCell(background, foreground, _ch_visible);
            this.ch_seen = new RLCell(RLColor.Black, RLColor.Gray, ch_seen);
        }


        public RLCell getVisibleChar() {
            return this.ch_visible;
        }


        public RLCell getSeenChar() {
            return this.ch_seen;
        }

    }
}
