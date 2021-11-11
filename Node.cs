using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Spaghetti_Labeling
{
    // Class for non-leaf nodes
    public class Node : AbstractNode
    {
        // (references to other nodes are null by default)
        private AbstractNode left;
        private AbstractNode right;

        // private char condition { get; set; }

        /* character describing the position of the pixel to check in this node (a-t)
        ('a', 'f', 'l', 'q' are never used)
        If the condition evaluates to true (pixel is in foreground), left subtree is used.
        Right subtree otherwise.
        */
        private char condition;

        public Node(char condition) {
            this.condition = condition;
        }

        public void SetChildren(AbstractNode left, AbstractNode right) {
            SetLeft(left);
            SetRight(right);
        }

        public void SetLeft(AbstractNode left) {
            this.left = left;
            this.left.SetName(GetName() + "l");
            this.left.AddParent(this);
        }

        public void SetRight(AbstractNode right) {
            this.right = right;
            this.right.SetName(GetName() + "r");
            this.right.AddParent(this);
        }

        public char GetCondition() {
            return condition;
        }

        public AbstractNode GetLeft() {
            return left;
        }

        public AbstractNode GetRight() {
            return right;
        }

        public override void MergeIdenticalBranches() {
            // Merges identical branches of this node and all subtrees.
            // If both subtrees are identical, this node is replaced by one of them

            //Console.WriteLine("In node " + GetName());

            left.MergeIdenticalBranches();
            //Console.WriteLine("Returned from left subtree to node " + GetName() + ". Going into the right subtree");
            right.MergeIdenticalBranches();
            //Console.WriteLine("Returned from right subtree to node " + GetName() + ". Checking subtree equality");

            //if (left.Equals(right)) {
            if (left.IsEqualIgnoringLeafIndices(right)) {
                ReplaceByLeft();            // arbitarily chosen child 
                //Console.WriteLine("Merging! node " + GetName());
            }
            else {
                //Console.WriteLine("Left subtree of node " + GetName() + " is NOT equal to the right subtree");
            }
        }

        public void ReplaceByLeft() {
            ReplaceByChild(left);
        }

        public void ReplaceByRight() {
            ReplaceByChild(right);
        }

        private void ReplaceByChild(AbstractNode child) {
            // This method is only called when working with pure trees, not graphs.
            AbstractNode replacement = child;
            List<Node> parents = GetParents();
            replacement.RemoveParent(this);
            if (parents.Count != 0) {
                // This node has a parent
                Node parent = parents[0];
                if (IsLeftChild(parent)) {
                    parent.SetLeft(replacement);
                } else {
                    parent.SetRight(replacement);
                }
                //Console.WriteLine("Has parent.");
            } 
            else {
                // This node doesn't have a parent
                //Console.WriteLine("No parent, setting new root to the tree.");
                Tree tree = GetTree();
                tree.SetRoot(replacement);
                replacement.SetTree(tree);
            }
        }

        public override bool IsEqual(AbstractNode abstractNode, bool showDebugInfo=false) {
            // Returns true if two AbstractNodes are equal.
            // I cannot override the standard Equals() method because I need to pass additional arguments
            if (abstractNode == null || GetType() != abstractNode.GetType()) {
                return false;
            }
            
            Node otherNode = (Node) abstractNode;
            if (this.condition != otherNode.GetCondition()) {
                return false;
            }

            bool leftEquals = left.IsEqual(otherNode.GetLeft(), showDebugInfo);
            bool rightEquals = right.IsEqual(otherNode.GetRight(), showDebugInfo);
            if (showDebugInfo) {
                Console.WriteLine("Node " + GetName() + ". Left equals? " + leftEquals + 
                                  ". Right equals? " + rightEquals);
            }
            return leftEquals && rightEquals;
        }

        public override bool IsEqualIgnoringLeafIndices(AbstractNode abstractNode, bool showDebugInfo=false) {
            if (abstractNode == null || GetType() != abstractNode.GetType()) {
                return false;
            }
            
            Node otherNode = (Node) abstractNode;
            if (this.condition != otherNode.GetCondition()) {
                return false;
            }

            return left.IsEqualIgnoringLeafIndices(otherNode.GetLeft(), showDebugInfo) && 
                   right.IsEqualIgnoringLeafIndices(otherNode.GetRight(), showDebugInfo);
        }

        public override void DFS_Rec() {
            if (GetVisited()) {
                Console.WriteLine("Node " + GetName() + " (" + condition + ") was already visited. Returning.");
                return;
            }
            SetVisited(true);
            Console.WriteLine("Going through node " + GetName() + " (" + condition + ") into the left subtree");
            left.DFS_Rec();
            Console.WriteLine("Back in node " + GetName() + " (" + condition + "). Going into the right subtree");
            right.DFS_Rec();
            Console.WriteLine("Back in node " + GetName() + " (" + condition + "). Returning");
        }

        public override int InitNextTreeIndex(int index) {
            // Sets an initial index from 1 to numberOfLeaves to each leaf in ascending order starting
            // from the left-most leaf. Each index represents the next tree to be used
            index = left.InitNextTreeIndex(index);
            return right.InitNextTreeIndex(index);
        }

        public override void AdjustNextTreeIndicesAfterDeletion(int indexOfEqualTree, int indexOfDeletedTree) {
            left.AdjustNextTreeIndicesAfterDeletion(indexOfEqualTree, indexOfDeletedTree);
            right.AdjustNextTreeIndicesAfterDeletion(indexOfEqualTree, indexOfDeletedTree);
        }

        public override void UpdateName(string newName) {
            SetName(newName);
            left.UpdateName(newName + "l");
            right.UpdateName(newName + "r");
        }

        public override int CountNodes() {
            return 1 + left.CountNodes() + right.CountNodes();
        }

        public override AbstractNode DeepCopy() { 
            Node newNode = new Node(condition);
            newNode.SetName(GetName());
            newNode.SetChildren(left.DeepCopy(), right.DeepCopy());
            return newNode;
        } 

        public override string Stringify() {
            //return condition.ToString() + "(" + left.Stringify() + ")(" + right.Stringify() + ")";
            return String.Format("{0}({1})({2})", condition, left.Stringify(), right.Stringify());        
        }

        public override void AssignVisitedInSubtree(bool visited) {
            SetVisited(visited);
            left.AssignVisitedInSubtree(visited);
            right.AssignVisitedInSubtree(visited);
        }

        public override void AssignSubstitutedInSubtree(bool substituted) {
            SetSubstituted(substituted);
            left.AssignSubstitutedInSubtree(substituted);
            right.AssignSubstitutedInSubtree(substituted);
        }

        public override void AssignIdInSubtree(int id) {
            SetID(id);
            left.AssignIdInSubtree(id);
            right.AssignIdInSubtree(id);
        }

        public override void UpdateActionsInSubtree(List<HashSet<int>> actionsList) {
            left.UpdateActionsInSubtree(actionsList);
            right.UpdateActionsInSubtree(actionsList);
        }

        public static class Tests 
        {
            public static void Run() {
                TestIsEqual();
                TestMergeIdenticalBranches();
                TestStringify();
            }

            private static void TestIsEqual() {
                Node node1 = (Node) TestTrees.Tree1().GetRoot();
                Debug.Assert(!node1.IsEqual(node1.GetLeft()));
                
                Debug.Assert(node1.GetLeft().IsEqual(node1.GetRight()));
                Debug.Assert(node1.IsEqual(node1));

                AbstractNode leaf1 = TestTrees.TreeLeaf1().GetRoot();
                AbstractNode leaf2 = TestTrees.TreeLeaf2().GetRoot();
                Debug.Assert(leaf1.IsEqual(leaf1));
                Debug.Assert(!leaf1.IsEqual(leaf2));

                Node node4 = (Node) TestTrees.Tree4().GetRoot();
                Debug.Assert(node4.IsEqual(node4));
            }

            private static void TestMergeIdenticalBranches() {
                Tree tree2 = TestTrees.Tree2();
                ((Node) tree2.GetRoot()).MergeIdenticalBranches();
                Tree treeLeaf2 = TestTrees.TreeLeaf2();
                Tree treeLeaf3 = TestTrees.TreeLeaf3();
                Debug.Assert(!tree2.IsEqual(treeLeaf2));
                Debug.Assert(tree2.IsEqual(treeLeaf3));
                
                Tree tree3 = TestTrees.Tree3();
                tree2 = TestTrees.Tree2();
                ((Node) tree3.GetRoot()).MergeIdenticalBranches();
                ((Node) tree2.GetRoot()).MergeIdenticalBranches();
                Debug.Assert(tree3.IsEqual(tree2));
                
                Tree tree4 = TestTrees.Tree4();
                Tree tree5 = TestTrees.Tree5();
                ((Node) tree5.GetRoot()).MergeIdenticalBranches();
                Debug.Assert(tree4.IsEqual(tree5));
            }

            private static void TestStringify() {
                Tree tree13 = TestTrees.Tree13();
                string str = tree13.GetRoot().Stringify();
                Debug.Assert(str == "o(i(1)(2))(i(n(1)(1))(2))");
            }
        }
    }
}