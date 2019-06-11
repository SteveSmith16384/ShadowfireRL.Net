using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeFramework.astar {

    public class WayPoints : List<Point> {

        /*public void insertRoute(int pos, WayPoints w) {
            this.addAll(pos, w);
        }*/

        public void truncate(int amt) {
            while (this.Count > amt) {
                this.RemoveAt(this.Count - 1);
            }
        }

        public void remove(int x, int y) {
            for (int i = 0; i < this.Count; i++) {
                Point p = this[i];
                if (p.X == x && p.Y == y) {
                    this.Remove(p);
                }
            }
        }

        public bool contains(int x, int y) {
            for (int i = 0; i < this.Count ; i++) {
                Point p = this[i];
                if (p.X == x && p.Y == y) {
                    return true;
                }
            }
            return false;
        }

        public Point getClosestPoint(int x, int y) {
            double closest_dist = 9999;// Double.MAX_VALUE;
            int closest_point = -1;
            for (int p = 0; p < this.Count; p++) {
                Point pnt = this[p];
                double dist = AStar.Distance(x, y, pnt.X, pnt.Y);
                if (dist < closest_dist) {
                    closest_dist = dist;
                    closest_point = p;
                }
            }
            return this[closest_point];
        }

        public Point getNextPoint() {
            if (this.hasAnotherPoint()) {
                return this[0];
            } else {
                throw new Exception("todo");
            }
        }

        public Point getLastPoint() {
            return this[this.Count - 1];
        }

        public Point getPenultimatePoint() {
            if (this.Count >= 2) {
                return this[this.Count - 2];
            } else {
                throw new Exception("todo");
            }
        }

        public void removeCurrentPoint() {
            this.RemoveAt(0);
        }

        public bool hasAnotherPoint() {
            return this.Count > 0;
        }


        public void add(int x, int y) {
            this.Add(new Point(x, y));
        }


        public WayPoints copy() {
            WayPoints w = new WayPoints();
            for (int i = 0; i < this.Count; i++) {
                Point p = this[i];
                w.Add(p);
            }
            return w;
        }

    }
}
