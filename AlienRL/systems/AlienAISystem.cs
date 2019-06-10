﻿using AlienRL.components;
using RoguelikeFramework.components;
using RoguelikeFramework.systems;
using System;
using System.Collections.Generic;

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

        public AlienAISystem(CheckMapVisibilitySystem _cmvs, List<AbstractEntity> _entities) {
            this.cmvs = _cmvs;
            this.entities = _entities;
        }


        public override void ProcessEntity(AbstractEntity entity) {
            AlienComponent sosc = (AlienComponent)entity.GetComponent(nameof(AlienComponent));
            if (sosc != null) { // Are we an alien?
                MobDataComponent us = (MobDataComponent)entity.GetComponent(nameof(MobDataComponent));
                if (us.actionPoints > 0) {
                    PositionComponent pos = (PositionComponent)entity.GetComponent(nameof(PositionComponent));
                    AbstractEntity target = this.GetTarget(pos.x, pos.y, us.side);
                    if (target != null) {
                        Console.WriteLine($"Alien can see {target.name}");
                        // todo - move towards/attack
                    } else {
                        us.actionPoints -= 100; // Waiting....
                    }
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
