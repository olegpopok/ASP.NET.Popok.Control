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
            BinaryTreeTest();
            HashTableTest();
          
        }

        private static void Print<T>(IEnumerable<T> items)
        {
            foreach(var item in items)
            {
                System.Console.Write(item.ToString()+" ");
            }
            System.Console.WriteLine();
        }

        private static void BinaryTreeTest()
        {
            System.Console.WriteLine("BinaryTree:");
            var binaryTree = new Library.BinaryTree<int>();
            binaryTree.Add(150);
            binaryTree.Add(250);
            binaryTree.Add(100);
            binaryTree.Add(130);
            binaryTree.Add(50);
            binaryTree.Add(140);
            binaryTree.Add(145);
            binaryTree.Add(135);
            binaryTree.Add(115);
            binaryTree.Add(120);
            binaryTree.Add(110);
            binaryTree.Add(25);
            binaryTree.Add(20);
            binaryTree.Add(30);
            binaryTree.Add(70);
            binaryTree.Add(80);
            binaryTree.Add(60);
            binaryTree.Add(112);

            Print(binaryTree.InOrder());
            System.Console.ReadKey();
            binaryTree.Remove(140);
            Print(binaryTree.InOrder());
            System.Console.ReadKey();
        }

        private static void HashTableTest()
        {
            var hashTable = new Library.HashTable<int, string>();
            char value = 'a';
            for (int key = 0; key < 10; key++, value++)
            {
                hashTable.Add(key, value.ToString());
            }
            System.Console.WriteLine("HashTable");
            Print(hashTable);
            System.Console.WriteLine("Count: {0}", hashTable.Count());
            System.Console.ReadKey();
            for (int key = 0; key < 5; key++)
            {
                hashTable.Remove(key);
            }
            Print(hashTable);
            System.Console.WriteLine("Count: {0}", hashTable.Count());
            System.Console.WriteLine("Get Value by Key = {0} : {1}", 5, hashTable.Get(5));
            System.Console.ReadKey();

        }
    }
}
