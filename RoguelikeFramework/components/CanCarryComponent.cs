using System.Collections.Generic;

namespace RoguelikeFramework.components {

    public class CanCarryComponent : AbstractComponent {

        private List<AbstractEntity> items = new List<AbstractEntity>();
        public AbstractEntity CurrentItem;
        public float maxWeight;

        public CanCarryComponent(float max) {
            this.maxWeight = max;
        }


        public void AddItem(AbstractEntity item) {
            this.items.Add(item);
            if (this.CurrentItem == null) {
                this.CurrentItem = item;
            }
        }


        public List<AbstractEntity> GetItems() {
            return this.items;
        }


        public void RemoveItem(AbstractEntity item) {
            this.items.Remove(item);
            if (this.CurrentItem == item) {
                this.CurrentItem = null;
            }
        }

    }

}
