using RoguelikeFramework.components;
using RoguelikeFramework.models;
using System.Collections.Generic;

namespace RoguelikeFramework.systems {

    public class MovementSystem : AbstractSystem {

        private MapData map_data;

        public MovementSystem(MapData _map_data) {
            this.map_data = _map_data;
        }


        public override void processEntity(AbstractEntity entity) {
            MovementDataComponent md = (MovementDataComponent)entity.getComponent(nameof(MovementDataComponent));
            PositionComponent p = (PositionComponent)entity.getComponent(nameof(PositionComponent));
            if (md != null && p != null) {
                if (md.offX != 0 || md.offY != 0) {

                    // Check the square is accessible
                    if (this.isAccessible(this.map_data.map[p.x + md.offX, p.y + md.offY])) {
                        this.map_data.map[p.x, p.y].Remove(entity);

                        p.x += md.offX;
                        p.y += md.offY;

                        this.map_data.map[p.x, p.y].Add(entity);
                    }
                    // Reset movement for next turn
                    md.offX = 0;
                    md.offY = 0;
                }
            }
        }


        private bool isAccessible(List<AbstractEntity> entities) {
            foreach (AbstractEntity entity in entities) {
                PositionComponent p = (PositionComponent)entity.getComponent(nameof(PositionComponent));
                if (p.blocks_movement) {
                    return false;
                }
            }
            return true;
        }

    }

}
