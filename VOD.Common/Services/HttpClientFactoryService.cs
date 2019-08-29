﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VOD.Common.Exceptions;
using VOD.Common.Extensions;

namespace VOD.Common.Services
{
    public class HttpClientFactoryService : IHttpClientFactoryService
    {
        #region Variables
        private readonly IHttpClientFactory _httpClientFactory;
        private CancellationTokenSource _cancellationTokenSource;
        private readonly CancellationToken _cancellationToken;
        #endregion

        #region Constructor
        public HttpClientFactoryService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
        }
        #endregion

        public async Task<List<TResponse>> GetListAsync<TResponse>(string uri, string serviceName, string token = "") where TResponse : class
        {
            try
            {
                if (new string[] { uri, serviceName }.IsNullOrEmptyOrWhiteSpace())
                    throw new HttpResponseException(HttpStatusCode.NotFound, "Could not find the resource");
                var httpClient = _httpClientFactory.CreateClient(serviceName);
                return await httpClient.GetListAsync<TResponse>(uri.ToLower(), _cancellationToken, token);
            }
            catch
            {
                throw;
            }
        }

        public async Task<TResponse> GetAsync<TResponse>(string uri, string serviceName, string token = "")
        {
            try
            {
                if (new[] { uri, serviceName }.IsNullOrEmptyOrWhiteSpace())
                    throw new HttpResponseException(HttpStatusCode.NotFound, "Could not find the resource");
                var httpClient = _httpClientFactory.CreateClient(serviceName);
                return await httpClient.GetAsync<TResponse, string>(uri.ToLower(), _cancellationToken, null, token);
            }
            catch
            {
                throw;
            }
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(TRequest content, string uri, string serviceName, string token = "") where TResponse : class where TRequest : class
        {
            try
            {
                if (new string[] { uri, serviceName }.IsNullOrEmptyOrWhiteSpace())
                    throw new HttpResponseException(HttpStatusCode.NotFound, "Could not find the resource");
                var httpClient = _httpClientFactory.CreateClient(serviceName);
                return await httpClient.PostAsync<TRequest, TResponse>(uri.ToLower(), content, _cancellationToken, token);
            }
            catch
            {
                throw;
            }
        }
        public async Task<TResponse> PutAsync<TRequest, TResponse>(TRequest content, string uri, string serviceName, string token = "") where TResponse : class where TRequest : class
        {
            try
            {
                if (new string[] { uri, serviceName }.IsNullOrEmptyOrWhiteSpace())
                    throw new HttpResponseException(HttpStatusCode.NotFound,
                        "Could not find the resource");
                var httpClient = _httpClientFactory.CreateClient(serviceName);
                return await httpClient.PutAsync<TRequest, TResponse>(
                    uri.ToLower(), content, _cancellationToken, token);
            }
            catch
            {
                throw;
            }
        }
        public async Task<string> DeleteAsync(string uri, string serviceName, string token = "")
        {
            try
            {
                if (new string[] { uri, serviceName }.IsNullOrEmptyOrWhiteSpace())
                    throw new HttpResponseException(HttpStatusCode.NotFound, "Could not find the resource");
                var httpClient = _httpClientFactory.CreateClient(serviceName);
                return await httpClient.DeleteAsync(uri.ToLower(), _cancellationToken, token);
            }
            catch
            {
                throw;
            }
        }
    }
}
