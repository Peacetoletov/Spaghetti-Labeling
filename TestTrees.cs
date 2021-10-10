using System;
using System.Collections.Generic;

namespace Spaghetti_Labeling
{
    // Pre-built trees used for testing
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

        public static Tree TreeLeaf1() {
            /*
                {1, 2, 3}
            */
            Leaf root = new Leaf(new HashSet<int> {1, 2, 3});
            return new Tree(root);
        }

        public static Tree TreeLeaf2() {
            /*
                {2, 4}
            */
            Leaf root = new Leaf(new HashSet<int> {2, 4});
            return new Tree(root);
        }
    }
}