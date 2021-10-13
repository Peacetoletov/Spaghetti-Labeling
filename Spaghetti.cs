using System;

namespace Spaghetti_Labeling
{
    class Spaghetti
    {
        /* TODO
        1) Create a forest of reduced trees.
        2) Merge identical branches of each tree. (method for merging DONE)
        3) Remove duplicates trees.
        4) ...
        */
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Bolelli!");
            
            Node treeRoot = (Node) (new ODTree().GetTree().GetRoot());
            //treeRoot.InfoDFS();

            RunTests();
        }

        private static void RunTests() {
            Node.Tests.Run();
            ForestManager.Tests.Run();
            Console.WriteLine("All tests passed!");
        }
    }
}
