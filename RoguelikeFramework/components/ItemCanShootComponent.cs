namespace RoguelikeFramework.components {

    public class ItemCanShootComponent : AbstractComponent {

        public float range, power;

        public ItemCanShootComponent(float r, float p) {
            this.range = r;
            this.power = p;
        }
    }
}
