namespace Spaghetti_Labeling
{
    public abstract class AbstractNode
    {
        private AbstractNode parent;
        private string name = "";    // only for testing


        public AbstractNode() {
            //this.name = "";
        }


        // Prints out information about the tree through DFS (only for testing)
        public abstract void InfoDFS();

        // Prints out information about the tree through inorder traversal (only for testing)
        public abstract void InfoInorder();
        
        public string GetName() {
            return name;
        }

        public void SetName(string name) {
            this.name = name;
        }

        public void SetParent(Node parent) {
            this.parent = parent;
        }
    }
}