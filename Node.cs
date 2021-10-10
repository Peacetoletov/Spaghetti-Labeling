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
            this.left.SetIsLeft(false);
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

        public void MergeIdenticalBranches() {
            // If both subtrees are identical, this node is replaced by one of them

            // TODO: write tests covering this function

            if (left.Equals(right)) {
                AbstractNode replacement = left;        // arbitarily chosen child 
                Node parent = GetParent();
                if (parent != null) {
                    // This node has a parent
                    replacement.SetParent(parent);
                    if (IsLeft()) {
                        parent.SetLeft(replacement);
                    } else {
                        parent.SetRight(replacement);
                    }
                } 
                else {
                    // This node doesn't have a parent
                    replacement.SetParent(null);
                    GetTree().SetRoot(replacement);
                }
            }
        }

        public override bool Equals(object obj) {
            if (obj == null || GetType() != obj.GetType()) {
                return false;
            }
            
            Node root = (Node) obj;
            if (this.condition != root.GetCondition()) {
                return false;
            }

            return left.Equals(root.GetLeft()) && right.Equals(root.GetRight());
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
            }

            private static void TestMergeIdenticalBranches() {
                // TODO: write tests
                Debug.Assert(false);
            }
        }
    }
}