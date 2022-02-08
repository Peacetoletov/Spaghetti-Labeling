﻿using System;
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

        private const bool testing = true;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello Bolelli!");

            /*
            TODO: clean up ForestCreator and do more tests.
            */

            //Image image = ImageProcessor.SpaghettiCCL(Image.TestImages.GenerateRandomImage(11, 9, fileName: "test"));
            //image.Print();

            //Image image = ImageProcessor.SpaghettiCCL(Image.TestImages.BinaryImage18());
            //image.Print();

            
            //(List<Tree> forest, int _) = ForestCreator.MainForestFirstRow(ODTree.GetTree());
            /*
            for (int i = 0; i < forest.Count; i++) {
                Console.WriteLine("Tree {0}:", i + 1);
                forest[i].GetRoot().InfoDFS();
            }
            */
            //forest[0].GetRoot().InfoDFS(); 

            /*
            GraphManager gm1 = new GraphManager(GraphManager.GraphType.FirstRow);
            GraphManager gm2 = new GraphManager(GraphManager.GraphType.MiddleRows);
            GraphManager gm3 = new GraphManager(GraphManager.GraphType.LastRow);
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
