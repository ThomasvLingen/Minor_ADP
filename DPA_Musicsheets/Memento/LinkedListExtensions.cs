using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Courtesy of Martijn Schuurmans

namespace DPA_Musicsheets.Memento {
    public static class LinkedListExtensions
    {
        public static LinkedListNode<T> ReplaceNext<T>(this LinkedListNode<T> node, LinkedListNode<T> next)
        {
            while(node.List.Last != node)
            {
                node.List.RemoveLast();
            }
            node.List.AddLast(next);

            return node.List.Last;
        }

        public static LinkedListNode<T> ReplaceNext<T>(this LinkedListNode<T> node, T next)
        {
            return node.ReplaceNext(new LinkedListNode<T>(next));
        }
    }
}
