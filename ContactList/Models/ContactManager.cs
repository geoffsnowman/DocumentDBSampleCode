using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System.Threading.Tasks;

namespace ContactList.Models
{
    public class ContactManager
    {
        //Copy these three strings from the Azure portal after creating a new DocumentDB account
        //The sample URI and PRIMARY_KEY here will not work as they refer to an account that no
        //longer exists, although they are examples of the correct format for the data.
        private const string URI = "https://geoffsnowmandb1.documents.azure.com:443/";
        private const string PRIMARY_KEY = "brBVzVNdWRpWlug8YiFLOaZqc1JVWvoVUm509VVglDsZXp3zXtrrCA9KK8CzOofgVmj5c8jmiuWwicnXNXTkgA==";
        
        //You can leave the database name and collection name unchanged if you want or 
        //you can change either one of them to any string that strikes your fancy. After
        //running the app, you should be able to find the new database and collection in the portal. 
        private const string DB_ID = "ContactsDb";
        private const string COLLECTION_ID = "ContactsCollection";

        private DocumentClient _client;

        private async Task InitializeClientAsync()
        {
            _client = new DocumentClient(new Uri(URI), PRIMARY_KEY);
            await _client.CreateDatabaseIfNotExistsAsync(new Database { Id = DB_ID });
            await _client.CreateDocumentCollectionIfNotExistsAsync(
                UriFactory.CreateDatabaseUri(DB_ID),
                new DocumentCollection { Id = COLLECTION_ID }
            );
            return;
        }

        private void DisposeClient()
        {
            if (_client != null)
            {
                _client.Dispose();
            }
        }

        public async Task<List<Contact>> GetAllAsync()
        {
            await InitializeClientAsync();
            try
            {
                IQueryable<Contact> qry = _client.CreateDocumentQuery<Contact>(
                    UriFactory.CreateDocumentCollectionUri(DB_ID, COLLECTION_ID),
                    new FeedOptions { MaxItemCount = -1 }
                );
                return qry.ToList<Contact>();
            }
            finally
            {
                DisposeClient();
            }
        }

        //TBD - Add search page into application
        //public List<Contact> Search(string name)
        //{
        //    var result = new List<Contact>();
        //    result.Add(new Contact
        //    {
        //        Id = "111111",
        //        Name = "Geoff",
        //        Address1 = "1614 Trumbulls Court",
        //        City = "Crofton",
        //        State = "MD",
        //        Zip = "21114"
        //    });
        //    result.Add(new Contact
        //    {
        //        Id = "222222",
        //        Name = "Betsy",
        //        Address1 = "106 Conduit Street",
        //        City = "Annapolis",
        //        State = "MD",
        //        Zip = "21401"
        //    });
        //    return result;
        //}

        public async Task<Contact> GetByIdAsync(string id)
        {
            await InitializeClientAsync();
            try
            {
                var resp = await _client.ReadDocumentAsync<Contact>
                (
                    UriFactory.CreateDocumentUri(DB_ID, COLLECTION_ID, id)
                );
                return resp.Document;
            }
            finally
            {
                DisposeClient();
            }
        }

        public async Task CreateAsync(Contact newContact)
        {
            await InitializeClientAsync();
            try
            {
                await _client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DB_ID, COLLECTION_ID), newContact);
                return;
            }
            finally
            {
                DisposeClient();
            }
        }

        public async Task UpdateAsync(Contact updatedContact)
        {
            await InitializeClientAsync();
            try
            {
                await _client.ReplaceDocumentAsync(
                    UriFactory.CreateDocumentUri(DB_ID, COLLECTION_ID, updatedContact.Id), 
                    updatedContact
                );
                return;
            }
            finally
            {
                DisposeClient();
            }
        }

        public async Task DeleteAsync(string id)
        {
            await InitializeClientAsync();
            try
            {
                await _client.DeleteDocumentAsync(
                    UriFactory.CreateDocumentUri(DB_ID, COLLECTION_ID, id)
                );
                return;
            }
            finally
            {
                DisposeClient();
            }
        }

    }
}