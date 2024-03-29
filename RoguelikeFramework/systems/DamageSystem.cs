﻿using RoguelikeFramework.components;
using RoguelikeFramework.models;

namespace RoguelikeFramework.systems {

    public class DamageSystem : AbstractSystem {

        private GameLog log;

        public DamageSystem(BasicEcs ecs, GameLog _log) : base(ecs, false) {
            this.log = _log;
        }


        public void Damage(AbstractEntity entity, float damage, string reason) {
            HealthComponent hc = (HealthComponent)entity.GetComponent(nameof(HealthComponent));
            if (hc != null) {
                hc.health -= damage;
                this.log.Add($"{entity.name} has been wounded {damage}: {reason}");
                if (hc.health <= 0) {
                    this.log.Add($"{entity.name} has been killed");
                    entity.markForRemoval = true;

                    // Create corpse
                }
            }
        }
    }

}
