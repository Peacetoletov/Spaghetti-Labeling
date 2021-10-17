using System;
using System.Collections.Generic;

namespace Spaghetti_Labeling
{
    // Pre-built trees used for testing
    // Note: never modify existing trees, create a new tree instead.
    public static class TestTrees
    {

        public static Tree Tree1() {
            /*
                   a
                 /   \
                b     b
               / \   / \          
              1   2 1   2
            */
            Node root = new Node('a');
            Tree tree = new Tree(root);

            Node l = new Node('b');
            Node r = new Node('b');
            root.SetChildren(l, r);

            Leaf ll = new Leaf(new HashSet<int> {1});
            Leaf lr = new Leaf(new HashSet<int> {2});
            l.SetChildren(ll, lr);

            Leaf rl = new Leaf(new HashSet<int> {1});
            Leaf rr = new Leaf(new HashSet<int> {2});
            r.SetChildren(rl, rr);

            return tree;
        }

        public static Tree Tree2() {
            /*
                   a
                 /   \
               1,2    1,2
            */
            Node root = new Node('a');
            Tree tree = new Tree(root);

            Leaf l = new Leaf(new HashSet<int> {1, 2});
            Leaf r = new Leaf(new HashSet<int> {1, 2});
            root.SetChildren(l, r);

            return tree;
        }

        public static Tree Tree3() {
            /*
                                    a
                         /                     \
                        b                       b
                   /         \             /         \
                  c           c           c           c 
                /   \       /   \       /   \       /   \ 
              1,2   1,2   1,2   1,2   1,2   1,2   1,2   1,2
            */
            Node root = new Node('a');
            Tree tree = new Tree(root);

            Node l = new Node('b');
            Node r = new Node('b');
            root.SetChildren(l, r);

            Node ll = new Node('c');
            Node lr = new Node('c');
            l.SetChildren(ll, lr);

            Node rl = new Node('c');
            Node rr = new Node('c');
            r.SetChildren(rl, rr);

            Leaf lll = new Leaf(new HashSet<int> {1,2});
            Leaf llr = new Leaf(new HashSet<int> {1,2});
            ll.SetChildren(lll, llr);

            Leaf lrl = new Leaf(new HashSet<int> {1,2});
            Leaf lrr = new Leaf(new HashSet<int> {1,2});
            lr.SetChildren(lrl, lrr);

            Leaf rll = new Leaf(new HashSet<int> {1,2});
            Leaf rlr = new Leaf(new HashSet<int> {1,2});
            rl.SetChildren(rll, rlr);

            Leaf rrl = new Leaf(new HashSet<int> {1,2});
            Leaf rrr = new Leaf(new HashSet<int> {1,2});
            rr.SetChildren(rrl, rrr);

            return tree;
        }

        public static Tree Tree4() {
            /*
                   a
                 /   \
                b     3
               / \         
              1   2
            */
            Node root = new Node('a');
            Tree tree = new Tree(root);

            Node l = new Node('b');
            Leaf r = new Leaf(new HashSet<int> {3});
            root.SetChildren(l, r);

            Leaf ll = new Leaf(new HashSet<int> {1});
            Leaf lr = new Leaf(new HashSet<int> {2});
            l.SetChildren(ll, lr);

            return tree;
        }

        public static Tree Tree5() {
            /*
                    a
                 /     \
                b       c
               / \     / \      
              1   2   3   3
            */

            Node root = new Node('a');
            Tree tree = new Tree(root);

            Node l = new Node('b');
            Node r = new Node('c');
            root.SetChildren(l, r);

            Leaf ll = new Leaf(new HashSet<int> {1});
            Leaf lr = new Leaf(new HashSet<int> {2});
            l.SetChildren(ll, lr);

            Leaf rl = new Leaf(new HashSet<int> {3});
            Leaf rr = new Leaf(new HashSet<int> {3});
            r.SetChildren(rl, rr);

            return tree;
        }

        public static Tree Tree6() {
            /*
                                    o
                         /                     \
                        i                       k
                   /         \             /         \
                  m           g           5           6
                /   \       /   \       
               1     2     3     4      
            */
            
            Node root = new Node('o');
            Tree tree = new Tree(root);

            Node l = new Node('i');
            Node r = new Node('k');
            root.SetChildren(l, r);

            Node ll = new Node('m');
            Node lr = new Node('g');
            l.SetChildren(ll, lr);

            Leaf rl = new Leaf(new HashSet<int> {5});
            Leaf rr = new Leaf(new HashSet<int> {6});
            r.SetChildren(rl, rr);

            Leaf lll = new Leaf(new HashSet<int> {1});
            Leaf llr = new Leaf(new HashSet<int> {2});
            ll.SetChildren(lll, llr);

            Leaf lrl = new Leaf(new HashSet<int> {3});
            Leaf lrr = new Leaf(new HashSet<int> {4});
            lr.SetChildren(lrl, lrr);

            return tree;
        }

