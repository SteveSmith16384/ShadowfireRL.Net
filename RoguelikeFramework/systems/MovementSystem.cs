﻿using RoguelikeFramework.components;
using RoguelikeFramework.models;
using System.Collections.Generic;
using System.Drawing;

namespace RoguelikeFramework.systems {

    public class MovementSystem : AbstractSystem {

        private MapData map_data;
        private CheckMapVisibilitySystem checkMapVisibilitySystem;

        public MovementSystem(MapData _map_data, CheckMapVisibilitySystem _checkMapVisibilitySystem) {
            this.map_data = _map_data;
            this.checkMapVisibilitySystem = _checkMapVisibilitySystem;
        }


        public override void ProcessEntity(AbstractEntity entity) {
            MovementDataComponent md = (MovementDataComponent)entity.GetComponent(nameof(MovementDataComponent));
            PositionComponent pos = (PositionComponent)entity.GetComponent(nameof(PositionComponent));
            if (md != null && pos != null) {
                if (md.dest != null && md.dest.Count > 0) { // Do they have a path set up?
                    Point dest = md.dest[0];
                    if (this.Move(entity, pos, dest)) {
                        /*if (this.isAccessible(this.map_data.map[dest.X, dest.Y])) {
                            this.map_data.map[pos.x, pos.y].Remove(entity);

                            p.x = dest.X;
                            p.y = dest.Y;

                            this.map_data.map[p.x, p.y].Add(entity);

                            this.checkMapVisibilitySystem.ReCheckVisibility = true;
                        }*/
                        md.dest.RemoveAt(0);
                    }
                } else if (md.offX != 0 || md.offY != 0) {
                    if (pos.x + md.offX >= 0 && pos.x + md.offX < this.map_data.getWidth() && pos.y + md.offY >= 0 && pos.y + md.offY < this.map_data.getHeight()) {
                        this.Move(entity, pos, new Point(pos.x + md.offX, pos.y + md.offY));
                        /*if (this.isAccessible(this.map_data.map[p.x + md.offX, p.y + md.offY])) {
                            this.map_data.map[p.x, p.y].Remove(entity);

                            p.x += md.offX;
                            p.y += md.offY;

                            this.map_data.map[p.x, p.y].Add(entity);

                            this.checkMapVisibilitySystem.ReCheckVisibility = true;
                        }*/
                    }
                    // Reset movement for next turn
                    md.offX = 0;
                    md.offY = 0;
                }
            }
        }


        private bool Move(AbstractEntity entity, PositionComponent p, Point dest) {
            MobDataComponent mdc = (MobDataComponent)entity.GetComponent(nameof(MobDataComponent));
            if (this.isAccessible(this.map_data.map[dest.X, dest.Y])) {
                if (mdc.actionPoints > 0) {
                    mdc.actionPoints -= 50; // todo - check diagonal
                    this.map_data.map[p.x, p.y].Remove(entity);

                    p.x = dest.X;
                    p.y = dest.Y;

                    this.map_data.map[p.x, p.y].Add(entity);

                    this.checkMapVisibilitySystem.ReCheckVisibility = true;
                    return true;
                } else {
                    // todo - close combat
                }
            }
            return false;
        }


        private bool isAccessible(List<AbstractEntity> entities) {
            foreach (AbstractEntity entity in entities) {
                PositionComponent p = (PositionComponent)entity.GetComponent(nameof(PositionComponent));
                if (p.blocks_movement) {
                    return false;
                }
            }
            return true;
        }


    }

}