using RoguelikeFramework.components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeFramework.systems {

    public class ShootOnSightSystem : AbstractSystem {

        private List<AbstractEntity> entities;
        private readonly CheckMapVisibilitySystem cmvs;

        public ShootOnSightSystem(CheckMapVisibilitySystem _cmvs, List<AbstractEntity> _entities) {
            this.cmvs = _cmvs;
            this.entities = _entities;
        }


        public virtual void ProcessEntity(AbstractEntity entity) {
            ShootOnSightComponent sosc = (ShootOnSightComponent)entity.getComponent(nameof(ShootOnSightComponent));
            if (sosc != null) {
                MobDataComponent us = (MobDataComponent)entity.getComponent(nameof(MobDataComponent));
                PositionComponent pos = (PositionComponent)entity.getComponent(nameof(PositionComponent));
                CanCarryComponent ccc = (CanCarryComponent)entity.getComponent(nameof(CanCarryComponent));
                if (ccc != null && ccc.CurrentItem != null) {
                    ItemCanShootComponent icsc = (ItemCanShootComponent)ccc.CurrentItem.getComponent(nameof(ItemCanShootComponent));
                    if (icsc != null) {
                        AbstractEntity target = this.GetTarget(pos.x, pos.y, us.side);
                        Console.WriteLine($"Target {target.name} shot");
                    }
                }
            }
        }


        private AbstractEntity GetTarget(int ourX, int ourY, int ourSide) {
            foreach (var e  in this.entities) {
                MobDataComponent att = (MobDataComponent)e.getComponent(nameof(MobDataComponent));
                if (att != null && att.side != ourSide) {
                    PositionComponent pos = (PositionComponent)e.getComponent(nameof(PositionComponent));
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
