namespace RoguelikeFramework.components {

    /**
     * Call representing a typical walking fighting character, either controlled by player or not.
     * 
     */
    public class MobDataComponent : AbstractComponent {

        public int side; // Player = side 0!
        public int actionPoints = 100; // todo

        public MobDataComponent(int _side) {
            this.side = _side;
        }

    }

}
