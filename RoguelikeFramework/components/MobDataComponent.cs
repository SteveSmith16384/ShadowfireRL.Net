namespace RoguelikeFramework.components {

    /**
     * Call representing a typical walking fighting character, either controlled by player or not.
     * 
     */
    public class MobDataComponent : AbstractComponent {

        public int side; // Player = side 0!
        public int actionPoints;
        public int apsPerTurn;

        public MobDataComponent(int _side, int aps) {
            this.side = _side;
            this.apsPerTurn = aps;
            this.actionPoints = aps;
        }

    }

}
