using RoguelikeFramework;

namespace AlienRL.components {

    public class AlienComponent : AbstractComponent {

        public bool moveWhenNoEnemy = true;
        public bool impregnateNextEnemy;

        public AlienComponent() {
            this.impregnateNextEnemy = Misc.random.Next(1, 2) == 1;
        }

    }

}
