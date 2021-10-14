using System.Collections.Generic;

namespace Spaghetti_Labeling
{
    // Class for the optimal decision tree
    public static class ODTree
    {
        public static Tree GetTree() {
            // The tree was manually checked using DFS to make sure there were no mistakes
            // Root
            Node root = new Node('o');
            Tree tree = new Tree(root);
            
            // Depth 1
            Node l = new Node('s');
            Node r = new Node('n');
            root.SetChildren(l, r);

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
            Leaf llrlrrl = new Leaf(new HashSet<int> {10});
            Leaf llrlrrr = new Leaf(new HashSet<int> {4, 5});
            llrlrr.SetChildren(llrlrrl, llrlrrr);

            Node lrrllll = new Node('i');
            Node lrrlllr = new Node('i');
            lrrlll.SetChildren(lrrllll, lrrlllr);

            Node lrrlrll = new Node('i');
            Node lrrlrlr = new Node('d');
            lrrlrl.SetChildren(lrrlrll, lrrlrlr);

            Leaf lrrlrrl = new Leaf(new HashSet<int> {11});
            Node lrrlrrr = new Node('h');
            lrrlrr.SetChildren(lrrlrrl, lrrlrrr);

            Leaf lrrrlrl = new Leaf(new HashSet<int> {12});
            Node lrrrlrr = new Node('i');
            lrrrlr.SetChildren(lrrrlrl, lrrrlrr);

            Leaf lrrrrll = new Leaf(new HashSet<int> {11});
            Node lrrrrlr = new Node('h');
            lrrrrl.SetChildren(lrrrrll, lrrrrlr);

            Leaf rllllll = new Leaf(new HashSet<int> {2});
            Leaf rlllllr = new Leaf(new HashSet<int> {3});
            rlllll.SetChildren(rllllll, rlllllr);

            Node rlllrll = new Node('h');
            Leaf rlllrlr = new Leaf(new HashSet<int> {4});
            rlllrl.SetChildren(rlllrll, rlllrlr);

            Node rlllrrl = new Node('h');   
            Node rlllrrr = new Node('d');
            rlllrr.SetChildren(rlllrrl, rlllrrr);

            Leaf rllrlrl = new Leaf(new HashSet<int> {7});
            Leaf rllrlrr = new Leaf(new HashSet<int> {3, 4});
            rllrlr.SetChildren(rllrlrl, rllrlrr);

            Leaf rlrllll = new Leaf(new HashSet<int> {6});
            Node rlrlllr = new Node('m');
            rlrlll.SetChildren(rlrllll, rlrlllr);

            Leaf rlrllrl = new Leaf(new HashSet<int> {9});
            Leaf rlrllrr = new Leaf(new HashSet<int> {3, 6});
            rlrllr.SetChildren(rlrllrl, rlrllrr);

            Node rlrlrll = new Node('i');
            Node rlrlrlr = new Node('m');
            rlrlrl.SetChildren(rlrlrll, rlrlrlr);

            Node rlrlrrl = new Node('i');
            Node rlrlrrr = new Node('h');
            rlrlrr.SetChildren(rlrlrrl, rlrlrrr);

            Leaf rlrrlll = new Leaf(new HashSet<int> {11});
            Node rlrrllr = new Node('c');
            rlrrll.SetChildren(rlrrlll, rlrrllr);

            Leaf rlrrrll = new Leaf(new HashSet<int> {11});
            Node rlrrrlr = new Node('b');
            rlrrrl.SetChildren(rlrrrll, rlrrrlr);

            Node rlrrrrl = new Node('c');
            Leaf rlrrrrr = new Leaf(new HashSet<int> {3, 4, 6});
            rlrrrr.SetChildren(rlrrrrl, rlrrrrr);

            Node rrlrrrl = new Node('c');
            Leaf rrlrrrr = new Leaf(new HashSet<int> {4, 5, 6});
            rrlrrr.SetChildren(rrlrrrl, rrlrrrr);

            Leaf rrrlrll = new Leaf(new HashSet<int> {11});
            Node rrrlrlr = new Node('b');
            rrrlrl.SetChildren(rrrlrll, rrrlrlr);

            // Depth 8
            Leaf lrrlllll = new Leaf(new HashSet<int> {2});
            Leaf lrrllllr = new Leaf(new HashSet<int> {4});
            lrrllll.SetChildren(lrrlllll, lrrllllr);

            Leaf lrrlllrl = new Leaf(new HashSet<int> {5});
            Node lrrlllrr = new Node('d');
            lrrlllr.SetChildren(lrrlllrl, lrrlllrr);

            Leaf lrrlrlll = new Leaf(new HashSet<int> {6});
            Node lrrlrllr = new Node('m');
            lrrlrll.SetChildren(lrrlrlll, lrrlrllr);

            Node lrrlrlrl = new Node('i');
            Node lrrlrlrr = new Node('m');
            lrrlrlr.SetChildren(lrrlrlrl, lrrlrlrr);

            Node lrrlrrrl = new Node('g');
            Node lrrlrrrr = new Node('i');
            lrrlrrr.SetChildren(lrrlrrrl, lrrlrrrr);

            Node lrrrlrrl = new Node('c');
            Leaf lrrrlrrr = new Leaf(new HashSet<int> {4, 5, 6});
            lrrrlrr.SetChildren(lrrrlrrl, lrrrlrrr);

            Node lrrrrlrl = new Node('g');
            Leaf lrrrrlrr = new Leaf(new HashSet<int> {3, 4, 6});
            lrrrrlr.SetChildren(lrrrrlrl, lrrrrlrr);

            Leaf rlllrlll = new Leaf(new HashSet<int> {2});
            Leaf rlllrllr = new Leaf(new HashSet<int> {3});
            rlllrll.SetChildren(rlllrlll, rlllrllr);

            Leaf rlllrrll = new Leaf(new HashSet<int> {5});
            Node rlllrrlr = new Node('d');
            rlllrrl.SetChildren(rlllrrll, rlllrrlr);

            Leaf rlllrrrl = new Leaf(new HashSet<int> {10});
            Leaf rlllrrrr = new Leaf(new HashSet<int> {4, 5});
            rlllrrr.SetChildren(rlllrrrl, rlllrrrr);

            Leaf rlrlllrl = new Leaf(new HashSet<int> {11});
            Node rlrlllrr = new Node('g');
            rlrlllr.SetChildren(rlrlllrl, rlrlllrr);

            Leaf rlrlrlll = new Leaf(new HashSet<int> {6});
            Node rlrlrllr = new Node('m');
            rlrlrll.SetChildren(rlrlrlll, rlrlrllr);

            Leaf rlrlrlrl = new Leaf(new HashSet<int> {9});
            Leaf rlrlrlrr = new Leaf(new HashSet<int> {3, 6});
            rlrlrlr.SetChildren(rlrlrlrl, rlrlrlrr);

            Node rlrlrrll = new Node('h');
            Node rlrlrrlr = new Node('d');
            rlrlrrl.SetChildren(rlrlrrll, rlrlrrlr);

            Node rlrlrrrl = new Node('d');
            Node rlrlrrrr = new Node('d');
            rlrlrrr.SetChildren(rlrlrrrl, rlrlrrrr);

            Leaf rlrrllrl = new Leaf(new HashSet<int> {14});
            Leaf rlrrllrr = new Leaf(new HashSet<int> {9, 11});
            rlrrllr.SetChildren(rlrrllrl, rlrrllrr);

            Leaf rlrrrlrl = new Leaf(new HashSet<int> {7, 11});
            Node rlrrrlrr = new Node('i');
            rlrrrlr.SetChildren(rlrrrlrl, rlrrrlrr);

            Leaf rlrrrrll = new Leaf(new HashSet<int> {7, 11});
            Leaf rlrrrrlr = new Leaf(new HashSet<int> {3, 4, 6});
            rlrrrrl.SetChildren(rlrrrrll, rlrrrrlr);

            Leaf rrlrrrll = new Leaf(new HashSet<int> {11, 12});
            Node rrlrrrlr = new Node('h');
            rrlrrrl.SetChildren(rrlrrrll, rrlrrrlr);

            Leaf rrrlrlrl = new Leaf(new HashSet<int> {7, 11});
            Leaf rrrlrlrr = new Leaf(new HashSet<int> {3, 4, 6});
            rrrlrlr.SetChildren(rrrlrlrl, rrrlrlrr);

            // Depth 9
            Leaf lrrlllrrl = new Leaf(new HashSet<int> {10});
            Leaf lrrlllrrr = new Leaf(new HashSet<int> {4, 5});
            lrrlllrr.SetChildren(lrrlllrrl, lrrlllrrr);

            Leaf lrrlrllrl = new Leaf(new HashSet<int> {11});
            Node lrrlrllrr = new Node('h');
            lrrlrllr.SetChildren(lrrlrllrl, lrrlrllrr);

            Leaf lrrlrlrll = new Leaf(new HashSet<int> {12});
            Node lrrlrlrlr = new Node('m');
            lrrlrlrl.SetChildren(lrrlrlrll, lrrlrlrlr);

            Leaf lrrlrlrrl = new Leaf(new HashSet<int> {11, 12});
            Node lrrlrlrrr = new Node('h');
            lrrlrlrr.SetChildren(lrrlrlrrl, lrrlrlrrr);

            Leaf lrrlrrrll = new Leaf(new HashSet<int> {11});
            Node lrrlrrrlr = new Node('b');
            lrrlrrrl.SetChildren(lrrlrrrll, lrrlrrrlr);

            Node lrrlrrrrl = new Node('c');
            Leaf lrrlrrrrr = new Leaf(new HashSet<int> {3, 4, 6});
            lrrlrrrr.SetChildren(lrrlrrrrl, lrrlrrrrr);

            Leaf lrrrlrrll = new Leaf(new HashSet<int> {11, 12});
            Node lrrrlrrlr = new Node('h');
            lrrrlrrl.SetChildren(lrrrlrrll, lrrrlrrlr);

            Leaf lrrrrlrll = new Leaf(new HashSet<int> {11});
            Node lrrrrlrlr = new Node('b');
            lrrrrlrl.SetChildren(lrrrrlrll, lrrrrlrlr);

            Leaf rlllrrlrl = new Leaf(new HashSet<int> {8});
            Node rlllrrlrr = new Node('c');
            rlllrrlr.SetChildren(rlllrrlrl, rlllrrlrr);

            Leaf rlrlllrrl = new Leaf(new HashSet<int> {11});
            Node rlrlllrrr = new Node('b');
            rlrlllrr.SetChildren(rlrlllrrl, rlrlllrrr);

            Leaf rlrlrllrl = new Leaf(new HashSet<int> {11});
            Node rlrlrllrr = new Node('g');
            rlrlrllr.SetChildren(rlrlrllrl, rlrlrllrr);

            Leaf rlrlrrlll = new Leaf(new HashSet<int> {12});
            Node rlrlrrllr = new Node('d');
            rlrlrrll.SetChildren(rlrlrrlll, rlrlrrllr);

            Leaf rlrlrrlrl = new Leaf(new HashSet<int> {16});
            Leaf rlrlrrlrr = new Leaf(new HashSet<int> {11, 12});
            rlrlrrlr.SetChildren(rlrlrrlrl, rlrlrrlrr);

            Node rlrlrrrll = new Node('i');
            Node rlrlrrrlr = new Node('g');
            rlrlrrrl.SetChildren(rlrlrrrll, rlrlrrrlr);

            Leaf rlrlrrrrl = new Leaf(new HashSet<int> {8, 12});
            Node rlrlrrrrr = new Node('i');
            rlrlrrrr.SetChildren(rlrlrrrrl, rlrlrrrrr);

            Node rlrrrlrrl = new Node('c');
            Leaf rlrrrlrrr = new Leaf(new HashSet<int> {3, 4, 6});
            rlrrrlrr.SetChildren(rlrrrlrrl, rlrrrlrrr);

            Node rrlrrrlrl = new Node('g');
            Leaf rrlrrrlrr = new Leaf(new HashSet<int> {3, 4, 5, 6});
            rrlrrrlr.SetChildren(rrlrrrlrl, rrlrrrlrr);

            // Depth 10
            Node lrrlrllrrl = new Node('g');
            Leaf lrrlrllrrr = new Leaf(new HashSet<int> {3, 4, 6});
            lrrlrllrr.SetChildren(lrrlrllrrl, lrrlrllrrr);

            Leaf lrrlrlrlrl = new Leaf(new HashSet<int> {16});
            Node lrrlrlrlrr = new Node('h');
            lrrlrlrlr.SetChildren(lrrlrlrlrl, lrrlrlrlrr);

            Node lrrlrlrrrl = new Node('g');
            Node lrrlrlrrrr = new Node('i');
            lrrlrlrrr.SetChildren(lrrlrlrrrl, lrrlrlrrrr);

            Leaf lrrlrrrlrl = new Leaf(new HashSet<int> {7, 11});
            Node lrrlrrrlrr = new Node('i');
            lrrlrrrlr.SetChildren(lrrlrrrlrl, lrrlrrrlrr);

            Leaf lrrlrrrrll = new Leaf(new HashSet<int> {7, 11});
            Leaf lrrlrrrrlr = new Leaf(new HashSet<int> {3, 4, 6});
            lrrlrrrrl.SetChildren(lrrlrrrrll, lrrlrrrrlr);

            Node lrrrlrrlrl = new Node('g');
            Leaf lrrrlrrlrr = new Leaf(new HashSet<int> {3, 4, 5, 6});
            lrrrlrrlr.SetChildren(lrrrlrrlrl, lrrrlrrlrr);

            Leaf lrrrrlrlrl = new Leaf(new HashSet<int> {7, 11});
            Leaf lrrrrlrlrr = new Leaf(new HashSet<int> {3, 4, 6});
            lrrrrlrlr.SetChildren(lrrrrlrlrl, lrrrrlrlrr);

            Leaf rlllrrlrrl = new Leaf(new HashSet<int> {7, 8});
            Leaf rlllrrlrrr = new Leaf(new HashSet<int> {3, 4, 5});
            rlllrrlrr.SetChildren(rlllrrlrrl, rlllrrlrrr);

            Leaf rlrlllrrrl = new Leaf(new HashSet<int> {7, 11});
            Leaf rlrlllrrrr = new Leaf(new HashSet<int> {3, 4, 6});
            rlrlllrrr.SetChildren(rlrlllrrrl, rlrlllrrrr);

            Leaf rlrlrllrrl = new Leaf(new HashSet<int> {11});
            Node rlrlrllrrr = new Node('b');
            rlrlrllrr.SetChildren(rlrlrllrrl, rlrlrllrrr);

            Leaf rlrlrrllrl = new Leaf(new HashSet<int> {15});
            Node rlrlrrllrr = new Node('c');
            rlrlrrllr.SetChildren(rlrlrrllrl, rlrlrrllrr);

            Leaf rlrlrrrlll = new Leaf(new HashSet<int> {12});
            Node rlrlrrrllr = new Node('g');
            rlrlrrrll.SetChildren(rlrlrrrlll, rlrlrrrllr);

            Leaf rlrlrrrlrl = new Leaf(new HashSet<int> {11, 12});
            Node rlrlrrrlrr = new Node('b');
            rlrlrrrlr.SetChildren(rlrlrrrlrl, rlrlrrrlrr);

            Node rlrlrrrrrl = new Node('c');
            Leaf rlrlrrrrrr = new Leaf(new HashSet<int> {3, 4, 5, 6});
            rlrlrrrrr.SetChildren(rlrlrrrrrl, rlrlrrrrrr);

            Leaf rlrrrlrrll = new Leaf(new HashSet<int> {7, 11});
            Leaf rlrrrlrrlr = new Leaf(new HashSet<int> {3, 4, 6});
            rlrrrlrrl.SetChildren(rlrrrlrrll, rlrrrlrrlr);

            Leaf rrlrrrlrll = new Leaf(new HashSet<int> {11, 12});
            Node rrlrrrlrlr = new Node('b');
            rrlrrrlrl.SetChildren(rrlrrrlrll, rrlrrrlrlr);

            // Depth 11
            Leaf lrrlrllrrll = new Leaf(new HashSet<int> {11});
            Node lrrlrllrrlr = new Node('b');
            lrrlrllrrl.SetChildren(lrrlrllrrll, lrrlrllrrlr);

            Node lrrlrlrlrrl = new Node('g');
            Leaf lrrlrlrlrrr = new Leaf(new HashSet<int> {8, 10, 12});
            lrrlrlrlrr.SetChildren(lrrlrlrlrrl, lrrlrlrlrrr);

            Leaf lrrlrlrrrll = new Leaf(new HashSet<int> {11, 12});
            Node lrrlrlrrrlr = new Node('b');
            lrrlrlrrrl.SetChildren(lrrlrlrrrll, lrrlrlrrrlr);

            Node lrrlrlrrrrl = new Node('c');
            Leaf lrrlrlrrrrr = new Leaf(new HashSet<int> {3, 4, 5, 6});
            lrrlrlrrrr.SetChildren(lrrlrlrrrrl, lrrlrlrrrrr);

            Node lrrlrrrlrrl = new Node('c');
            Leaf lrrlrrrlrrr = new Leaf(new HashSet<int> {3, 4, 6});
            lrrlrrrlrr.SetChildren(lrrlrrrlrrl, lrrlrrrlrrr);

            Leaf lrrrlrrlrll = new Leaf(new HashSet<int> {11, 12});
            Node lrrrlrrlrlr = new Node('b');
            lrrrlrrlrl.SetChildren(lrrrlrrlrll, lrrrlrrlrlr);

            Leaf rlrlrllrrrl = new Leaf(new HashSet<int> {7, 11});
            Leaf rlrlrllrrrr = new Leaf(new HashSet<int> {3, 4, 6});
            rlrlrllrrr.SetChildren(rlrlrllrrrl, rlrlrllrrrr);

            Leaf rlrlrrllrrl = new Leaf(new HashSet<int> {14, 15});
            Leaf rlrlrrllrrr = new Leaf(new HashSet<int> {9, 11, 12});
            rlrlrrllrr.SetChildren(rlrlrrllrrl, rlrlrrllrrr);

            Leaf rlrlrrrllrl = new Leaf(new HashSet<int> {16});
            Node rlrlrrrllrr = new Node('b');
            rlrlrrrllr.SetChildren(rlrlrrrllrl, rlrlrrrllrr);

            Leaf rlrlrrrlrrl = new Leaf(new HashSet<int> {7, 8, 11, 12});
            Node rlrlrrrlrrr = new Node('i');
            rlrlrrrlrr.SetChildren(rlrlrrrlrrl, rlrlrrrlrrr);

            Leaf rlrlrrrrrll = new Leaf(new HashSet<int> {7, 8, 11, 12});
            Leaf rlrlrrrrrlr = new Leaf(new HashSet<int> {3, 4, 5, 6});
            rlrlrrrrrl.SetChildren(rlrlrrrrrll, rlrlrrrrrlr);

            Leaf rrlrrrlrlrl = new Leaf(new HashSet<int> {7, 8, 11, 12});
            Leaf rrlrrrlrlrr = new Leaf(new HashSet<int> {3, 4, 5, 6});
            rrlrrrlrlr.SetChildren(rrlrrrlrlrl, rrlrrrlrlrr);

            // Depth 12
            Leaf lrrlrllrrlrl = new Leaf(new HashSet<int> {7, 11});
            Leaf lrrlrllrrlrr = new Leaf(new HashSet<int> {3, 4, 6});
            lrrlrllrrlr.SetChildren(lrrlrllrrlrl, lrrlrllrrlrr);

            Leaf lrrlrlrlrrll = new Leaf(new HashSet<int> {16});
            Node lrrlrlrlrrlr = new Node('b');
            lrrlrlrlrrl.SetChildren(lrrlrlrlrrll, lrrlrlrlrrlr);

            Leaf lrrlrlrrrlrl = new Leaf(new HashSet<int> {7, 8, 11, 12});
            Node lrrlrlrrrlrr = new Node('i');
            lrrlrlrrrlr.SetChildren(lrrlrlrrrlrl, lrrlrlrrrlrr);

            Leaf lrrlrlrrrrll = new Leaf(new HashSet<int> {7, 8, 11, 12});
            Leaf lrrlrlrrrrlr = new Leaf(new HashSet<int> {3, 4, 5, 6});
            lrrlrlrrrrl.SetChildren(lrrlrlrrrrll, lrrlrlrrrrlr);

            Leaf lrrlrrrlrrll = new Leaf(new HashSet<int> {7, 11});
            Leaf lrrlrrrlrrlr = new Leaf(new HashSet<int> {3, 4, 6});
            lrrlrrrlrrl.SetChildren(lrrlrrrlrrll, lrrlrrrlrrlr);

            Leaf lrrrlrrlrlrl = new Leaf(new HashSet<int> {7, 8, 11, 12});
            Leaf lrrrlrrlrlrr = new Leaf(new HashSet<int> {3, 4, 5, 6});
            lrrrlrrlrlr.SetChildren(lrrrlrrlrlrl, lrrrlrrlrlrr);

            Leaf rlrlrrrllrrl = new Leaf(new HashSet<int> {13, 16});
            Leaf rlrlrrrllrrr = new Leaf(new HashSet<int> {8, 10, 12});
            rlrlrrrllrr.SetChildren(rlrlrrrllrrl, rlrlrrrllrrr);

            Node rlrlrrrlrrrl = new Node('c');
            Leaf rlrlrrrlrrrr = new Leaf(new HashSet<int> {3, 4, 5, 6});
            rlrlrrrlrrr.SetChildren(rlrlrrrlrrrl, rlrlrrrlrrrr);

            // Depth 13
            Leaf lrrlrlrlrrlrl = new Leaf(new HashSet<int> {13, 16});
            Leaf lrrlrlrlrrlrr = new Leaf(new HashSet<int> {8, 10, 12});
            lrrlrlrlrrlr.SetChildren(lrrlrlrlrrlrl, lrrlrlrlrrlrr);

            Node lrrlrlrrrlrrl = new Node('c');
            Leaf lrrlrlrrrlrrr = new Leaf(new HashSet<int> {3, 4, 5, 6});
            lrrlrlrrrlrr.SetChildren(lrrlrlrrrlrrl, lrrlrlrrrlrrr);

            Leaf rlrlrrrlrrrll = new Leaf(new HashSet<int> {7, 8, 11, 12});
            Leaf rlrlrrrlrrrlr = new Leaf(new HashSet<int> {3, 4, 5, 6});
            rlrlrrrlrrrl.SetChildren(rlrlrrrlrrrll, rlrlrrrlrrrlr);

            // Depth 14
            Leaf lrrlrlrrrlrrll = new Leaf(new HashSet<int> {7, 8, 11, 12});
            Leaf lrrlrlrrrlrrlr = new Leaf(new HashSet<int> {3, 4, 5, 6});
            lrrlrlrrrlrrl.SetChildren(lrrlrlrrrlrrll, lrrlrlrrrlrrlr);


            return tree;
        }
    }
}