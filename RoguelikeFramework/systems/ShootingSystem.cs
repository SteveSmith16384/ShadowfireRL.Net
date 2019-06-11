using RoguelikeFramework.models;
using ShadowfireRL.effects;
using System;

namespace RoguelikeFramework.systems {

    public class ShootingSystem : AbstractSystem {

        private GameLog gameLog;

        public ShootingSystem(BasicEcs ecs, GameLog log): base(ecs, false) {
            this.gameLog = log;
        }


        public void EntityShotByEntity(AbstractEntity shooter, int x1, int y1, AbstractEntity target, int x2, int y2) {
            this.gameLog.Add($"Target {target.name} shot by {shooter.name}");

            BulletEffect be = new BulletEffect(x1, y1, x2, y2);
            EffectsSystem es = (EffectsSystem)this.ecs.GetSystem(nameof(EffectsSystem));
            es.effects.Add(be);

            // todo - calc acc, damage etc...

        }

    }

}
