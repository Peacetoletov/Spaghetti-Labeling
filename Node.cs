using System;

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

        public override void InfoDFS() {
            /*
            // TODO: remove the ifs after the whole tree is implemented!!!
            Console.WriteLine("Going through node " + GetName() + " (" + condition + ") into the left subtree");
            if (left != null) {
                left.InfoDFS();
            }
            
            Console.WriteLine("Back in node " + GetName() + " (" + condition + "). Going into the right subtree");
            if (left != null) {
                right.InfoDFS();
            }
            Console.WriteLine("Back in node " + GetName() + " (" + condition + "). Returning");
            */

            Console.WriteLine("Going through node " + GetName() + " (" + condition + ") into the left subtree");
            left.InfoDFS();
            
            Console.WriteLine("Back in node " + GetName() + " (" + condition + "). Going into the right subtree");
            right.InfoDFS();
            
            Console.WriteLine("Back in node " + GetName() + " (" + condition + "). Returning");
        }
    }
}