        public static Tree Tree7() {
            /*
                                    o
                         /                     \
                        i                       k
                   /         \             /         \
                  1           3           5           6   
            */

            /* This tree was created by reducing Tree6 with either of the following constraints:
            {o=0, i=0, m=0}
            {o=0, i=0, m=1}
            Shifted constraints: {m=0, g=0} */

            Node root = new Node('o');
            Tree tree = new Tree(root);

            Node l = new Node('i');
            Node r = new Node('k');
            root.SetChildren(l, r);

            Leaf ll = new Leaf(new HashSet<int> {1});
            Leaf lr = new Leaf(new HashSet<int> {3});
            l.SetChildren(ll, lr);

            Leaf rl = new Leaf(new HashSet<int> {5});
            Leaf rr = new Leaf(new HashSet<int> {6});
            r.SetChildren(rl, rr);

            return tree;
        }

        public static Tree Tree8() {
            /*
                                    o
                         /                     \
                        i                       k
                   /         \             /         \
                  1           4           5           6
            */

            /* This tree was created by reducing Tree6 with either of the following constraints:
            {o=0, i=1, g=0}
            {o=0, i=1, g=1}
            Shifted constraints: {m=0, g=1} */
            
            Node root = new Node('o');
            Tree tree = new Tree(root);

            Node l = new Node('i');
            Node r = new Node('k');
            root.SetChildren(l, r);

            Leaf ll = new Leaf(new HashSet<int> {1});
            Leaf lr = new Leaf(new HashSet<int> {4});
            l.SetChildren(ll, lr);

            Leaf rl = new Leaf(new HashSet<int> {5});
            Leaf rr = new Leaf(new HashSet<int> {6});
            r.SetChildren(rl, rr);

            return tree;
        }

        public static Tree Tree9() {
            /*
                                    o
                         /                     \
                        2                       k
                                           /         \
                                          5           6
            */

            /* This tree was created by reducing Tree6 with the following constraints:
            {o=1, k=0}
            Shifted constraints: {m=1, i=0} */
            
            Node root = new Node('o');
            Tree tree = new Tree(root);

            Leaf l = new Leaf(new HashSet<int> {2});
            Node r = new Node('k');
            root.SetChildren(l, r);

            Leaf rl = new Leaf(new HashSet<int> {5});
            Leaf rr = new Leaf(new HashSet<int> {6});
            r.SetChildren(rl, rr);

            return tree;
        }

        public static Tree Tree10() {
            /*
                                    o
                         /                     \
                        g                       k
                   /         \             /         \
                  3           4           5           6
            */

            /* This tree was created by reducing Tree6 with the following constraints:
            {o=1, k=1}
            Shifted constraints: {m=1, i=1} */
            
            Node root = new Node('o');
            Tree tree = new Tree(root);

            Node l = new Node('g');
            Node r = new Node('k');
            root.SetChildren(l, r);

            Leaf ll = new Leaf(new HashSet<int> {3});
            Leaf lr = new Leaf(new HashSet<int> {4});
            l.SetChildren(ll, lr);

            Leaf rl = new Leaf(new HashSet<int> {5});
            Leaf rr = new Leaf(new HashSet<int> {6});
            r.SetChildren(rl, rr);

            return tree;
        }

        public static Tree Tree11() {
            /*
                                     o
                             /               \
                           s                  j
                      /         \            / \
                     p           p          p  4,6
                  /     \       / \        / \
                 t       j     6   j      6   k
                / \     / \        / \        / \
               1   2   k   4      k  4,6     6   d
                      / \        / \            / \
                     4   d      6   d         12  4,5,6
                        / \        / \
                      10  4,5    12  4,5,6
            */
            // Tree from page 5 of Spaghetti paper, excluding next tree indices

            Node root = new Node('o');
            Tree tree = new Tree(root);

            
            Node l = new Node('s');
            Node r = new Node('j');
            root.SetChildren(l, r);
            
            Node ll = new Node('p');
            Node lr = new Node('p');
            l.SetChildren(ll, lr);
            
            Node rl = new Node('p');
            Leaf rr = new Leaf(new HashSet<int> {4, 6});
            r.SetChildren(rl, rr);

            Node lll = new Node('t');
            Node llr = new Node('j');
            ll.SetChildren(lll, llr);

            Leaf lrl = new Leaf(new HashSet<int> {6});
            Node lrr = new Node('j');
            lr.SetChildren(lrl, lrr);

            Leaf rll = new Leaf(new HashSet<int> {6});
            Node rlr = new Node('k');
            rl.SetChildren(rll, rlr);

            Leaf llll = new Leaf(new HashSet<int> {1});
            Leaf lllr = new Leaf(new HashSet<int> {2});
            lll.SetChildren(llll, lllr);

            Node llrl = new Node('k');
            Leaf llrr = new Leaf(new HashSet<int> {4});
            llr.SetChildren(llrl, llrr);

            Node lrrl = new Node('k');
            Leaf lrrr = new Leaf(new HashSet<int> {4, 6});
            lrr.SetChildren(lrrl, lrrr);

            Leaf rlrl = new Leaf(new HashSet<int> {6});
            Node rlrr = new Node('d');
            rlr.SetChildren(rlrl, rlrr);

            Leaf llrll = new Leaf(new HashSet<int> {4});
            Node llrlr = new Node('d');
            llrl.SetChildren(llrll, llrlr);

            Leaf lrrll = new Leaf(new HashSet<int> {6});
            Node lrrlr = new Node('d');
            lrrl.SetChildren(lrrll, lrrlr);

            Leaf rlrrl = new Leaf(new HashSet<int> {12});
            Leaf rlrrr = new Leaf(new HashSet<int> {4, 5, 6});
            rlrr.SetChildren(rlrrl, rlrrr);

            Leaf llrlrl = new Leaf(new HashSet<int> {10});
            Leaf llrlrr = new Leaf(new HashSet<int> {4, 5});
            llrlr.SetChildren(llrlrl, llrlrr);

            Leaf lrrlrl = new Leaf(new HashSet<int> {12});
            Leaf lrrlrr = new Leaf(new HashSet<int> {4, 5, 6});
            lrrlr.SetChildren(lrrlrl, lrrlrr);

            return tree;
        }

