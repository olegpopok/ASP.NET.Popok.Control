using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class BinaryTree<TValue>
    {
        private class Node
        {
            public TValue value;
            public Node right;
            public Node left;

            public Node(TValue value)
            {
                this.value = value;
                this.left = null;
                this.right = null;
            }
        }

        private Node _root;
        private int _count;
        private  IComparer<TValue> comparer;

        public BinaryTree() : this(null) { }

        public BinaryTree(IComparer<TValue> comparer)
        {
            this.comparer = comparer ?? Comparer<TValue>.Default;
        }

        public void Add(TValue value)
        {
            Insert(value, ref _root);
        }

        public bool Remove(TValue value)
        {
            return RemoveHelper(value, ref _root);
        }

        public int Count ()
        {
            return _count;
        }
       
        public void InOrder(Action<TValue> action)
        {
            if(action == null)
            {
                throw new ArgumentNullException();
            }

            InOrderHelper(action, _root);
        }

        public void PostOrder(Action<TValue> action)
        {
            if(action == null)
            {
                throw new ArgumentNullException();
            }
            PostOrderHelper(action, _root);
        }

        private void InOrderHelper(Action<TValue> action, Node node)
        {
            if(node == null)
            {
                return;
            }
            InOrderHelper(action, node.left);
            action(node.value);
            InOrderHelper(action, node.right);
        }

        private void PostOrderHelper(Action<TValue> action, Node node)
        {
            if(node == null)
            {
                return;
            }
            PostOrderHelper(action, node.left);
            PostOrderHelper(action, node.right);
            action(node.value);
        }

        private bool RemoveHelper(TValue value, ref Node node)
        {
            if(node == null)
            {
                return false;
            }

            int comp = comparer.Compare(node.value, value);
            if(comp > 0)
            {
                return RemoveHelper(value, ref node.right);
            }
            if( comp < 0)
            {
                return RemoveHelper(value, ref node.left);
            }

            node = null;
            return true;
        }

        private void Insert(TValue value, ref Node node)
        {
            if(node == null)
            {
                node = new Node(value);
                return;
            }

            if(comparer.Compare(node.value, value) > 0)
            {
                Insert(value, ref node.right);
            }
            else
            {
                Insert(value, ref node.left);
            }
        }
    }
}
