using RLNET;
using System;

namespace RoguelikeFramework {
    public abstract class AbstractEffect {

        private readonly int frameInterval;
        private int frameTimeLeft;
        private int lastTick;

        public AbstractEffect(int _frameInterval) {
            this.frameInterval = _frameInterval;
            this.frameTimeLeft = this.frameInterval;

            this.lastTick = Environment.TickCount;
        }

        public void process() {
            int thiscount = Environment.TickCount;
            int diff = this.lastTick - this.lastTick;
            this.frameTimeLeft -= diff;
            if (this.frameTimeLeft <= 0) {
                this.subprocess();
                this.frameTimeLeft = this.frameInterval;
            }
        }

        protected abstract void subprocess();

        public abstract bool hasEnded();

        public abstract void draw(RLConsole map);

    }
}
