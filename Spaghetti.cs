/*
Created by Lukáš Osouch for bachelor's thesis Connected Component Labeling Using Directed Acyclic Graphs.
Masaryk University
2022
*/

using System;


namespace Spaghetti_Labeling
{
    class Spaghetti
    {
        private const bool testing = false;

        static void Main(string[] args)
        {
            // Console.WriteLine("Hello Bolelli!");

            //Image image = ImageProcessor.SpaghettiCCL(Image.TestImages.GenerateRandomImage(1000, 1000, fgProb: 0.3), GraphManager.CreateManagers());
            //Image image = Image.TestImages.GenerateRandomImage(1000, 1000, fgProb: 0.3);
            //image.SaveAsBinary("Test images/c_random.png");

            
            if (args.Length < 2 || args.Length > 3) {
                PrintUsage();
            } 
            else {
                try {
                    if (args.Length == 2) {
                        Image input = new Image(args[0]);
                        Image labeled = ImageProcessor.SpaghettiCCL(input, GraphManager.CreateManagers());
                        labeled.Save(args[1]);
                    } else {
                        if (args[0] != "S" && args[0] != "T" && args[0] != "F") {
                            Console.WriteLine("Unrecognized labeling algorithm. Possible options: " +
                                              "'S', 'T', 'F'.");
                            return;
                        }
                        Image input = new Image(args[1]);
                        Image labeled = null;
                        if (args[0] == "S") {
                            labeled = ImageProcessor.SpaghettiCCL(input, GraphManager.CreateManagers());
                        } else if (args[0] == "T") {
                            labeled = ImageProcessor.ClassicCCL(input);
                        } else {
                            labeled = ImageProcessor.FloodFillCCL(input);
                        }
                        labeled.Save(args[2]);
                    }
                } catch (ArgumentException) {
                    Console.WriteLine("Input file does not exist!");
                } catch (ApplicationException) {
                    Console.WriteLine("Input file contains too many connected components, " +
                                      "cannot save as an RGB file.");
                }
            }
            

            //Measurements.PrintPeakMemory();
            //Measurements.PrintAverageLabelingTime();


            /*
            // Create images for the last chapter
            Image randomImage = Image.TestImages.GenerateRandomImage(1000, 1000, fgProb: 0.3);
            randomImage.SaveAsBinary("Output images/Original.png");
            Image imageFloodFill = ImageProcessor.FloodFillCCL(randomImage);
            Image imageClassic = ImageProcessor.ClassicCCL(randomImage);
            Image imageSpaghetti = ImageProcessor.SpaghettiCCL(randomImage, GraphManager.CreateManagers());
            imageFloodFill.Save("Output images/FloodFill.png");
            imageFloodFill.Save("Output images/Classic.png");
            imageFloodFill.Save("Output images/Spaghetti.png");
            */
            
            /*
            if (testing) {
                RunTests();
            } else {
                Console.WriteLine("WARNING: Tests are turned off.");
            }
            */
                    
        }

        private static void PrintUsage() {
            Console.WriteLine("Usage: ./Spaghetti-Labeling [CCL algorithm] input_image output_image");
            Console.WriteLine("[CCL algorithm] is a voluntary argument which specifies what labeling " +
                              "algorithm will be used. Option 'S' is for the spaghetti " +
                              "algorithm and is used by default. Other options include 'T' for " +
                              "two-pass algorithm with equivalence table and 'F' for flood fill " +
                              "algorithm.");
            Console.WriteLine("input_image specifies the name of an existing image.");
            Console.WriteLine("output_image specifies the name of the labeled image which will be " +
                              "created.");
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
