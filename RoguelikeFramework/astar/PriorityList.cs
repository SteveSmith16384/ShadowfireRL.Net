using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeFramework.astar {
    public class PriorityList : List<Node> {

        public void Add(Node n) {
            if (this.Count == 0) {
                base.Add(n);
                return;
            }

            //ListIterator<Node> it = this.listIterator();
            //Node node;
            int count = 0;
            foreach (var node in this) { 
            /*while (it.hasNext()) {
                node = (Node)it.next();*/
                if (n.GetHeuristic() < node.GetHeuristic()) {
                    base.Insert(count, n); // loop?
                    return;
                }
                count++;
            }
            // Got this far so it must be last.
            base.Add(n);
        }


        /*
        public void printList() {
            //System.out.println("List: " + this.size());

            ListIterator<Node> it = this.listIterator();
            Node node;
            while (it.hasNext()) {
                node = (Node)it.next();
                System.out.println("Dist=" + node.getHeuristic() + " (" + node.x + "," + node.z + ")");
            }
        }*/

    }
}
