using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Spaghetti_Labeling
{
    // Class for converting the ODTree into a forest of reduced trees
    public static class ForestCreator
    {
        // Observations: Main forest is reduced to just 13 trees. End forest even also constains 13 trees, 
        // end forest odd is further reduced to 9 trees. Odd trees also have a very small number of nodes (17 at most).

        /*
        public static (List<Tree>, int) MainForest(Func<Tree> newTree) {
            // Creates a forest of reduced trees with merged identical branches, removes duplicate trees
            // and assigns an index of the next tree to each leaf of each tree.

            List<HashSet<(char, bool)>> constraintsList = MainTreesConstraints(newTree);
            List<Tree> forest = CreateForestOfReducedTrees(newTree, constraintsList);
            Tree startTree = forest[forest.Count - 1];      // Start tree constraints are added last, therefore start tree will be at the last index
            MergeIdenticalBranches(forest);
            RemoveDuplicateMainTrees(forest);
  
            return (forest, GetStartTreeIndex(startTree, forest));
        }
        */

        public static (List<Tree>, int) MainForestMiddleRows(Func<Tree> newTree) {
            // Returns a forest for labeling middle rows, alongside the index of the start tree
            return MainForest(newTree, MainTreesConstraints(newTree));
        }

        public static (List<Tree>, int) MainForestFirstRow(Func<Tree> newTree) {
            // Returns a forest for labeling the first row, alongside the index of the start tree
            // hmm, what do I do next? I think I should do it the same way as with middle row end trees, meaning that I first
            // create the basic forest using basic constraint, then perform additional pruning using the additional constraints
            // ...but will this approach be consistant with the first tree index that I'm returning?
            // solution: before the additional pruning, store the root of the start tree. After the additional pruning,
            // find the start tree using by comparing all trees to the stored root.

            // TODO: this
            //return (null, 0);

            (List<Tree> forest, int startTreeIndex) = MainForest(newTree, MainTreesConstraints(newTree));
            //Tree startTree = forest[startTreeIndex - 1];
            PruneForest(forest, FirstRowConstraints());
            startTreeIndex = RemoveDuplicateMainTrees(forest, startTreeIndex);
            //return (forest, GetStartTreeIndex(startTree, forest));
            return (forest, startTreeIndex);

            // As I feared, this approach of keeping track of the first tree doesn't work.
            // Idea: I can modify RemoveDuplicateMainTrees() to take the start tree index as an argument and make it return
            // a new (possibly modified) start tree index
        }

        private static (List<Tree>, int) MainForest(Func<Tree> newTree, List<HashSet<(char, bool)>> constraintsList) {
            // Creates a forest of reduced trees with merged identical branches, removes duplicate trees
            // and assigns an index of the next tree to each leaf of each tree.
            List<Tree> forest = CreateForestOfReducedTrees(newTree, constraintsList);
            //Tree startTree = forest[forest.Count - 1];      // Start tree constraints are added last, therefore start tree will be at the last index
            int startTreeIndex = forest.Count;
            MergeIdenticalBranches(forest);
            startTreeIndex = RemoveDuplicateMainTrees(forest, startTreeIndex);
  
            //return (forest, GetStartTreeIndex(startTree, forest));
            return (forest, startTreeIndex);
        }

        /*
        private static List<(Tree, List<int>)> EndForest(List<Tree> mainForest, bool even) {
            // Returns a list of end trees together with indices of all main trees that each 
            // end tree is associated with (these indices start at 1).
            // Parameter even is determined the type of end forest to be created (even/odd).

            
            //1) Copy main forest.
            //2) Perform further reduction and branch merging, put all newly reduced trees into 
            //   a list together with a list containing the index of the associated main tree.
            //3) Delete duplicate trees, add the deleted tree's index to the list of the other 
            //   tree (the one that is equal but wasn't deleted).
            //   Note that only odd constraints result in having duplicate trees. Even trees
            //  are all different despite reduction.
            

            List<Tree> endForest = copyForest(mainForest);
            HashSet<(char, bool)> constraintsList = even ? EndEvenConstraints() : EndOddConstraints();
            foreach (Tree tree in endForest) {
                ReduceTree(tree, constraintsList);
            }
            MergeIdenticalBranches(endForest);

            //Each end tree is associated with a main tree. Whenever an end tree is removed due to
              // being a duplicate, the other tree's list of associated main trees must be updated.
              // This list stores the end trees with their assocaited main trees. 
            List<(Tree, List<int>)> endTreesWithMainTreeIndices = new List<(Tree, List<int>)>();
            for (int i = 0; i < endForest.Count; i++) {
                endTreesWithMainTreeIndices.Add((endForest[i], new List<int> {i + 1}));
            }
            RemoveDuplicateEndTrees(endTreesWithMainTreeIndices);

            return endTreesWithMainTreeIndices;
        }
        */

        public static List<(Tree, List<int>)> EndForestEvenMiddleRows(List<Tree> mainForest) {
            // Returns a list of even end trees together with indices of all main trees that each 
            // end tree is associated with (these indices start at 1).
            return EndForest(mainForest, true, EndEvenConstraints());
        }

        public static List<(Tree, List<int>)> EndForestOddMiddleRows(List<Tree> mainForest) {
            // Returns a list of odd end trees together with indices of all main trees that each 
            // end tree is associated with (these indices start at 1).
            return EndForest(mainForest, false, EndOddConstraints());
        }

        public static List<(Tree, List<int>)> EndForestEvenFirstRow(List<Tree> mainForest) {
            // Returns a list of even end trees together with indices of all main trees that each 
            // end tree is associated with (these indices start at 1).
            HashSet<(char, bool)> constraints = EndEvenConstraints();
            constraints.UnionWith(FirstRowConstraints());
            return EndForest(mainForest, true, constraints);
        }

        public static List<(Tree, List<int>)> EndForestOddFirstRow(List<Tree> mainForest) {
            // Returns a list of even end trees together with indices of all main trees that each 
            // end tree is associated with (these indices start at 1).
            HashSet<(char, bool)> constraints = EndOddConstraints();
            constraints.UnionWith(FirstRowConstraints());
            return EndForest(mainForest, false, constraints);
        }

        private static List<(Tree, List<int>)> EndForest(List<Tree> mainForest, bool even, HashSet<(char, bool)> constraints) {
            // Parameter even is determined the type of end forest to be created (even/odd).
            /*
            1) Copy main forest.
            2) Perform further reduction and branch merging and put all newly reduced trees into 
               a list together with a list containing the index of the associated main tree.
            3) Delete duplicate trees, add the deleted tree's index to the list of the other 
               tree (the one that is equal but wasn't deleted).
               Note that in middle rows, only odd constraints result in having duplicate trees. 
               Even trees are all different despite reduction.
            */

            List<Tree> endForest = copyForest(mainForest);
            /*
            foreach (Tree tree in endForest) {
                ReduceTree(tree, constraints);
            }
            MergeIdenticalBranches(endForest);
            */
            PruneForest(endForest, constraints);

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

        private static void PruneForest(List<Tree> forest, HashSet<(char, bool)> constraints) {
            // Prunes a given forest using a set of constraints, then merges identical branches
            foreach (Tree tree in forest) {
                ReduceTree(tree, constraints);
            }
            MergeIdenticalBranches(forest);
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

        private static List<HashSet<(char, bool)>> MainTreesConstraints(Func<Tree> newTree) {
            List<HashSet<(char, bool)>> constraintsList = new List<HashSet<(char, bool)>>();
            GatherConstraints(newTree().GetRoot(), new HashSet<(char, bool)>(), constraintsList);
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

        private static Tree InitIndicesAndReduce(Func<Tree> newTree, HashSet<(char, bool)> constraints) {
            Tree tree = newTree();
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
            // Note that firstTreeIndex starts at 1 as opposed to 0, so it must be first normalized.
            int startTreeIndexNormalized = startTreeIndex - 1;

            //Console.WriteLine("Number of trees before the removal of duplicates: {0}. firstTreeIndexNormalized = {1}", forest.Count, startTreeIndexNormalized);
            
            for (int i = 0; i < forest.Count - 1; i++) {
                for (int j = i + 1; j < forest.Count; j++) {
                    if (forest[i].IsEqual(forest[j])) {
                        //Console.WriteLine("Reduced trees " + i + " and " + j + " are equal.");
                        // Adjust next tree indices in tree leaves if needed
                        foreach (Tree tree in forest) {
                            tree.GetRoot().AdjustNextTreeIndicesAfterDeletion(i + 1, j + 1);
                            // Arguments need to be incremented by 1 because these nested loops start indexing from 0,
                            // but next tree indices in leaves start indexing from 1 (to stay consistent with the Spaghetti paper)
                        }
                        // Adjust startTreeIndexNormalized if needed
                        if (startTreeIndexNormalized > j) {
                            // first tree index is higher than the deleted tree's index, must be decremented
                            startTreeIndexNormalized--;
                            //Console.WriteLine("Updated startTreeIndexNormalized: {0}", startTreeIndexNormalized);
                        } else if (startTreeIndexNormalized == j) {
                            // first tree index is equal to the deleted tree's index, meaning that the forest[i] tree is now
                            // the first tree
                            startTreeIndexNormalized = i;
                            //Console.WriteLine("Updated startTreeIndexNormalized: {0}", startTreeIndexNormalized);
                        }
                        // do nothing with the first tree index if startTreeIndexNormalized < j

                        // Remove the duplicate tree
                        forest.RemoveAt(j);
                        j--;        // Decrement j to counterbalance removing a tree, otherwise one tree would be skipped
                    }
                }
            }
            //Console.WriteLine("Number of trees after the removal of duplicates: " + forest.Count);

            return startTreeIndexNormalized + 1;        // Add 1 to remove the effect of normalizing
        }

        /*
        private static void RemoveDuplicateMainTrees(List<Tree> forest) {
            //Console.WriteLine("Number of trees before the removal of duplicates: " + forest.Count);
            for (int i = 0; i < forest.Count - 1; i++) {
                for (int j = i + 1; j < forest.Count; j++) {
                    if (forest[i].IsEqual(forest[j])) {
                        //Console.WriteLine("Reduced trees " + i + " and " + j + " are equal.");
                        foreach (Tree tree in forest) {
                            tree.GetRoot().AdjustNextTreeIndicesAfterDeletion(i + 1, j + 1);
                            // Arguments need to be incremented by 1 because these nested loops start indexing from 0,
                            // but next tree indices in leaves start indexing from 1 (to stay consistent with the Spaghetti paper)
                        }
                        forest.RemoveAt(j);
                        j--;        // Decrement j to counterbalance removing a tree, otherwise one tree would be skipped
                    }
                }
            }
            //Console.WriteLine("Number of trees after the removal of duplicates: " + forest.Count);
        }
        */

        private static void RemoveDuplicateEndTrees(List<(Tree, List<int>)> endTreesWithMainTreeIndices) {
            //Console.WriteLine("Number of trees before the removal of duplicates: " + endTreesWithMainTreeIndices.Count);
            for (int i = 0; i < endTreesWithMainTreeIndices.Count - 1; i++) {
                Tree tree1 = endTreesWithMainTreeIndices[i].Item1;
                //Console.WriteLine("Tree " + i + " has " + tree1.GetRoot().CountNodes() + " nodes.");
                for (int j = i + 1; j < endTreesWithMainTreeIndices.Count; j++) {
                    Tree tree2 = endTreesWithMainTreeIndices[j].Item1;
                    if (tree1.IsEqual(tree2)) {
                        // Add the duplicate tree's index to the other tree's indices
                        endTreesWithMainTreeIndices[i].Item2.Add(endTreesWithMainTreeIndices[j].Item2[0]);

                        //Console.WriteLine("Tree " + j + " was a duplicate of tree " + i + " and was removed.");

                        // Delete the duplicate 
                        endTreesWithMainTreeIndices.RemoveAt(j);
                        j--;        // Decrement j to counterbalance removing a tree, otherwise one tree would be skipped
                    }
                }
            }
            //Console.WriteLine("Tree " + (endTreesWithMainTreeIndices.Count - 1) + " has " + endTreesWithMainTreeIndices[endTreesWithMainTreeIndices.Count - 1].Item1.GetRoot().CountNodes() + " nodes.");
            //Console.WriteLine("Number of trees after the removal of duplicates: " + endTreesWithMainTreeIndices.Count);
        }

        private static List<Tree> CreateForestOfReducedTrees(Func<Tree> newTree, 
                                                     List<HashSet<(char, bool)>> constraintsList) {
            // Creates a reduced tree for each leaf of the ODTree 
            List<Tree> forest = new List<Tree>();
            foreach (HashSet<(char, bool)> constraints in constraintsList) {
                forest.Add(InitIndicesAndReduce(newTree, constraints));
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
                TestFinalForest();
                TestRangeOfNextTreeIndicesInReducedTrees();
                TestPage5Tree();

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
                List<Tree> forest = CreateForestOfReducedTrees(TestTrees.Tree6, constraintsList);
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
                List<Tree> forest = CreateForestOfReducedTrees(TestTrees.Tree12, constraintsList);
                RemoveDuplicateMainTrees(forest, 0);

                Tree ref0 = TestTrees.Tree13();
                Tree ref1 = TestTrees.Tree14();

                Debug.Assert(forest.Count == 2);
                Debug.Assert(forest[0].IsEqual(ref0));
                Debug.Assert(forest[1].IsEqual(ref1));

                constraintsList = new List<HashSet<(char, bool)>>();
                GatherConstraints(TestTrees.Tree15().GetRoot(), new HashSet<(char, bool)>(), constraintsList);
                forest = CreateForestOfReducedTrees(TestTrees.Tree15, constraintsList);
                RemoveDuplicateMainTrees(forest, 0);

                ref0 = TestTrees.Tree16();
                ref1 = TestTrees.Tree17();
                Tree ref2 = TestTrees.Tree18();

                Debug.Assert(forest.Count == 3);
                Debug.Assert(forest[0].IsEqual(ref0));
                Debug.Assert(forest[1].IsEqual(ref1));
                Debug.Assert(forest[2].IsEqual(ref2));
            }

            private static void TestFinalForest() {
                /*
                Console.WriteLine("ODTree test");
                FinalForest(ODTree.GetTree);

                // TODO: actual tests
                // UPDATE: this can wait, as manual testing was good enough for now
                */
            }

            private static void TestRangeOfNextTreeIndicesInReducedTrees() {
                /* This isn't a test of any one method or functionality, rather it checks if
                each leaf of each reduced tree has a nextTreeIndex in a valid range (between
                1 and numberOfTrees, inclusive), and also if every tree is reachable from at
                least one leaf, meaning there are no "dangling" trees that cannot be reached
                from any leaf.
                */

                (List<Tree> forest, int _) = MainForestMiddleRows(ODTree.GetTree);

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
                (List<Tree> forest, int _) = MainForestMiddleRows(ODTree.GetTree);
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
            }
        }        
    }
}