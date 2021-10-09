using System;
using System.Diagnostics;

namespace Spaghetti_Labeling
{
    // Class for non-leaf nodes
    public class Node : AbstractNode
    {
        // (references to other nodes are null by default)
        private AbstractNode parent;
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
        }

        public void SetRight(AbstractNode right) {
            this.right = right;
            this.right.SetName(GetName() + "r");
            this.right.SetParent(this);
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

            // TODO: this, utilizing the IsTreeEqual function. Remember to write tests covering this function
        }

        public override bool IsTreeEqual(AbstractNode abstractRoot) {
            // Determines whether this tree and another tree are equal
            
            if (!(abstractRoot is Node)) {
                return false;
            }
            Node root = (Node) abstractRoot;

            if (this.condition != root.GetCondition()) {
                return false;
            }

            return left.IsTreeEqual(root.GetLeft()) && right.IsTreeEqual(root.GetRight());
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
                TestIsTreeEqual();
            }

            private static void TestIsTreeEqual() {
                Node tree1 = TestTrees.Tree1();
                
                Debug.Assert(!tree1.IsTreeEqual(tree1.GetLeft()));
                Debug.Assert(tree1.GetLeft().IsTreeEqual(tree1.GetRight()));
            }
        }
    }
}