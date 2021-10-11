using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Spaghetti_Labeling
{
    // Class for converting the ODTree into a forest of reduced trees
    public class ForestManager
    {
        
        public ForestManager() {

        }

        public List<Tree> CreateForestOfReducedTrees(Tree tree=null) {
            // TODO: implement this method, then add another test
            if (tree == null) {
                tree = new ODTree().GetTree();
            }

            List<HashSet<(char, bool)>> constraints = new List<HashSet<(char, bool)>>();
            GatherConstraints(tree.GetRoot(), new HashSet<(char, bool)>(), out constraints);

            return null;
        }

        private void GatherConstraints(AbstractNode node, HashSet<(char, bool)> constraints,
                                       out List<HashSet<(char, bool)>> constraintsList) {
            // Recursively traverse the tree and gather constraints on the way down, then append them to
            // a list of constraints upon reaching a leaf

            // TODO continue here
            if (node is Node) {
                
            }

            return null;
        } 

        private Tree ReduceTree() {
            return null;
        }

        public static class ForestManagerTests 
        {
            public static void Run() {
                // TODO: add a test with the tree from the paper on apge 5

                ForestManager fm = new ForestManager();
                Tree toReduce = TestTrees.Tree6();
                List<Tree> forest = fm.CreateForestOfReducedTrees(toReduce);
                Tree tree7 = TestTrees.Tree7();
                Tree tree8 = TestTrees.Tree8();
                Tree tree9 = TestTrees.Tree9();
                Tree tree10 = TestTrees.Tree10();

                Debug.Assert(forest.Count == 6);
                Debug.Assert(forest[0].Equals(tree7));
                Debug.Assert(forest[1].Equals(tree7));
                Debug.Assert(forest[2].Equals(tree8));
                Debug.Assert(forest[3].Equals(tree8));
                Debug.Assert(forest[4].Equals(tree9));
                Debug.Assert(forest[5].Equals(tree10));
            }
        }        
    }
}