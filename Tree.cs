using System;

namespace Spaghetti_Labeling
{
    public class Tree
    {
        private AbstractNode root;

        public Tree(AbstractNode root) {
            this.root = root;
            root.SetTree(this);
        }

        public AbstractNode GetRoot() {
            return root;
        }

        public void SetRoot(AbstractNode root) {
            // A tree's root can change when branches merge
            this.root = root;
        }

        // override object.Equals
        public override bool Equals(object obj) {
            if (obj == null || GetType() != obj.GetType()) {
                return false;
            }

            Tree anotherTree = (Tree) obj;            
            return root.Equals(anotherTree.GetRoot());
        }
        
        // override object.GetHashCode
        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }
}