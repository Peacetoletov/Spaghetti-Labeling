using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Spaghetti_Labeling
{
    // Class for converting the ODTree into a forest of reduced trees
    public class ForestManager
    {
        // TODO: I will need to add an index to the next tree to each reduced tree. I'm still unsure
        // where, when and how this will be done.

        // TODO: After creating a forest of reduced trees, look for duplicates and remove those.
        // TODO: Then, add a special tree for the beginning of rows.
        
        public ForestManager() {

        }

        public List<Tree> FinalForest(Func<Tree> newTree) {
            // Creates a forest of reduced trees with merged identical branches, removes duplicate trees
            // and assigns an index of the next tree to each leaf of each tree.

            // TODO: create tests in Tree for initial next node indices DONE

            // TODO: implement this and write tests for the RemoveDuplicateTrees method and a test for 
            // this whole method (with a simplified ODTree) 
            List<Tree> forest = CreateForestOfReducedTrees(newTree);
            MergeIdenticalBranches(forest);
            RemoveDuplicateTrees(forest);
            
            return null;
        }

        private void MergeIdenticalBranches(List<Tree> forest) {
            foreach (Tree tree in forest) {
                tree.GetRoot().MergeIdenticalBranches();
            }
        }

        private void RemoveDuplicateTrees(List<Tree> forest) {
            // TODO: Create another test tree and associated reduced test trees to be used in tests

            for (int i = 0; i < forest.Count - 1; i++) {
                for (int j = i + 1; j < forest.Count; j++) {
                    if (forest[i].Equals(forest[j])) {
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
        }

        public List<Tree> CreateForestOfReducedTrees(Func<Tree> newTree) {
            // Creates a reduced tree for each leaf of the ODTree 

            List<HashSet<(char, bool)>> constraintsList = new List<HashSet<(char, bool)>>();
            GatherConstraints(newTree().GetRoot(), new HashSet<(char, bool)>(), constraintsList);

            List<Tree> forest = new List<Tree>();
            foreach (HashSet<(char, bool)> constraints in constraintsList) {
                Tree tree = newTree();
                tree.InitNextTreeIndices();
                ReduceTree(tree, constraints);
                forest.Add(tree);
            }

            return forest;
        }


        private void GatherConstraints(AbstractNode abstractNode, HashSet<(char, bool)> constraints,
                                       List<HashSet<(char, bool)>> constraintsList) {
            // Recursively traverse the tree and gather constraints on the way down, then append them to
            // a list of constraints upon reaching a leaf
            if (abstractNode is Node) {
                GatherConstraints((Node) abstractNode, constraints, constraintsList);
            } else {
                GatherConstraints(constraints, constraintsList);
            }
        } 

        private void GatherConstraints(Node node, HashSet<(char, bool)> constraints,
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

        private void GatherConstraints(HashSet<(char, bool)> constraints,
                                       List<HashSet<(char, bool)>> constraintsList) {
            // Append constraints to a list of constraints upon reaching a leaf
            constraintsList.Add(constraints);
        }

        private char ShiftCondition(char condition) {
            // Returns the shifted condition if applicable, return the same condition otherwise
            List<int> unshiftable = new List<int> {'a', 'b', 'g', 'h', 'm', 'n', 'q', 'r'};
            if (unshiftable.Contains(condition)) {
                return condition;
            }
            return (char) (condition - 2);
        }

        private void ReduceTree(Tree tree, HashSet<(char, bool)> constraints) {
            // Returns a reduced tree
            ReduceSubtree(tree.GetRoot(), constraints);
        }

        private void ReduceSubtree(AbstractNode abstractNode, HashSet<(char, bool)> constraints) {
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
                TestRemoveDuplicateTrees();
                TestFinalForest();
            }

            private static void TestReduceTreeAndMergeBranches() {
                HashSet<(char, bool)> constraints = new HashSet<(char, bool)> 
                    {('h', false), ('n', true), ('i', true)};
                ForestManager fm = new ForestManager();
                Tree reduced = ODTree.GetTree();
                fm.ReduceTree(reduced, constraints);
                List<Tree> tmp = new List<Tree> {reduced};
                fm.MergeIdenticalBranches(tmp);
                reduced = tmp[0];

                Tree reference =  TestTrees.Tree11();
                Debug.Assert(reduced.Equals(reference));
            }

            private static void TestCreateForestOfReducedTrees() {
                ForestManager fm = new ForestManager();
                List<Tree> forest = fm.CreateForestOfReducedTrees(TestTrees.Tree6);
                Tree tree7 = TestTrees.Tree7();
                Tree tree8 = TestTrees.Tree8();
                Tree tree9 = TestTrees.Tree9();
                Tree tree10 = TestTrees.Tree10();
                
                Debug.Assert(forest.Count == 6);
                Debug.Assert(forest[0].EqualsIgnoreLeafIndices(tree7));
                Debug.Assert(forest[1].EqualsIgnoreLeafIndices(tree7));
                Debug.Assert(forest[2].EqualsIgnoreLeafIndices(tree8));
                Debug.Assert(forest[3].EqualsIgnoreLeafIndices(tree8));
                Debug.Assert(forest[4].EqualsIgnoreLeafIndices(tree9));
                Debug.Assert(forest[5].EqualsIgnoreLeafIndices(tree10));   
            }

            private static void TestRemoveDuplicateTrees() {
                ForestManager fm = new ForestManager();
                List<Tree> forest = fm.CreateForestOfReducedTrees(TestTrees.Tree12);
                fm.RemoveDuplicateTrees(forest);

                Tree ref0 = TestTrees.Tree13();
                Tree ref1 = TestTrees.Tree14();

                Debug.Assert(forest.Count == 2);
                Debug.Assert(forest[0].Equals(ref0));
                Debug.Assert(forest[1].Equals(ref1));

                
                forest = fm.CreateForestOfReducedTrees(TestTrees.Tree15);
                fm.RemoveDuplicateTrees(forest);

                ref0 = TestTrees.Tree16();
                ref1 = TestTrees.Tree17();
                Tree ref2 = TestTrees.Tree18();

                Debug.Assert(forest.Count == 3);
                Debug.Assert(forest[0].Equals(ref0));
                Debug.Assert(forest[1].Equals(ref1));
                Debug.Assert(forest[2].Equals(ref2));
            }

            private static void TestFinalForest() {
                ForestManager fm = new ForestManager();
                //fm.FinalForest(ODTree.GetTree);

                // TODO: actual tests
            }
        }        
    }
}