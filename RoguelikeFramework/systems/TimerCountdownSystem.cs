using RoguelikeFramework.components;

namespace RoguelikeFramework.systems {
    public class TimerCountdownSystem : AbstractSystem {

        private ExplosionSystem explosionSystem;

        public TimerCountdownSystem(BasicEcs ecs, ExplosionSystem _expl) : base(ecs, true) {
            this.explosionSystem = _expl;
        }


        public override void ProcessEntity(AbstractEntity entity) {
            TimerCanBeSetComponent tcbsc = (TimerCanBeSetComponent)entity.GetComponent(nameof(TimerCanBeSetComponent));
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
            ExplodesWhenTimerExpiresComponent explodes = (ExplodesWhenTimerExpiresComponent)entity.GetComponent(nameof(ExplodesWhenTimerExpiresComponent));
            if (explodes != null) {

            }
        }


    }
}
