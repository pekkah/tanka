namespace Tanka.WebTests
{
    using System;
    using Raven.Client;
    using Raven.Client.Document;
    using Raven.Client.Embedded;

    public abstract class DatabaseTestBase : IDisposable
    {
        protected DatabaseTestBase()
        {
            Store = NewDocumentStore();
        }

        protected IDocumentStore Store { get; private set; }

        public void Dispose()
        {
            Store.Dispose();
        }

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