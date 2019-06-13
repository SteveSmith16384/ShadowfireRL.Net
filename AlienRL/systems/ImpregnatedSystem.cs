using AlienRL.components;
using RoguelikeFramework.components;
using RoguelikeFramework.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlienRL.systems {
    public class ImpregnatedSystem : AbstractSystem {

        private MapData mapData;

        public ImpregnatedSystem(BasicEcs ecs, MapData _mapData) : base(ecs, true) {
            this.mapData = _mapData;
        }


        public override void ProcessEntity(AbstractEntity entity) {
            // Todo
            ImpregnatedComponent ic = (ImpregnatedComponent)entity.GetComponent(nameof(ImpregnatedComponent));
            if (ic != null) {
                ic.turnsUntilBurst--;
                if (ic.turnsUntilBurst <= 0) {
                    PositionComponent pos = (PositionComponent)entity.GetComponent(nameof(PositionComponent));
                    AlienEntityFactory.CreateAlien(this.ecs, this.mapData, pos.x, pos.y);
                    entity.markForRemoval = true;
                }
            }


        }
    }

}