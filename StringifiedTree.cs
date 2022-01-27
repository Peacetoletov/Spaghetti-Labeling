using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Spaghetti_Labeling
{
    public class StringifiedTree : IComparable
    {
        private readonly string tree;
        private readonly AbstractNode root;
        private readonly List<HashSet<int>> actions;
        public StringifiedTree(AbstractNode abstractNode) {
            this.tree = abstractNode.Stringify();
            this.root = abstractNode;
            this.actions = abstractNode.GatherActions();
            //AddActionsToList(abstractNode, actions);
        }

        /*
        private void AddActionsToList(AbstractNode abstractNode, List<HashSet<int>> actions) {
            if (abstractNode is Leaf) {
                actions.Add(((Leaf) abstractNode).GetActions());
            } else {
                AddActionsToList(((Node) abstractNode).GetLeft(), actions); 
                AddActionsToList(((Node) abstractNode).GetRight(), actions);
            }
        }
        */

        public int CompareTo(object obj) {
            if (obj == null) {
                return -1;
            }

            StringifiedTree other = obj as StringifiedTree;
            if (tree.Length > other.GetTree().Length) {
                return -1;
            }
            if (tree.Length < other.GetTree().Length) {
                return 1;
            }
            // Same length, sort lexicographically
            for (int i = 0; i < tree.Length; i++) {
                if (tree[i] < other.GetTree()[i]) {
                    return -1;
                }
                if (tree[i] > other.GetTree()[i]) {
                    return 1;
                }
            }
            return 0;
        }

        public List<HashSet<int>> IntersectedActions(List<HashSet<int>> otherActions) {
            // Returns a new list containing intersections of this tree's actions with another tree's actions
            Debug.Assert(actions.Count == otherActions.Count);

            List<HashSet<int>> intersected = new List<HashSet<int>>();
            for (int i = 0; i < actions.Count; i++) {
                HashSet<int> newSet = new HashSet<int>(actions[i]);
                newSet.IntersectWith(otherActions[i]);
                intersected.Add(newSet);
            }
            return intersected;
        }

        public string Name() {
            if (root is Node) {
                return ((Node) root).GetCondition().ToString() + root.GetID().ToString();
            }
            Leaf leaf =  ((Leaf) root);
            return "{" + string.Join(", ", leaf.GetActions()) + "} - " + leaf.GetNextTreeIndex();
        }

        public string GetTree() {
            return tree;
        }

        public AbstractNode GetRoot() {
            return root;
        } 

        public List<HashSet<int>> GetActions() {
            return actions;
        }

        public static string GetActionsListAsString(List<HashSet<int>> actionsList) {
            // Used for debugging
            string actionsAsString = "";
            foreach (HashSet<int> actionsSet in actionsList) {
                actionsAsString += "{";
                foreach (int action in actionsSet) {
                    actionsAsString += action;
                    actionsAsString += ", ";
                }
                actionsAsString = actionsAsString.Remove(actionsAsString.Length - 2, 2);
                actionsAsString += "}, ";
            }
            actionsAsString = actionsAsString.Remove(actionsAsString.Length - 2, 2);
            return actionsAsString;
        }

        public class Tests
        {
            public static void Run() {
                TestConstructor();
                TestIntersectActions();
            }

            private static void TestConstructor() {
                /*
                                    o
                         /                     \
                        i                       i
                   /         \             /         \
                 1-1         2-2          n          6-2   
                                        /   \
                                      4-1   5-1
                */
                Tree tree13 = TestTrees.Tree13();
                StringifiedTree st = new StringifiedTree(tree13.GetRoot());

                Debug.Assert(st.GetTree() == "o(i(1)(2))(i(n(1)(1))(2))");
                Debug.Assert(st.root == tree13.GetRoot());
                Debug.Assert(st.GetActions().Count == 5);
                Debug.Assert(st.GetActions()[0].SetEquals(new HashSet<int> {1}));
                Debug.Assert(st.GetActions()[1].SetEquals(new HashSet<int> {2}));
                Debug.Assert(st.GetActions()[2].SetEquals(new HashSet<int> {4}));
                Debug.Assert(st.GetActions()[3].SetEquals(new HashSet<int> {5}));
                Debug.Assert(st.GetActions()[4].SetEquals(new HashSet<int> {6}));


                /*
                       a
                     /   \   
                  1-1     2,3-2
                */
                Tree tree19 = TestTrees.Tree19();
                StringifiedTree st2 = new StringifiedTree(tree19.GetRoot());

                Debug.Assert(st2.GetTree() == "a(1)(2)");
                Debug.Assert(st2.root == tree19.GetRoot());
                Debug.Assert(st2.GetActions().Count == 2);
                Debug.Assert(st2.GetActions()[0].SetEquals(new HashSet<int> {1}));
                Debug.Assert(st2.GetActions()[1].SetEquals(new HashSet<int> {2, 3}));
            }

            private static void TestIntersectActions() {
                Tree tree20 = TestTrees.Tree20();
                Tree tree21 = TestTrees.Tree21();
                StringifiedTree st1 = new StringifiedTree(tree20.GetRoot());
                StringifiedTree st2 = new StringifiedTree(tree21.GetRoot());
                List<HashSet<int>> intersected = st1.IntersectedActions(st2.GetActions());

                Debug.Assert(intersected.Count == 3);
                Debug.Assert(intersected[0].SetEquals(new HashSet<int> {1}));
                Debug.Assert(intersected[1].SetEquals(new HashSet<int> {2, 4}));
                Debug.Assert(intersected[2].SetEquals(new HashSet<int> {}));
                //Console.WriteLine("Hi Life");
            }
        }
    }
}