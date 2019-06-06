using RoguelikeFramework.components;
using RoguelikeFramework.models;
using System.Collections.Generic;

namespace RoguelikeFramework.systems {

    public class MovementSystem : AbstractSystem {

        private MapData map_data;
        private CheckMapVisibilitySystem checkMapVisibilitySystem;

        public MovementSystem(MapData _map_data, CheckMapVisibilitySystem _checkMapVisibilitySystem) {
            this.map_data = _map_data;
            this.checkMapVisibilitySystem = _checkMapVisibilitySystem;
        }


        public override void processEntity(AbstractEntity entity) {
            MovementDataComponent md = (MovementDataComponent)entity.getComponent(nameof(MovementDataComponent));
            PositionComponent p = (PositionComponent)entity.getComponent(nameof(PositionComponent));
            if (md != null && p != null) {
                if (md.offX != 0 || md.offY != 0) {
                    if (p.x + md.offX >= 0 && p.x + md.offX < this.map_data.getWidth() && p.y + md.offY >= 0 && p.y + md.offY < this.map_data.getHeight()) {
                        // Check the square is accessible
                        if (this.isAccessible(this.map_data.map[p.x + md.offX, p.y + md.offY])) {
                            this.map_data.map[p.x, p.y].Remove(entity);

                            p.x += md.offX;
                            p.y += md.offY;

                            this.map_data.map[p.x, p.y].Add(entity);

                            this.checkMapVisibilitySystem.ReCheckVisibility = true;
                        }
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
