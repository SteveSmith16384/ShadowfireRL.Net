using System.Collections.Generic;

namespace RoguelikeFramework.systems {

    public class EffectsSystem : AbstractSystem {

        public List<AbstractEffect> effects = new List<AbstractEffect>();

        public EffectsSystem(BasicEcs ecs) : base(ecs, false) {

        }


        public void Process() {
            foreach (var effect in this.effects) {
                effect.process();
                if (effect.hasEnded()) {
                    this.effects.Remove(effect);
                }
            }
        }


        public bool HasEffects() {
            return this.effects.Count > 0;
        }

    }

}
