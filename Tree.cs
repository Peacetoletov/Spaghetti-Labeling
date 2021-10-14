using System;
using System.Diagnostics;

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
            return root.Equals(anotherTree.GetRoot()) && root.GetTree() == this && 
                   anotherTree.GetRoot().GetTree() == anotherTree;
        }
        
        // override object.GetHashCode
        public override int GetHashCode() {
            return base.GetHashCode();
        }

        public bool EqualsIgnoreLeafIndices(Tree anotherTree) {
        return root.EqualsIgnoreLeafIndices(anotherTree.GetRoot()) && root.GetTree() == this && 
               anotherTree.GetRoot().GetTree() == anotherTree;
        }

        public void InitNextTreeIndices() {
            root.InitNextTreeIndex(1);
        }

        public static class Tests
        {
            public static void Run() {
                TestInitNextTreeIndices();
            }

            private static void TestInitNextTreeIndices() {
                // Tests if each leaf has an index to a next tree, starting with 1 in the left-most leaf
                // and increasing by 1 in each next leaf (so the right-most leaf has index equal to the
                // number of leaves).
                Tree tree = TestTrees.Tree6();
                tree.InitNextTreeIndices();

                Node root = (Node) tree.GetRoot();
                Node l = (Node) root.GetLeft();
                Node r = (Node) root.GetRight();
                
                Node ll = (Node) l.GetLeft();
                Node lr = (Node) l.GetRight();

                Leaf lll = (Leaf) ll.GetLeft();
                Leaf llr = (Leaf) ll.GetRight();

                Leaf lrl = (Leaf) lr.GetLeft();
                Leaf lrr = (Leaf) lr.GetRight();

                Leaf rl = (Leaf) r.GetLeft();
                Leaf rr = (Leaf) r.GetRight();

                Debug.Assert(lll.GetNextTreeIndex() == 1);
                Debug.Assert(llr.GetNextTreeIndex() == 2);
                Debug.Assert(lrl.GetNextTreeIndex() == 3);
                Debug.Assert(lrr.GetNextTreeIndex() == 4);
                Debug.Assert(rl.GetNextTreeIndex() == 5);
                Debug.Assert(rr.GetNextTreeIndex() == 6);
            }
        }
    }
}