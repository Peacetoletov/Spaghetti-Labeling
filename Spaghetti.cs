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

            // TODO 1: find out why ClassicCCL incorrectly labels BinaryImage16 and fix it.
            // UPDATE: I found out why it was happening and applied a band-aid. It's not completely fixed and cannot be used for cross-comparison
            // testing with SpaghettiCCL but currently has lower priority than TODO 2.

            // TODO 2: fix ActionPerformer

            // TODO 3: Fix ResolveLabelEquivalencies for good

            // TODO 4: do more testing, clean up the code

            /*
            UPDATE: Randomized images discovered new bugs in labeling. Fix ASAP. Reference image: BinaryImage15.
            UPDATE 2: BinaryImage16 seems to be producing weird results even in ClassicalCCL, wtf? this might be due to 
            some inconsistencies in equivalent label resolution. 
            UPDATE 3: Looks like I successfully reach the correct leaf, but the performing the given action gives results
            in incorrect labels. If I instead used the AND-decision table instead of the OR-decision table, I would get
            the correct label. Could this be the cause of the problem? I need to study the papers more. 
            UPDATE 4.1: According to the secret sauce, this appears to be the problem. TODO: Rewrite most of the ActionPerformer
            class because actions shouldn't be derived from the OR-table, rather from the AND-table.
            UPDATE 4.2: Actually, it's not the AND-table for the Rosenfeld mask, rather it is an OR-table for the BBDT mask.
            See https://iris.unimore.it/retrieve/handle/11380/630658/3377/TIP10.pdf (and write a comment about it in the
            ActionPerformer class).
            */

            //Image image = ImageProcessor.SpaghettiCCL(Image.TestImages.GenerateRandomImage(10, 10, fileName: "test"));
            
            
            //Image image1 = ImageProcessor.SpaghettiCCL(Image.TestImages.BinaryImage16());
            //image1.Print();
            

            //Image image2 = ImageProcessor.ClassicCCL(Image.TestImages.BinaryImage16());
            //image2.Print();


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
