namespace Gamefreak130.Common.Structures
{
    public interface IWeightedGraph<T> : IGraph<T>
    {
        /// <summary>
        /// Sets the weight of the edge between two nodes in the graph, adding it if it does not already exist
        /// </summary>
        /// <param name="u">The first vertex of the edge</param>
        /// <param name="v">The second vertex of the edge</param>
        /// <param name="weight">The weight value the edge will have</param>
        public void SetEdge(T u, T v, int weight = 1);

        /// <summary>
        /// Gets the weight of the edge between two nodes in the graph, if such an edge exists
        /// </summary>
        /// <param name="u">The first vertex of the edge</param>
        /// <param name="v">The second vertex of the edge</param>
        /// <param name="weight">The weight value of the edge, if it exists</param>
        /// <returns><c>true</c> if an edge between <paramref name="u"/> and <paramref name="v"/> exists in the graph; otherwise, <c>false</c></returns>
        public bool TryGetEdge(T u, T v, out int weight);
    }
}
