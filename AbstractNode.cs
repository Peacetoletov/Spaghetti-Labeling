using System.Diagnostics;

namespace Spaghetti_Labeling
{
    public abstract class AbstractNode
    {
        private Node parent;
        private Tree tree;
        private bool isLeft = false;        // if this node has a parent, is it the left child?
        private string name = "";    // only for testing

        public void SetTree(Tree tree) {
            // Pointer to the Tree structure is only set on parentless nodes
            Debug.Assert(parent == null);
            this.tree = tree;
        }

        public Tree GetTree() {
            return tree;
        }


        // Prints out information about the tree through DFS traversal (only for testing)
        public abstract void InfoDFS();
        
        public string GetName() {
            return name;
        }

        public void SetName(string name) {
            this.name = name;
        }

        public void SetParent(Node parent) {
            this.parent = parent;
        }

        public Node GetParent() {
            return parent;
        }

        public void SetIsLeft(bool isLeft) {
            this.isLeft = isLeft;
        }

        public bool IsLeft() {
            return isLeft;
        }
    }
}