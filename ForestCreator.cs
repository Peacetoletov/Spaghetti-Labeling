using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Spaghetti_Labeling
{
    // Class for converting the ODTree into a forest of reduced trees
    public static class ForestCreator
    {
        // Observations: Main middle rows forest is reduced to just 13 trees. End forest even also constains 13 trees, 
        // end forest odd is further reduced to 9 trees. Odd trees also have a very small number of nodes (17 at most).

        public static (List<Tree>, int) MainForestMiddleRows(Tree seedTree) {
            // Returns a forest for labeling middle rows, alongside the index of the start tree
            return MainForest(seedTree);
        }

        public static (List<Tree>, int) MainForestFirstRow(Tree seedTree) {
            // Returns a forest for labeling the first row, alongside the index of the start tree
            ReduceTree(seedTree, FirstRowConstraints());
            seedTree.GetRoot().MergeIdenticalBranches();
            return MainForest(seedTree);     
        }

        public static (List<Tree>, int) MainForestLastRow(Tree seedTree) {
            // Returns a forest for labeling the last row in an image with an odd number of rows,
            // alongside the index of the start tree
            ReduceTree(seedTree, LastRowConstraints());
            seedTree.GetRoot().MergeIdenticalBranches();
            return MainForest(seedTree);
        }

        private static (List<Tree>, int) MainForest(Tree seedTree) {
            // Creates a forest of reduced trees with merged identical branches, removes duplicate trees
            // and assigns an index of the next tree to each leaf of each tree.
            List<HashSet<(char, bool)>> constraintsList = MainTreesConstraints(seedTree);
            List<Tree> forest = CreateForestOfReducedTrees(seedTree, constraintsList);
            int startTreeIndex = forest.Count;
            startTreeIndex = RemoveDuplicateMainTrees(forest, startTreeIndex);       
            return (forest, startTreeIndex);
        }

        public static List<(Tree, List<int>)> EndForestEvenMiddleRows(Tree seedTree) {
            // Returns a list of even end trees together with indices of all main trees that each 
            // end tree is associated with (these indices start at 1).
            (List<Tree> mainForest, int _) = MainForestMiddleRows(seedTree);
            return EndForest(mainForest, EndEvenConstraints());
        }

        public static List<(Tree, List<int>)> EndForestOddMiddleRows(Tree seedTree) {
            // Returns a list of odd end trees together with indices of all main trees that each 
            // end tree is associated with (these indices start at 1).
            (List<Tree> mainForest, int _) = MainForestMiddleRows(seedTree);
            return EndForest(mainForest, EndOddConstraints());
        }

        public static List<(Tree, List<int>)> EndForestEvenFirstRow(Tree seedTree) {
            // Returns a list of even end trees together with indices of all main trees that each 
            // end tree is associated with (these indices start at 1).
            (List<Tree> mainForest, int _) = MainForestFirstRow(seedTree);
            return EndForest(mainForest, EndEvenConstraints());
        }

        public static List<(Tree, List<int>)> EndForestOddFirstRow(Tree seedTree) {
            // Returns a list of odd end trees together with indices of all main trees that each 
            // end tree is associated with (these indices start at 1).
            (List<Tree> mainForest, int _) = MainForestFirstRow(seedTree);
            return EndForest(mainForest, EndOddConstraints());
        }

        public static List<(Tree, List<int>)> EndForestEvenLastRow(Tree seedTree) {
            // Returns a list of even end trees together with indices of all main trees that each 
            // end tree is associated with (these indices start at 1).
            (List<Tree> mainForest, int _) = MainForestLastRow(seedTree);
            return EndForest(mainForest, EndEvenConstraints());
        }

        public static List<(Tree, List<int>)> EndForestOddLastRow(Tree seedTree) {
            // Returns a list of odd end trees together with indices of all main trees that each 
            // end tree is associated with (these indices start at 1).
            (List<Tree> mainForest, int _) = MainForestLastRow(seedTree);
            return EndForest(mainForest, EndOddConstraints());
        }

        private static List<(Tree, List<int>)> EndForest(List<Tree> mainForest, HashSet<(char, bool)> constraints) {
            /*
            1) Copy main forest.
            2) Perform further reduction and branch merging and put all newly reduced trees into 
               a list together with a list containing the index of the associated main tree.
            3) Delete duplicate trees, add the deleted tree's index to the list of the other 
               tree (the one that is equal but wasn't deleted).
            */
            List<Tree> endForest = copyForest(mainForest);
            foreach (Tree tree in endForest) {
                ReduceTree(tree, constraints);
            }

            /* Each end tree is associated with a main tree. Whenever an end tree is removed due to
               being a duplicate, the other tree's list of associated main trees must be updated.
               This list stores the end trees with their assocaited main trees. 
            */
            List<(Tree, List<int>)> endTreesWithMainTreeIndices = new List<(Tree, List<int>)>();
            for (int i = 0; i < endForest.Count; i++) {
                endTreesWithMainTreeIndices.Add((endForest[i], new List<int> {i + 1}));
            }
            RemoveDuplicateEndTrees(endTreesWithMainTreeIndices);

            return endTreesWithMainTreeIndices;
        }

        private static int GetStartTreeIndex(Tree startTree, List<Tree> forest) {
            for (int i = 0; i < forest.Count; i++) {
                if (startTree == forest[i]) {
                    //Console.WriteLine("Start tree has index {0}", i);
                    // Start tree of main forest middle has index 12 (which is the last index, as expected)
                    // 1 is added to the index to adjust for the fact that in tree leaves, next tree indices start at 1,
                    // therefore 13 is returned
                    return i + 1;
                }
            }
            throw new NotSupportedException("Critical error: Start tree not found.");
        }

        private static HashSet<(char, bool)> EndEvenConstraints() {
            return new HashSet<(char, bool)> {
                ('e', false), ('f', false), ('k', false), ('l', false),
            };
        }

        private static HashSet<(char, bool)> EndOddConstraints() {
            return new HashSet<(char, bool)> {
                ('e', false), ('f', false), ('k', false), ('l', false),
                ('d', false), ('j', false), ('p', false), ('t', false),
            };
        }
        
        private static List<Tree> copyForest(List<Tree> forest) {
            List<Tree> copy = new List<Tree>();
            foreach (Tree tree in forest) {
                copy.Add(new Tree(tree));
            }
            return copy;
        }

        private static List<HashSet<(char, bool)>> MainTreesConstraints(Tree seedTree) {
            List<HashSet<(char, bool)>> constraintsList = new List<HashSet<(char, bool)>>();
            GatherConstraints(seedTree.GetRoot(), new HashSet<(char, bool)>(), constraintsList);
            constraintsList.Add(RowBeginningConstraints());
            return constraintsList;
        }

        private static HashSet<(char, bool)> RowBeginningConstraints() {
            return new HashSet<(char, bool)> {
                ('a', false), ('b', false), ('g', false), ('h', false),
                ('m', false), ('n', false), ('q', false), ('r', false)
            };
        }   

        private static HashSet<(char, bool)> FirstRowConstraints() {
            return new HashSet<(char, bool)> {
                ('a', false), ('b', false), ('c', false), ('d', false),
                ('e', false), ('f', false), ('g', false), ('h', false),
                ('i', false), ('j', false), ('k', false), ('l', false)
            };
        }

        private static HashSet<(char, bool)> LastRowConstraints() {
            return new HashSet<(char, bool)> {
                ('q', false), ('r', false), ('s', false), ('t', false)
            };
        }

        private static Tree InitIndicesAndReduce(Tree seedTree, HashSet<(char, bool)> constraints) {
            Tree tree = new Tree(seedTree);
            tree.InitNextTreeIndices();
            ReduceTree(tree, constraints);
            return tree;
        }

        private static void MergeIdenticalBranches(List<Tree> forest) {
            foreach (Tree tree in forest) {
                tree.GetRoot().MergeIdenticalBranches();
            }
        }

        private static int RemoveDuplicateMainTrees(List<Tree> forest, int startTreeIndex) {
            // Removes duplicate trees from a forest of main trees and returns the (possibly modified) index of the start tree.
            // Note that startTreeIndex starts at 1 as opposed to 0, so it must be first normalized.
            int startTreeIndexNormalized = startTreeIndex - 1;
        
            for (int i = 0; i < forest.Count - 1; i++) {
                for (int j = i + 1; j < forest.Count; j++) {
                    if (forest[i].IsEqual(forest[j])) {
                        // Adjust next tree indices in tree leaves if needed
                        foreach (Tree tree in forest) {
                            tree.GetRoot().AdjustNextTreeIndicesAfterDeletion(i + 1, j + 1);
                            // Arguments need to be incremented by 1 because these nested loops start indexing from 0,
                            // but next tree indices in leaves start indexing from 1 (to stay consistent with the Spaghetti paper)
                        }
                        // Adjust startTreeIndexNormalized if needed
                        if (startTreeIndexNormalized > j) {
                            // start tree index is higher than the deleted tree's index, must be decremented
                            startTreeIndexNormalized--;
                        } else if (startTreeIndexNormalized == j) {
                            // start tree index is equal to the deleted tree's index, meaning that the forest[i] tree is now
                            // the start tree
                            startTreeIndexNormalized = i;
                        }
                        // do nothing with the start tree index if startTreeIndexNormalized < j

                        // Remove the duplicate tree
                        forest.RemoveAt(j);
                        j--;        // Decrement j to counterbalance removing a tree, otherwise one tree would be skipped
                    }
                }
            }
            return startTreeIndexNormalized + 1;        // Add 1 to remove the effect of normalizing
        }

        private static void RemoveDuplicateEndTrees(List<(Tree, List<int>)> endTreesWithMainTreeIndices) {
            for (int i = 0; i < endTreesWithMainTreeIndices.Count - 1; i++) {
                Tree tree1 = endTreesWithMainTreeIndices[i].Item1;
                for (int j = i + 1; j < endTreesWithMainTreeIndices.Count; j++) {
                    Tree tree2 = endTreesWithMainTreeIndices[j].Item1;
                    if (tree1.IsEqual(tree2)) {
                        // Add the duplicate tree's index to the other tree's indices
                        endTreesWithMainTreeIndices[i].Item2.Add(endTreesWithMainTreeIndices[j].Item2[0]);

                        // Delete the duplicate 
                        endTreesWithMainTreeIndices.RemoveAt(j);
                        j--;        // Decrement j to counterbalance removing a tree, otherwise one tree would be skipped
                    }
                }
            }
        }

        private static List<Tree> CreateForestOfReducedTrees(Tree seedTree, 
                                                             List<HashSet<(char, bool)>> constraintsList) {
            // Creates a reduced tree for each leaf of the ODTree 
            List<Tree> forest = new List<Tree>();
            foreach (HashSet<(char, bool)> constraints in constraintsList) {
                forest.Add(InitIndicesAndReduce(seedTree, constraints));
            }
            return forest;
        }


        private static void GatherConstraints(AbstractNode abstractNode, HashSet<(char, bool)> constraints,
                                       List<HashSet<(char, bool)>> constraintsList) {
            // Recursively traverse the tree and gather constraints on the way down, then append them to
            // a list of constraints upon reaching a leaf
            if (abstractNode is Node) {
                GatherConstraints((Node) abstractNode, constraints, constraintsList);
            } else {
                GatherConstraints(constraints, constraintsList);
            }
        } 

        private static void GatherConstraints(Node node, HashSet<(char, bool)> constraints,
                                       List<HashSet<(char, bool)>> constraintsList) {
            // Recursively traverse the tree and gather constraints on the way down
            char condition = node.GetCondition();

            // The original constraints set is passed to the left subtree, a copy is passed to the right subtree
            HashSet<(char, bool)> constraintsCopy = new HashSet<(char, bool)>(constraints);
            char conditionShifted = ShiftCondition(condition);
            if (conditionShifted != condition) {
                constraints.Add((conditionShifted, false));
                constraintsCopy.Add((conditionShifted, true));
            }
            GatherConstraints(node.GetLeft(), constraints, constraintsList);
            GatherConstraints(node.GetRight(), constraintsCopy, constraintsList);
        }

        private static void GatherConstraints(HashSet<(char, bool)> constraints,
                                              List<HashSet<(char, bool)>> constraintsList) {
            // Append constraints to a list of constraints upon reaching a leaf
            constraintsList.Add(constraints);
        }

        private static char ShiftCondition(char condition) {
            // Returns the shifted condition if applicable, return the same condition otherwise
            List<int> unshiftable = new List<int> {'a', 'b', 'g', 'h', 'm', 'n', 'q', 'r'};
            if (unshiftable.Contains(condition)) {
                return condition;
            }
            return (char) (condition - 2);
        }

        private static void ReduceTree(Tree tree, HashSet<(char, bool)> constraints) {
            // Reduces a given tree
            ReduceSubtree(tree.GetRoot(), constraints);
        }

        private static void ReduceSubtree(AbstractNode abstractNode, HashSet<(char, bool)> constraints) {
            if (abstractNode is Node) {
                Node node = (Node) abstractNode;
                ReduceSubtree(node.GetLeft(), constraints);
                ReduceSubtree(node.GetRight(), constraints);

                char condition = node.GetCondition();
                if (constraints.Contains((condition, false))) {
                    node.ReplaceByLeft();
                } else if (constraints.Contains((condition, true))) {
                    node.ReplaceByRight();
                }
            }            
        }

        public static class Tests 
        {
            public static void Run() {
                TestCreateForestOfReducedTrees();
                TestReduceTreeAndMergeBranches();
                TestRemoveDuplicateMainTrees();
                TestRangeOfNextTreeIndicesInReducedTrees();

                // The creation of end forests *seems* to be working correctly. As testing it is hard to automate, I did a couple of manual tests instead.
            }

            private static void TestReduceTreeAndMergeBranches() {
                HashSet<(char, bool)> constraints = new HashSet<(char, bool)> 
                    {('h', false), ('n', true), ('i', true)};
                Tree reduced = ODTree.GetTree();
                ReduceTree(reduced, constraints);
                List<Tree> tmp = new List<Tree> {reduced};
                MergeIdenticalBranches(tmp);
                reduced = tmp[0];

                Tree reference =  TestTrees.Tree11();
                Debug.Assert(reduced.IsEqualIgnoringLeafIndices(reference));
            }

            private static void TestCreateForestOfReducedTrees() {
                List<HashSet<(char, bool)>> constraintsList = new List<HashSet<(char, bool)>>();
                GatherConstraints(TestTrees.Tree6().GetRoot(), new HashSet<(char, bool)>(), constraintsList);
                List<Tree> forest = CreateForestOfReducedTrees(TestTrees.Tree6(), constraintsList);
                Tree tree7 = TestTrees.Tree7();
                Tree tree8 = TestTrees.Tree8();
                Tree tree9 = TestTrees.Tree9();
                Tree tree10 = TestTrees.Tree10();
                
                Debug.Assert(forest.Count == 6);
                Debug.Assert(forest[0].IsEqualIgnoringLeafIndices(tree7));
                Debug.Assert(forest[1].IsEqualIgnoringLeafIndices(tree7));
                Debug.Assert(forest[2].IsEqualIgnoringLeafIndices(tree8));
                Debug.Assert(forest[3].IsEqualIgnoringLeafIndices(tree8));
                Debug.Assert(forest[4].IsEqualIgnoringLeafIndices(tree9));
                Debug.Assert(forest[5].IsEqualIgnoringLeafIndices(tree10));   
            }

            private static void TestRemoveDuplicateMainTrees() {
                
                List<HashSet<(char, bool)>> constraintsList = new List<HashSet<(char, bool)>>();
                GatherConstraints(TestTrees.Tree12().GetRoot(), new HashSet<(char, bool)>(), constraintsList);
                List<Tree> forest = CreateForestOfReducedTrees(TestTrees.Tree12(), constraintsList);
                RemoveDuplicateMainTrees(forest, 0);

                Tree ref0 = TestTrees.Tree13();
                Tree ref1 = TestTrees.Tree14();

                Debug.Assert(forest.Count == 2);
                Debug.Assert(forest[0].IsEqual(ref0));
                Debug.Assert(forest[1].IsEqual(ref1));

                constraintsList = new List<HashSet<(char, bool)>>();
                GatherConstraints(TestTrees.Tree15().GetRoot(), new HashSet<(char, bool)>(), constraintsList);
                forest = CreateForestOfReducedTrees(TestTrees.Tree15(), constraintsList);
                RemoveDuplicateMainTrees(forest, 0);

                ref0 = TestTrees.Tree16();
                ref1 = TestTrees.Tree17();
                Tree ref2 = TestTrees.Tree18();

                Debug.Assert(forest.Count == 3);
                Debug.Assert(forest[0].IsEqual(ref0));
                Debug.Assert(forest[1].IsEqual(ref1));
                Debug.Assert(forest[2].IsEqual(ref2));
            }

            private static void TestRangeOfNextTreeIndicesInReducedTrees() {
                /* This isn't a test of any one method or functionality, rather it checks if
                each leaf of each reduced tree has a nextTreeIndex in a valid range (between
                1 and numberOfTrees, inclusive), and also if every tree is reachable from at
                least one leaf, meaning there are no "dangling" trees that cannot be reached
                from any leaf.
                */

                (List<Tree> forest, int _) = MainForestMiddleRows(ODTree.GetTree());

                HashSet<int> unusedIncides = new HashSet<int>();
                for (int i = 1; i < forest.Count; i++) {
                    unusedIncides.Add(i);
                }

                foreach (Tree tree in forest) {
                    TestRangeOfNextTreeIndicesInReducedTreesHelper(tree.GetRoot(), unusedIncides, forest.Count);
                }
                
                Debug.Assert(unusedIncides.Count == 0);
            }

            private static void TestRangeOfNextTreeIndicesInReducedTreesHelper(
                    AbstractNode abstractNode, HashSet<int> unusedIncides, int forestCount) {

                if (abstractNode is Node) {
                    Node node = (Node) abstractNode;
                    TestRangeOfNextTreeIndicesInReducedTreesHelper(node.GetLeft(), unusedIncides, forestCount);
                    TestRangeOfNextTreeIndicesInReducedTreesHelper(node.GetRight(), unusedIncides, forestCount);
                } else {
                    int nextTreeIndex = ((Leaf) abstractNode).GetNextTreeIndex();
                    if (unusedIncides.Contains(nextTreeIndex)) {
                        unusedIncides.Remove(nextTreeIndex);
                    }
                    Debug.Assert(nextTreeIndex >= 1);
                    Debug.Assert(nextTreeIndex <= forestCount);
                }
            }

            private static void TestPage5Tree() {
                // Tests if one of my reduced trees exactly matches the one on page 5 of the Spaghetti paper
                (List<Tree> forest, int _) = MainForestMiddleRows(ODTree.GetTree());
                Tree refTree = TestTrees.Tree11();

                bool matches = false;
                foreach (Tree reduced in forest) {
                    if (reduced.IsEqual(refTree)) {
                        matches = true;
                        // Tree at index 4 is matching the page 5 tree.
                        break;
                    }
                }
                Debug.Assert(matches);

                /*
                Update: this test is currently unused. I had to remove general merging of seemingly equal branches because
                it lead to important pieces of information being lost when creating the forest for first row labeling.
                This resulted in this test failing, however since the forest creation works anyway (as proven by more
                advanced tests passing), I figured out this test is not useful anymore. The branches that could be merged
                during forest creation are merged during the merging of equivalent nodes during graph creation. 
                */
            }
        }        
    }
}