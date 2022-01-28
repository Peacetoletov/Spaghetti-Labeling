using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Spaghetti_Labeling
{
    /*
    This class contains static methods that perform actions based on their identifying number (1-16). 
    */
    public static class Actions
    {
        public static void Perform(int action, int x, int y, Image output, List<HashSet<int>> equivalentLabels) {
            // Performs a given action on a given block and updates the output image and the list of equivalent labels
            switch (action) {
                case 1:
                    break;      // No action
                case 2:
                    NewLabel(x, y, output);
                    break;
                // TODO: this
            }
        }

        private static void NewLabel(int x, int y, Image output) {
            // TODO: this
        }
    }
}