using System;

namespace Spaghetti_Labeling
{
    class Spaghetti
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Bolelli!");
            
            Node treeRoot = new ODTree().GetRoot();
            treeRoot.InfoDFS();    
        }
    }
}
