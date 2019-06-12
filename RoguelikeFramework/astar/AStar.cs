using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeFramework.astar {

    public class AStar {

        private IAStarMapInterface map_interface;
        private Node[,] map;
        private PriorityList open;
        private bool[,] checked2;
        private int start_x, start_z, end_x, end_z, max_dist;
        //private bool finding_path = false;
        private bool failed;
        private List<Point> route;
        //private bool can_timeout;
        //private long max_dur, timeout_time;
        //public volatile static int tot_threads = 0; // How many concurrent instances are there?

        // Debugging vars
        private string[,] strmap;

        /*public AStar(IAStarMapInterface intface, long max_duration) : this(intface) {
            //this.can_timeout = true;
            //this.max_dur = max_duration;
        }*/


        public AStar(IAStarMapInterface intface) {
            //this.setDaemon(true);
            this.map_interface = intface;

            int w = intface.GetMapWidth();
            int h = intface.GetMapHeight();

            this.strmap = new string[w, h];
        }


        public void FindPath(int start_x, int start_z, int targ_x, int targ_z) {
            this.FindPath(start_x, start_z, targ_x, targ_z, -1);

        }

        /**
         * This is the main findPath function.
         * 
         * @param start_x
         * @param start_z
         * @param targ_x
         * @param targ_z
         * @param max_dist - will stop searching once a certain distance is reached.
         * @param thread - Run in a thread (otherwise block until finished).
         * 
         */
        public void FindPath(int start_x, int start_z, int targ_x, int targ_z, int max_dist) {
            /*if (this.can_timeout) {
                //timeout_time = System.currentTimeMillis() + max_dur;
            }*/

            /*if (this.finding_path) {
                throw new Exception("Trying to find path concurrently!");
            }*/
            //System.out.println("Finding path from " + start_x + "," + start_z + " to " + targ_x+ "," + targ_z + ".");
            this.route = new List<Point>();
            //this.finding_path = true;
            this.failed = false;
            this.start_x = start_x;
            this.start_z = start_z;
            this.end_x = targ_x;
            this.end_z = targ_z;
            this.max_dist = max_dist;
            /*if (thread) {
                start();
            } else {*/
            this.FindRoute();
            //}
        }

        private void FindRoute() {
            //tot_threads++;
            //System.out.println("Tot A* threads:" + this.tot_threads);

            int w = this.map_interface.GetMapWidth();
            int h = this.map_interface.GetMapHeight();

            this.map = new Node[w, h]; // Reset the map

            for (int z = 0; z < h; z++) {
                for (int x = 0; x < w; x++) {
                    this.strmap[x, z] = "-";
                }
            }
            this.strmap[this.start_x, this.start_z] = "S";
            this.strmap[this.end_x, this.end_z] = "F";

            this.open = new PriorityList();
            this.checked2 = new bool[w, h];

            // Now find the path
            Node node = new Node(this.start_x, this.start_z);
            this.map[this.start_x, this.start_z] = node;
            node.SetHeuristic(null, this.end_x, this.end_z, 1);
            this.open.Add(node);
            //System.out.println("Starting to find path.");
            while (node.x != this.end_x || node.z != this.end_z) {
                /*if (can_timeout) {
                    if (System.currentTimeMillis() > this.timeout_time) {
                        this.failed = true;
                        break;
                    }
                }*/
                //System.out.println("Node: " + node.x + "," + node.z);
                this.open.Remove(node);
                this.checked2[node.x, node.z] = true;
                this.GetAdjacentSquares(0, -1, node, this.end_x, this.end_z, this.max_dist, 1);
                this.GetAdjacentSquares(1, 0, node, this.end_x, this.end_z, this.max_dist, 1);
                this.GetAdjacentSquares(0, 1, node, this.end_x, this.end_z, this.max_dist, 1);
                this.GetAdjacentSquares(-1, 0, node, this.end_x, this.end_z, this.max_dist, 1);

                // Check diagonals
                /*this.getAdjacentSquares(-1, -1, node, end_x, end_z, max_dist, 1.4f);
                this.getAdjacentSquares(1, 1, node, end_x, end_z, max_dist, 1.4f);
                this.getAdjacentSquares(-1, 1, node, end_x, end_z, max_dist, 1.4f);
                this.getAdjacentSquares(1, -1, node, end_x, end_z, max_dist, 1.4f);
    */
                if (this.open.Count > 0) {
                    // Get the next square and go from there.
                    node = (Node)this.open[0];
                    //System.out.println("Next node: " + node.x + "," + node.z);
                } else {
                    // Cannot get to the destination!
                    this.failed = true;
                    //System.out.println("Cannot get from " + start_x + "," + start_z + " to " + end_x + "," + end_z);
                    //route = new WayPoints();
                    break;
                }
            }

            // We have finished, so iterate through the parents.
            while (node.GetParent() != null) {
                this.route.Insert(0, new Point(node.x, node.z));
                node = node.GetParent();
                this.strmap[node.x, node.z] = "X";
            }
            //route.add(0, new Point(start_x, start_z)); // add the start pos just in case
            /*if (failed == false) {
                route.add(new Point(end_x, end_z)); // add the end pos just in case
            }*/

            this.strmap[this.start_x, this.start_z] = "S";
            this.strmap[this.end_x, this.end_z] = "F";
            //showMap();

            //this.finding_path = false;

            //tot_threads--;
            //System.out.println("Finished finding path. (threads:" + this.tot_threads + ")");
        }


        /*public bool IsFindingPath() {
            return this.finding_path;
        }*/


        public bool WasSuccessful() {
            return this.failed == false;
        }


        public List<Point> GetRoute() {
            return this.route;
        }

        /**
         * Draw the map and route.
         *
         */
        public string GetMapAsString() {
            try {
                int w = this.map_interface.GetMapWidth();
                int h = this.map_interface.GetMapHeight();

                StringBuilder str = new StringBuilder();
                for (int z = 0; z < h; z++) {
                    for (int x = 0; x < w; x++) {
                        str.Append(this.strmap[x, z]);
                    }
                    str.Append("\n");
                }
                return str.ToString();
                //System.out.println(str;
            } catch (Exception e) {
                //e.printStackTrace();
                return null;
            }

        }


        private void GetAdjacentSquares(int off_x, int off_z, Node prnt, int targ_x, int targ_z, int max_dist, float mult) {
            int x = prnt.x + off_x;
            int z = prnt.z + off_z;
            try {
                // Check if we're checking the distance
                if (max_dist > 0) {
                    if (prnt.GetDistFromStart() >= max_dist) {
                        return;
                    }
                }
                if (this.checked2[x, z] == false) {
                    if (this.map_interface.IsMapSquareTraversable(x, z) || (x == targ_x && z == targ_z)) {
                        Node n = new Node(x, z);
                        float dist = this.map_interface.GetMapSquareDifficulty(x, z) * mult;
                        n.SetHeuristic(prnt, targ_x, targ_z, dist);
                        // Check if there's a node already in open at the same co-ords
                        bool found = false;
                        if (this.map[x, z] != null) {
                            Node other = this.map[x, z];
                            if (other.GetHeuristic() <= n.GetHeuristic()) {
                                // Its a better node
                                found = true;
                            } else {
                                // Its worse so remove this one.
                                this.map[x, z] = null;
                                this.open.Remove(other);
                            }
                        }
                        if (found == false) {
                            this.map[x, z] = n;
                            this.open.Add(n);
                            //if (SHOW_MAP) {
                            if (this.strmap[x, z] == "-") {
                                this.strmap[x, z] = "+"; // Mark it as checked
                            }
                            //}
                        }
                    } else {
                        //if (SHOW_MAP) {
                        this.strmap[x, z] = "B";
                        //}
                    }
                } else {
                    //if (SHOW_MAP) {
                    if (this.strmap[x, z] != "B") {
                        this.strmap[x, z] = "W";
                    }
                    //}
                }
            } catch (Exception ex2) {
                // Do nothing
            }
        }


        public static double Distance(int x1, int y1, int x2, int y2) {
            return Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }

    }

}

