using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Spaghetti_Labeling
{
    public static class ImageProcessor
    {
        private static Image SpaghettiAssignLabels(Image input, List<HashSet<int>> equivalentLabels) {
            /*
            TODO: Create a special set of main and end trees for the first row, then use it for first row labeling. 
            */

            /* NOTE: Due to the lack of special forests for the first and last row, all images labeled with the
            Spaghetti algortihm are required to have first 2 rows filled with background pixels and the number
            of rows must be even.
            Once the special trees are implemented, this condition will disappear.
            */
            List<List<int>> inputMatrix = input.GetMatrix();
            Debug.Assert(inputMatrix.Count % 2 == 0);

            GraphManager gmFirst = new GraphManager(GraphManager.GraphType.FirstRow);
            GraphManager gmMiddle = new GraphManager(GraphManager.GraphType.MiddleRows);
            // ^ For better performance, GraphManager could be created once at the start of the program and then 
            // passed as an argument

            int width = inputMatrix[0].Count;
            int height = inputMatrix.Count;
            Image output = new Image(InitMatrixWithZeroes(width, height));
            ActionPerformer ap = new ActionPerformer(input, output, equivalentLabels);
            //Console.WriteLine("Initiated spaghetti labeling");

            for (int y = 0; y < height; y += 2) {
                if (y == 0) {
                    // First row
                    Console.WriteLine("Labeling blocks in first row.");
                    SpaghettiLabelBlocks(y, gmFirst, input, ap);
                } 
                else if (y != height - 1) {
                    // Middle rows
                    SpaghettiLabelBlocks(y, gmMiddle, input, ap);
                }
                else {
                    // Last row (only used in images with an odd number of rows)
                    // currently does nothing. First implement middle rows and first row, then this.
                }
            }

            /*
            Console.WriteLine("\nEach row contains a set of equivalent labels:");
            foreach (HashSet<int> labelSet in equivalentLabels) {
                foreach (int label in labelSet) {
                    Console.Write("{0} ", label);
                }
                Console.WriteLine();
            }
            */

            return output;
        }


        private static void SpaghettiLabelBlocks(int y, GraphManager gm, Image input, ActionPerformer ap) {
            // row y means that pixels at positions y and y+1 will be labeled
            List<List<int>> inputMatrix = input.GetMatrix();
            int width = inputMatrix[0].Count;
            int nextTreeIndex = gm.GetStartTreeIndex();
            int action = -1;
            for (int x = 0; x < width; x += 2) {
                Console.Write("Block in column {0}. ", x);
                if (x < width - 2) {
                    // Use main tree
                    Console.Write("Using main tree with index {0}. ", nextTreeIndex);
                    (action, nextTreeIndex) = GetActionAndNextTreeIndex(gm.AdjustIndexAndGetRootInMainGraph(nextTreeIndex), input, x, y);
                         
                } else {
                    Console.Write("Using end tree with index {0}. ", nextTreeIndex);
                    if (x == width - 2) {
                        // Use even tree
                        action = GetAction(gm.AdjustIndexAndGetRootInEndGraphEven(nextTreeIndex), input, x, y);
                    } else {
                        // Use odd tree
                        action = GetAction(gm.AdjustIndexAndGetRootInEndGraphOdd(nextTreeIndex), input, x, y);
                    }
                }
                Console.WriteLine("Chosen action: {0}", action);
                ap.Perform(action, x, y);
            }
        }

        private static (int, int) GetActionAndNextTreeIndex(AbstractNode root, Image input, int x, int y) {
            // Traverses the tree from a given root node based on the "input" matrix and returns a tuple containing the 
            // action to be performed and next tree index
            List<List<int>> inputMatrix = input.GetMatrix();
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

        private static int GetAction(AbstractNode root, Image input, int x, int y) {
            // Traverses the tree from a given end root node based on the "input" matrix and returns the action to be performed.
            // This method is only used for end trees, as no next tree index is returned.
            List<List<int>> inputMatrix = input.GetMatrix();
            AbstractNode curNode = root;
            while (curNode is Node) {
                if (ConditionToPixelValue(((Node) curNode).GetCondition(), x, y, input) == 0) {
                    curNode = ((Node) curNode).GetLeft();
                } else {
                    curNode = ((Node) curNode).GetRight();
                }
            }
            // curNode now contains the correct leaf based on the pixels in surrounding blocks
            return GetAnyHashsetElement(((Leaf) curNode).GetActions());
        }

        private static T GetAnyHashsetElement<T>(HashSet<T> set) {
            // This is probably super dumb but I couldn't find a built-in method that does this
            foreach (T elem in set) {
                return elem;
            }
            throw new IndexOutOfRangeException("Set must not be empty!");
        }

        private static int ConditionToPixelValue(char condition, int x, int y, Image input) {
            // Takes a node condition (given by a char in range a-t) and the current position of a block (corresponding to the 'o' char
            // of the block) and returns the value of the pixel at the position specified by the condition.
            List<List<int>> inputMatrix = input.GetMatrix();
            switch (condition) {
                case 'a':
                    return inputMatrix[y - 2][x - 2];
                case 'b':
                    return inputMatrix[y - 2][x - 1];
                case 'c':
                    return inputMatrix[y - 2][x];
                case 'd':
                    return inputMatrix[y - 2][x + 1];
                case 'e':
                    return inputMatrix[y - 2][x + 2];
                case 'f':
                    return inputMatrix[y - 2][x + 3];
                case 'g':
                    return inputMatrix[y - 1][x - 2];
                case 'h':
                    return inputMatrix[y - 1][x - 1];
                case 'i':
                    return inputMatrix[y - 1][x];
                case 'j':
                    return inputMatrix[y - 1][x + 1];
                case 'k':
                    return inputMatrix[y - 1][x + 2];
                case 'l':
                    return inputMatrix[y - 1][x + 3];
                case 'm':
                    return inputMatrix[y][x - 2];
                case 'n':
                    return inputMatrix[y][x - 1];
                case 'o':
                    return inputMatrix[y][x];
                case 'p':
                    return inputMatrix[y][x + 1];
                case 'q':
                    return inputMatrix[y + 1][x - 2];
                case 'r':
                    return inputMatrix[y + 1][x - 1];
                case 's':
                    return inputMatrix[y + 1][x];
                case 't':
                    return inputMatrix[y + 1][x + 1];
                default:
                    throw new NotSupportedException("Critical error: condition not found.");
            }
        }

        public static Image SpaghettiCCL(Image input) {
            return CCL(input, SpaghettiAssignLabels);
        }

        // TODO: override SpaghettiCCL and ClassicCCL methods to work with a path to an image as the argument

        public static Image ClassicCCL(Image input) {
            return CCL(input, ClassicCCL_AssignLabels);
        }

        private static Image CCL(Image input, Func<Image, List<HashSet<int>>, Image> assignLabels) {
            List<HashSet<int>> equivalentLabels = new List<HashSet<int>>();
            Image output = assignLabels(input, equivalentLabels);
            ResolveLabelEquivalencies(output, equivalentLabels);
            return output;
        }

        private static Image ClassicCCL_AssignLabels(Image input, List<HashSet<int>> equivalentLabels) {
            List<List<int>> inputMatrix = input.GetMatrix();
            int width = inputMatrix[0].Count;
            int height = inputMatrix.Count;
            Image image = new Image(InitMatrixWithZeroes(width, height));
            int highestLabel = 0;
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    if (inputMatrix[y][x] == 1) {
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
            // Note that the only pixels which can theoretically be equivalent are the 'r' pixel and
            // either the 'p' or 's' pixel.
            ClassicCCL_ManageEquivalencies(image, 'p', x, y, equivalentLabels);
            ClassicCCL_ManageEquivalencies(image, 's', x, y, equivalentLabels);
        }

        private static void ClassicCCL_ManageEquivalencies(Image image, char pixel, int x, int y, 
                                                           List<HashSet<int>> equivalentLabels) {
            // Checks if the 'r' pixel and the pixel passed as argument ('p' or 's') are equivalent,
            // and if so, updates the equivalentLabels list
            
            // Return if either of the relevant pixels is out of bounds
            List<List<int>> imageMatrix = image.GetMatrix();
            int width = imageMatrix[0].Count;
            if (x == 0 || y == 0 || x == width - 1) {
                return;
            }

            // Return if either of the relevant pixels is background, or if they both have the same label
            Debug.Assert(pixel == 'p' || pixel == 's');
            int label1 = imageMatrix[y - 1][x + 1];
            int label2 = pixel == 'p' ? imageMatrix[y - 1][x - 1] : imageMatrix[y][x - 1];
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

        public static (HashSet<int>, HashSet<int>) FindSetsWithLabels(List<HashSet<int>> equivalentLabels, int label1, int label2) {
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
                TestClassicCCL();
                TestSpaghettiCCL();
            }

            private static void TestClassicCCL() {
                Image labeled1 = ClassicCCL(Image.TestImages.BinaryImage1());
                Image reference1 = Image.TestImages.LabeledImage1();
                Debug.Assert(labeled1.Equals(reference1));

                Image labeled2 = ClassicCCL(Image.TestImages.BinaryImage2());
                Image reference2 = Image.TestImages.LabeledImage2();
                Debug.Assert(labeled2.Equals(reference2));

                Image labeled3 = ClassicCCL(Image.TestImages.BinaryImage3());
                Image reference3 = Image.TestImages.LabeledImage3();
                Debug.Assert(labeled3.Equals(reference3));
            }

            private static void TestSpaghettiCCL() {
                Image spaghetti3 = SpaghettiCCL(Image.TestImages.BinaryImage3());
                Image classic3 = ClassicCCL(Image.TestImages.BinaryImage3());
                Debug.Assert(spaghetti3.Equals(classic3));

                Image spaghetti4 = SpaghettiCCL(Image.TestImages.BinaryImage4());
                Image classic4 = ClassicCCL(Image.TestImages.BinaryImage4());
                Debug.Assert(spaghetti4.Equals(classic4));

                // This test is failing (BinaryImage5)
                Image spaghetti5 = SpaghettiCCL(Image.TestImages.BinaryImage5());
                Image classic5 = ClassicCCL(Image.TestImages.BinaryImage5());
                Debug.Assert(spaghetti5.Equals(classic5));
                

                Image spaghetti6 = SpaghettiCCL(Image.TestImages.BinaryImage6());
                Image classic6 = ClassicCCL(Image.TestImages.BinaryImage6());
                Debug.Assert(spaghetti6.Equals(classic6));
            } 
        }
    }
}