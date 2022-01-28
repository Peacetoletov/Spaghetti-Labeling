using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Spaghetti_Labeling
{
    public static class ImageProcessor
    {
        private static Image SpaghettiAssignLabels(List<List<int>> input,  
                                                   List<HashSet<int>> equivalentLabels) {
            // TODO: this
            /* UPDATED TODO:
               Implement the basic version (only labeling middle rows, requiring the first 2 rows of pixels to
               be filled with 0s and the number of rows to be even), then thoroughly test every part of the
               labeling process. Some tests can be automated, other parts will do with just manual tests.
               After this is done, I may implement the rest, depending on how much time I will have.
            */

            /* NOTE: Due to the lack of special forests for the first and last row, all images labeled with the
            Spaghetti algortihm are required to have first 2 rows filled with background pixels and the number
            of rows must be even.
            Once the special trees are implemented, this condition will disappear.
            */
            Debug.Assert(input.Count % 2 == 0);

            GraphManager gm = new GraphManager();

            int width = input[0].Count;
            int height = input.Count;
            Image output = new Image(InitMatrixWithZeroes(width, height));
            Console.WriteLine("Initiated spaghetti labeling");

            for (int y = 0; y < height; y += 2) {
                if (y == 0) {
                    // First row
                    // currently does nothing. First implement middle rows, then this.
                    Console.WriteLine("Skipping blocks in row 0.");
                } 
                else if (y != height - 1) {
                    // Middle rows
                    // TODO: ???
                    //SpaghettiLabelBlocksInRow()
                    Console.WriteLine("Beginning labeling blocks in row {0}", y);
                    SpaghettiLabelBlocksInMiddleRow(gm, input, output, y);
                }
                else {
                    // Last row (only used in images with an odd number of rows)
                    // currently does nothing. First implement middle rows and first row, then this.
                }
            }

            return null;
        }


        private static void SpaghettiLabelBlocksInMiddleRow(GraphManager gm, List<List<int>> input, Image output, int y) {
            // row y means that pixels at positions y and y+1 will be labeled
            int width = input[0].Count;
            int nextTreeIndex = gm.GetStartTreeIndex();
            int action = -1;
            for (int x = 0; x < width; x += 2) {
                Console.Write("Block in column {0}. ", x);
                if (x < width - 2) {
                    // Use main tree
                    Console.Write("Using main tree with index {0}. ", nextTreeIndex);
                    (action, nextTreeIndex) = GetActionAndNextTreeIndex(gm.AdjustIndexAndGetMainGraphRoot(nextTreeIndex), input, x, y);
                    Console.WriteLine("Chosen action: {0}", action);

                    // TODO: perform the chosen action (implement class Actions)
                    

                } else {
                    Console.WriteLine("End of row. End trees not implemented yet, doing nothing.");
                    if (x == width - 2) {
                        // Use even tree

                    } else {
                        // Use odd tree

                        // (...)
                    }
                }
                //SpaghettiLabelMiddleBlock(input, image, 666, x, y, width);
            }
        }

        /*
        // Turns out I don't need this method.
        private static bool DoesBlockContainForegroundPixels(char block, int x, int y, List<List<int>> input) {
            //Block layout:
            //    __________
            //   | P | Q | R |
            //   |___|___|___|
            //   | S | X |
            //   |___|___|
            int xOffset;
            int yOffset;
            switch (block) {
                case 'P':
                    xOffset = -2;
                    yOffset = -2;
                    break;
                case 'Q':
                    xOffset = 0;
                    yOffset = -2;
                    break;
                case 'R':
                    xOffset = 2;
                    yOffset = -2;
                    break;
                case 'S':
                    xOffset = -2;
                    yOffset = 0;
                    break;
                case 'X':
                    xOffset = 0;
                    yOffset = 0;
                    break;
                default:
                    throw new NotSupportedException("Critical error: block not found.");
            }
            x += xOffset;
            y += yOffset;
            return input[y][x] == 1 || input[y + 1][x] == 1 || input[y][x + 1] == 1 || input[y + 1][x + 1] == 1;
        }
        */

        private static (int, int) GetActionAndNextTreeIndex(AbstractNode root, List<List<int>> input, int x, int y) {
            // Traverses the tree from a given node based on the "input" matrix and returns a tuple containing the 
            // action to be performed and next tree index
            AbstractNode curNode = root;
            while (curNode is Node) {
                if (ConditionToPixelValue(((Node) curNode).GetCondition(), x, y, input) == 0) {
                    curNode = ((Node) curNode).GetLeft();
                } else {
                    curNode = ((Node) curNode).GetRight();
                }
            }
            // curNode now contains the correct leaf based on the pixels in surrounding blocks
            int action = GetAnyHashsetElement(((Leaf) curNode).GetActions());
            int nextTreeIndex = ((Leaf) curNode).GetNextTreeIndex();

            return (action, nextTreeIndex);
        }

        private static T GetAnyHashsetElement<T>(HashSet<T> set) {
            // This is probably super dumb but I couldn't find a built-in method that does this
            foreach (T elem in set) {
                return elem;
            }
            throw new IndexOutOfRangeException("Set must not be empty!");
        }

        private static int ConditionToPixelValue(char condition, int x, int y, List<List<int>> input) {
            // Takes a node condition (given by a char in range a-t) and the current position of a block (corresponding to the 'o' char
            // of the block) and returns the value of the pixel at the position specified by the condition.
            switch (condition) {
                case 'a':
                    return input[y - 2][x - 2];
                case 'b':
                    return input[y - 2][x - 1];
                case 'c':
                    return input[y - 2][x];
                case 'd':
                    return input[y - 2][x + 1];
                case 'e':
                    return input[y - 2][x + 2];
                case 'f':
                    return input[y - 2][x + 3];
                case 'g':
                    return input[y - 1][x - 2];
                case 'h':
                    return input[y - 1][x - 1];
                case 'i':
                    return input[y - 1][x];
                case 'j':
                    return input[y - 1][x + 1];
                case 'k':
                    return input[y - 1][x + 2];
                case 'l':
                    return input[y - 1][x + 3];
                case 'm':
                    return input[y][x - 2];
                case 'n':
                    return input[y][x - 1];
                case 'o':
                    return input[y][x];
                case 'p':
                    return input[y][x + 1];
                case 'q':
                    return input[y + 1][x - 2];
                case 'r':
                    return input[y + 1][x - 1];
                case 's':
                    return input[y + 1][x];
                case 't':
                    return input[y + 1][x + 1];
                default:
                    throw new NotSupportedException("Critical error: condition not found.");
            }
        }

        /*
        private static int SpaghettiLabelMiddleBlock(List<List<int>> input, Image image, int treeIndex, int x, int y, int width) {
            // Labels the 2x2 block, with x and y being the upper left corner of the block.
            // Returns the index of the next tree to be used.

            return 0;
        }
        */

        public static Image SpaghettiCCL(List<List<int>> binaryImage) {
            return CCL(binaryImage, SpaghettiAssignLabels);
        }

        // TODO: override SpaghettiCCL and ClassicCCL methods to work with a path to an image as the argument

        public static Image ClassicCCL(List<List<int>> binaryImage) {
            return CCL(binaryImage, ClassicCCL_AssignLabels);
        }

        private static Image CCL(List<List<int>> binaryImage, Func<List<List<int>>, List<HashSet<int>>, Image> assignLabels) {
            List<HashSet<int>> equivalentLabels = new List<HashSet<int>>();
            Image output = assignLabels(binaryImage, equivalentLabels);
            //ResolveLabelEquivalencies(output, equivalentLabels);      // TEMPORARILY COMMENTED OUT
            return output;
        }

        private static Image ClassicCCL_AssignLabels(List<List<int>> input,  
                                                     List<HashSet<int>> equivalentLabels) {
            int width = input[0].Count;
            int height = input.Count;
            Image image = new Image(InitMatrixWithZeroes(width, height));
            int highestLabel = 0;
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    if (input[y][x] == 1) {
                        // Assign label if current pixel is foreground 
                        highestLabel = ClassicCCL_LabelPixel(image, x, y, highestLabel, equivalentLabels);

                        // Store information about equivalent labels
                        ClassicCCL_ManageEquivalencies(image, x, y, equivalentLabels);
                    }
                }
            }

            return image;
        }

        private static int ClassicCCL_LabelPixel(Image image, int x, int y, int highestLabel, List<HashSet<int>> equivalentLabels) {
            List<List<int>> imageMatrix = image.GetMatrix();
            int width = imageMatrix[0].Count;
            if (x - 1 >= 0 && y - 1 >= 0 && imageMatrix[y - 1][x - 1] != 0) {
                imageMatrix[y][x] = imageMatrix[y - 1][x - 1];
            } else if (y - 1 >= 0 && imageMatrix[y - 1][x] != 0) {
                imageMatrix[y][x] = imageMatrix[y - 1][x];
            } else if (x + 1 < width && y - 1 >= 0 && imageMatrix[y - 1][x + 1] != 0) {
                imageMatrix[y][x] = imageMatrix[y - 1][x + 1];
            } else if (x - 1 >= 0 && imageMatrix[y][x - 1] != 0) {
                imageMatrix[y][x] = imageMatrix[y][x - 1];
            } else {
                highestLabel++;
                imageMatrix[y][x] = highestLabel;
                equivalentLabels.Add(new HashSet<int> {highestLabel});
            }
            return highestLabel;
        }

        private static void ClassicCCL_ManageEquivalencies(Image image, int x, int y, 
                                                           List<HashSet<int>> equivalentLabels) {
            /* Note that the only pixels which can theoretically be equivalent and haven't been marked
            as equivalent yet are the ones in the upper left and right corner. */
            // Return if either of the relevant pixels is out of bounds
            List<List<int>> imageMatrix = image.GetMatrix();
            int width = imageMatrix[0].Count;
            if (x == 0 || y == 0 || x == width - 1) {
                return;
            }

            // Return if either of the relevant pixels is background, or if they both have the same label
            int label1 = imageMatrix[y - 1][x - 1];
            int label2 = imageMatrix[y - 1][x + 1];
            if (label1 == 0 || label2 == 0 || label1 == label2) {
                return;
            }

            // If both labels are in different sets, join the sets together
            (HashSet<int> setWithLabel1, HashSet<int> setWithLabel2) = FindSetsWithLabels(equivalentLabels, label1, label2);
            if (setWithLabel1 != setWithLabel2) {
                setWithLabel1.UnionWith(setWithLabel2);
                equivalentLabels.Remove(setWithLabel2);
            }
        }

        private static (HashSet<int>, HashSet<int>) FindSetsWithLabels(List<HashSet<int>> equivalentLabels, int label1, int label2) {
            HashSet<int> setWithLabel1 = null;
            HashSet<int> setWithLabel2 = null;
            foreach (HashSet<int> set in equivalentLabels) {
                if (set.Contains(label1)) {
                    setWithLabel1 = set;
                }
                if (set.Contains(label2)) {
                    setWithLabel2 = set;
                }
            }
            return (setWithLabel1, setWithLabel2);
        }

        private static List<List<int>> InitMatrixWithZeroes(int width, int height) {
            List<List<int>> matrix = new List<List<int>>();
            for (int row = 0; row < height; row++) {
                matrix.Add(new List<int>());
                for (int col = 0; col < width; col++) {
                    matrix[row].Add(0);
                }
            }
            return matrix;
        }

        private static void ResolveLabelEquivalencies(Image image, List<HashSet<int>> equivalentLabels) {
            List<List<int>> imageMatrix = image.GetMatrix();
            int width = imageMatrix[0].Count;
            int height = imageMatrix.Count;
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    for (int i = 0; i < equivalentLabels.Count; i++) {
                        HashSet<int> set = equivalentLabels[i];
                        if (set.Contains(imageMatrix[y][x])) {
                            imageMatrix[y][x] = i + 1;
                        }
                    }
                }
            }
        }

        public static class Tests 
        {
            public static void Run() {
                //TestClassicCCL();         // TEMPORARILY COMMENTED OUT
            }

            private static void TestClassicCCL() {
                Image labeled1 = ClassicCCL(Image.TestImages.BinaryImage1().GetMatrix());
                Image reference1 = Image.TestImages.LabeledImage1();
                Debug.Assert(labeled1.Equals(reference1));

                Image labeled2 = ClassicCCL(Image.TestImages.BinaryImage2().GetMatrix());
                Image reference2 = Image.TestImages.LabeledImage2();
                Debug.Assert(labeled2.Equals(reference2));

                /*
                List<List<int>> labeledMatrix = labeled2.GetMatrix();
                foreach (List<int> row in labeledMatrix) {
                    foreach (int label in row) {
                        Console.Write(label);
                    }
                    Console.WriteLine();
                }
                */                

                //Console.WriteLine("Classic CCL works");
            }
        }
    }
}