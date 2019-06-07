using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeFramework.systems {

    public class EffectsSystem {

        public List<AbstractEffect> effects = new List<AbstractEffect>();

        public void process() {
            foreach (var effect in this.effects) {
                effect.process();
                if (effect.hasEnded()) {
                    this.effects.Remove(effect);
                }
            }
        }

    }

}
