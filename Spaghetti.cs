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
        5) Add row end trees DONE
        6) Convert trees to DRAGs DONE
        7) Actions, labeling, label equivalence resolution
        ...
        */
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Bolelli!");
            
            //Node treeRoot = (Node) (ODTree.GetTree().GetRoot());
            //treeRoot.InfoDFS();

            /*
            List<Tree> mainForest = ForestCreator.MainForest(ODTree.GetTree);
            List<(Tree, List<int>)> endForestEven = ForestCreator.EndForest(mainForest, true);
            List<(Tree, List<int>)> endForestOdd = ForestCreator.EndForest(mainForest, false);
            Graph mainGraph = new Graph(mainForest);
            */

            ImageProcessor.SpaghettiCCL(Image.TestImages.BinaryImage2().GetMatrix());

            RunTests();
        }

        private static void RunTests() {
            Node.Tests.Run();
            ForestCreator.Tests.Run();
            Tree.Tests.Run();
            Graph.Tests.Run();
            GraphManager.Tests.Run();
            StringifiedTree.Tests.Run();
            ImageProcessor.Tests.Run();
            Console.WriteLine("All tests passed!");
        }
    }
}
