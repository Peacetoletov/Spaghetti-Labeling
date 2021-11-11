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
            for (int i = 0; i < stringifiedTrees.Count; i++) {
                Console.WriteLine("Tree {0}: {1}", i, stringifiedTrees[i].GetTree());
            }
            */

            // TODO: write automatic tests for this function... it won't be easy but I need to make sure this 
            // really works

            for (int i = 0; i < stringifiedTrees.Count - 1; i++) {
                StringifiedTree st1 = stringifiedTrees[i];
                if (st1.GetRoot().GetSubstituted()) {
                    // Skip already substituted subtrees
                    //Console.WriteLine("Skipping primary tree {0} because it was already substituted.", i);
                    continue;
                }
                for (int j = i + 1; j < stringifiedTrees.Count; j++) {
                    StringifiedTree st2 = stringifiedTrees[j];
                    // Skip already substituted subtrees
                    if (st2.GetRoot().GetSubstituted()) {
                        //Console.WriteLine("Skipping secondary tree {0} because it was already substituted.", j);
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
                        /*
                        Console.WriteLine("Skipping trees {0}, {1} because they have empty actions intersection.", i, j);
                        Console.WriteLine("1st tree: {0}", st1.GetTree());
                        Console.WriteLine("2nd tree: {0}", st2.GetTree());
                        */
                        continue;
                    }
                    // Two subtrees are compatible and one can be substituted with the other
                    /*
                    Console.WriteLine("Substituting tree {0} with tree {1}", j, i);
                    Console.WriteLine("1st tree: {0}", st1.GetTree());
                    Console.WriteLine("2nd tree: {0}", st2.GetTree());
                    */
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
                // TODO: this. First create a decently sized test forest, then create a graph from it
                // and check the results.

                // TODO: fix bugs. For some reason, l1 has two parents, and they are both different nodes.
                // I can investigate this further by checking the parents of these parents to see which trees
                // they belong to. The current hypothesis is that smaller substitutions are made before
                // larger ones and this causes problems, however I can't imagine how this could happen
                // because I sort the stringified trees before making any substitutions.
                // UPDATE: Maybe it gets first merged because of equality (not equivalence) and this causes
                // problems with subsequent substitutions?
                // BETTER HYPOTHESIS: I'm pretty sure I figured out at least part of the problem. In the graph
                // creation process, I first merge equal subtrees, which may lead to a leaf having two
                // parents. Then, when performing equivalence substitutions, a subtree leading to that leaf
                // may get substituted away in a node above the leaf's parents, which leads to the leaf
                // retaining both parents in the "parents" list, one of which will have the "substituted" flag
                // set to true. This can be easily fixed by going through each node of the complete graph
                // and removing those nodes from the "parents" lists, which have the flag set to true.

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
                Node c3 = (Node) a3.GetRight();
                Node e3 = (Node) b3.GetRight();
                Node f3 = (Node) c3.GetLeft();
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
                /*
                Debug.Assert(((Leaf) d1.GetLeft()).GetActions().SetEquals(new HashSet<int> {1}));
                Debug.Assert(((Leaf) d1.GetRight()).GetActions().SetEquals(new HashSet<int> {3, 4}));
                Debug.Assert(((Leaf) e1.GetLeft()).GetActions().SetEquals(new HashSet<int> {1}));
                Debug.Assert(((Leaf) e1.GetRight()).GetActions().SetEquals(new HashSet<int> {2}));
                Debug.Assert(((Leaf) f1.GetLeft()).GetActions().SetEquals(new HashSet<int> {6}));
                Debug.Assert(((Leaf) f1.GetRight()).GetActions().SetEquals(new HashSet<int> {5}));
                Debug.Assert(((Leaf) g1.GetLeft()).GetActions().SetEquals(new HashSet<int> {7}));
                Debug.Assert(((Leaf) f2.GetRight()).GetActions().SetEquals(new HashSet<int> {7}));
                Debug.Assert(((Leaf) e3.GetLeft()).GetActions().SetEquals(new HashSet<int> {1}));
                Debug.Assert(((Leaf) e3.GetRight()).GetActions().SetEquals(new HashSet<int> {3}));

                // TODO: check if the structure exactly matches 
                // TODO: uncomment every assert
                Debug.Assert(a1.GetLeft() == b1);
                Debug.Assert(a1.GetRight() == c1);
                Debug.Assert(a1.GetParents().Count == 0);
                Debug.Assert(b1.GetLeft() == d1);
                Debug.Assert(b1.GetLeft() == d1);
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
                //Debug.Assert(g1.GetRight() == l3);
                Debug.Assert(MatchingParents(l1, new List<AbstractNode> {d1}));
                Debug.Assert(MatchingParents(l2, new List<AbstractNode> {d1}));
                Debug.Assert(MatchingParents(l3, new List<AbstractNode> {e1, g1}));
                Debug.Assert(MatchingParents(l4, new List<AbstractNode> {e1}));
                Debug.Assert(MatchingParents(l5, new List<AbstractNode> {f1, f2, f3}));
                Debug.Assert(MatchingParents(l6, new List<AbstractNode> {f1}));
                Debug.Assert(MatchingParents(l7, new List<AbstractNode> {g1, c2, c3}));
                Debug.Assert(a2.GetLeft() == b1);
                Debug.Assert(a2.GetRight() == c2);
                Debug.Assert(a2.GetParents().Count == 0);
                Debug.Assert(c2.GetLeft() == f2);
                Debug.Assert(c2.GetRight() == l7);
                Debug.Assert(MatchingParents(c2, new List<AbstractNode> {a2}));
                Debug.Assert(f2.GetLeft() == l5);
                Debug.Assert(f2.GetRight() == l8);
                Debug.Assert(MatchingParents(f2, new List<AbstractNode> {c2}));
                Debug.Assert(MatchingParents(l8, new List<AbstractNode> {f2, f3}));
                Debug.Assert(a3.GetLeft() == b3);
                Debug.Assert(a3.GetRight() == c3);
                Debug.Assert(a3.GetParents().Count == 0);
                Debug.Assert(b3.GetLeft() == d1);
                Debug.Assert(b3.GetRight() == e3);
                Debug.Assert(MatchingParents(b3, new List<AbstractNode> {a3}));
                Debug.Assert(c3.GetLeft() == f3);
                Debug.Assert(c3.GetRight() == l7);
                Debug.Assert(MatchingParents(c3, new List<AbstractNode> {a3}));
                Debug.Assert(e3.GetLeft() == l9);
                Debug.Assert(e3.GetRight() == l10);
                Debug.Assert(MatchingParents(e3, new List<AbstractNode> {b3}));
                Debug.Assert(f3.GetLeft() == l5);
                Debug.Assert(f3.GetRight() == l8);
                Debug.Assert(MatchingParents(f3, new List<AbstractNode> {c3}));
                Debug.Assert(MatchingParents(l9, new List<AbstractNode> {e3}));
                Debug.Assert(MatchingParents(l10, new List<AbstractNode> {e3}));
                */

                foreach (AbstractNode p in l1.GetParents()) {
                    Console.WriteLine("\nDFS on parent of l1:");
                    p.AssignVisitedInSubtree(false);
                    p.InfoDFS();
                }
                Console.WriteLine("Are both l1 parent equal? {0}", l1.GetParents()[0] == l1.GetParents()[1]);

                Console.WriteLine("pls");
            }

            private static bool MatchingParents(AbstractNode node, List<AbstractNode> parents) {
                // This method checks if the parents of "node" exactly match the content of
                // the "parents" list, while ignoring the order in both lists

                /*
                Console.WriteLine("DFS on node:");
                node.AssignVisitedInSubtree(false);
                node.InfoDFS();

                foreach (AbstractNode p in parents) {
                    Console.WriteLine("\nDFS on parent:");
                    p.AssignVisitedInSubtree(false);
                    p.InfoDFS();
                }
                */

                Console.WriteLine("New call");

                if (node.GetParents().Count != parents.Count) {
                    Console.WriteLine("List lengths are not equal! Node parents length: {0}. Supposed length: {1}",
                                      node.GetParents().Count, parents.Count);
                    return false;
                }
                /*
                foreach (AbstractNode p1 in node.GetParents()) {
                    bool matching = false;
                    foreach (AbstractNode p2 in parents) {
                        if (p1 == p2) {
                            matching = true;
                        }
                    }
                    if (!matching) {
                        return false;
                    }
                }
                */
                
                
                for (int i = 0; i < node.GetParents().Count; i++) {
                    bool matching = false;
                    for (int j = 0; j < parents.Count; j++) {
                        if (node.GetParents()[i] == parents[j]) {
                            Console.WriteLine("parent {0} matches parent {1}", i, j);
                            matching = true;
                        }
                        else {
                            Console.WriteLine("parent {0} does not match parent {1}", i, j);
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