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

        public enum GraphType {
            FirstRow,
            MiddleRows,
            LastRow
        }

        public GraphManager(GraphType type) {
            if (type == GraphType.FirstRow) {
                Construct(ForestCreator.MainForestFirstRow, ForestCreator.EndForestEvenFirstRow,
                          ForestCreator.EndForestOddFirstRow);
            }
            else if (type == GraphType.MiddleRows) {
                Construct(ForestCreator.MainForestMiddleRows, ForestCreator.EndForestEvenMiddleRows, 
                          ForestCreator.EndForestOddMiddleRows);
            }
            else {
                Construct(ForestCreator.MainForestLastRow, ForestCreator.EndForestEvenLastRow, 
                          ForestCreator.EndForestOddLastRow);
            }
        }

        public void Construct(Func<Tree, (List<Tree>, int)> CreateMainForest,
                              Func<Tree, List<(Tree, List<int>)>> CreateEndForestEven,
                              Func<Tree, List<(Tree, List<int>)>> CreateEndForestOdd) {
            (List<Tree> mainForest, int startTreeIndex) = CreateMainForest(ODTree.GetTree());
            this.startTreeIndex = startTreeIndex;
            (List<Tree> endForestEvenTrees, List<List<int>> endForestEvenIndices) = SplitListOfPairs(CreateEndForestEven(ODTree.GetTree()));
            (List<Tree> endForestOddTrees, List<List<int>> endForestOddIndices) = SplitListOfPairs(CreateEndForestOdd(ODTree.GetTree()));

            // Shift all tree indices such that they start from 0 instead of 1
            DecrementList(endForestEvenIndices);
            DecrementList(endForestOddIndices);            

            // Create graphs
            this.mainGraph = new MainGraph(mainForest, endForestEvenIndices, endForestOddIndices);
            this.endGraphEven = new Graph(endForestEvenTrees);
            this.endGraphOdd = new Graph(endForestOddTrees);
        }

        private (List<Tree>, List<List<int>>) SplitListOfPairs(List<(Tree, List<int>)> endForest) {
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

        public AbstractNode AdjustIndexAndGetRootInMainGraph(int index) {
            return mainGraph.GetRoot(index - 1);
        }

        public AbstractNode AdjustIndexAndGetRootInEndGraphEven(int index) {
            return endGraphEven.GetRoot(mainGraph.GetEndTreeEvenIndex(index - 1));
        }

        public AbstractNode AdjustIndexAndGetRootInEndGraphOdd(int index) {
            return endGraphOdd.GetRoot(mainGraph.GetEndTreeOddIndex(index - 1));
        }

        public static class Tests 
        {
            public static void Run() {
                // Cosmetic tests LUL
            }
        }    
    }
}