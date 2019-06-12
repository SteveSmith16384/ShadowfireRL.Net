using AlienRL.components;
using RoguelikeFramework;
using RoguelikeFramework.components;
using RoguelikeFramework.systems;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace AlienRL.systems {

    /**
     * Alogo:
     * 1) Wait until enemy seen
     * 2) Run after and attack enemy
     * 3) Move to random location
     * 4) Repeat
     * 
     */
    public class AlienAISystem : AbstractSystem {

        private List<AbstractEntity> entities;
        private readonly CheckMapVisibilitySystem cmvs;

        public AlienAISystem(BasicEcs ecs, CheckMapVisibilitySystem _cmvs, List<AbstractEntity> _entities) : base(ecs, true) {
            this.cmvs = _cmvs;
            this.entities = _entities;
        }


        public override void ProcessEntity(AbstractEntity entity) {
            AlienComponent sosc = (AlienComponent)entity.GetComponent(nameof(AlienComponent));
            if (sosc == null) { // Are we an alien?
                return;
            }
            MobDataComponent us = (MobDataComponent)entity.GetComponent(nameof(MobDataComponent));
            if (us.actionPoints <= 0) { // Only do stuff if we've got any APs
                return;
            }

            PositionComponent pos = (PositionComponent)entity.GetComponent(nameof(PositionComponent));
            AbstractEntity target = this.GetTarget(pos.x, pos.y, us.side);
            MovementDataComponent mdc = (MovementDataComponent)entity.GetComponent(nameof(MovementDataComponent));
            if (target != null) {
                Console.WriteLine($"Alien can see {target.name}");
                PositionComponent targetPos = (PositionComponent)target.GetComponent(nameof(PositionComponent));
                mdc.route = Misc.GetLine(pos.x, pos.y, targetPos.x, targetPos.y, true);
                sosc.moveWhenNoEnemy = true;
            } else {
                if (sosc.moveWhenNoEnemy) {
                    sosc.moveWhenNoEnemy = false;

                    // Move to a random point on the map
                    MovementSystem ms = (MovementSystem)this.ecs.GetSystem(nameof(MovementSystem));
                    Point p = ms.GetRandomAccessibleSquare();
                    mdc.route = ms.GetAStarRoute(pos.x, pos.y, p.X, p.Y);
                }
                if (mdc.route == null || mdc.route.Count == 0) {
                    us.actionPoints -= 100; // Waiting....
                }
            }
        }


        private AbstractEntity GetTarget(int ourX, int ourY, int ourSide) {
            foreach (var e in this.entities) {
                MobDataComponent att = (MobDataComponent)e.GetComponent(nameof(MobDataComponent));
                if (att != null && att.side != ourSide) {
                    PositionComponent pos = (PositionComponent)e.GetComponent(nameof(PositionComponent));
                    if (pos != null) {
                        if (this.cmvs.CanSee(ourX, ourY, pos.x, pos.y)) {
                            return e;
                        }
                    }
                }
            }
            return null;
        }

    }

}
