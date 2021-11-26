using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Spaghetti_Labeling
{
    public class MainGraph : Graph
    {
        private List<int> endEvenIndices;
        private List<int> endOddIndices;


        public MainGraph() {
            // Empty constructor, only used for testing
        }

        public MainGraph(List<Tree> forest, List<List<int>> endForestEvenIndices, 
                         List<List<int>> endForestOddIndices) : base(forest) {
            // Initialize lists for end trees, with length equal to the number of roots in this graph
            endEvenIndices = new List<int>(new int[forest.Count + 1]);
            endOddIndices = new List<int>(new int[forest.Count + 1]);

            // Fill the lists with end indices
            SetIndicesOfEndGraphRoots(endForestEvenIndices, true);
            SetIndicesOfEndGraphRoots(endForestOddIndices, false);
        }

        private void SetIndicesOfEndGraphRoots(List<List<int>> endForestIndices, bool isEven) {
            // Assigns the indices of roots of trees in an end graph.
            /* The index of an element in endForestIndices is the index of the end tree, and the
            list in endForestIndices[i] contains indices of main trees that would normally be used,
            but in case that the end column of the image is reached, this end tree will be used instead.
            */
            for (int endTreeIndex = 0; endTreeIndex < endForestIndices.Count; endTreeIndex++) {
                foreach (int mainTreeIndex in endForestIndices[endTreeIndex]) {
                    if (isEven) {
                        endEvenIndices[mainTreeIndex] = endTreeIndex + 1;
                    } else {
                        endOddIndices[mainTreeIndex] = endTreeIndex + 1;
                    }
                }
            }

            // Manual testing
            /*
            for (int i = 0; i < endEvenIndices.Count; i++) {
                Console.WriteLine("Main tree {0} has even end tree {1}", i, endEvenIndices[i]);
            }
            for (int i = 0; i < endOddIndices.Count; i++) {
                Console.WriteLine("Main tree {0} has even end tree {1}", i, endOddIndices[i]);
            }
            */
        }
    }
}