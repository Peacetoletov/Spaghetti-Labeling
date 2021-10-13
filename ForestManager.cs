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
        
        public ForestManager() {

        }

        public List<Tree> CreateForestOfReducedTrees(Func<Tree> newTree) {
            // Creates a reduced tree for each leaf of the ODTree and merges identical branches 

            List<HashSet<(char, bool)>> constraintsList = new List<HashSet<(char, bool)>>();
            GatherConstraints(newTree().GetRoot(), new HashSet<(char, bool)>(), constraintsList);

            List<Tree> forest = new List<Tree>();
            foreach (HashSet<(char, bool)> constraints in constraintsList) {
                Tree reduced = ReduceTree(newTree(), constraints);
                forest.Add(reduced);
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

        private Tree ReduceTree(Tree tree, HashSet<(char, bool)> constraints) {
            // Returns a reduced tree with identical branches merged
            ReduceSubtree(tree.GetRoot(), constraints);
            tree.GetRoot().MergeIdenticalBranches();
            return tree;
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
                } else {
                }
            }            
        }

        public static class Tests 
        {
            public static void Run() {
                TestCreateForestOfReducedTrees();
                TestReduceTree();
            }

            private static void TestReduceTree() {
                HashSet<(char, bool)> constraints = new HashSet<(char, bool)> 
                    {('h', false), ('n', true), ('i', true)};
                ForestManager fm = new ForestManager();
                Tree reduced = fm.ReduceTree(new ODTree().GetTree(), constraints);
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