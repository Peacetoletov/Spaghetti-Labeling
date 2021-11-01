using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Spaghetti_Labeling
{
    public class Graph
    {
        private List<AbstractNode> roots;


        public Graph() {
            // Empty constructor, only used for testing
        }

        public Graph(List<Tree> forest) {
            
            /*
            Each node in a tree has one parent at most. When converting a forest into a graph, each node
            may have an arbitrary number of parents. The Node class doesn't support multiple parents but
            this is not an issue, as the parent property isn't necessary to precisely describe a graph 
            (information about children is sufficient). 
            Because of this, I can simply ignore the parent property, as it is only useful for testing
            trees anyway.
            */

            /*
            Go through all node pairs (For each node of each tree, go thorugh all nodes of all trees) and
            check equality (NOT equivalence!). If the given subtrees are equal, replace the whole second
            subtree with the first subtree.
            This will be easier to implement if instead of replacing the current node, its children are replaced
            instead, as it will be clear if left subtree or right subtree should be replaced. This can be done 
            because the root node will never be replaced.
            */

            // Store the root of each tree
            this.roots = new List<AbstractNode>();
            foreach (Tree tree in forest) {
                this.roots.Add(tree.GetRoot());
            }

            // Merge equal subtrees
            MergeEqualSubtrees(forest);
        }

        public void MergeEqualSubtrees(List<Tree> forest) {
            /* For each pair of inner nodes, check if their left (right) subtrees are equal. If so, replace 
            the left (right) subtree of the second node with the left (right) subtree of the first node.
            */
            // TODO: test this both manually and automatically
            List<AbstractNode> nodes = ForestToNodes(forest);
            for (int i = 0; i < nodes.Count - 1; i++) {
                for (int j = i + 1; j < nodes.Count; j++) {
                    if (nodes[i] is Leaf || nodes[j] is Leaf) {
                        // Skip leaves
                        continue;
                    }
                    Node node1 = (Node) nodes[i];
                    Node node2 = (Node) nodes[j];
                    if (node1.GetLeft().IsEqual(node2.GetLeft())) {
                        //Console.WriteLine("Left subtree of node " + i + " is equal to the left subtree of node " + j);
                        node2.SetLeft(node1.GetLeft());
                    }
                    if (node1.GetRight().IsEqual(node2.GetRight())) {
                        node2.SetRight(node1.GetRight());
                    }
                }
            }
        }

        private List<AbstractNode> ForestToNodes(List<Tree> forest) {
            // Returns a list containing each node of each tree in a given forest
            List<AbstractNode> nodes = new List<AbstractNode>();
            foreach (Tree tree in forest) {
                List<AbstractNode> curTreeNodes = new List<AbstractNode>();
                AddNodesOfSubtreeToList(tree.GetRoot(), curTreeNodes);
                nodes.AddRange(curTreeNodes);
                //Console.WriteLine("Tree has " + curTreeNodes.Count + " nodes.");
            }
            //Console.WriteLine(nodes.Count + " nodes in total.");
            //forest[10].GetRoot().InfoDFS();
            return nodes;
        }

        private void AddNodesOfSubtreeToList(AbstractNode abstractNode, List<AbstractNode> abstractNodes) {
            abstractNodes.Add(abstractNode);
            if (abstractNode is Node) {
                Node node = (Node) abstractNode;
                AddNodesOfSubtreeToList(node.GetLeft(), abstractNodes);
                AddNodesOfSubtreeToList(node.GetRight(), abstractNodes);
            }
        }


        public static class Tests
        {
            public static void Run() {
                TestEqualSubtreeMerging();
            }

            private static void TestEqualSubtreeMerging() {
                Graph g = new Graph();
                Tree tree13 = TestTrees.Tree13();
                /* Tree 13:
                                    o
                         /                     \
                        i                       i
                   /         \             /         \
                 1-1         2-2          n          6-2   
                                        /   \
                                      4-1   5-1
                */
                Tree tree14 = TestTrees.Tree14();
                /* Tree 14:
                                    o
                         /                     \
                        i                       i
                   /         \             /         \
                 1-1         3-2          n          6-2      
                                        /   \
                                      4-1   5-1
                */
                List<Tree> forest = new List<Tree> {tree13, tree14};
                g.MergeEqualSubtrees(forest);

                Debug.Assert(!tree13.IsEqual(tree14));
                AbstractNode l13 = ((Node) tree13.GetRoot()).GetLeft();
                AbstractNode l14 = ((Node) tree14.GetRoot()).GetLeft();
                Debug.Assert(!l13.IsEqual(l14));
                AbstractNode ll13 = ((Node) l13).GetLeft();
                AbstractNode ll14 = ((Node) l14).GetLeft();
                Debug.Assert(ll13 == ll14);
                AbstractNode r13 = ((Node) tree13.GetRoot()).GetRight();
                AbstractNode r14 = ((Node) tree14.GetRoot()).GetRight();
                Debug.Assert(r13 == r14);
                //Console.WriteLine(":)");
            }
        }
    }
}