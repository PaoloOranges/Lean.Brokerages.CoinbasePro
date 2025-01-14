/*
 * QUANTCONNECT.COM - Democratizing Finance, Empowering Individuals.
 * Lean Algorithmic Trading Engine v2.0. Copyright 2014 QuantConnect Corporation.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
*/

using System;
using QuantConnect.Util;
using QuantConnect.Data;
using QuantConnect.Packets;
using QuantConnect.Interfaces;
using QuantConnect.Configuration;
using System.Collections.Generic;

namespace QuantConnect.CoinbaseBrokerage
{
    /// <summary>
    /// An implementation of <see cref="IDataQueueHandler"/> for Coinbase
    /// </summary>
    public partial class CoinbaseBrokerage : IDataQueueHandler
    {
        /// <summary>
        /// Data Aggregator
        /// </summary>
        /// <remarks>
        /// Aggregates ticks and bars
        /// </remarks>
        protected IDataAggregator _aggregator;

        /// <summary>
        /// Subscribe to the specified configuration
        /// </summary>
        /// <param name="dataConfig">defines the parameters to subscribe to a data feed</param>
        /// <param name="newDataAvailableHandler">handler to be fired on new data available</param>
        /// <returns>The new enumerator for this subscription request</returns>
        public IEnumerator<BaseData> Subscribe(SubscriptionDataConfig dataConfig, EventHandler newDataAvailableHandler)
        {
            if (!CanSubscribe(dataConfig.Symbol))
            {
                return null;
            }

            var enumerator = _aggregator.Add(dataConfig, newDataAvailableHandler);
            SubscriptionManager.Subscribe(dataConfig);

            return enumerator;
        }

        /// <summary>
        /// Removes the specified configuration
        /// </summary>
        /// <param name="dataConfig">Subscription config to be removed</param>
        public void Unsubscribe(SubscriptionDataConfig dataConfig)
        {
            SubscriptionManager.Unsubscribe(dataConfig);
            _aggregator.Remove(dataConfig);
        }

        /// <summary>
        /// Sets the job we're subscribing for
        /// </summary>
        /// <param name="job">Job we're subscribing for</param>
        public void SetJob(LiveNodePacket job)
        {
            var aggregator = Composer.Instance.GetExportedValueByTypeName<IDataAggregator>(
                Config.Get("data-aggregator", "QuantConnect.Lean.Engine.DataFeeds.AggregationManager"), forceTypeNameOnExisting: false);

            Initialize(
                webSocketUrl: job.BrokerageData["coinbase-url"],
                apiKey: job.BrokerageData["coinbase-api-key"],
                apiSecret: job.BrokerageData["coinbase-api-secret"],
                restApiUrl: job.BrokerageData["coinbase-rest-api"],
                algorithm: null,
                orderProvider: null,
                aggregator: aggregator,
                job: job
            );

            if (!IsConnected)
            {
                Connect();
            }
        }
    }
}
