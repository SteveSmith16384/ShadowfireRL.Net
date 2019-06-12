using RoguelikeFramework.components;
using System.Collections.Generic;

namespace RoguelikeFramework.systems {
    public class CloseCombatSystem : AbstractSystem {

        //private DamageSystem damageSystem;

        public CloseCombatSystem(BasicEcs ecs) : base(ecs, false) {/*DamageSystem _damageSystem) {
            this.damageSystem = _damageSystem;*/
        }


        /**
         * Returns true if combat took place, as opposed to a unit simply walking into a wall.
         */
        public bool Combat(AbstractEntity e1, List<AbstractEntity> list) {
            foreach (AbstractEntity e2 in list) {
                AttackAttackableComponent aac = (AttackAttackableComponent)e2.GetComponent(nameof(AttackAttackableComponent));
                if (aac != null) {
                    this.DoCombat(e1, e2);
                    return true;
                }
            }
            return false;
        }


        private void DoCombat(AbstractEntity e1, AbstractEntity e2) {
            AttackAttackableComponent aa1 = (AttackAttackableComponent)e1.GetComponent(nameof(AttackAttackableComponent));
            AttackAttackableComponent aa2 = (AttackAttackableComponent)e2.GetComponent(nameof(AttackAttackableComponent));

            // Todo - take into account weapons being carried
            int att1 = Misc.random.Next(aa1.combatSkill);
            int att2 = Misc.random.Next(aa2.combatSkill);

            DamageSystem damageSystem = (DamageSystem)this.ecs.GetSystem(nameof(DamageSystem));
            if (att1 > att2) {
                damageSystem.Damage(e2, aa1.combatDamage, $"Hit by {e1.name}");
            } else
            if (att1 < att2) {
                damageSystem.Damage(e1, aa2.combatDamage, $"Hit by {e2.name}");
            }

        }
    }
}
