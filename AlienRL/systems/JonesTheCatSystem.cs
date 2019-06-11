using AlienRL.components;
using RoguelikeFramework.components;

namespace AlienRL.systems {

    class JonesTheCatSystem : AbstractSystem {

        public JonesTheCatSystem(BasicEcs ecs) : base(ecs, true) {
        }


        public override void ProcessEntity(AbstractEntity entity) {
            JonesTheCatComponent sosc = (JonesTheCatComponent)entity.GetComponent(nameof(JonesTheCatComponent));
            if (sosc != null) { // Are we a cat?
                MobDataComponent us = (MobDataComponent)entity.GetComponent(nameof(MobDataComponent));
                if (us.actionPoints > 0) {
                    PositionComponent pos = (PositionComponent)entity.GetComponent(nameof(PositionComponent));
                    // todo

                }
            }
        }


        private void GetRoute() {

        }


    }

}
