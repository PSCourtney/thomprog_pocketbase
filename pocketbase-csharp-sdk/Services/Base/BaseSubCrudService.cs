﻿using FluentResults;
using pocketbase_csharp_sdk.Services;
using pocketbase_csharp_sdk.Services.Interfaces;
using pocketbase_csharp_sdk.Models;
using pocketbase_csharp_sdk.Models.Files;
using System.Collections;
using System.Net.Http;

namespace pocketbase_csharp_sdk.Services.Base
{
    public abstract class BaseSubCrudService : BaseService
    {
        private readonly PocketBase _client;
        readonly string _collectionName;

       

      


        protected BaseSubCrudService(PocketBase client, string collectionName)
        {
            //this._realtimeService = realtimeService;
            this._collectionName = collectionName;
            this._client = client;
           

        }

        public virtual Result<PagedCollectionModel<T>> List<T>(int page = 1, int perPage = 30, string? filter = null,  string? expand = null, string? sort = null, CancellationToken cancellationToken = default)
        {
            var path = BasePath(_collectionName);
            var query = new Dictionary<string, object?>()
            {
                { "filter", filter },
                { "expand", expand },
                { "page", page },
                { "perPage", perPage },
                { "sort", sort }
            };

            return _client.Send<PagedCollectionModel<T>>(path, HttpMethod.Get, query: query, cancellationToken: cancellationToken);
        }

        public virtual Task<Result<PagedCollectionModel<T>>> ListAsync<T>(int page = 1, int perPage = 30, string? filter = null, string? expand = null, string? sort = null, CancellationToken cancellationToken = default)
        {
            var path = BasePath(_collectionName);
            var query = new Dictionary<string, object?>()
            {
                { "filter", filter },
                { "expand", expand },
                { "page", page },
                { "perPage", perPage },
                { "sort", sort }
            };
            return _client.SendAsync<PagedCollectionModel<T>>(path, HttpMethod.Get, query: query, cancellationToken: cancellationToken); ;
        }

        public virtual Result<IEnumerable<T>> GetFullList<T>(int batch = 100, string? filter = null, string? sort = null, string? expand = null, CancellationToken cancellationToken = default)
        {
            List<T> result = new();
            int currentPage = 1;
            Result<PagedCollectionModel<T>> lastResponse;
            do
            {
                lastResponse = List<T>(page: currentPage, perPage: batch, filter: filter,expand: expand, sort: sort, cancellationToken: cancellationToken);
                if (lastResponse.IsSuccess && lastResponse.Value.Items is not null)
                {
                    result.AddRange(lastResponse.Value.Items);
                }
                currentPage++;
            } while (lastResponse.IsSuccess && lastResponse.Value.Items?.Count > 0 && lastResponse.Value.TotalItems > result.Count);

            return result;
        }

        public virtual async Task<Result<IEnumerable<T>>> GetFullListAsync<T>(int batch = 100, string? filter = null, string? expand = null, string? sort = null, CancellationToken cancellationToken = default)
        {
            List<T> result = new();
            int currentPage = 1;
            Result<PagedCollectionModel<T>> lastResponse;
            do
            {
                lastResponse = await ListAsync<T>(page: currentPage, perPage: batch, filter: filter, expand: expand, sort: sort, cancellationToken: cancellationToken);
                if (lastResponse.IsSuccess && lastResponse.Value.Items is not null)
                {
                    result.AddRange(lastResponse.Value.Items);
                }
                currentPage++;
            } while (lastResponse.IsSuccess && lastResponse.Value.Items?.Count > 0 && lastResponse.Value.TotalItems > result.Count);

            return result;
        }

        public virtual Result<T> GetOne<T>(string id)
        {
            string url = $"{BasePath(_collectionName)}/{UrlEncode(id)}";
            return _client.Send<T>(url, HttpMethod.Get);
        }

        public virtual Task<Result<T>> GetOneAsync<T>(string id)
        {
            string url = $"{BasePath(_collectionName)}/{UrlEncode(id)}";
            return _client.SendAsync<T>(url, HttpMethod.Get);
        }

        public Task<Result<T>> CreateAsync<T>(T item, string? expand = null, IDictionary<string, string>? headers = null, IEnumerable<IFile>? files = null, CancellationToken cancellationToken = default) where T : BaseModel
        {
            var query = new Dictionary<string, object?>()
            {
                { "expand", expand },
            };
            var body = ConstructBody(item);
            var url = this.BasePath(_collectionName);
            return _client.SendAsync<T>(url, HttpMethod.Post, body: body, headers: headers, query: query, files: files, cancellationToken: cancellationToken);
        }

