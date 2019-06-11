using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeFramework.astar {

    public class Node {

        private Node parent;
        public int x, z;
        private double heuristic;
        private float dist_from_start;

        public Node(int x, int z) {
            this.x = x;
            this.z = z;
        }


        public void SetHeuristic(Node prnt, int targ_x, int targ_z, float dist) {
            if (prnt != null) {
                this.parent = prnt;
                this.dist_from_start = prnt.dist_from_start + dist;
            }
            double dist_to_target = AStar.Distance(this.x, this.z, targ_x, targ_z);

            this.heuristic = this.dist_from_start + dist_to_target;
        }


        public Node GetParent() {
            return this.parent;
        }


        public double GetHeuristic() {
            return this.heuristic;
        }


        public double GetDistFromStart() {
            return this.dist_from_start;
        }
    }
}
