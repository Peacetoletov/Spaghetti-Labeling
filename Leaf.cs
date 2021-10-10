using System;
using System.Collections.Generic;

namespace Spaghetti_Labeling
{
    // Class for leaf nodes
    public class Leaf : AbstractNode
    {

        // Set of possible actions
        private HashSet<int> actions;

        /*
        // Index of the next tree
        //private int nextTree;           // I might need to use a list (or nested lists), still unsure if an int suffices
        */

        // Pointer to the root of the next tree
        private AbstractNode nextTree;

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
            
            Leaf root = (Leaf) obj;
            return actions.SetEquals(root.GetActions());
        }
        
        public override int GetHashCode() {
            return base.GetHashCode();
        }

        public override void InfoDFS() {
            Console.WriteLine("Leaf node " + GetName() + " with actions {" + string.Join(", ", actions) + "}");
        }
    }
}