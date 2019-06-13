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
            AlienComponent alienData = (AlienComponent)entity.GetComponent(nameof(AlienComponent));
            if (alienData == null) { // Are we an alien?
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
                alienData.moveWhenNoEnemy = true;
                PositionComponent targetPos = (PositionComponent)target.GetComponent(nameof(PositionComponent));
                if (this.CheckForImpregnation(alienData, pos, target, targetPos)) {
                    us.actionPoints -= 50;
                } else {
                    mdc.route = Misc.GetLine(pos.x, pos.y, targetPos.x, targetPos.y, true);
                }
            } else {
                if (alienData.moveWhenNoEnemy) {
                    alienData.moveWhenNoEnemy = false;

                    alienData.impregnateNextEnemy = Misc.random.Next(1, 2) == 1;

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


        private bool CheckForImpregnation(AlienComponent alienData, PositionComponent alienPos, AbstractEntity target, PositionComponent targetPos) {
            if (alienData.impregnateNextEnemy) {
                double dist = GeometryFunctions.Distance(alienPos.x, alienPos.y, targetPos.x, targetPos.y);
                if (dist < 2) {
                    target.AddComponent(new ImpregnatedComponent());
                    return true;
                }
            }
            return false;
        }


        private AbstractEntity GetTarget(int ourX, int ourY, int ourSide) {
            foreach (var e in this.entities) {
                ImpregnatedComponent ic = (ImpregnatedComponent)e.GetComponent(nameof(ImpregnatedComponent));
                if (ic == null) { // Don't go after anyone who's impregnated
                    MobDataComponent att = (MobDataComponent)e.GetComponent(nameof(MobDataComponent));
                    if (att != null && att.side != ourSide && att.side >= 0) {
                        PositionComponent pos = (PositionComponent)e.GetComponent(nameof(PositionComponent));
                        if (pos != null) {
                            if (this.cmvs.CanSee(ourX, ourY, pos.x, pos.y)) {
                                return e;
                            }
                        }
                    }
                }
            }
            return null;
        }

    }

}
