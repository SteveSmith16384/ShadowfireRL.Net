using AlienRL.components;
using RoguelikeFramework.components;
using RoguelikeFramework.systems;
using System.Drawing;

namespace AlienRL.systems {

    class JonesTheCatSystem : AbstractSystem {

        public JonesTheCatSystem(BasicEcs ecs) : base(ecs, true) {
        }


        public override void ProcessEntity(AbstractEntity entity) {
            JonesTheCatComponent cat = (JonesTheCatComponent)entity.GetComponent(nameof(JonesTheCatComponent));
            if (cat == null) { // Are we a cat?
                return;
            }
            CarryableComponent cc = (CarryableComponent)entity.GetComponent(nameof(CarryableComponent));
            if (cc.carrier != null) { // Don't move if we're bring carried
                return;
            }
            MobDataComponent us = (MobDataComponent)entity.GetComponent(nameof(MobDataComponent));
            if (us.actionPoints <= 0) { // Only do stuff if we've got any APs
                return;
            }

            MovementDataComponent mdc = (MovementDataComponent)entity.GetComponent(nameof(MovementDataComponent));
            if (mdc.route == null || mdc.route.Count == 0) {
                MovementSystem ms = (MovementSystem)this.ecs.GetSystem(nameof(MovementSystem));
                PositionComponent pos = (PositionComponent)entity.GetComponent(nameof(PositionComponent));
                Point p = ms.GetRandomAccessibleSquare();
                mdc.route = ms.GetAStarRoute(pos.x, pos.y, p.X, p.Y);
            }
        }


        private void GetRoute() {

        }


    }

}
