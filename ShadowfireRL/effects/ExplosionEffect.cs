using RLNET;
using RoguelikeFramework;

namespace ShadowfireRL.effects {

    public class ExplosionEffect : AbstractEffect {

        private int currentRange, maxRange;

        public ExplosionEffect(int range): base(200) {
            this.maxRange = range;
            this.currentRange = 1;
        }


        public override void draw(RLConsole map) {
            // todo
        }


        public override bool hasEnded() {
            return this.currentRange >= this.maxRange;
        }


        protected override void subprocess() {
            this.currentRange++;
        }

    }

}
