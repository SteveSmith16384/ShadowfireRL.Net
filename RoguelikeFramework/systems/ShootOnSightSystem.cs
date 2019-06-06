using RoguelikeFramework.components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeFramework.systems {

    public class ShootOnSightSystem : AbstractSystem {

        private List<AbstractEntity> entities;
        private CheckMapVisibilitySystem cmvs;

        public ShootOnSightSystem(CheckMapVisibilitySystem _cmvs, List<AbstractEntity> _entities) {
            this.cmvs = _cmvs;
            this.entities = _entities;
        }


        public virtual void processEntity(AbstractEntity entity) {
            ShootOnSightComponent sosc = (ShootOnSightComponent)entity.getComponent(nameof(ShootOnSightComponent));
            if (sosc != null) {
                CanCarryComponent ccc = (CanCarryComponent)entity.getComponent(nameof(CanCarryComponent));
                if (ccc != null && ccc.CurrentItem != null) {
                    ItemCanShootComponent icsc = (ItemCanShootComponent)ccc.CurrentItem.getComponent(nameof(ItemCanShootComponent));
                    if (icsc != null) {

                    }
                }
            }
        }


        private void GetTarget(int ourSide) {
            foreach (var e  in this.entities) {
                MobDataComponent att = (MobDataComponent)e.getComponent(nameof(MobDataComponent));
                if (att != null) {
                    PositionComponent pos = (PositionComponent)e.getComponent(nameof(PositionComponent));
                    if (pos != null) {

                    }
                }
                // todo
            }
        }

    }

}
