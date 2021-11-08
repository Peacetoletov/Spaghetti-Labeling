using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Spaghetti_Labeling
{
    public class Graph
    {
        private List<AbstractNode> roots;


        public Graph() {
            // Empty constructor, only used for testing
        }

        public Graph(List<Tree> forest) {

            /*
            Go through all node pairs (For each node of each tree, go thorugh all nodes of all trees) and
            check equality (NOT equivalence!). If the given subtrees are equal, replace the whole second
            subtree with the first subtree.
            This will be easier to implement if instead of replacing the current node, its children are replaced
            instead, as it will be clear if left subtree or right subtree should be replaced. This can be done 
            because the root node will never be replaced.
            */

            // Store the root of each tree
            this.roots = new List<AbstractNode>();
            foreach (Tree tree in forest) {
                this.roots.Add(tree.GetRoot());
            }

            // Merge equal subtrees
            MergeEqualSubtrees(forest);

            // Merge equivalent subtrees
            MergeEquivalentSubtrees(forest);
        }

        public void MergeEqualSubtrees(List<Tree> forest) {
            /* For each pair of inner nodes, check if their left (right) subtrees are equal. If so, replace 
            the left (right) subtree of the second node with the left (right) subtree of the first node.
            */
            List<AbstractNode> nodes = GetUniqueNodes(forest);
            for (int i = 0; i < nodes.Count - 1; i++) {
                for (int j = i + 1; j < nodes.Count; j++) {
                    if (nodes[i] is Leaf || nodes[j] is Leaf) {
                        // Skip leaves
                        continue;
                    }
                    Node node1 = (Node) nodes[i];
                    Node node2 = (Node) nodes[j];
                    if (node1.GetLeft().IsEqual(node2.GetLeft())) {
                        //Console.WriteLine("Left subtree of node " + i + " is equal to the left subtree of node " + j);
                        node2.SetLeft(node1.GetLeft());
                    }
                    if (node1.GetRight().IsEqual(node2.GetRight())) {
                        node2.SetRight(node1.GetRight());
                    }
                }
            }
        }

        public void MergeEquivalentSubtrees(List<Tree> forest) {
            // current TODO: create a tuple

            /* TODO
            1. Unroll each subtree into a string containing the tree structure, conditions in inner
               nodes and next tree indices in leaf nodes. The string does not contain actions. 
               DONE
            2. For each subtree, create an object containing the following:
                 a) the subtree as a string
                 b) pointer to the root node of the subtree
                 c) a list of sets of actions (one set for each leaf, with actions of the left-most
                   leaves being on the lowest positions in the list)
               DONE
            3. Add each object into a list, then sort the list primarily by the length of the string,
               secondarily lexicograhically.
               DONE
            4. Start going through the list. If the current string (i) has the "substituted" flag,
               shift the current string (i) by one. If the next string (j) has the "substituted" flag,
               shift the next string (j) by one. If the next string (j) is not equal to the current string,
               shift the current string (i) by one. If the current (i) and the next string (j) are equal, 
               check the intersection of their actions. If it's empty, shift the next string (j) by one.
               If the intersection is not empty, substitute the subtree associated with the next string (j)
               by the subtree associated with the current string (i). The process of substitution is
               described in 5.
               DONE 
            5. Substitution - change the tree strucutre, then assign the "substituted" flag of each node
               of the second subtree to true.
               DONE
            6. Keep going through the list until the end is reached.
               DONE (needs testing)
            */

            List<StringifiedTree> stringifiedTrees = CreateListOfStringifiedSubtrees(forest);
            /*
            Console.WriteLine("Before sorting");
            foreach (StringifiedTree st in stringifiedTrees) {
                Console.WriteLine(st.GetTree());
            }
            */
            stringifiedTrees.Sort();
            /*
            Console.WriteLine("After sorting");
            foreach (StringifiedTree st in stringifiedTrees) {
                Console.WriteLine(st.GetTree());
            }
            */

            // TODO: after implementing SubstituteSubtree (done), thoroughly manually test these nested loops

            for (int i = 0; i < stringifiedTrees.Count - 1; i++) {
                StringifiedTree st1 = stringifiedTrees[i];
                if (st1.GetRoot().GetSubstituted()) {
                    // Skip already substituted subtrees
                    continue;
                }
                for (int j = i + 1; j < stringifiedTrees.Count; j++) {
                    StringifiedTree st2 = stringifiedTrees[j];
                    // Skip already substituted subtrees
                    if (st2.GetRoot().GetSubstituted()) {
                        continue;
                    }
                    // Skip all subtrees with different strings
                    if (st1.GetTree() != st2.GetTree()) {
                        break;
                    }
                    // Skip subtrees with an empty intersection of actions 
                    List<HashSet<int>> intersectedActionsList = st1.IntersectedActions(st2.GetActions());
                    bool empty = false;
                    foreach (HashSet<int> actions in intersectedActionsList) {
                        if (actions.Count == 0) {
                            empty = true;
                        }
                    }
                    if (empty) {
                        continue;
                    }
                    // Two subtrees are compatible and one can be substituted with the other
                    SubstituteSubtree(st1.GetRoot(), st2.GetRoot(), intersectedActionsList);
                }    
            }
        }

        private void SubstituteSubtree(AbstractNode root1, AbstractNode root2,
                                       List<HashSet<int>> intersectedActionsList) {
            // root2 will be substituted with (replaced by) root1

            // Change the graph structure
            foreach (Node parent in root2.GetParents()) {
                if (root2.IsLeftChild(parent)) {
                    parent.SetLeft(root1);
                } else {
                    parent.SetRight(root1);
                }
            }

            // Set the 'substituted' flag of the root2 subtree to true
            root2.AssignSubstitutedInSubtree(true);

            // Update actions sets
            root1.UpdateActionsInSubtree(intersectedActionsList);
        }

        private List<StringifiedTree> CreateListOfStringifiedSubtrees(List<Tree> forest) {
            List<AbstractNode> nodes = GetUniqueNodes(forest);
            List<StringifiedTree> stringifiedTrees = new List<StringifiedTree>();

            foreach (AbstractNode node in nodes) {
                stringifiedTrees.Add(new StringifiedTree(node));
            }
            return stringifiedTrees;
        }

        private List<AbstractNode> GetUniqueNodes(List<Tree> forest) {
            // Returns a list containing all nodes in a forest. If a node belongs to multiple trees,
            // it is only added once.
            foreach (Tree tree in forest) {
                tree.GetRoot().AssignVisitedInSubtree(false);
            }
            List<AbstractNode> nodes = new List<AbstractNode>();
            foreach (Tree tree in forest) {
                List<AbstractNode> curTreeNodes = new List<AbstractNode>();
                AddUniqueNodesOfSubtreeToList(tree.GetRoot(), curTreeNodes);
                nodes.AddRange(curTreeNodes);
            }
            return nodes;
        }

        private void AddUniqueNodesOfSubtreeToList(AbstractNode abstractNode, List<AbstractNode> abstractNodes) {
            if (abstractNode.GetVisited()) {
                return;
            }
            abstractNode.SetVisited(true);
            abstractNodes.Add(abstractNode);
            if (abstractNode is Node) {
                Node node = (Node) abstractNode;
                AddUniqueNodesOfSubtreeToList(node.GetLeft(), abstractNodes);
                AddUniqueNodesOfSubtreeToList(node.GetRight(), abstractNodes);
            }
        }


        public static class Tests
        {
            public static void Run() {
                TestEqualSubtreeMerging();
                TestStringifiedTreeSorting();
                TestSubstituteSubtree();
            }

            private static void TestEqualSubtreeMerging() {
                Graph g = new Graph();
                Tree tree13 = TestTrees.Tree13();
                /* Tree 13:
                                    o
                         /                     \
                        i                       i
                   /         \             /         \
                 1-1         2-2          n          6-2   
                                        /   \
                                      4-1   5-1
                */
                Tree tree14 = TestTrees.Tree14();
                /* Tree 14:
                                    o
                         /                     \
                        i                       i
                   /         \             /         \
                 1-1         3-2          n          6-2      
                                        /   \
                                      4-1   5-1
                */
                List<Tree> forest = new List<Tree> {tree13, tree14};
                Debug.Assert(g.GetUniqueNodes(forest).Count == 18);
                g.MergeEqualSubtrees(forest);
                Debug.Assert(g.GetUniqueNodes(forest).Count == 12);

                Debug.Assert(!tree13.IsEqual(tree14));
                AbstractNode l13 = ((Node) tree13.GetRoot()).GetLeft();
                AbstractNode l14 = ((Node) tree14.GetRoot()).GetLeft();
                Debug.Assert(!l13.IsEqual(l14));
                AbstractNode ll13 = ((Node) l13).GetLeft();
                AbstractNode ll14 = ((Node) l14).GetLeft();
                Debug.Assert(ll13 == ll14);
                AbstractNode r13 = ((Node) tree13.GetRoot()).GetRight();
                AbstractNode r14 = ((Node) tree14.GetRoot()).GetRight();
                Debug.Assert(r13 == r14);
                
                //Console.WriteLine(":)");
            }

            private static void TestStringifiedTreeSorting() {
                Graph g = new Graph();
                List<StringifiedTree> stList = g.CreateListOfStringifiedSubtrees(new List<Tree> {TestTrees.Tree15()});
                /*
                                    k
                         /                     \
                        g                       i
                   /         \             /         \
                 1-1         2-2        3-3         4-4
                */
                stList.Sort();
                Debug.Assert(stList[0].GetTree() == "k(g(1)(2))(i(3)(4))");
                Debug.Assert(stList[1].GetTree() == "g(1)(2)");
                Debug.Assert(stList[2].GetTree() == "i(3)(4)");
                Debug.Assert(stList[3].GetTree() == "1");
                Debug.Assert(stList[4].GetTree() == "2");
                Debug.Assert(stList[5].GetTree() == "3");
                Debug.Assert(stList[6].GetTree() == "4");
            }

            private static void TestSubstituteSubtree() {
                Graph g = new Graph();
                Tree tree1 = TestTrees.Tree21();
                /*
                         a
                      /     \   
                 1,2-1       b
                           /   \
                    2,4,5-2     4-3
                */

                Tree tree2 = TestTrees.Tree22();
                /*
                              c
                           /     \   
                          b       1,2-1
                        /   \
                 4,5,6-2     4,5-3
                */

                StringifiedTree st1 = new StringifiedTree(((Node) tree1.GetRoot()).GetRight());
                StringifiedTree st2 = new StringifiedTree(((Node) tree2.GetRoot()).GetLeft());
                AbstractNode b = st1.GetRoot();
                AbstractNode toBeSubstituted = st2.GetRoot();
                g.SubstituteSubtree(b, toBeSubstituted, st1.IntersectedActions(st2.GetActions()));

                Debug.Assert(((Node) tree1.GetRoot()).GetRight() == b);
                Debug.Assert(((Node) tree2.GetRoot()).GetLeft() == b);
                Debug.Assert(b.GetParents().Count == 2);
                Debug.Assert(b.GetParents()[0] == tree1.GetRoot());
                Debug.Assert(b.GetParents()[1] == tree2.GetRoot());
                Leaf bLeft = (Leaf) ((Node) b).GetLeft();
                Leaf bRight = (Leaf) ((Node) b).GetRight();
                Debug.Assert(bLeft.GetActions().SetEquals(new HashSet<int> {4, 5}));
                Debug.Assert(bRight.GetActions().SetEquals(new HashSet<int> {4}));
                Debug.Assert(!b.GetSubstituted());
                Debug.Assert(!((Node) b).GetLeft().GetSubstituted());
                Debug.Assert(!((Node) b).GetRight().GetSubstituted());
                Debug.Assert(toBeSubstituted.GetSubstituted());
                Debug.Assert(((Node) toBeSubstituted).GetLeft().GetSubstituted());
                Debug.Assert(((Node) toBeSubstituted).GetRight().GetSubstituted());
                //Console.WriteLine("Poggies");
            }
        }
    }
}