        public static Tree Tree12() {
            /*
                                    o
                         /                     \
                        i                       i
                   /         \             /         \
                 1-1           g           n          6-6
                             /   \       /   \
                           2-2   3-3   4-4   5-5
            */
            
            // Tree with initial next node indices            
            
            Node root = new Node('o');
            Tree tree = new Tree(root);

            Node l = new Node('i');
            Node r = new Node('i');
            root.SetChildren(l, r);

            Leaf ll = new Leaf(new HashSet<int> {1});
            Node lr = new Node('g');
            l.SetChildren(ll, lr);

            Node rl = new Node('n');
            Leaf rr = new Leaf(new HashSet<int> {6});
            r.SetChildren(rl, rr);

            Leaf lrl = new Leaf(new HashSet<int> {2});
            Leaf lrr = new Leaf(new HashSet<int> {3});
            lr.SetChildren(lrl, lrr);

            Leaf rll = new Leaf(new HashSet<int> {4});
            Leaf rlr = new Leaf(new HashSet<int> {5});
            rl.SetChildren(rll, rlr);

            tree.InitNextTreeIndices();

            return tree;
        }

        public static Tree Tree13() {
            /*
                                    o
                         /                     \
                        i                       i
                   /         \             /         \
                 1-1         2-2          n          6-2   
                                        /   \
                                      4-1   5-1
            */
            
            // 1st tree in a forest of reduced trees created from Tree12 after removing duplicates

            Node root = new Node('o');
            Tree tree = new Tree(root);

            Node l = new Node('i');
            Node r = new Node('i');
            root.SetChildren(l, r);

            Leaf ll = new Leaf(new HashSet<int> {1});
            ll.SetNextTreeIndex(1);
            Leaf lr = new Leaf(new HashSet<int> {2});
            lr.SetNextTreeIndex(2);
            l.SetChildren(ll, lr);

            Node rl = new Node('n');
            Leaf rr = new Leaf(new HashSet<int> {6});
            rr.SetNextTreeIndex(2);
            r.SetChildren(rl, rr);

            Leaf rll = new Leaf(new HashSet<int> {4});
            rll.SetNextTreeIndex(1);
            Leaf rlr = new Leaf(new HashSet<int> {5});
            rlr.SetNextTreeIndex(1);
            rl.SetChildren(rll, rlr);

            return tree;
        }

        public static Tree Tree14() {
            /*
                                    o
                         /                     \
                        i                       i
                   /         \             /         \
                 1-1         3-2          n          6-2      
                                        /   \
                                      4-1   5-1
            */

            // 2nd and final tree in a forest of reduced trees created from Tree12 after removing duplicates
            
            Node root = new Node('o');
            Tree tree = new Tree(root);

            Node l = new Node('i');
            Node r = new Node('i');
            root.SetChildren(l, r);

            Leaf ll = new Leaf(new HashSet<int> {1});
            ll.SetNextTreeIndex(1);
            Leaf lr = new Leaf(new HashSet<int> {3});
            lr.SetNextTreeIndex(2);
            l.SetChildren(ll, lr);

            Node rl = new Node('n');
            Leaf rr = new Leaf(new HashSet<int> {6});
            rr.SetNextTreeIndex(2);
            r.SetChildren(rl, rr);

            Leaf rll = new Leaf(new HashSet<int> {4});
            rll.SetNextTreeIndex(1);
            Leaf rlr = new Leaf(new HashSet<int> {5});
            rlr.SetNextTreeIndex(1);
            rl.SetChildren(rll, rlr);

            return tree;
        }

        public static Tree TreeLeaf1() {
            /*
                1,2,3
            */
            Leaf root = new Leaf(new HashSet<int> {1, 2, 3});
            return new Tree(root);
        }

        public static Tree TreeLeaf2() {
            /*
                2,4
            */
            Leaf root = new Leaf(new HashSet<int> {2, 4});
            return new Tree(root);
        }

        public static Tree TreeLeaf3() {
            /*
                1,2
            */
            Leaf root = new Leaf(new HashSet<int> {1, 2});
            return new Tree(root);
        }
    }
}