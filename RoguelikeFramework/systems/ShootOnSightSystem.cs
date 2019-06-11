using RoguelikeFramework.components;
using System.Collections.Generic;

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
            if (sosc != null) {
                MobDataComponent us = (MobDataComponent)entity.GetComponent(nameof(MobDataComponent));
                PositionComponent pos = (PositionComponent)entity.GetComponent(nameof(PositionComponent));
                CanCarryComponent ccc = (CanCarryComponent)entity.GetComponent(nameof(CanCarryComponent));
                if (ccc != null && ccc.CurrentItem != null) {
                    ItemCanShootComponent icsc = (ItemCanShootComponent)ccc.CurrentItem.GetComponent(nameof(ItemCanShootComponent));
                    if (icsc != null) {
                        // todo - check APs
                        AbstractEntity target = this.GetTarget(pos.x, pos.y, us.side);
                        if (target != null) {
                            //todo - get dynamically - this.shootingSystem.EntityShotByEntity(entity, target);
                        }
                    }
                }
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
