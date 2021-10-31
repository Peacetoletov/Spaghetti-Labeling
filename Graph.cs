using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Spaghetti_Labeling
{
    public class Graph
    {
        private List<AbstractNode> roots;


        public Graph(List<Tree> forest) {
            
            /*
            Each node in a tree has one parent at most. When converting a forest into a graph, each node
            may have an arbitrary number of parents. The Node class doesn't support multiple parents but
            this is not an issue, as the parent property isn't necessary to precisely describe a graph 
            (information about children is sufficient). 
            Because of this, I can simply ignore the parent property, as it is only useful for testing
            trees anyway.
            */

            // TODO: merge equal subtrees among different trees of the same forest, then manually test it using
            // the improved InfoDFS method and automatically test it using tree equality methods with the addition
            // of testing if the merged subtrees are the same object in memory.

            /*
            Go through all node pairs (For each node of each tree, go thorugh all nodes of all trees) and
            check equality (NOT equivalence!). If the given subtrees are equal, replace the whole second
            subtree with the first subtree.
            This will be easier to implement if instead of replacing the current node, its children are replaced
            instead, as it will be clear if left subtree or right subtree should be replaced. This can be done 
            because the root node will never be replaced.
            */

            MergeEqualSubtrees(forest);
        }

        public void MergeEqualSubtrees(List<Tree> forest) {
            List<AbstractNode> nodes = ForestToNodes(forest);
            for (int i = 0; i < nodes.Count - 1; i++) {
                for (int j = i + 1; j < nodes.Count; j++) {

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
                Console.WriteLine("Tree has " + curTreeNodes.Count + " nodes.");
            }
            Console.WriteLine(nodes.Count + " nodes in total.");
            forest[10].GetRoot().InfoDFS();
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

            }
        }
    }
}