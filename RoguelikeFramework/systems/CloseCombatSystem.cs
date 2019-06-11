﻿using RoguelikeFramework.components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeFramework.systems {
    public class CloseCombatSystem {

        private DamageSystem damageSystem;

        public CloseCombatSystem(DamageSystem _damageSystem) {
            this.damageSystem = _damageSystem;
        }


        public void Combat(AbstractEntity e1, List<AbstractEntity> list) {
            foreach (AbstractEntity e2 in list) {
                AttackAttackableComponent aac = (AttackAttackableComponent)e2.GetComponent(nameof(AttackAttackableComponent));
                if (aac != null) {
                    this.DoCombat(e1, e2);
                    return;
                }
            }
        }


        private void DoCombat(AbstractEntity e1, AbstractEntity e2) {
            AttackAttackableComponent aa1 = (AttackAttackableComponent)e1.GetComponent(nameof(AttackAttackableComponent));
            AttackAttackableComponent aa2 = (AttackAttackableComponent)e2.GetComponent(nameof(AttackAttackableComponent));

            // Todo - take into account weapons being carried
            int att1 = Misc.random.Next(aa1.combatSkill);
            int att2 = Misc.random.Next(aa2.combatSkill);

            if (att1 > att2) {
                this.damageSystem.Damage(e2, aa1.combatDamage, $"Hit by {e1.name}");
            } else
            if (att1 < att2) {
                this.damageSystem.Damage(e1, aa2.combatDamage, $"Hit by {e2.name}");
            }

        }
    }
}
