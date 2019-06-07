using RoguelikeFramework.components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeFramework.systems {

    public class PickupItemSystem {

        public PickupItemSystem() {
        }


        public void PickupItem(AbstractEntity unit, AbstractEntity item) {
            CanCarryComponent ccc = (CanCarryComponent)unit.getComponent(nameof(CanCarryComponent));
            CarryableComponent cc = (CarryableComponent)item.getComponent(nameof(CarryableComponent));
            // todo - check weight etc...
            ccc.AddItem(item);
        }


        public void DropItem(AbstractEntity unit, AbstractEntity item, List<AbstractEntity> mapSquare) {
            CanCarryComponent ccc = (CanCarryComponent)unit.getComponent(nameof(CanCarryComponent));
            //CarryableComponent cc = (CarryableComponent)item.getComponent(nameof(CarryableComponent));
            if (ccc.GetItems().Contains(item)) {
                ccc.RemoveItem(item);
                mapSquare.Add(item);
            }
        }

    }

}
