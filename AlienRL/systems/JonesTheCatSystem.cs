using AlienRL.components;
using RoguelikeFramework;
using RoguelikeFramework.components;
using RoguelikeFramework.systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlienRL.systems {

    class JonesTheCatSystem : AbstractSystem {

        public JonesTheCatSystem() {
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
