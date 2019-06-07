using RoguelikeFramework.components;
using RoguelikeFramework.models;
using System.Collections.Generic;
using System.Drawing;

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
                if (md.dest != null && md.dest.Count > 0) {
                    Point dest = md.dest[0];
                    if (this.isAccessible(this.map_data.map[dest.X, dest.Y])) {
                        this.map_data.map[p.x, p.y].Remove(entity);

                        p.x = dest.X;
                        p.y = dest.Y;

                        this.map_data.map[p.x, p.y].Add(entity);

                        this.checkMapVisibilitySystem.ReCheckVisibility = true;
                    }
                } else if (md.offX != 0 || md.offY != 0) {
                    if (p.x + md.offX >= 0 && p.x + md.offX < this.map_data.getWidth() && p.y + md.offY >= 0 && p.y + md.offY < this.map_data.getHeight()) {
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