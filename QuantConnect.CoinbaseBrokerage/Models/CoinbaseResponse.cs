﻿/*
 * QUANTCONNECT.COM - Democratizing Finance, Empowering Individuals.
 * Lean Algorithmic Trading Engine v2.0. Copyright 2023 QuantConnect Corporation.
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

using Newtonsoft.Json;

namespace QuantConnect.CoinbaseBrokerage.Models;

/// <summary>
/// Coinbase default http response message
/// </summary>
public class CoinbaseResponse
{
    /// <summary>
    /// Whether there are additional pages for this query.
    /// </summary>
    [JsonProperty("has_next")]
    public bool HasNext { get; set; }

    /// <summary>
    /// Cursor for paginating. Users can use this string to pass in the next call to this endpoint, 
    /// and repeat this process to fetch all accounts through pagination.
    /// </summary>
    [JsonProperty("cursor")]
    public string Cursor { get; set; }
}