using RoguelikeFramework.components;
using RoguelikeFramework.models;
using System.Collections.Generic;

namespace RoguelikeFramework.systems {
    public class ExplosionSystem : AbstractSystem {

        private CheckMapVisibilitySystem checkVis;
        private MapData mapData;
        private List<AbstractEntity> entities;
        private DamageSystem damageSystem;

        public ExplosionSystem(CheckMapVisibilitySystem _checkVis, DamageSystem _damageSystem, MapData _mapData, List<AbstractEntity> _entities) {
            this.mapData = _mapData;
            this.checkVis = _checkVis;
            this.damageSystem = _damageSystem;
            this.entities = _entities;
        }


        public void Explosion(AbstractEntity entity) {
            ExplodesWhenTimerExpiresComponent ewtec = (ExplodesWhenTimerExpiresComponent)entity.GetComponent(nameof(ExplodesWhenTimerExpiresComponent));
            PositionComponent pos = ECSHelper.GetPosition(entity);

            foreach (var e in this.entities) {
                PositionComponent epos = ECSHelper.GetPosition(e);
                if (epos != null) {
                    if (GeometryFunctions.Distance(epos.x, epos.y, pos.x, pos.y) <= ewtec.range) {
                        if (this.checkVis.CanSee(epos.x, epos.y, pos.x, pos.y)) {
                            this.damageSystem.Damage(e, ewtec.power, $"Exploding {entity.name}");
                        }
                    }
                }

            }
            //todo
        }

    }

}
