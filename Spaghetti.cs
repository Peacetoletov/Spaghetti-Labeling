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
        7) Actions, labeling, label equivalence resolution DONE
        8) First row labeling
        9) Last row labeling
        ...
        */

        private const bool testing = false;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello Bolelli!");

            //(List<Tree> forest, int firstTreeIndex) = ForestCreator.MainForestFirstRow(ODTree.GetTree);
            //Console.WriteLine("MainForestFirstRow contains {0} trees. Index of first tree: {1}", forest.Count, firstTreeIndex);
            //Console.WriteLine("First tree:");
            //forest[firstTreeIndex - 1].GetRoot().InfoDFS();

            Image image = ImageProcessor.SpaghettiCCL(Image.TestImages.BinaryImage5());
            image.Print();

            //GraphManager gmFirst = new GraphManager(GraphManager.GraphType.FirstRow);

            /*
            TODO: Implement end tree creation for first row forest (should be easy, maybe it requires no changes and I can reuse the existing methods,
            just some renaming may be necessary).

            UPDATE: The simple test (BinaryImage5) is failing. Find out why and fix the issue.
            UPDATE 2: A slightly modified version of the image (BinaryImage6) is working correctly. This means that the problem is
                not in the end trees in general, rather end trees specifically in the first row. 
            */

            if (testing) {
                RunTests();
            } else {
                Console.WriteLine("WARNING: Tests are turned off.");
            }            
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
