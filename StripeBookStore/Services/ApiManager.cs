﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using StripeBookStore.Shared.Interfaces;
using StripeBookStore.Shared.Models.DTOs;
using StripeBookStore.Shared.Services;
using Xamarin.Essentials;
using Xamarin.Essentials.Interfaces;

namespace StripeBookStore.Services
{
    public class ApiManager : BaseApiService, IApiManager
    {
        IStripeBookStoreApi _stripeBookStoreApi;
        IConnectivity _connectivityService;
        bool IsConnected { get; set; }
        Dictionary<int, CancellationTokenSource> runningTasks = new Dictionary<int, CancellationTokenSource>();

        public ApiManager(IStripeBookStoreApi stripeBookStoreApi, IConnectivity connectivityService)
        {
            _stripeBookStoreApi = stripeBookStoreApi;
            _connectivityService = connectivityService;

            _connectivityService.ConnectivityChanged += OnConnectivityChanged;
        }

        ~ApiManager()
        {
            _connectivityService.ConnectivityChanged += OnConnectivityChanged;
        }

        private void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            IsConnected = e.NetworkAccess == NetworkAccess.Internet;

            if (!IsConnected)
            {
                //Cancel all running tasks
                var items = runningTasks.ToList();
                foreach (var item in items)
                {
                    item.Value.Cancel();
                    runningTasks.Remove(item.Key);
                }
            }
        }

        public async Task<HttpResponseMessage> GetStripePublicKeys(CancellationTokenSource cts)
        {
            var task = AttemptAndRetry(() => _stripeBookStoreApi.GetStripePublicKeys(), cts.Token);
            runningTasks.Add(task.Id, cts);

            return await task;
        }

        public async Task<HttpResponseMessage> PostPaymentIntent(PaymentRequest request, CancellationTokenSource cts)
        {
            var task = AttemptAndRetry(() => _stripeBookStoreApi.PostPaymentIntent(request), cts.Token);
            runningTasks.Add(task.Id, cts);

            return await task;
        }

        public Task<HttpResponseMessage> GetCustomers(CancellationTokenSource cts, string startingAfter = "", int pageSize = 25)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> GetProducts(CancellationTokenSource cts, string startingAfter = "", int pageSize = 25)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> GetPaymentIntents(CancellationTokenSource cts, string startingAfter = "", int pageSize = 25)
        {
            throw new NotImplementedException();
        }
    }
}
