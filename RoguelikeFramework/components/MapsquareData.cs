namespace RoguelikeFramework.components {

    public class MapsquareData : AbstractComponent {

        public bool visible, seen, blocksView, is_door, onFire;
        public float traverseCost;

        public MapsquareData(bool _blocksView, bool door) {
            this.visible = false;
            this.seen = false;
            this.blocksView = _blocksView;
            this.is_door = door;
            this.traverseCost = 1f;
            this.onFire = false;
        }
    }
}
