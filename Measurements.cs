/*
Created by Lukáš Osouch for bachelor's thesis Connected Component Labeling Using Directed Acyclic Graphs.
Masaryk University
2022
*/

using System;
using System.Diagnostics;

namespace Spaghetti_Labeling
{
    // Class for measuring time complexity and memory usage for all 3 labeling algorithms 
    // (classic, spaghetti, flood fill)
    public static class Measurements
    {
        public static void PrintPeakMemory() {
            Process currentProcess = Process.GetCurrentProcess();
            
            // Uncomment one of the algorithms and note down memory usage, then repeat this process
            // for other algorithms. Memory usage cannot be measured for multiple algorithms at once!

            /*
            Image randomImage = Image.TestImages.GenerateRandomImage(1000, 1000);
            Image spaghetti = ImageProcessor.SpaghettiCCL(randomImage, GraphManager.CreateManagers());
            //Image classic = ImageProcessor.ClassicCCL(randomImage);
            //Image flood = ImageProcessor.FloodFillCCL(randomImage);
            */

            //Image testImage = new Image("Test images/a_lines.png");
            //Image testImage = new Image("Test images/a_lol.png");
            Image testImage = new Image("Test images/a_tail.png");
            //Image spaghetti = ImageProcessor.SpaghettiCCL(testImage, GraphManager.CreateManagers());
            Image classic = ImageProcessor.ClassicCCL(testImage);
            //Image flood = ImageProcessor.FloodFillCCL(testImage);


            long peakMemory = currentProcess.PeakWorkingSet64;

            Console.WriteLine("Peak memory usage in kilobytes: {0}", peakMemory / 1000);
        }

        public static async void PrintAverageLabelingTime() {
            int numberOfImages = 10;
            Image[] randomImages = new Image[numberOfImages];
            for (int i = 0; i < numberOfImages; i++) {
                // randomImages[i] = Image.TestImages.GenerateRandomImage(1000, 1000);
                // Console.WriteLine("Generated random image {0}", i);
                if (i % 3 == 0) {
                    randomImages[i] = new Image("Test images/a_lines.png");
                } else if (i % 3 == 1) {
                    randomImages[i] = new Image("Test images/a_lol.png");
                } else {
                    randomImages[i] = new Image("Test images/a_tail.png");
                }
                Console.WriteLine("Loaded image {0}", i);
            }

            Console.WriteLine("Flood fill:");
            PrintAverageLabelingTime(ImageProcessor.FloodFillCCL, randomImages);
            Console.WriteLine("Classic:");
            PrintAverageLabelingTime(ImageProcessor.ClassicCCL, randomImages);
            Console.WriteLine("Spaghetti:");
            PrintAverageLabelingTimeSpaghetti(randomImages);
        }

        private static void PrintAverageLabelingTime(Func<Image, Image> CCL_Function, Image[] images) {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < images.Length; i++) {
                //labeled[i] = ImageProcessor.FloodFillCCL(randomImages[i]);
                CCL_Function(images[i]);
                Console.WriteLine("Finished labeling image {0}", i);
            }
            sw.Stop();

            Console.WriteLine("Elapsed time: {0}", sw.Elapsed);
            Console.WriteLine("Average time: {0}", sw.Elapsed / images.Length);
        }

        private static void PrintAverageLabelingTimeSpaghetti(Image[] images) {
            // Time necessary to create graphs does not count towards labeling time
            GraphManager[] gms = GraphManager.CreateManagers();

            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < images.Length; i++) {
                //labeled[i] = ImageProcessor.FloodFillCCL(randomImages[i]);
                ImageProcessor.SpaghettiCCL(images[i], gms);
                Console.WriteLine("Finished labeling image {0}", i);
            }
            sw.Stop();

            Console.WriteLine("Elapsed time: {0}", sw.Elapsed);
            Console.WriteLine("Average time: {0}", sw.Elapsed / images.Length);
        }
    }
}