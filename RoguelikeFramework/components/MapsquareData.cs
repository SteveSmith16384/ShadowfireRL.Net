using RLNET;

namespace RoguelikeFramework.components {

    public class MapsquareData : AbstractComponent {

        public bool visible, seen, blocksView;
        public float traverseCost;
        public RLCell seen_ch;

        public MapsquareData(bool _blocksView) {
            this.visible = false;
            this.seen = false;
            this.blocksView = _blocksView;
            this.traverseCost = 1f;

            if (this.blocksView) {
                this.seen_ch = new RLCell(RLColor.Gray, RLColor.Gray, ' ');
            } else {
                this.seen_ch = new RLCell(RLColor.Black, RLColor.Gray, '.');
            }
        }
    }
}
