using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Spaghetti_Labeling
{
    /*
    This class contains methods that perform actions based on their identifying number (1-16). 

    For more information about actions, see Optimized Block-Based Connected Components Labeling With 
    Decision Trees (Costantino Grana, Member, IEEE, Daniele Borghesani, and Rita Cucchiara, Member, IEEE)
    */
    public class ActionPerformer
    {
        int highestLabel = 0;
        List<List<int>> inputMatrix;
        List<List<int>> outputMatrix;
        Dictionary<int, HashSet<int>> equivalentLabels;

        public ActionPerformer(Image input, Image output, Dictionary<int, HashSet<int>> equivalentLabels) {
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
                    Assign('q', x, y, debug);
                    break;
                case 5:
                    Assign('r', x, y, debug);
                    break;
                case 6:
                    Assign('s', x, y, debug);
                    break;
                case 7:
                    Merge(new char[] {'p', 'q'}, x, y, debug);
                    break;
                case 8:
                    Merge(new char[] {'p', 'r'}, x, y, debug);
                    break;
                case 9:
                    Merge(new char[] {'p', 's'}, x, y, debug);
                    break;
                case 10:
                    Merge(new char[] {'q', 'r'}, x, y, debug);
                    break;
                case 11:
                    Merge(new char[] {'q', 's'}, x, y, debug);
                    break;
                case 12:
                    Merge(new char[] {'r', 's'}, x, y, debug);
                    break;
                // Action 13 is not used
                case 14:
                    Merge(new char[] {'p', 'q', 's'}, x, y, debug);
                    break;
                case 15:
                    Merge(new char[] {'p', 'r', 's'}, x, y, debug);
                    break;
                case 16:
                    Merge(new char[] {'q', 'r', 's'}, x, y, debug);
                    break;
                default:
                    throw new NotSupportedException("Critical error: action not found.");
            }
        }

        private void NewLabel(int x, int y, bool debug) {
            // Assigns a new label to the current block
            highestLabel++;
            LabelCurrentBlock(highestLabel, x, y);
            equivalentLabels[highestLabel] = new HashSet<int> {highestLabel};
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

        private void Merge(char[] blocks, int x, int y, bool debug) {
            // Arbitrarily assigns the label of the first block to the current block, then merges labels
            // of all given blocks
            HashSet<int> neighboringLabels = new HashSet<int>(); 
            foreach (char block in blocks) {
                neighboringLabels.Add(GetLabelOfBlock(block, x, y));
            }
            List<int> labels = new List<int>(neighboringLabels);
            LabelCurrentBlock(labels[0], x, y);
            for (int i = 1; i < labels.Count; i++) {
                equivalentLabels[labels[0]].Add(labels[i]);
                equivalentLabels[labels[i]].Add(labels[0]);
            }

            if (debug) {
                Console.WriteLine("Assigned label {0}.", GetLabelOfBlock(blocks[0], x, y));
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
                    // The following condition must be here in case the input matrix contains an odd
                    // number of columns or rows
                    if (y + yOffset + j < outputMatrix.Count && x + xOffset + i < outputMatrix[y + yOffset + j].Count) {
                        if (outputMatrix[y + yOffset + j][x + xOffset + i] != 0) {
                            return outputMatrix[y + yOffset + j][x + xOffset + i];
                        }
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

        public int getHighestLabel() {
            return highestLabel;
        }
    }
}