using RoguelikeFramework.components;
using RoguelikeFramework.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeFramework.systems {
    public class ThrowingSystem {

        private MapData mapData;

        public ThrowingSystem(MapData _mapData) {
            this.mapData = _mapData;
        }

        public void ThrowItem(AbstractEntity thrower, int destX, int destY) {
            CanCarryComponent ccc = (CanCarryComponent)thrower.getComponent(nameof(CanCarryComponent));
            if (ccc.CurrentItem != null) {
                PositionComponent pos = (PositionComponent)ccc.CurrentItem.getComponent(nameof(PositionComponent));
                this.mapData.map[pos.x, pos.y].Add(ccc.CurrentItem);

                ccc.CurrentItem = null;
                ccc.RemoveItem(ccc.CurrentItem);
            }

        }

    }



}