        public Result<T> Create<T>(T item, string? expand = null, IDictionary<string, string>? headers = null, IEnumerable<IFile>? files = null, CancellationToken cancellationToken = default) where T : BaseModel
        {
            var query = new Dictionary<string, object?>()
            {
                { "expand", expand },
            };
            var body = ConstructBody(item);
            var url = this.BasePath(_collectionName);
            return _client.Send<T>(url, HttpMethod.Post, body: body, headers: headers, query: query, files: files, cancellationToken: cancellationToken);
        }

        public Task<Result<T>> UpdateAsync<T>(T item, string? expand = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default) where T : BaseModel
        {
            var query = new Dictionary<string, object?>()
            {
                { "expand", expand },
            };
            var body = ConstructBody(item);
            var url = this.BasePath(_collectionName) + "/" + UrlEncode(item.Id);
            return _client.SendAsync<T>(url, HttpMethod.Patch, body: body, headers: headers, query: query, cancellationToken: cancellationToken);
        }

        public Task<Result<T>> DeleteAsync<T>(T item, string? expand = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default) where T : BaseModel
        {
            var query = new Dictionary<string, object?>()
            {
                { "expand", expand },
            };
            var body = ConstructBody(item);
            var url = this.BasePath(_collectionName) + "/" + UrlEncode(item.Id);
            return _client.SendAsync<T>(url, HttpMethod.Delete, body: body, headers: headers, query: query, cancellationToken: cancellationToken);
        }





        public Task<Result<T>> UpdateAsyncWithFile<T>(T item, string? expand = null, IDictionary<string, string>? headers = null, IEnumerable<IFile>? files = null, CancellationToken cancellationToken = default) where T : BaseModel
        {
            var query = new Dictionary<string, object?>()
            {
                { "expand", expand },
            };
            var body = ConstructBody(item);
            var url = this.BasePath(_collectionName) + "/" + UrlEncode(item.Id);
            return _client.SendAsync<T>(url, HttpMethod.Patch, body: body, headers: headers, query: query, files: files, cancellationToken: cancellationToken);
        }

        public Result<T> Update<T>(T item, string? expand = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default) where T : BaseModel
        {
            var query = new Dictionary<string, object?>()
            {
                { "expand", expand },
            };
            var body = ConstructBody(item);
            var url = this.BasePath(_collectionName) + "/" + UrlEncode(item.Id);
            return _client.Send<T>(url, HttpMethod.Patch, body: body, headers: headers, query: query, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// subscribe to the specified topic for realtime updates
        /// </summary>
        /// <param name="sub">the topic to subscribe to</param>
        /// <param name="recordId">the id of the specific record or * for the whole collection</param>
        /// <param name="callback">callback, that is invoked every time something changes</param>
        //public async void Subscribe(string recordId, Func<SseMessage, Task> callback)
        //{
        //    string subscribeTo = recordId != "*"
        //            ? $"{_collectionName}/{recordId}"
        //            : _collectionName;

        //    try
        //    {
        //        await _client.RealTime.SubscribeAsync(subscribeTo, callback);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        /// <summary>
        /// subscribe to the realtime events of the server
        /// </summary>
        /// <param name="topic">topic representing colection</param>
        /// <param name="callbackFun">Action to be called when there is alterations in the given topic collection</param>
        


        public async Task UploadFileAsync(string field, string fileName, Stream stream, CancellationToken cancellationToken = default)
        {
            var file = new StreamFile()
            {
                FileName = fileName,
                FieldName = field,
                Stream = stream
            };
            var url = this.BasePath(_collectionName);
            await _client.SendAsync(url, HttpMethod.Post, files: new[] { file }, cancellationToken: cancellationToken);
        }

        public void UploadFile(string field, string fileName, Stream stream, CancellationToken cancellationToken = default)
        {
            var file = new StreamFile()
            {
                FileName = fileName,
                FieldName = field,
                Stream = stream
            };
            var url = this.BasePath(_collectionName);
            _client.Send(url, HttpMethod.Post, files: new[] { file }, cancellationToken: cancellationToken);
        }

    }
}
