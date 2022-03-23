/*
Created by Lukáš Osouch for bachelor's thesis Connected Component Labeling Using Directed Acyclic Graphs.
Masaryk University
2022
*/

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
        11) Final polish, comparison of different CCL approaches DONE
        */

        private const bool testing = false;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello Bolelli!");     // RIP

            //Image image = ImageProcessor.ClassicCCL(Image.TestImages.GenerateRandomImage(1000, 1000, fgProb: 0.3));
            // image.Save("Test images/b_saved");

            /*
            if (args.Length < 2 || args.Length > 3) {
                PrintUsage();
            } else if (args.Length == 2) {
                Image input = new Image(args[0]);
                Image labeled = ImageProcessor.SpaghettiCCL(input);
                labeled.Save(args[1]);
            } else {
                if (args[0] != "S" && args[0] != "T" && args[0] != "F") {
                    Console.WriteLine(@"Unrecognized labeling algorithm. Possible options:
                                     'S', 'T', 'F'.");
                    return;
                }
                Image input = new Image(args[1]);
                Image labeled = null;
                if (args[0] == "S") {
                    labeled = ImageProcessor.SpaghettiCCL(input);
                } else if (args[0] == "T") {
                    labeled = ImageProcessor.ClassicCCL(input);
                } else {
                    labeled = ImageProcessor.FloodFillCCL(input);
                }
                labeled.Save(args[2]);
            }
            */

            //Measurements.PrintPeakMemory();
            Measurements.PrintAverageLabelingTime();

            
            if (testing) {
                RunTests();
            } else {
                Console.WriteLine("WARNING: Tests are turned off.");
            }
            
                    
        }

        private static void PrintUsage() {
            Console.WriteLine(@"Usage: .\Spaghetti-Labeling [CCL algorithm] input_image output_image");
            Console.WriteLine(@"[CCL algorithm] is a voluntary argument which specifies what 
                              labeling algorithm will be used. Option 'S' is for the spaghetti 
                              algorithm and is used by default. Other options include 'T' for
                              two-pass algorithm with equivalence table and 'F' for flood fill
                              algorithm.");
            Console.WriteLine(@"input_image specifies the path to an existing image.");
            Console.WriteLine(@"output_image specifies the path where a labeled image will be 
                              created");
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
