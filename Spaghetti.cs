using System;
using System.Collections.Generic;

namespace Spaghetti_Labeling
{
    class Spaghetti
    {
        /* TODO
        1) Create a forest of reduced trees. DONE
        2) Merge identical branches of each tree. DONE
        3) Remove duplicates trees. DONE
        4) Add row beginning tree DONE
        5) Add row end trees
        6) Convert trees to DRAGs
        ...
        */
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Bolelli!");
            
            //Node treeRoot = (Node) (ODTree.GetTree().GetRoot());
            //treeRoot.InfoDFS();

            ForestManager fm = new ForestManager();
            List<Tree> mainForest = fm.MainForest(ODTree.GetTree);
            fm.EndForest(mainForest, true);
            mainForest = fm.MainForest(ODTree.GetTree);
            fm.EndForest(mainForest, false);

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
