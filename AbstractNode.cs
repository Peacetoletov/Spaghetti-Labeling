using System.Diagnostics;

namespace Spaghetti_Labeling
{
    public abstract class AbstractNode
    {
        private Node parent;
        private Tree tree;
        private bool isLeft = false;        // if this node has a parent, is it the left child?
        private string name = "";    // only for testing
        // Note that the name only corresponds to the tree structure at the creation of the tree, it
        // may or may not correspond after modifications of the tree are made

        public void SetTree(Tree tree) {
            // Pointer to the Tree structure is only set on parentless nodes
            Debug.Assert(parent == null);
            this.tree = tree;
        }

        public Tree GetTree() {
            return tree;
        }

        // Returns true if two AbstractNodes are equal.
        // I cannot override the standard Equals() method because I need to pass additional arguments
        public abstract bool IsEqual(AbstractNode abstractNode, bool showDebugInfo=false);

        /*
        public virtual bool IsEqual(AbstractNode abstractNode) {
            return IsEqual(abstractNode, false);
        }
        */

        // Prints out information about the tree through DFS traversal (only for testing)
        public abstract void InfoDFS();

        // Merges identical branches of this node and all subtrees
        public abstract void MergeIdenticalBranches();

        // Sets an initial index from 1 to numberOfLeaves to each leaf in ascending order starting
        // from the left-most leaf. Each index represents the next tree to be used
        public abstract int InitNextTreeIndex(int index);

        public abstract bool IsEqualIgnoringLeafIndices(AbstractNode abstractNode, bool showDebugInfo=false);

        public abstract void AdjustNextTreeIndicesAfterDeletion(int indexOfEqualTree, int indexOfDeletedTree);
            
        public abstract void UpdateName(string newName);

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