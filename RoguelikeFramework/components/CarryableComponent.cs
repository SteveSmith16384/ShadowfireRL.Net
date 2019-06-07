namespace RoguelikeFramework.components {

    public class CarryableComponent : AbstractComponent {

        public float weight;
        public AbstractEntity carrier;

        public CarryableComponent(float w) {
            this.weight = w;
        }
    }
}
