namespace Gamefreak130.Common.Structures
{
    using System.Collections.Generic;

    public interface IGraph<T>
    {
        interface IEdge<TItem>
        {
        }

        /// <summary>
        /// Gets a sequence of all items contained within the graph
        /// </summary>
        public IEnumerable<T> Nodes { get; }

        /// <summary>
        /// Gets a sequence of all edges in the graph, represented as an <see cref="IEdge{T}"/>
        /// </summary>
        public IEnumerable<IEdge<T>> Edges { get; }

        /// <summary>
        /// Gets the number of nodes in the graph
        /// </summary>
        public int NodeCount { get; }

        /// <summary>
        /// Gets the number of edges in the graph
        /// </summary>
        public int EdgeCount { get; }

        /// <summary>
        /// Adds a new node to the graph representing an item <typeparamref name="T"/>
        /// </summary>
        /// <param name="item">The item the new node will represent</param>
        public void AddNode(T item);

        /// <summary>
        /// Determines whether the graph contains a given item
        /// </summary>
        /// <param name="item">The item to search for</param>
        /// <returns><see langword="true"/> if <paramref name="item"/> is contained in the graph; otherwise, <see langword="false"/></returns>
        public bool ContainsNode(T item);

        /// <summary>
        /// Removes an item from the graph, if it exists
        /// </summary>
        /// <param name="item">The item to remove</param>
        public void RemoveNode(T item);

        /// <summary>
        /// Determines whether the graph contains an edge between two nodes within it
        /// </summary>
        /// <param name="u">The first vertex of the edge</param>
        /// <param name="v">The second vertex of the edge</param>
        /// <returns></returns>
        public bool ContainsEdge(T u, T v);

        /// <summary>
        /// Removes an edge between two nodes in the graph, if such an edge exists
        /// </summary>
        /// <param name="u">The first vertex of the edge</param>
        /// <param name="v">The second vertex of the edge</param>
        public void RemoveEdge(T u, T v);

        /// <summary>
        /// Returns a sequence of the items connected to a given item in the graph
        /// </summary>
        /// <param name="item">The starting item</param>
        public IEnumerable<T> GetNeighbors(T item);
    }
}
