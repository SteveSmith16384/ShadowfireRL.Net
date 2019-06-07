using RoguelikeFramework.components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeFramework.systems {

    public class PickupDropSystem {

        public PickupDropSystem() {
        }


        public void PickupItem(AbstractEntity unit, AbstractEntity item, List<AbstractEntity> mapSquare) {
            CanCarryComponent ccc = (CanCarryComponent)unit.getComponent(nameof(CanCarryComponent));
            CarryableComponent cc = (CarryableComponent)item.getComponent(nameof(CarryableComponent));
            // todo - check weight etc...
            ccc.AddItem(item);
            cc.carrier = unit;
            // No! item.RemoveComponent(nameof(PositionComponent));
            mapSquare.Remove(item);
        }


        public void DropItem(AbstractEntity unit, AbstractEntity item, List<AbstractEntity> mapSquare) {
            CanCarryComponent ccc = (CanCarryComponent)unit.getComponent(nameof(CanCarryComponent));
            PositionComponent unitpos = (PositionComponent)item.getComponent(nameof(PositionComponent));
            PositionComponent itempos = (PositionComponent)item.getComponent(nameof(PositionComponent));
            CarryableComponent cc = (CarryableComponent)item.getComponent(nameof(CarryableComponent));
            if (ccc.GetItems().Contains(item)) {
                ccc.RemoveItem(item);
                cc.carrier = null;
                mapSquare.Add(item);
                itempos.x = unitpos.x;
                itempos.y = unitpos.y;
            }
        }

    }

}
