using System;
using System.Diagnostics;

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
            this.left.SetParent(this);
            this.left.SetIsLeft(true);
        }

        public void SetRight(AbstractNode right) {
            this.right = right;
            this.right.SetName(GetName() + "r");
            this.right.SetParent(this);
            this.right.SetIsLeft(false);
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

            if (left.Equals(right)) {
                //Console.WriteLine("Left subtree of node " + GetName() + " is equal to the right subtree");
                AbstractNode replacement = left;        // arbitarily chosen child 
                Node parent = GetParent();
                if (parent != null) {
                    // This node has a parent
                    replacement.SetParent(parent);
                    if (IsLeft()) {
                        parent.SetLeft(replacement);
                        //Console.WriteLine("Setting node " + GetName() + " as the left subtree of node " + GetParent().GetName());
                    } else {
                        parent.SetRight(replacement);
                        //Console.WriteLine("Setting node " + GetName() + " as the right subtree of node " + GetParent().GetName());
                    }
                    //Console.WriteLine("Has parent.");
                } 
                else {
                    // This node doesn't have a parent
                    replacement.SetParent(null);
                    //Console.WriteLine("No parent, setting new root to the tree.");
                    Tree tree = GetTree();
                    tree.SetRoot(replacement);
                    replacement.SetTree(tree);
                }
            }
            else {
                //Console.WriteLine("Left subtree of node " + GetName() + " is NOT equal to the right subtree");
            }
        }

        public override bool Equals(object obj) {
            if (obj == null || GetType() != obj.GetType()) {
                return false;
            }
            
            Node otherNode = (Node) obj;
            if (this.condition != otherNode.GetCondition()) {
                return false;
            }

            return left.Equals(otherNode.GetLeft()) && right.Equals(otherNode.GetRight());
        }
        
        public override int GetHashCode() {
            return base.GetHashCode();
        }

        public override void InfoDFS() {
            Console.WriteLine("Going through node " + GetName() + " (" + condition + ") into the left subtree");
            left.InfoDFS();
            Console.WriteLine("Back in node " + GetName() + " (" + condition + "). Going into the right subtree");
            right.InfoDFS();
            Console.WriteLine("Back in node " + GetName() + " (" + condition + "). Returning");
        }


        public static class NodeTests {
            public static void Run() {
                TestEquals();
                TestMergeIdenticalBranches();
            }

            private static void TestEquals() {
                Node node1 = (Node) TestTrees.Tree1().GetRoot();
                Debug.Assert(!node1.Equals(node1.GetLeft()));
                Debug.Assert(node1.GetLeft().Equals(node1.GetRight()));
                Debug.Assert(node1.Equals(node1));

                AbstractNode leaf1 = TestTrees.TreeLeaf1().GetRoot();
                AbstractNode leaf2 = TestTrees.TreeLeaf2().GetRoot();
                Debug.Assert(leaf1.Equals(leaf1));
                Debug.Assert(!leaf1.Equals(leaf2));

                Node node4 = (Node) TestTrees.Tree4().GetRoot();
                Debug.Assert(node4.Equals(node4));
            }

            private static void TestMergeIdenticalBranches() {
                Tree tree2 = TestTrees.Tree2();
                ((Node) tree2.GetRoot()).MergeIdenticalBranches();
                Tree treeLeaf2 = TestTrees.TreeLeaf2();
                Tree treeLeaf3 = TestTrees.TreeLeaf3();
                Debug.Assert(!CheckTreeEqualityAndRootCorrectness(tree2, treeLeaf2));
                Debug.Assert(CheckTreeEqualityAndRootCorrectness(tree2, treeLeaf3));
                
                Tree tree3 = TestTrees.Tree3();
                tree2 = TestTrees.Tree2();
                ((Node) tree3.GetRoot()).MergeIdenticalBranches();
                ((Node) tree2.GetRoot()).MergeIdenticalBranches();
                Debug.Assert(CheckTreeEqualityAndRootCorrectness(tree3, tree2));
                
                Tree tree4 = TestTrees.Tree4();
                Tree tree5 = TestTrees.Tree5();
                ((Node) tree5.GetRoot()).MergeIdenticalBranches();
                Debug.Assert(CheckTreeEqualityAndRootCorrectness(tree4, tree5));
            }

            private static bool CheckTreeEqualityAndRootCorrectness(Tree t1, Tree t2) {
                return t1.Equals(t2) && t1.GetRoot().GetTree() == t1 && t2.GetRoot().GetTree() == t2;
            }
        }
    }
}