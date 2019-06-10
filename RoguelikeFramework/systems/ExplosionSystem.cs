using RoguelikeFramework.components;
using RoguelikeFramework.models;

namespace RoguelikeFramework.systems {
    public class ExplosionSystem : AbstractSystem {

        private CheckMapVisibilitySystem checkVis;
        private MapData mapData;

        public ExplosionSystem(CheckMapVisibilitySystem _checkVis, MapData _mapData) {
            this.mapData = _mapData;
            this.checkVis = _checkVis;
        }


        public void Explosion(AbstractEntity entity) {
            ExplodesWhenTimerExpiresComponent ewtec = (ExplodesWhenTimerExpiresComponent)entity.GetComponent(nameof(ExplodesWhenTimerExpiresComponent));
            PositionComponent pos = ECSHelper.GetPosition(entity);

            //todo
        }

    }

}
