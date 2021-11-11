using System.Diagnostics;
using System.Collections.Generic;

namespace Spaghetti_Labeling
{
    public abstract class AbstractNode
    {
        private List<Node> parents = new List<Node>();
        private Tree tree;
        private bool visited = false;   	// whether this node was already visited when working with nodes in graphs
        private bool substituted = false;   // whether this node was already substituted by another node when resolving subtree equivalences


        // Variables only for testing, used in DFS
        private string name = "";
        // Note that the name only corresponds to the tree structure at the creation of the tree, it
        // may or may not correspond after modifications of the tree are made
        private int id = 666;
        
        public void SetTree(Tree tree) {
            // Pointer to the Tree structure is only set on parentless nodes
            Debug.Assert(parents.Count == 0);
            this.tree = tree;
        }

        public Tree GetTree() {
            return tree;
        }

        // Returns true if two AbstractNodes are equal.
        // I cannot override the standard Equals() method because I need to pass additional arguments
        public abstract bool IsEqual(AbstractNode abstractNode, bool showDebugInfo=false);

        //public abstract bool IsEquivalent(AbstractNode abstractNode, bool showDebugInfo=false);

        // Prints out information about the tree through DFS traversal (only for testing)
        public void InfoDFS() {
            //AssignIDsInSubtree();
            //AssignVisitedInSubtree(false);
            DFS_Rec();
        }

        // Helper method for DFS, should not be called outside of InfoDFS.
        public abstract void DFS_Rec();

        // Merges identical branches of this node and all subtrees
        public abstract void MergeIdenticalBranches();

        // Sets an initial index from 1 to numberOfLeaves to each leaf in ascending order starting
        // from the left-most leaf. Each index represents the next tree to be used
        public abstract int InitNextTreeIndex(int index);

        public abstract bool IsEqualIgnoringLeafIndices(AbstractNode abstractNode, bool showDebugInfo=false);

        public abstract void AdjustNextTreeIndicesAfterDeletion(int indexOfEqualTree, int indexOfDeletedTree);
            
        public abstract void UpdateName(string newName);

        // Counts the total number of inner nodes and leaves in the subtree of this abstract node
        public abstract int CountNodes();

        public abstract AbstractNode DeepCopy();

        public abstract string Stringify();

        public string GetName() {
            return name;
        }

        public void SetName(string name) {
            this.name = name;
        }

        public void AddParent(Node parent) {
            this.parents.Add(parent);
        }

        public void RemoveParent(Node toRemove) {
            for (int i = 0; i < parents.Count; i++) {
                if (parents[i] == toRemove) {
                    parents.RemoveAt(i);
                }
            }
        }

        public List<Node> GetParents() {
            return parents;
        }

        public bool IsLeftChild(Node parent) {
            return parent.GetLeft() == this;
        }

        public void SetVisited(bool visited) {
            this.visited = visited;
        }

        public bool GetVisited() {
            return visited;
        }

        public abstract void AssignVisitedInSubtree(bool visited);

        public void SetSubstituted(bool substituted) {
            this.substituted = substituted;
        }

        public bool GetSubstituted() {
            return substituted;
        }

        public abstract void AssignSubstitutedInSubtree(bool substituted);

        public void SetID(int id) {
            this.id = id;
        }

        public int GetID() {
            return id;
        }

        public abstract void AssignIdInSubtree(int id);

        // Updates actions in each leaf. The left-most gets assigned the actions on position 0 in the list,
        // the second left-most leaf position 1 etc.
        public abstract void UpdateActionsInSubtree(List<HashSet<int>> actionsList);
    }
}