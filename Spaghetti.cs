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
        8) First row labeling DONE
        9) Last row labeling DONE
        ...
        */

        private const bool testing = false;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello Bolelli!");

            /*
            CURRENT TODO: Clean up the code, optimize testing, test on big randomized images loaded from a file.

            UPDATE: Randomized images discovered new bugs in labeling. Fix ASAP. Reference image: BinaryImage15.
            */

            //Image image = ImageProcessor.SpaghettiCCL(Image.TestImages.GenerateRandomImage(10, 10, fileName: "test"));
            Image image = ImageProcessor.ClassicCCL(Image.TestImages.BinaryImage15());
            image.Print();

            //Image.TestImages.GenerateRandomImage(50, 50, fileName: "test");


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
