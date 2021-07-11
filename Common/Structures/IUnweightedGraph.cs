namespace Gamefreak130.Common.Structures
{
    public interface IUnweightedGraph<T> : IGraph<T>
    {
        /// <summary>
        /// A simple data structure representing an edge between two vertices of an unweighted graph
        /// </summary>
        /// <typeparam name="TItem">The type of the edge vertices</typeparam>
        public class Edge<TItem> : IGraph<TItem>.IEdge<TItem>
        {
            public TItem Vertex1 { get; private set; }

            public TItem Vertex2 { get; private set; }

            public Edge(TItem item1, TItem item2)
            {
                Vertex1 = item1;
                Vertex2 = item2;
            }
        }

        /// <summary>
        /// Adds an edge between two nodes in the graph if one does not already exist
        /// </summary>
        /// <param name="u">The first vertex of the edge</param>
        /// <param name="v">The second vertex of the edge</param>
        public void AddEdge(T u, T v);
    }
}
