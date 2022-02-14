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
        10) Test, read images from files, create an executable file DONE
        11) Final polish
        */

        private const bool testing = false;

        static void Main(string[] args)
        {
            // Console.WriteLine("Hello Bolelli!");     // RIP

            /*
            TODO: implement external binary image reading and create an executable file. 

            Creating the exe file: 
            https://docs.microsoft.com/en-us/dotnet/core/tutorials/publishing-with-visual-studio-code?pivots=dotnet-6-0
            */

            //Image input = new Image("Test images/pngImage.png");
            //Image labeled = ImageProcessor.SpaghettiCCL(input);
            //labeled.Print();

            if (args.Length == 0) {
                Console.WriteLine("Specify the path to the image to be labeled.");
            } else if (args.Length == 1) {
                Image input = new Image(args[0]);
                Image labeled = ImageProcessor.SpaghettiCCL(input);
                labeled.Print();
            } else {
                Console.WriteLine("Only 1 argument is allowed.");
            }


            /*
            if (testing) {
                RunTests();
            } else {
                Console.WriteLine("WARNING: Tests are turned off.");
            }
            */            
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
