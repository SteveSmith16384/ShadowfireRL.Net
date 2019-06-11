using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
