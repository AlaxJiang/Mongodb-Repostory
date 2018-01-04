using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdeSAAS.DataService
{
    public interface IConnect : IDisposable
    {
        MongoDatabaseSettings DatabaseSettings
        {
            get;
        }

        IMongoCollection<T> Collection<T>(string collectionName);
    }
}
