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

            /*
            // purely for testing
            for (int i = 0; i < forest.Count; i++) {
                forest[i].GetRoot().AssignIdInSubtree(i + 1);
            }
            List<AbstractNode> nodes = GetUniqueNodes(forest);
            foreach (AbstractNode an in nodes) {
                Debug.Assert(an.GetID() != 666);
            }
            */

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
            /* Process of merging equivalent subtrees:
            1. Unroll each subtree into a string containing the tree structure, conditions in inner
               nodes and next tree indices in leaf nodes. The string does not contain actions. 
            2. For each subtree, create an object containing the following:
                 a) the subtree as a string
                 b) pointer to the root node of the subtree
                 c) a list of sets of actions (one set for each leaf, with actions of the left-most
                   leaves being on the lowest positions in the list)
            3. Add each object into a list, then sort the list primarily by the length of the string,
               secondarily lexicograhically.
            4. Start going through the list. If the current string (i) has the "substituted" flag,
               shift the current string (i) by one. If the next string (j) has the "substituted" flag,
               shift the next string (j) by one. If the next string (j) is not equal to the current string,
               shift the current string (i) by one. If the current (i) and the next string (j) are equal, 
               check the intersection of their actions. If it's empty, shift the next string (j) by one.
               If the intersection is not empty, substitute the subtree associated with the next string (j)
               by the subtree associated with the current string (i). The process of substitution is
               described in 5.
            5. Substitution - change the tree strucutre, then assign the "substituted" flag of each node
               of the second subtree to true.
            6. Keep going through the list until the end is reached.
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
            for (int i = 0; i < stringifiedTrees.Count; i++) {
                Console.WriteLine("Tree {0}: {1}", i, stringifiedTrees[i].GetTree());
            }
            */

            for (int i = 0; i < stringifiedTrees.Count - 1; i++) {
                StringifiedTree st1 = stringifiedTrees[i];
                if (st1.GetRoot().GetSubstituted()) {
                    // Skip already substituted primary subtrees
                    continue;
                }
                for (int j = i + 1; j < stringifiedTrees.Count; j++) {
                    StringifiedTree st2 = stringifiedTrees[j];
                    // Skip already substituted secondary subtrees
                    if (st2.GetRoot().GetSubstituted()) {
                        continue;
                    }
                    // Skip all subtrees with different strings
                    if (st1.GetTree() != st2.GetTree()) {
                        break;
                    }
                    // Skip subtrees with an empty intersection of actions 
                    List<HashSet<int>> intersectedActionsList = st1.IntersectedActions(st2.GetActions());
                    if (!ContainsEmptySet(intersectedActionsList)) {
                        // Two subtrees are compatible and one can be substituted with the other
                        SubstituteSubtree(st1.GetRoot(), st2.GetRoot(), intersectedActionsList);
                    }
                }    
            }

            // Remove references (in "parents" lists) to trees which were substituted away
            foreach (Tree tree in forest) {
                RemoveReferencesToSubstitutedTrees(tree.GetRoot());
            }
        }

        private bool ContainsEmptySet<T>(List<HashSet<T>> list) {
            foreach (HashSet<T> elem in list) {
                if (elem.Count == 0) {
                    return true;
                }
            }
            return false;
        }

        private void RemoveReferencesToSubstitutedTrees(AbstractNode abstractNode) {
            // Remove references (in "parents" lists of the given subtree) to trees which were substituted away
            List<Node> parents = abstractNode.GetParents(); 
            for (int i = 0; i < parents.Count; i++) {
                if (parents[i].GetSubstituted()) {
                    parents.RemoveAt(i);
                    i--;
                }
            }
            if (abstractNode is Node) {
                Node node = (Node) abstractNode;
                RemoveReferencesToSubstitutedTrees(node.GetLeft());
                RemoveReferencesToSubstitutedTrees(node.GetRight());
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

            /* If root1 and root2 had a common subtree, it will be marked as substituted, despite still being a valid
            subtree. To prevent this, all nodes in the root1 subtree will have the "substituted" flag set to false. */
            root1.AssignSubstitutedInSubtree(false);

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
                TestGraph();
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

            public static void TestGraph() {
                Tree tree23 = TestTrees.Tree23();
                Tree tree24 = TestTrees.Tree24();
                Tree tree25 = TestTrees.Tree25();

                Graph g = new Graph(new List<Tree> {tree23, tree24, tree25});
                Node a1 = (Node) tree23.GetRoot();
                Node b1 = (Node) a1.GetLeft();
                Node c1 = (Node) a1.GetRight();
                Node d1 = (Node) b1.GetLeft();
                Node e1 = (Node) b1.GetRight();
                Node f1 = (Node) c1.GetLeft();
                Node g1 = (Node) c1.GetRight();
                Node a2 = (Node) tree24.GetRoot();
                Node c2 = (Node) a2.GetRight();
                Node f2 = (Node) c2.GetLeft();
                Node a3 = (Node) tree25.GetRoot();
                Node b3 = (Node) a3.GetLeft();
                Node e3 = (Node) b3.GetRight();
                Leaf l1 = (Leaf) d1.GetLeft();
                Leaf l2 = (Leaf) d1.GetRight();
                Leaf l3 = (Leaf) e1.GetLeft();
                Leaf l4 = (Leaf) e1.GetRight();
                Leaf l5 = (Leaf) f1.GetLeft();
                Leaf l6 = (Leaf) f1.GetRight();
                Leaf l7 = (Leaf) g1.GetLeft();
                Leaf l8 = (Leaf) f2.GetRight();
                Leaf l9 = (Leaf) e3.GetLeft();
                Leaf l10 = (Leaf) e3.GetRight();

                // check if leaves have correct actions 
                Debug.Assert(l1.GetActions().SetEquals(new HashSet<int> {1}));
                Debug.Assert(l2.GetActions().SetEquals(new HashSet<int> {3, 4}));
                Debug.Assert(l3.GetActions().SetEquals(new HashSet<int> {1}));
                Debug.Assert(l4.GetActions().SetEquals(new HashSet<int> {2}));
                Debug.Assert(l5.GetActions().SetEquals(new HashSet<int> {6}));
                Debug.Assert(l6.GetActions().SetEquals(new HashSet<int> {5}));
                Debug.Assert(l7.GetActions().SetEquals(new HashSet<int> {7}));
                Debug.Assert(l8.GetActions().SetEquals(new HashSet<int> {7}));
                Debug.Assert(l9.GetActions().SetEquals(new HashSet<int> {1}));
                Debug.Assert(l10.GetActions().SetEquals(new HashSet<int> {3}));

                // TODO: check if the structure exactly matches 
                Debug.Assert(a1.GetLeft() == b1);
                Debug.Assert(a1.GetRight() == c1);
                Debug.Assert(a1.GetParents().Count == 0);
                Debug.Assert(b1.GetLeft() == d1);
                Debug.Assert(b1.GetRight() == e1);
                Debug.Assert(MatchingParents(b1, new List<AbstractNode> {a1, a2}));
                Debug.Assert(c1.GetLeft() == f1);
                Debug.Assert(c1.GetRight() == g1);
                Debug.Assert(MatchingParents(c1, new List<AbstractNode> {a1}));
                Debug.Assert(d1.GetLeft() == l1);
                Debug.Assert(d1.GetRight() == l2);
                Debug.Assert(MatchingParents(d1, new List<AbstractNode> {b1, b3}));
                Debug.Assert(e1.GetLeft() == l3);
                Debug.Assert(e1.GetRight() == l4);
                Debug.Assert(MatchingParents(e1, new List<AbstractNode> {b1}));
                Debug.Assert(f1.GetLeft() == l5);
                Debug.Assert(f1.GetRight() == l6);
                Debug.Assert(MatchingParents(f1, new List<AbstractNode> {c1}));
                Debug.Assert(g1.GetLeft() == l7);
                Debug.Assert(g1.GetRight() == l3);
                Debug.Assert(MatchingParents(l1, new List<AbstractNode> {d1}));
                Debug.Assert(MatchingParents(l2, new List<AbstractNode> {d1}));
                Debug.Assert(MatchingParents(l3, new List<AbstractNode> {e1, g1}));
                Debug.Assert(MatchingParents(l4, new List<AbstractNode> {e1}));
                Debug.Assert(MatchingParents(l5, new List<AbstractNode> {f1, f2}));
                Debug.Assert(MatchingParents(l6, new List<AbstractNode> {f1}));
                Debug.Assert(MatchingParents(l7, new List<AbstractNode> {g1, c2}));
                Debug.Assert(a2.GetLeft() == b1);
                Debug.Assert(a2.GetRight() == c2);
                Debug.Assert(a2.GetParents().Count == 0);
                Debug.Assert(c2.GetLeft() == f2);
                Debug.Assert(c2.GetRight() == l7);
                Debug.Assert(MatchingParents(c2, new List<AbstractNode> {a2, a3}));
                Debug.Assert(f2.GetLeft() == l5);
                Debug.Assert(f2.GetRight() == l8);
                Debug.Assert(MatchingParents(f2, new List<AbstractNode> {c2}));
                Debug.Assert(MatchingParents(l8, new List<AbstractNode> {f2}));
                Debug.Assert(a3.GetLeft() == b3);
                Debug.Assert(a3.GetRight() == c2);
                Debug.Assert(a3.GetParents().Count == 0);
                Debug.Assert(b3.GetLeft() == d1);
                Debug.Assert(b3.GetRight() == e3);
                Debug.Assert(MatchingParents(b3, new List<AbstractNode> {a3}));
                Debug.Assert(e3.GetLeft() == l9);
                Debug.Assert(e3.GetRight() == l10);
                Debug.Assert(MatchingParents(e3, new List<AbstractNode> {b3}));
                Debug.Assert(MatchingParents(l9, new List<AbstractNode> {e3}));
                Debug.Assert(MatchingParents(l10, new List<AbstractNode> {e3}));

                //Console.WriteLine("Yay!");
            }

            private static bool MatchingParents(AbstractNode node, List<AbstractNode> parents) {
                // This method checks if the parents of "node" exactly match the content of
                // the "parents" list, while ignoring the order in both lists

                if (node.GetParents().Count != parents.Count) {
                    return false;
                }                
                
                for (int i = 0; i < node.GetParents().Count; i++) {
                    bool matching = false;
                    for (int j = 0; j < parents.Count; j++) {
                        if (node.GetParents()[i] == parents[j]) {
                            //Console.WriteLine("parent {0} matches parent {1}", i, j);
                            matching = true;
                        }
                        else {
                            //Console.WriteLine("parent {0} does not match parent {1}", i, j);
                        }
                    }
                    if (!matching) {
                        return false;
                    }
                }

                return true;
            }
        }
    }
}