using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library;

namespace Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var list = new Library.List<int>();
            for (int i = 0; i < 10; i++)
            {
                list.Add(i);
            }
            System.Console.Write("List: \n");
            Print(list);
            for (int i = 0; i < 5; i++)
            {
                list.Remove(i);
            }
            Print(list);

            System.Console.ReadKey();
            System.Console.WriteLine("HashTable:");
            var hashTable = new Library.HashTable<int, string>();
            for (int i = 1; i < 5; i++)
            {
                hashTable.Add(i, "HashTable");
            }
            Print(hashTable);
            System.Console.WriteLine();
            for (int i = 1; i < 3; i++)
            {
                hashTable.Remove(i);
            }
            Print(hashTable);

            System.Console.ReadKey();
            System.Console.WriteLine("BinaryTree:");
            var binaryTree = new Library.BinaryTree<int>();
            binaryTree.Add(5);
            for (int i = 0; i < 10; i++)
            {
                binaryTree.Add(i);
            }
            binaryTree.InOrder(x => System.Console.WriteLine(x.ToString()));

                System.Console.ReadKey();
        }

        private static void Print<T>(IEnumerable<T> items)
        {
            foreach(var item in items)
            {
                System.Console.Write(item.ToString());
            }
            System.Console.WriteLine();
        }

        

    
    }
}
