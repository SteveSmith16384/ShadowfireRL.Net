using RLNET;

namespace RoguelikeFramework.components {

    public class MapsquareData : AbstractComponent {

        public bool visible, seen, blocksView, is_door;
        public float traverseCost;
        //public RLCell seen_ch;

        public MapsquareData(bool _blocksView, bool door) {
            this.visible = false;
            this.seen = false;
            this.blocksView = _blocksView;
            this.is_door = door;
            this.traverseCost = 1f;
            /*
            if (this.is_door) {
                this.seen_ch = new RLCell(RLColor.Green, RLColor.Gray, 'D');
            } else if (this.blocksView) {
                this.seen_ch = new RLCell(RLColor.Gray, RLColor.Gray, 'S');
            } else {
                this.seen_ch = new RLCell(RLColor.Black, RLColor.Gray, '.');
            }*/
        }
    }
}
