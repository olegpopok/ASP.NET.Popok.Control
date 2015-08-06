using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class BinaryTree<TValue> : IEnumerable<TValue>
    {
        private class Node : IEnumerable<TValue>
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

            public IEnumerator<TValue> GetEnumerator()
            {
                yield return value;

                if(left != null)
                {
                    foreach(var item in left)
                    {
                        yield return item;
                    }
                }
                if(right != null)
                {
                    foreach(var item in right)
                    {
                        yield return item;
                    }
                }
            }

            public IEnumerable<TValue> InOrder()
            {
                if (left != null)
                {
                    foreach (var item in left.InOrder())
                    {
                        yield return item;
                    }
                }
                yield return value;

                if (right != null)
                {
                    foreach (var item in right.InOrder())
                    {
                        yield return item;
                    }
                }
            }

            public IEnumerable<TValue> PostOrder()
            {
                if (left != null)
                {
                    foreach (var item in left)
                    {
                        yield return item;
                    }
                }

                if (right != null)
                {
                    foreach (var item in right)
                    {
                        yield return item;
                    }
                }
                yield return value;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
               return GetEnumerator();
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
            return Delete(value, ref _root);
        }

        public int Count()
        {
            return _count;
        }
       
        public IEnumerable<TValue> InOrder()
        {
            if(_root != null)
            {
                foreach(var value in _root.InOrder())
                {
                    yield return value;
                }
            }
        }

        public IEnumerable<TValue> PostOrder()
        {
            if(_root != null)
            {
                foreach(var value in _root.PostOrder())
                {
                    yield return value;
                }
            }
        }

        public IEnumerator<TValue> GetEnumerator()
        {
           if(_root != null)
           {
               foreach(var val in _root)
               {
                   yield return val;
               }
           }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private bool Delete(TValue value, ref Node node)
        {
            if(node == null)
            {
                return false;
            }

            int comp = comparer.Compare(node.value, value);
            if(comp < 0)
            {
                return Delete(value, ref node.right);
            }
            if( comp > 0)
            {
                return Delete(value, ref node.left);
            }

            DeleteNode(ref node);
            _count--;
            return true;
        }

        private void DeleteNode(ref Node node)
        {
            if (node.left == null && node.right == null)
            {
                node = null;
            }
            else if (node.left == null || node.right == null)
            {
                node = node.right ?? node.left;
            }
            else
            {
                if (node.right.left == null)
                {
                    node.right.left = node.left;
                    node = node.right;
                }
                else
                {
                    var next = node.right.left;
                    var paret = node.right;
                    while (next.left != null)
                    {
                        paret = next;
                        next = next.left;
                    }
                    paret.left = next.right;
                    next.right = node.right;
                    next.left = node.left;
                    node = next;
                }
            }
        }

        private void Insert(TValue value, ref Node node)
        {
            if(node == null)
            {
                node = new Node(value);
                _count++;
                return;
            }
            int comp = comparer.Compare(value, node.value);
            if(comp == 0)
            {
                return;
            }
            if( comp < 0)
            {
                Insert(value, ref node.left);
            }
            else
            {
                Insert(value, ref node.right);
            }
        }
    }
}
