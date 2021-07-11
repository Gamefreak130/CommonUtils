namespace Gamefreak130.Common.Structures
{
    using Sims3.SimIFace;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A simple unweighted and directed adjacency list graph implementation with unique node values
    /// </summary>
    /// <typeparam name="T">The type of item the graph will contain</typeparam>
    [Persistable]
    public class UDGraph<T> : IUnweightedGraph<T>
    {

        private readonly Dictionary<T, List<T>> mSpine = new();

        public IEnumerable<T> Nodes => mSpine.Keys;

        IEnumerable<IGraph<T>.IEdge<T>> IGraph<T>.Edges => Edges.Cast<IGraph<T>.IEdge<T>>();

        public IEnumerable<IUnweightedGraph<T>.Edge<T>> Edges => mSpine.SelectMany(kvp => kvp.Value.Select(item => new IUnweightedGraph<T>.Edge<T>(kvp.Key, item)));

        public int NodeCount => mSpine.Count;

        public int EdgeCount => mSpine.Sum(kvp => kvp.Value.Count());

        public UDGraph()
        {
        }

        public UDGraph(params T[] items)
        {
            foreach (T item in items)
            {
                AddNode(item);
            }
        }

        public void AddNode(T item)
        {
            if (item is null)
            {
                throw new ArgumentNullException();
            }
            if (ContainsNode(item))
            {
                throw new ArgumentException("Item already exists in graph");
            }
            mSpine[item] = new();
        }

        public bool ContainsNode(T item) => mSpine.ContainsKey(item);

        public void RemoveNode(T item)
        {
            if (ContainsNode(item))
            {
                mSpine.Remove(item);
                foreach (T node in mSpine.Keys)
                {
                    mSpine[node].Remove(item);
                }
            }
        }

        public void AddEdge(T from, T to)
        {
            if (!ContainsNode(from))
            {
                throw new ArgumentException("Item does not exist in graph", "from");
            }
            if (!ContainsNode(to))
            {
                throw new ArgumentException("Item does not exist in graph", "to");
            }
            if (mSpine[from].Contains(to))
            {
                throw new ArgumentException("Edge already exists in graph");
            }
            mSpine[from].Add(to);
        }

        public bool ContainsEdge(T from, T to) => !ContainsNode(from)
                ? throw new ArgumentException("Item does not exist in graph", "from")
                : !ContainsNode(to) ? throw new ArgumentException("Item does not exist in graph", "to") : mSpine[from].Contains(to);

        public void RemoveEdge(T from, T to)
        {
            if (!ContainsNode(from))
            {
                throw new ArgumentException("Item does not exist in graph", "from");
            }
            if (!ContainsNode(to))
            {
                throw new ArgumentException("Item does not exist in graph", "to");
            }
            mSpine[from].Remove(to);
        }

        public IEnumerable<T> GetNeighbors(T item) => !ContainsNode(item) ? throw new ArgumentException("Item does not exist in graph", "item") : mSpine[item];
    }
}
