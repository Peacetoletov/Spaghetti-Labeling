using System;

namespace Spaghetti_Labeling
{
    class Spaghetti
    {
        /* TODO
        1) Create a forest of reduced trees. DONE
        2) Merge identical branches of each tree. (method for merging DONE)
        3) Remove duplicates trees. DONE
        4) Add row beginning tree DONE
        5) Add row end trees 
        */
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Bolelli!");
            
            Node treeRoot = (Node) (ODTree.GetTree().GetRoot());
            //treeRoot.InfoDFS();

            RunTests();
        }

        private static void RunTests() {
            Node.Tests.Run();
            ForestManager.Tests.Run();
            Tree.Tests.Run();
            Console.WriteLine("All tests passed!");
        }
    }
}
