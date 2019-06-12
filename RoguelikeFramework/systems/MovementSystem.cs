using RoguelikeFramework.astar;
using RoguelikeFramework.components;
using RoguelikeFramework.models;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace RoguelikeFramework.systems {

    public class MovementSystem : AbstractSystem, IAStarMapInterface {

        private MapData map_data;
        private CheckMapVisibilitySystem checkMapVisibilitySystem;
        private CloseCombatSystem closeCombatSystem;

        public MovementSystem(BasicEcs ecs, MapData _map_data, CheckMapVisibilitySystem _checkMapVisibilitySystem, CloseCombatSystem _closeCombatSystem) : base(ecs, true) {
            this.map_data = _map_data;
            this.checkMapVisibilitySystem = _checkMapVisibilitySystem;
            this.closeCombatSystem = _closeCombatSystem;
        }


        public override void ProcessEntity(AbstractEntity entity) {
            MovementDataComponent md = (MovementDataComponent)entity.GetComponent(nameof(MovementDataComponent));
            PositionComponent pos = (PositionComponent)entity.GetComponent(nameof(PositionComponent));
            if (md != null && pos != null) {
                if (md.route != null && md.route.Count > 0) { // Do they have a path set up?
                    Point dest = md.route[0];
                    if (pos.x == dest.X && pos.y == dest.Y) {
                        md.route.RemoveAt(0); // We're on the actual square.  This should never happen!
                    } else if (this.Move(entity, pos, dest)) {
                        md.route.RemoveAt(0);
                    }
                } else if (md.offX != 0 || md.offY != 0) {
                    if (pos.x + md.offX >= 0 && pos.x + md.offX < this.map_data.getWidth() && pos.y + md.offY >= 0 && pos.y + md.offY < this.map_data.getHeight()) {
                        this.Move(entity, pos, new Point(pos.x + md.offX, pos.y + md.offY));
                    }
                    // Reset movement for next turn
                    md.offX = 0;
                    md.offY = 0;
                }
            }
        }


        private bool Move(AbstractEntity entity, PositionComponent p, Point dest) {
            if (this.IsAccessible(dest.X, dest.Y)) {// this.map_data.map[dest.X, dest.Y])) {
                MobDataComponent mdc = (MobDataComponent)entity.GetComponent(nameof(MobDataComponent));
                if (mdc == null || mdc.actionPoints > 0) {
                    int cost = 50;
                    if (p.x != dest.X && p.y != dest.Y) { // Diagonal
                        cost = 70;
                    }
                    mdc.actionPoints -= cost;
                    this.map_data.map[p.x, p.y].Remove(entity);

                    p.x = dest.X;
                    p.y = dest.Y;

                    this.map_data.map[p.x, p.y].Add(entity);

                    this.checkMapVisibilitySystem.ReCheckVisibility = true;
                    return true;
                }
            } else {
                this.closeCombatSystem.Combat(entity, this.map_data.map[dest.X, dest.Y]);
            }
            return false;
        }


        private bool IsAccessible(int x, int y) {
            List<AbstractEntity> entities = this.map_data.map[x, y];
            foreach (AbstractEntity entity in entities) {
                PositionComponent p = (PositionComponent)entity.GetComponent(nameof(PositionComponent));
                if (p.blocks_movement) {
                    return false;
                }
            }
            return true;
        }


        public List<Point> GetAStarRoute(int x1, int y1, int x2, int y2) {
            AStar astar = new AStar(this);
            astar.FindPath(x1, y1, x2, y2);
            if (astar.WasSuccessful()) {
                //Console.WriteLine(astar.GetMapAsString());
                return astar.GetRoute();
            } else {
                return null;
            }
        }


        public Point GetRandomAccessibleSquare() {
            int sqX = 0;
            int sqY = 0;
            while (this.IsAccessible(sqX, sqY) == false) {
                sqX = Misc.random.Next(this.map_data.map.GetLength(0));
                sqY = Misc.random.Next(this.map_data.map.GetLength(1));
            }
            return new Point(sqX, sqY);
        }


        public int GetMapWidth() {
            return this.map_data.map.GetLength(0);
        }


        public int GetMapHeight() {
            return this.map_data.map.GetLength(1);
        }


        public bool IsMapSquareTraversable(int x, int y) {
            return this.IsAccessible(x, y);
        }


        public float GetMapSquareDifficulty(int x, int y) {
            return 1;
        }

    }

}