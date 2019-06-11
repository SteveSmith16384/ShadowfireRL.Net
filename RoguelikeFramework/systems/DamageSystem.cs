﻿using RoguelikeFramework.components;
using RoguelikeFramework.models;
using System;

namespace RoguelikeFramework.systems {

    public class DamageSystem {

        private GameLog log;

        public DamageSystem(GameLog _log) {
        }


        public void Damage(AbstractEntity entity, float damage, string reason) {
            HealthComponent hc = (HealthComponent)entity.GetComponent(nameof(HealthComponent));
            if (hc != null) {
                hc.health -= damage;
                log.Add($"{entity.name} has been wounded {damage}: {reason}");
                if (hc.health <= 0) {
                    log.Add($"{entity.name} has been killed");
                    entity.markForRemoval = true;
                }
            }
        }
    }

}
