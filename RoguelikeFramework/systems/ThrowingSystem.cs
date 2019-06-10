using RoguelikeFramework.components;
using RoguelikeFramework.models;

namespace RoguelikeFramework.systems {
    public class ThrowingSystem {

        private MapData mapData;

        public ThrowingSystem(MapData _mapData) {
            this.mapData = _mapData;
        }

        public void ThrowItem(AbstractEntity thrower, int destX, int destY) {
            // todo - check APs
            CanCarryComponent ccc = (CanCarryComponent)thrower.GetComponent(nameof(CanCarryComponent));
            if (ccc.CurrentItem != null) {
                PositionComponent pos = (PositionComponent)ccc.CurrentItem.GetComponent(nameof(PositionComponent));
                this.mapData.map[pos.x, pos.y].Add(ccc.CurrentItem);

                ccc.CurrentItem = null;
                ccc.RemoveItem(ccc.CurrentItem);
            }

        }

    }



}

