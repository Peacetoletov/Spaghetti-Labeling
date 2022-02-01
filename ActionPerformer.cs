using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Spaghetti_Labeling
{
    // TODO: test this class

    /*
    This class contains methods that perform actions based on their identifying number (1-17). 
    */
    public class ActionPerformer
    {
        int highestLabel = 0;
        List<List<int>> inputMatrix;
        List<List<int>> outputMatrix;
        List<HashSet<int>> equivalentLabels;

        public ActionPerformer(Image input, Image output, List<HashSet<int>> equivalentLabels) {
            this.inputMatrix = input.GetMatrix();
            this.outputMatrix = output.GetMatrix();
            this.equivalentLabels = equivalentLabels;
        }

        public void Perform(int action, int x, int y, bool debug=false) {
            // Performs a given action on a given block and updates the output image and the list of equivalent labels
            switch (action) {
                case 1:
                    if (debug) {
                        Console.WriteLine("Doing nothing.");
                    }
                    break;      // No action
                case 2:
                    NewLabel(x, y, debug);
                    break;
                case 3:
                    Assign('p', x, y, debug);
                    break;
                case 4:
                case 7:
                case 10:
                case 11:
                case 13:
                case 14:
                case 16:
                case 17:
                    Assign('q', x, y, debug);
                    break;
                case 5:
                    Assign('r', x, y, debug);
                    break;
                case 6:
                case 9:
                    Assign('s', x, y, debug);
                    break;
                case 8:
                    Merge('p', x, y, debug);
                    break;
                case 12:
                case 15:
                    Merge('s', x, y, debug);
                    break;
                default:
                    throw new NotSupportedException("Critical error: action not found.");
            }
        }

        private void NewLabel(int x, int y, bool debug) {
            // Assigns a new label to the current block
            highestLabel++;
            LabelCurrentBlock(highestLabel, x, y);
            equivalentLabels.Add(new HashSet<int> {highestLabel});
            if (debug) {
                Console.WriteLine("Assigned new label: {0}", highestLabel);
            }
        }

        private void Assign(char block, int x, int y, bool debug) {
            // Assigns the label of the given block (p, q, r, s) to the current block (x)
            int label = GetLabelOfBlock(block, x, y);
            LabelCurrentBlock(label, x, y);
            if (debug) {
                Console.WriteLine("Assigned label {0} of block {1}", label, block);
            }
        }

        private void Merge(char block, int x, int y, bool debug) {
            // Merges labels on block r and the given block passed as argument (p, s), then
            // arbitrarily assigns one of them to the current block. 
            int label1 = GetLabelOfBlock('r', x, y);
            int label2 = GetLabelOfBlock(block, x, y);
            LabelCurrentBlock(label1, x, y);

            // If both labels are different and in different sets, join the sets together
            if (label1 != label2) {                
                (HashSet<int> setWithLabel1, HashSet<int> setWithLabel2) = ImageProcessor.FindSetsWithLabels(equivalentLabels, label1, label2);
                if (setWithLabel1 != setWithLabel2) {
                    setWithLabel1.UnionWith(setWithLabel2);
                    equivalentLabels.Remove(setWithLabel2);
                }
            }
            if (debug) {
                Console.WriteLine("Merged block r and {0}, assigned label {1}", block, label1);
            }
        }

        private int GetLabelOfBlock(char block, int x, int y) {
            int xOffset;
            int yOffset;
            switch (block) {
                case 'p':
                    xOffset = -2;
                    yOffset = -2;
                    break;
                case 'q':
                    xOffset = 0;
                    yOffset = -2;
                    break;
                case 'r':
                    xOffset = 2;
                    yOffset = -2;
                    break;
                case 's':
                    xOffset = -2;
                    yOffset = 0;
                    break;
                default:
                    throw new NotSupportedException("Critical error: block not found.");
            }
            for (int i = 0; i < 2; i++) {
                for (int j = 0; j < 2; j++) {
                    if (outputMatrix[y + yOffset + j][x + xOffset + i] != 0) {
                        return outputMatrix[y + yOffset + j][x + xOffset + i];
                    }
                }
            }
            throw new NotSupportedException("Critical error: tried to get the label of a block with no label.");
        }

        private void LabelCurrentBlock(int label, int x, int y) {
            for (int i = 0; i < 2; i++) {
                for (int j = 0; j < 2; j++) {
                    // The following condition must be here in case the input matrix contains an odd
                    // number of columns or rows
                    if (y + j < inputMatrix.Count && x + i < inputMatrix[y + j].Count) {
                        if (inputMatrix[y + j][x + i] == 1) {
                            outputMatrix[y + j][x + i] = label;
                        } 
                    }
                }
            }
        }
    }
}