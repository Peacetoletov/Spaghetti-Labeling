using System;
using System.Collections.Generic;

namespace Spaghetti_Labeling
{
    // Class for leaf nodes
    public class Leaf : AbstractNode
    {
        private AbstractNode parent;

        // Set of possible actions
        private HashSet<int> actions;

        /*
        // Index of the next tree
        //private int nextTree;           // I might need to use a list (or nested lists), still unsure if an int suffices
        */

        // Pointer to the root of the next tree
        private AbstractNode nextTree;

        
        public Leaf(HashSet<int> actions) {
            this.actions = actions;
        }

        public override void InfoDFS() {
            Console.WriteLine("Leaf node " + GetName() + " with actions {" + string.Join(", ", actions) + "}");
        }
    }
}