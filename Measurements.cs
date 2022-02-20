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

            Image randomImage = Image.TestImages.GenerateRandomImage(100, 100);
            Image spaghetti = ImageProcessor.SpaghettiCCL(randomImage);
            //Image classic = ImageProcessor.ClassicCCL(randomImage);
            //Image flood = ImageProcessor.FloodFillCCL(randomImage);


            long peakMemory = currentProcess.PeakWorkingSet64;

            Console.WriteLine("Peak memory usage in kilobytes: {0}", peakMemory / 1000);
        }

        public static void PrintAverageLabelingTime() {
            // TODO: this
            /* UPDATE: resolving label equivalencies is extremely inefficient and renders measuring time
               complexity pointless.
               When I commented out everything related to equivalent labels, the labeling time went down
               significantly. 
               I have no idea how to fix this so it will probably be best to just note it down and move on.
               */
        }
    }
}