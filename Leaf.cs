using System;
using System.Collections.Generic;

namespace Spaghetti_Labeling
{
    // Class for leaf nodes
    public class Leaf : AbstractNode
    {

        // Set of possible actions
        private HashSet<int> actions;

        
        // Index of the next tree
        private int nextTreeIndex = -1;         // -1 represents no tree
        

        // Pointer to the root of the next tree
        //private Tree nextTree;

        public Leaf(HashSet<int> actions, Tree tree) {
            this.actions = actions;
        }
        
        public Leaf(HashSet<int> actions) {
            this.actions = actions;
        }

        public HashSet<int> GetActions() {
            return actions;
        }

        public override bool Equals(object obj) {
            if (obj == null || GetType() != obj.GetType()) {
                return false;
            }
            
            Leaf anotherLeaf = (Leaf) obj;
            return actions.SetEquals(anotherLeaf.GetActions()) &&
                   nextTreeIndex == anotherLeaf.GetNextTreeIndex();
        }
        
        public override int GetHashCode() {
            return base.GetHashCode();
        }

        public override bool EqualsIgnoreLeafIndices(object obj) {
            if (obj == null || GetType() != obj.GetType()) {
                return false;
            }
            
            Leaf anotherLeaf = (Leaf) obj;
            return actions.SetEquals(anotherLeaf.GetActions());                   
        }

        public override void InfoDFS() {
            Console.WriteLine("Leaf node " + GetName() + ": {" + string.Join(", ", actions) + "} - " + nextTreeIndex);
        }

        public override int InitNextTreeIndex(int index) {
            // Sets an initial index from 1 to numberOfLeaves to each leaf in ascending order starting
            // from the left-most leaf. Each index represents the next tree to be used
            this.nextTreeIndex = index;
            return index + 1;
        }

        public override void AdjustNextTreeIndicesAfterDeletion(int indexOfEqualTree, int indexOfDeletedTree) {
            if (nextTreeIndex == indexOfDeletedTree) {
                this.nextTreeIndex = indexOfEqualTree;
            }
            else if (nextTreeIndex > indexOfDeletedTree) {
                this.nextTreeIndex--;
            }
            // Do nothing if nextTreeIndex < indexOfDeletedTree
        }

        public int GetNextTreeIndex() {
            return nextTreeIndex;
        }

        public void SetNextTreeIndex(int index) {
            this.nextTreeIndex = index;
        }

        public override void MergeIdenticalBranches() {
            return;
        }
    }
}