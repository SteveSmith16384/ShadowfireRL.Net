using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeFramework.components {
    public class ItemIsWeaponComponent : AbstractComponent {

        public float damage;

        public ItemIsWeaponComponent(float d) {
            this.damage = d;
        }

    }
}
