namespace RoguelikeFramework.components {

    public class AttackAttackableComponent : AbstractComponent {

        public int combatSkill;
        public int combatDamage;

        public AttackAttackableComponent(int ccskill, int ccdamage) {
            this.combatSkill = ccskill;
            this.combatDamage = ccdamage;
        }

    }

}
