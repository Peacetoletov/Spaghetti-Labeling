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
            Because of this, I can simply ignore the parent property.
            */

            // TODO: create methods IsEquivalent and TryToReplaceByEquivalent in AbstractNode
            // TODO: test both methods before implementing more methods in this class

            /*
            Go through all node pairs (For each node of each tree, go thorugh all nodes of all trees) and
            check equivalences. If the given subtrees are equivalent, modify the leaves of the first subtree
            by replacing actions with the intersection of both trees' actions, then replace the whole second
            subtree with the first subtree.
            This will be easier to implement if instead of replacing the current node, its children are replaced
            instead, as it will be clear if left subtree or right subtree should be replaced. This can be done 
            because the root node will never be replaced.
            */

        }


        public static class Tests
        {
            public static void Run() {

            }
        }
    }
}