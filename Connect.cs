using AdeSAAS.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdeSAAS.DataService
{
    [Serializable]
    public class Connect : IDisposable, IConnect
    {
        private bool disposed;

        protected MongoClient Client
        {
            get;
            private set;
        }

        protected IMongoDatabase DataBase
        {
            get;
            private set;
        }

        public MongoDatabaseSettings DatabaseSettings
        {
            get; private set;
        }

        //public Connect(DBConfigration configuration)
        //{
        //    string mongoConnectionString = configuration.ConnectionString;
        //    string mongoDatabase = configuration.Database;
        //    this.Client = new MongoClient(mongoConnectionString);
        //    this.DataBase = this.Client.GetDatabase(mongoDatabase, null);
        //    this.DatabaseSettings = this.DataBase.Settings;
        //}

        //public Connect(DBConfigration configuration, MongoDatabaseSettings databaseSettings)
        //{
        //    string mongoConnectionString = configuration.ConnectionString;
        //    string mongoDatabase = configuration.Database;
        //    this.Client = new MongoClient(mongoConnectionString);
        //    this.DataBase = this.Client.GetDatabase(mongoDatabase, databaseSettings);
        //    this.DatabaseSettings = this.DataBase.Settings;
        //}
        public Connect(MongoUrl mongoUrl)
        {
            this.Client = new MongoClient(mongoUrl);
            this.DataBase = this.Client.GetDatabase(mongoUrl.DatabaseName);
            this.DatabaseSettings = this.DataBase.Settings;
        }
        public Connect(string connectionString, string databaseName)
        {
            this.Client = new MongoClient(connectionString);
            this.DataBase = this.Client.GetDatabase(databaseName, null);
            this.DatabaseSettings = this.DataBase.Settings;
        }

        public Connect(string connectionString, string databaseName, MongoDatabaseSettings databaseSettings)
        {
            this.Client = new MongoClient(connectionString);
            this.DataBase = this.Client.GetDatabase(databaseName, databaseSettings);
            this.DatabaseSettings = databaseSettings;
        }

        public IMongoCollection<T> Collection<T>(string collectionName)
        {
            return this.DataBase.GetCollection<T>(collectionName, null);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.DataBase = null;
                    this.Client = null;
                }
                this.disposed = true;
            }
        }
        ~Connect()
        {
            this.Dispose(false);
        }
    }
}
