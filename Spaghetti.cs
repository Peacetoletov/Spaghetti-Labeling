using System;

namespace Spaghetti_Labeling
{
    class Spaghetti
    {
        /* TODO
        1) Create a forest of reduced trees.
        2) Merge identical branches of each tree.
        3) Remove duplicates trees.
        4) ...
        */
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Bolelli!");
            
            //Node treeRoot = new ODTree().GetRoot();
            //treeRoot.InfoDFS();

            RunTests();
        }

        private static void RunTests() {
            Node.NodeTests.Run();
            Console.WriteLine("All tests passed!");
        }
    }
}
