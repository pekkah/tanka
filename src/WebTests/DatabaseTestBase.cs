namespace Tanka.WebTests
{
    using Raven.Client;
    using Raven.Client.Document;
    using Raven.Client.Embedded;

    public abstract class DatabaseTestBase
    {
        protected static IDocumentStore NewDocumentStore()
        {
            return new EmbeddableDocumentStore
            {
                RunInMemory = true,
                Conventions =
                {
                    DefaultQueryingConsistency = ConsistencyOptions.AlwaysWaitForNonStaleResultsAsOfLastWrite
                }
            }.Initialize();
        }
    }
}