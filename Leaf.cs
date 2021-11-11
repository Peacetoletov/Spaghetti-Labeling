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
        

        public Leaf(HashSet<int> actions) {
            this.actions = actions;
        }

        public HashSet<int> GetActions() {
            return actions;
        }

        public override bool IsEqual(AbstractNode abstractNode, bool showDebugInfo=false) {
            if (abstractNode == null || GetType() != abstractNode.GetType()) {
                return false;
            }
            
            if (showDebugInfo) {
                Console.WriteLine("In leaf " + GetName());
            }

            Leaf anotherLeaf = (Leaf) abstractNode;
            return actions.SetEquals(anotherLeaf.GetActions()) &&
                   nextTreeIndex == anotherLeaf.GetNextTreeIndex();
        }
        
        public override bool IsEqualIgnoringLeafIndices(AbstractNode abstractNode, bool showDebugInfo=false) {
            if (abstractNode == null || GetType() != abstractNode.GetType()) {
                return false;
            }
            
            Leaf anotherLeaf = (Leaf) abstractNode;
            return actions.SetEquals(anotherLeaf.GetActions());                   
        }

        public override void DFS_Rec() {
            Console.WriteLine("Leaf node " + GetName() + ": {" + string.Join(", ", actions) + "} - " + nextTreeIndex);
            //Console.WriteLine("Leaf node " + GetName() + " " + GetID() +  ": {" + string.Join(", ", actions) + "} - " + nextTreeIndex);
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

        public override void UpdateName(string newName) {
            SetName(newName);
        }

        public override int CountNodes() {
            return 1;
        }

        public override AbstractNode DeepCopy() {
            return new Leaf(new HashSet<int>(actions));
        }

        public override string Stringify() {
            if (nextTreeIndex != -1) {
                return nextTreeIndex.ToString();
            }
            return "";          // return empty string in end trees
        }

        public override void AssignVisitedInSubtree(bool visited) {
            SetVisited(visited);
        }

        public override void AssignSubstitutedInSubtree(bool substituted) {
            SetSubstituted(substituted);
        }

        public override void AssignIdInSubtree(int id) {
            SetID(id);
        }
        
        public override void UpdateActionsInSubtree(List<HashSet<int>> actionsList) {
            this.actions = actionsList[0];
            actionsList.RemoveAt(0);
        }
    }
}