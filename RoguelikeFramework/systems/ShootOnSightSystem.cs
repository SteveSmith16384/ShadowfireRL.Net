using RoguelikeFramework.components;
using System.Collections.Generic;
using System.Drawing;

namespace RoguelikeFramework.systems {

    public class ShootOnSightSystem : AbstractSystem {

        private List<AbstractEntity> entities;
        private readonly CheckMapVisibilitySystem cmvs;
        //private ShootingSystem shootingSystem;

        public ShootOnSightSystem(BasicEcs ecs, CheckMapVisibilitySystem _cmvs, List<AbstractEntity> _entities) : base(ecs, true) {
            this.cmvs = _cmvs;
            //this.shootingSystem = _shootingSystem;
            this.entities = _entities;
        }


        public override void ProcessEntity(AbstractEntity entity) {
            ShootOnSightComponent sosc = (ShootOnSightComponent)entity.GetComponent(nameof(ShootOnSightComponent));
            if (sosc == null) {
                return;
            }
            PositionComponent shooterPos = (PositionComponent)entity.GetComponent(nameof(PositionComponent));
            MobDataComponent us = (MobDataComponent)entity.GetComponent(nameof(MobDataComponent));
            AbstractEntity target = this.GetTarget(shooterPos.x, shooterPos.y, us.side);
            if (target != null) {
                PositionComponent targetPos = (PositionComponent)target.GetComponent(nameof(PositionComponent));
                ShootingSystem ss = (ShootingSystem)this.ecs.GetSystem(nameof(ShootingSystem));
                ss.EntityShootingAtEntity(entity, new Point(shooterPos.x, shooterPos.y), target, new Point(targetPos.x, targetPos.y));
            }
        }


        private AbstractEntity GetTarget(int ourX, int ourY, int ourSide) {
            foreach (var e in this.entities) {
                MobDataComponent att = (MobDataComponent)e.GetComponent(nameof(MobDataComponent));
                if (att != null && att.side != ourSide) {
                    PositionComponent pos = (PositionComponent)e.GetComponent(nameof(PositionComponent));
                    if (pos != null) {
                        if (this.cmvs.CanSee(ourX, ourY, pos.x, pos.y)) {
                            return e;
                        }
                    }
                }
            }
            return null;
        }

    }

}
