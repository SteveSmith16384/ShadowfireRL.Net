using RoguelikeFramework.components;
using RoguelikeFramework.models;
using ShadowfireRL.effects;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace RoguelikeFramework.systems {

    public class ShootingSystem : AbstractSystem {

        private GameLog gameLog;

        public ShootingSystem(BasicEcs ecs, GameLog log) : base(ecs, false) {
            this.gameLog = log;
        }


        public void EntityShootingAtEntity(AbstractEntity shooter, Point shooterPos, AbstractEntity target, Point targetPos) {
            CanCarryComponent ccc = (CanCarryComponent)shooter.GetComponent(nameof(CanCarryComponent));
            if (ccc != null && ccc.CurrentItem != null) {
                ItemCanShootComponent icsc = (ItemCanShootComponent)ccc.CurrentItem.GetComponent(nameof(ItemCanShootComponent));
                if (icsc != null) {
                    MobDataComponent us = (MobDataComponent)shooter.GetComponent(nameof(MobDataComponent));
                    if (us.actionPoints > 0) {
                        this.EntityShotByEntity(shooter, shooterPos.X, shooterPos.Y, target, targetPos.X, targetPos.Y);
                        us.actionPoints -= 50;
                    }
                }
            }

        }

        private void EntityShotByEntity(AbstractEntity shooter, int x1, int y1, AbstractEntity target, int x2, int y2) {
            this.gameLog.Add($"Target {target.name} shot by {shooter.name}");

            BulletEffect be = new BulletEffect(x1, y1, x2, y2);
            EffectsSystem es = (EffectsSystem)this.ecs.GetSystem(nameof(EffectsSystem));
            es.effects.Add(be);

            // todo - calc acc, damage etc...

        }


        public AbstractEntity GetTargetAt(List<AbstractEntity> entities, int ourX, int ourY, int ourSide) {
            CheckMapVisibilitySystem cmvs = (CheckMapVisibilitySystem)this.ecs.GetSystem(nameof(CheckMapVisibilitySystem));
            foreach (var e in entities) {
                MobDataComponent att = (MobDataComponent)e.GetComponent(nameof(MobDataComponent));
                if (att != null && att.side != ourSide) {
                    PositionComponent pos = (PositionComponent)e.GetComponent(nameof(PositionComponent));
                    if (pos != null) {
                        if (cmvs.CanSee(ourX, ourY, pos.x, pos.y)) {
                            return e;
                        }
                    }
                }
            }
            return null;
        }


    }

}
