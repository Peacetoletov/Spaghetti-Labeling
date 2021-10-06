using System.Collections.Generic;

namespace Spaghetti_Labeling
{
    // Class for the optimal decision tree
    public class ODTree
    {
        private Node root { get; }


        public ODTree() {
            // Root
            this.root = new Node('o');
            
            // Depth 1
            Node l = new Node('s');
            Node r = new Node('n');
            this.root.SetChildren(l, r);

            // Depth 2
            Node ll = new Node('p');
            Node lr = new Node('p');
            l.SetChildren(ll, lr);

            Node rl = new Node('r');
            Node rr = new Node('j');
            r.SetChildren(rl, rr);

            // Depth 3
            Node lll = new Node('t');
            Node llr = new Node('j');
            ll.SetChildren(lll, llr);

            Node lrl = new Node('r');
            Node lrr = new Node('n');
            lr.SetChildren(lrl, lrr);

    	    Node rll = new Node('j');
            Node rlr = new Node('j');
            rl.SetChildren(rll, rlr);

            Node rrl = new Node('p');
            Node rrr = new Node('i');
            rr.SetChildren(rrl, rrr);

            // Depth 4
            Leaf llll = new Leaf(new HashSet<int> {1});
            Leaf lllr = new Leaf(new HashSet<int> {2});
            lll.SetChildren(llll, lllr);

            Node llrl = new Node('k');
            Leaf llrr = new Leaf(new HashSet<int> {4});
            llr.SetChildren(llrl, llrr);

            Node lrll = new Node('n');
            Leaf lrlr = new Leaf(new HashSet<int> {6});
            lrl.SetChildren(lrll, lrlr);

            Node lrrl = new Node('r');
            Node lrrr = new Node('j');
            lrr.SetChildren(lrrl, lrrr);

            Node rlll = new Node('p');
            Node rllr = new Node('i');
            rll.SetChildren(rlll, rllr);

            Node rlrl = new Node('p');
            Node rlrr = new Node('m');
            rlr.SetChildren(rlrl, rlrr);
            
            Leaf rrll = new Leaf(new HashSet<int> {6});
            Node rrlr = new Node('k');
            rrl.SetChildren(rrll, rrlr);

            Node rrrl = new Node('c');
            Leaf rrrr = new Leaf(new HashSet<int> {4, 6});
            rrr.SetChildren(rrrl, rrrr);

            // Depth 5
            Node llrll = new Node('i');
            Node llrlr = new Node('i');
            llrl.SetChildren(llrll, llrlr);

            Leaf lrlll = new Leaf(new HashSet<int> {2});
            Leaf lrllr = new Leaf(new HashSet<int> {6});
            lrll.SetChildren(lrlll, lrllr);

            Node lrrll = new Node('j');
            Node lrrlr = new Node('j');
            lrrl.SetChildren(lrrll, lrrlr);

            Node lrrrl = new Node('k');
            Node lrrrr = new Node('i');
            lrrr.SetChildren(lrrrl, lrrrr);

            Node rllll = new Node('i');
            Node rlllr = new Node('k');
            rlll.SetChildren(rllll, rlllr);

            Node rllrl = new Node('h');
            Leaf rllrr = new Leaf(new HashSet<int> {4});
            rllr.SetChildren(rllrl, rllrr);

            Node rlrll = new Node('h');
            Node rlrlr = new Node('k');
            rlrl.SetChildren(rlrll, rlrlr);

            Node rlrrl = new Node('i');
            Node rlrrr = new Node('h');
            rlrr.SetChildren(rlrrl, rlrrr);

            Leaf rrlrl = new Leaf(new HashSet<int> {6});
            Node rrlrr = new Node('d');
            rrlr.SetChildren(rrlrl, rrlrr);

            Leaf rrrll = new Leaf(new HashSet<int> {11});
            Node rrrlr = new Node('h');
            rrrl.SetChildren(rrrll, rrrlr);

            // Depth 6
            Leaf llrlll = new Leaf(new HashSet<int> {2});
            Leaf llrllr = new Leaf(new HashSet<int> {4});
            llrll.SetChildren(llrlll, llrllr);

            Leaf llrlrl = new Leaf(new HashSet<int> {5});
            Node llrlrr = new Node('d');
            llrlr.SetChildren(llrlrl, llrlrr);

            Node lrrlll = new Node('k');
            Leaf lrrllr = new Leaf(new HashSet<int> {4});
            lrrll.SetChildren(lrrlll, lrrllr);

            Node lrrlrl = new Node('k');
            Node lrrlrr = new Node('m');
            lrrlr.SetChildren(lrrlrl, lrrlrr);

            Leaf lrrrll = new Leaf(new HashSet<int> {6});
            Node lrrrlr = new Node('d');
            lrrrl.SetChildren(lrrrll, lrrrlr);

            Node lrrrrl = new Node('c');
            Leaf lrrrrr = new Leaf(new HashSet<int> {4, 6});
            lrrrr.SetChildren(lrrrrl, lrrrrr);

            Node rlllll = new Node('h');
            Leaf rllllr = new Leaf(new HashSet<int> {4});
            rllll.SetChildren(rlllll, rllllr);

            Node rlllrl = new Node('i');
            Node rlllrr = new Node('i');
            rlllr.SetChildren(rlllrl, rlllrr);

            Leaf rllrll = new Leaf(new HashSet<int> {4});
            Node rllrlr = new Node('c');
            rllrl.SetChildren(rllrll, rllrlr);

            Node rlrlll = new Node('i');
            Node rlrllr = new Node('m');
            rlrll.SetChildren(rlrlll, rlrllr);

            Node rlrlrl = new Node('h');
            Node rlrlrr = new Node('m');
            rlrlr.SetChildren(rlrlrl, rlrlrr);

            Node rlrrll = new Node('h');
            Leaf rlrrlr = new Leaf(new HashSet<int> {11});
            rlrrl.SetChildren(rlrrll, rlrrlr);

            Node rlrrrl = new Node('g');
            Node rlrrrr = new Node('i');
            rlrrr.SetChildren(rlrrrl, rlrrrr);

            Leaf rrlrrl = new Leaf(new HashSet<int> {12});
            Node rrlrrr = new Node('i');
            rrlrr.SetChildren(rrlrrl, rrlrrr);

            Node rrrlrl = new Node('g');
            Leaf rrrlrr = new Leaf(new HashSet<int> {3, 4, 6});
            rrrlr.SetChildren(rrrlrl, rrrlrr);

            // Depth 7
            // Before implementing depth 7, implement DFS and Inorder to make sure the tree is correct so far

        }
    }
}