using System;
using System.Diagnostics;
using System.Collections.Generic;


namespace Spaghetti_Labeling
{
    // Class for creating and managing DRAGs (directed rooted acyclic graphs)
    // Each forest (main, end even, end odd) is converted separately, resulting in 3 disjoint DRAGs
    public class GraphManager
    {
        private Graph mainGraph;
        private Graph endGraphEven;
        private Graph endGraphOdd;

        public GraphManager() {
            List<Tree> mainForest = ForestCreator.MainForest(ODTree.GetTree);
            this.mainGraph = new Graph(mainForest);
            
            List<(Tree, List<int>)> endForestEvenWithMainTreeIndices = ForestCreator.EndForest(mainForest, true);
            List<Tree> endForestEven = GetEndForest(endForestEvenWithMainTreeIndices);
            this.endGraphEven = new Graph(endForestEven);

            List<(Tree, List<int>)> endForestOddWithMainTreeIndices = ForestCreator.EndForest(mainForest, false);
            List<Tree> endForestOdd = GetEndForest(endForestOddWithMainTreeIndices);
            this.endGraphOdd = new Graph(endForestOdd);

            // TODO: I will need to store the indices somewhere (maybe in this class, maybe in each tree... still unsure)
        }

        private List<Tree> GetEndForest(List<(Tree, List<int>)> endForestEvenWithMainTreeIndices) {
            List<Tree> endForest = new List<Tree>();
            foreach((Tree tree, List<int> indices) in endForestEvenWithMainTreeIndices) {
                endForest.Add(tree);
            }
            return endForest;
        }

        public static class Tests 
        {
            public static void Run() {

            }
        }    
    }
}