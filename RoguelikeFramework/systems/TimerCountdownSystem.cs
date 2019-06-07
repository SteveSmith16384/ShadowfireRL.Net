using RoguelikeFramework.components;

namespace RoguelikeFramework.systems {
    public class TimerCountdownSystem : AbstractSystem {

        private ExplosionSystem explosionSystem;

        public TimerCountdownSystem(ExplosionSystem _expl) {
            this.explosionSystem = _expl;
        }


        public virtual void processEntity(AbstractEntity entity) {
            TimerCanBeSetComponent tcbsc = (TimerCanBeSetComponent)entity.getComponent(nameof(TimerCanBeSetComponent));
            if (tcbsc != null) {
                if (tcbsc.activated) {
                    tcbsc.timeLeft--;
                    if (tcbsc.timeLeft <= 0) {
                        this.TimeExpired(entity);
                    }
                }
            }
        }


        private void TimeExpired(AbstractEntity entity) {
            ExplodesWhenTimerExpiresComponent explodes = (ExplodesWhenTimerExpiresComponent)entity.getComponent(nameof(ExplodesWhenTimerExpiresComponent));
            if (explodes != null) {

            }
        }


    }
}
