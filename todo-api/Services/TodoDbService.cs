using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using System.Net;
using System.Collections;

namespace TodoApi.Models
{



    public class Item
    {
        public string Name { get; set; }
        public int Store { get; set; }
    }


    public interface ITodoDbService
    {
        Task<TodoItem> AddItemAsync(TodoItem item);
        Task<TodoItem> UpdateItemAsync(TodoItem item);
        Task<TodoItem> DeleteItemAsync(string _id, string _rev);
        Task<dynamic> GetTodoItemsAsync();
    }

    public class TodoDbService : ITodoDbService
    {
        private static HttpClient httpClient = null;

        private static string dbname = "todos";

        private static String CLOUDANT_SECRET = "CLOUDANT_SECRET";

        private readonly ILogger _logger;

        public TodoDbService(ILogger<TodoDbService> logger)
        {
            _logger = logger;
        }

        public async Task<TodoItem> AddItemAsync(TodoItem item)
        {
            HttpClient dbclient = await TodoDbService.CouchDbClientAsync();
            PostTodoItem postitem = new PostTodoItem();
            postitem.done = item.done;
            postitem.text = item.text;
            postitem.type = item.type;

            var postData = JsonConvert.SerializeObject(postitem);
            var content = new StringContent(postData, Encoding.UTF8, "application/json");

            var response = await dbclient.PostAsync(dbname, content);
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                JObject o = JObject.Parse(responseJson);
                item._id =  o.GetValue("id").ToString();
                item._rev = o.GetValue("rev").ToString();
                return item;
            }
            return null;
        }

        public async Task<TodoItem> UpdateItemAsync(TodoItem item)
        {
            HttpClient dbclient = await TodoDbService.CouchDbClientAsync();
            PostTodoItem postitem = new PostTodoItem();
            postitem.done = item.done;
            postitem.text = item.text;
            postitem.type = item.type;

            var postData = JsonConvert.SerializeObject(postitem);
            var content = new StringContent(postData, Encoding.UTF8, "application/json");

            var response = await dbclient.PutAsync(dbname + "/" + WebUtility.UrlEncode(item._id) + "?rev=" + WebUtility.UrlEncode(item._rev), content);
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                JObject o = JObject.Parse(responseJson);
                item._id = o.GetValue("id").ToString();
                item._rev = o.GetValue("rev").ToString();
                return item;
            }
            return null;
        }

        public async Task<TodoItem> DeleteItemAsync(string _id, string _rev)
        {
            HttpClient dbclient = await TodoDbService.CouchDbClientAsync();

            var response = await dbclient.DeleteAsync(dbname + "/" + WebUtility.UrlEncode(_id) + "?rev=" + WebUtility.UrlEncode(_rev));
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                TodoItem ret = new TodoItem();
                JObject o = JObject.Parse(responseJson);
                ret._id = o.GetValue("id").ToString();
                ret._rev = o.GetValue("rev").ToString();
                return ret;
            }
            return null;
        }

        public async Task<dynamic> GetTodoItemsAsync()
        {
            List<TodoItem> mylist = new List<TodoItem>();
            // TODO create a View with the right content to avoid Json Parse ...
            HttpClient dbclient = await TodoDbService.CouchDbClientAsync();
            var res = await dbclient.GetAsync(dbname + "/_all_docs?include_docs=true");
            if (res.IsSuccessStatusCode)
            {
                //force synchron
                string body = res.Content.ReadAsStringAsync().Result;
                dynamic o = JObject.Parse(body);
                // test with https://jsonpath.curiousconcept.com/
                IEnumerable<JToken> values = o.SelectTokens("$.rows..doc");
                foreach(JToken token in values)
                {
                    string test = token.ToString();
                    Console.WriteLine(test);
                    TodoItem item = JsonConvert.DeserializeObject<TodoItem>(test);
                    mylist.Add(item);
                }

                return mylist;
            }
            string msg = "Error: " + res.StatusCode + " " + res.ReasonPhrase;
            _logger.LogDebug(msg);
            return mylist;
        }

        private static async Task<HttpClient> CouchDbClientAsync()
        {

            if (httpClient == null)
            {
                foreach (DictionaryEntry de in Environment.GetEnvironmentVariables())
                    Console.WriteLine("  {0} = {1}", de.Key, de.Value);

                string env = Environment.GetEnvironmentVariable(CLOUDANT_SECRET);
                env = env.Replace("\\", string.Empty);
                env = env.Replace("'", string.Empty);
                JObject o = JObject.Parse(env);
                string host = o.GetValue("host").ToString();
                string username = o.GetValue("username").ToString();
                string password = o.GetValue("password").ToString();
                if (host == null || username == null || password == null)
                {
                    throw new Exception("Missing Cloudant NoSQL DB service credentials");
                }
                string url = "https://" + host;
                var authValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password));
                httpClient = HttpClientFactory.Create();
                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",authValue);

                // create Database if not exists
                var response = await httpClient.PutAsync(dbname, null);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Database todos created");
                }
                else if(response.StatusCode == HttpStatusCode.PreconditionFailed) //412 database exists
                {
                    Console.WriteLine("Database todos exists");
                }

            }
            return httpClient;
        }

    }

}
