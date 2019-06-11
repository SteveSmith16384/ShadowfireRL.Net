using RoguelikeFramework.components;
using System;

namespace RoguelikeFramework.systems {

    public class DamageSystem {

        // todo

        public void Damage(AbstractEntity entity, float damage, string reason) {
            HealthComponent hc = (HealthComponent)entity.GetComponent(nameof(HealthComponent));
            if (hc != null) {
                hc.health -= damage;
                Console.WriteLine($"{entity.name} has been wounded {damage}: {reason}");
                if (hc.health <= 0) {
                    entity.markForRemoval = true;
                }
            }
        }
    }

}
