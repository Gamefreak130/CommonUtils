namespace Gamefreak130.Common.Structures
{
    using Sims3.SimIFace;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A simple unweighted and undirected adjacency list graph implementation with unique node values
    /// </summary>
    /// <typeparam name="T">The type of item the graph will contain</typeparam>
    [Persistable]
    public class UUGraph<T> : IUnweightedGraph<T>
    {
        private readonly UDGraph<T> mGraph;

        public IEnumerable<T> Nodes => mGraph.Nodes;

        IEnumerable<IGraph<T>.IEdge<T>> IGraph<T>.Edges => Edges.Cast<IGraph<T>.IEdge<T>>();

        public IEnumerable<IUnweightedGraph<T>.Edge<T>> Edges
        {
            get
            {
                List<T> visited = new();
                foreach (var edge in mGraph.Edges)
                {
                    if (!visited.Contains(edge.Vertex1))
                    {
                        visited.Add(edge.Vertex1);
                    }
                    if (!visited.Contains(edge.Vertex2) || edge.Vertex1.Equals(edge.Vertex2))
                    {
                        yield return edge;
                    }
                }
            }
        }

        public int NodeCount => mGraph.NodeCount;

        public int EdgeCount => Edges.Count();

        public UUGraph() => mGraph = new();

        public UUGraph(params T[] items) => mGraph = new(items);

        public void AddNode(T item) => mGraph.AddNode(item);

        public bool ContainsNode(T item) => mGraph.ContainsNode(item);

        public void RemoveNode(T item) => mGraph.RemoveNode(item);

        public void AddEdge(T u, T v)
        {
            mGraph.AddEdge(u, v);
            if (!u.Equals(v))
            {
                mGraph.AddEdge(v, u);
            }
        }

        public bool ContainsEdge(T u, T v) => mGraph.ContainsEdge(u, v);

        public void RemoveEdge(T u, T v)
        {
            mGraph.RemoveEdge(u, v);
            if (!u.Equals(v))
            {
                mGraph.RemoveEdge(v, u);
            }
        }

        public IEnumerable<T> GetNeighbors(T item) => mGraph.GetNeighbors(item);
    }
}
