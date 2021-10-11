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

        // TODO: add another post-reduction tree, identical to the one in the paper on page 5

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