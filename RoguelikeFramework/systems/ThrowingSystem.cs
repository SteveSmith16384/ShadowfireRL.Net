using RoguelikeFramework.components;
using RoguelikeFramework.models;

namespace RoguelikeFramework.systems {

    public class ThrowingSystem : AbstractSystem {

        private MapData mapData;
        private GameLog gameLog;

        public ThrowingSystem(BasicEcs ecs, MapData _mapData, GameLog log) : base(ecs, false) {
            this.mapData = _mapData;
            this.gameLog = log;
        }


        public void ThrowItem(AbstractEntity thrower, int destX, int destY) {
            MobDataComponent mdc = (MobDataComponent)thrower.GetComponent(nameof(MobDataComponent));
            if (mdc == null || mdc.actionPoints > 0) {
                mdc.actionPoints -= 50;
                CanCarryComponent ccc = (CanCarryComponent)thrower.GetComponent(nameof(CanCarryComponent));
                if (ccc.CurrentItem != null) {
                    PositionComponent pos = (PositionComponent)ccc.CurrentItem.GetComponent(nameof(PositionComponent));
                    this.mapData.map[pos.x, pos.y].Add(ccc.CurrentItem);

                    this.gameLog.Add($"{ccc.CurrentItem.name} thrown.");

                    ccc.RemoveItem(ccc.CurrentItem);

                }
            }
        }

    }



}

