using RoguelikeFramework.components;
using System.Collections.Generic;

namespace RoguelikeFramework.systems {

    public class PickupDropSystem {

        public PickupDropSystem() {
        }


        public void PickupItem(AbstractEntity unit, AbstractEntity item, List<AbstractEntity> mapSquare) {
            // todo - check APs
            CanCarryComponent ccc = (CanCarryComponent)unit.GetComponent(nameof(CanCarryComponent));
            CarryableComponent cc = (CarryableComponent)item.GetComponent(nameof(CarryableComponent));
            // todo - check weight etc...
            ccc.AddItem(item);
            cc.carrier = unit;
            // No! item.RemoveComponent(nameof(PositionComponent));
            mapSquare.Remove(item);
        }


        public void DropItem(AbstractEntity unit, AbstractEntity item, List<AbstractEntity> mapSquare) {
            // todo - check APs
            CanCarryComponent ccc = (CanCarryComponent)unit.GetComponent(nameof(CanCarryComponent));
            PositionComponent unitpos = (PositionComponent)item.GetComponent(nameof(PositionComponent));
            PositionComponent itempos = (PositionComponent)item.GetComponent(nameof(PositionComponent));
            CarryableComponent cc = (CarryableComponent)item.GetComponent(nameof(CarryableComponent));
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
