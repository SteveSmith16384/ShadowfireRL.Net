using RLNET;

namespace RoguelikeFramework.components {

    public class GraphicComponent : AbstractComponent {

        private RLCell ch_visible, ch_seen;
        public int zOrder;

        public GraphicComponent(int _ch_visible, RLColor foreground, RLColor background, int ch_seen, int _zOrder) {
            this.ch_visible = new RLCell(background, foreground, _ch_visible);
            this.ch_seen = new RLCell(RLColor.Black, RLColor.Gray, ch_seen);
            this.zOrder = _zOrder;
        }


        public RLCell getVisibleChar() {
            return this.ch_visible;
        }


        public RLCell getSeenChar() {
            return this.ch_seen;
        }

    }
}
