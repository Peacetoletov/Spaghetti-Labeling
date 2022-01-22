using System;
using System.Diagnostics;
using System.Collections.Generic;


namespace Spaghetti_Labeling
{
    // Class for creating and managing DRAGs (directed rooted acyclic graphs)
    // Each forest (main, end even, end odd) is converted separately, resulting in 3 disjoint DRAGs
    public class GraphManager
    {
        private MainGraph mainGraph;
        int startTreeIndex;
        private Graph endGraphEven;
        private Graph endGraphOdd;

        public GraphManager() {
            // Create forests
            (List<Tree> mainForest, int startTreeIndex) = ForestCreator.MainForest(ODTree.GetTree);
            this.startTreeIndex = startTreeIndex;
            (List<Tree> endForestEvenTrees, List<List<int>> endForestEvenIndices) = SplitList(ForestCreator.EndForest(mainForest, true));
            (List<Tree> endForestOddTrees, List<List<int>> endForestOddIndices) = SplitList(ForestCreator.EndForest(mainForest, false));

            // Shift all tree indices such that they start from 0 instead of 1
            DecrementList(endForestEvenIndices);
            DecrementList(endForestOddIndices);

            // Create graphs
            this.mainGraph = new MainGraph(mainForest, endForestEvenIndices, endForestOddIndices);
            this.endGraphEven = new Graph(endForestEvenTrees);
            this.endGraphOdd = new Graph(endForestOddTrees);
        }

        private (List<Tree>, List<List<int>>) SplitList(List<(Tree, List<int>)> endForest) {
            List<Tree> trees = new List<Tree>();
            List<List<int>> indicesList = new List<List<int>>();
            foreach ((Tree tree, List<int> indices) in endForest) {
                trees.Add(tree);
                indicesList.Add(indices);
            }
            return (trees, indicesList);
        }

        private void DecrementList(List<List<int>> listOfLists) {
            foreach (List<int> list in listOfLists) {
                for (int i = 0; i < list.Count; i++) {
                    list[i]--;
                }
            }
        }

        public int GetStartTreeIndex() {
            return startTreeIndex;
        }

        public AbstractNode AdjustIndexAndGetMainGraphRoot(int index) {
            return mainGraph.GetRoot(index - 1);
        }

        public AbstractNode GetEndGraphEvenRoot(int index) {
            return endGraphEven.GetRoot(mainGraph.GetEndTreeEvenIndex(index));
        }

        public AbstractNode GetEndGraphOddRoot(int index) {
            return endGraphOdd.GetRoot(mainGraph.GetEndTreeOddIndex(index));
        }

        public static class Tests 
        {
            public static void Run() {

            }
        }    
    }
}