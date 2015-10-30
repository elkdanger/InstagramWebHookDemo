using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MongoDB.Driver;

namespace InstagramWebHookDemo.Models
{
    public class UserRepository
    {
        private readonly MongoClient client;
        private readonly IMongoCollection<UserInfo> collection;
        private readonly IMongoDatabase db;

        public UserRepository()
        {
            this.client = new MongoDB.Driver.MongoClient("mongodb://test:test@ds040888.mongolab.com:40888/testingdb");
            this.db = client.GetDatabase("testingdb");
            this.collection = db.GetCollection<UserInfo>("users");
        }

        public IMongoCollection<UserInfo> Collection { get { return this.collection; } }

        public async Task<UserInfo> GetUser(string username)
        {
            var users = await this.collection.FindAsync(u => u.Username == "steve");

            await users.MoveNextAsync();

            return users.Current.FirstOrDefault();
        }

        public async Task InsertUser(UserInfo user)
        {
            await this.collection.InsertOneAsync(user);
        }
    }
}