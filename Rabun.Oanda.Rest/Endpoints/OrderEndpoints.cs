﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rabun.Oanda.Rest.Base;
using Rabun.Oanda.Rest.Models;

namespace Rabun.Oanda.Rest.Endpoints
{
    public class OrderEndpoints : Endpoint
    {
        private readonly string _ordersRoute = "/v1/accounts/:accountId/orders";
        private readonly string _orderRoute = "/v1/accounts/:accountId/orders/:orderId";
        private readonly int _accountId;

        public OrderEndpoints(string key, AccountType accountType, int accountId)
            : base(key, accountType)
        {
            this._accountId = accountId;
        }

        #region GetOrders

        public class OrdersWrapper
        {
            public List<Order> Orders { get; set; }
        }

        /// <summary>
        /// This will return all pending orders for an account. 
        /// Note: pending take profit or stop loss orders are recorded in the open trade object, and will not be returned in this request.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Order>> GetOrders()
        {
            Dictionary<string, string> routeParams = new Dictionary<string, string>();
            routeParams.Add("accountId", _accountId.ToString());

            OrdersWrapper ordersWrapper = await Get<OrdersWrapper>(routeParams, null, _ordersRoute);
            return ordersWrapper.Orders;

        }

        /// <summary>
        /// This will return all pending orders for an account. 
        /// Note: pending take profit or stop loss orders are recorded in the open trade object, and will not be returned in this request.
        /// </summary>
        /// <param name="instrument">Retrieve open orders for a specific instrument only</param>
        /// <returns></returns>
        public async Task<List<Order>> GetOrders(string instrument)
        {
            Dictionary<string, string> routeParams = new Dictionary<string, string>();
            routeParams.Add("accountId", _accountId.ToString());

            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("instrument", instrument);

            OrdersWrapper ordersWrapper = await Get<OrdersWrapper>(routeParams, properties, _ordersRoute);
            return ordersWrapper.Orders;

        }

        /// <summary>
        /// This will return all pending orders for an account. 
        /// Note: pending take profit or stop loss orders are recorded in the open trade object, and will not be returned in this request.
        /// </summary>
        /// <param name="instrument">Retrieve open orders for a specific instrument only</param>
        /// <param name="count">Maximum number of open orders to return. Default: 50. Max value: 500</param>
        /// <returns></returns>
        public async Task<List<Order>> GetOrders(string instrument, int count)
        {
            Dictionary<string, string> routeParams = new Dictionary<string, string>();
            routeParams.Add("accountId", _accountId.ToString());

            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("instrument", instrument);
            properties.Add("count", count);

            OrdersWrapper ordersWrapper = await Get<OrdersWrapper>(routeParams, properties, _ordersRoute);
            return ordersWrapper.Orders;

        }

        /// <summary>
        /// This will return all pending orders for an account. 
        /// Note: pending take profit or stop loss orders are recorded in the open trade object, and will not be returned in this request.
        /// </summary>
        /// <param name="instrument">Retrieve open orders for a specific instrument only</param>
        /// <param name="count">Maximum number of open orders to return. Default: 50. Max value: 500</param>
        /// <param name="maxId">The server will return orders with id less than or equal to this, in descending order (for pagination)</param>
        /// <param name="ids">An URL encoded comma (%2C) separated list of orders to retrieve. 
        /// Maximum number of ids: 50. No other parameter may be specified with the ids paramete</param>
        /// <returns></returns>
        public async Task<List<Order>> GetOrders(string instrument, int? count, int? maxId, string ids)
        {
            Dictionary<string, string> routeParams = new Dictionary<string, string>();
            routeParams.Add("accountId", _accountId.ToString());

            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("instrument", instrument);
            if (count != null) properties.Add("count", count);
            if (maxId != null) properties.Add("maxId", maxId);
            if (!string.IsNullOrWhiteSpace(ids)) properties.Add("ids", ids);

            OrdersWrapper ordersWrapper = await Get<OrdersWrapper>(routeParams, properties, _ordersRoute);
            return ordersWrapper.Orders;

        }

        #endregion

        #region GetOrder

        /// <summary>
        /// Get information for an order
        /// </summary>
        /// <param name="orderId">Order id</param>
        /// <returns></returns>
        public async Task<Order> GetOrder(int orderId)
        {
            Dictionary<string, string> routeParams = new Dictionary<string, string>();
            routeParams.Add("accountId", _accountId.ToString());
            routeParams.Add("orderId", orderId.ToString());

            Order order = await Get<Order>(routeParams, null, _ordersRoute);
            return order;
        }

        #endregion

        #region CreateOrder

        /// <summary>
        /// Create a new order
        /// </summary>
        /// <param name="instrument">Required Instrument to open the order on</param>
        /// <param name="units">Required The number of units to open order for</param>
        /// <param name="side">Required Direction of the order, either "buy" or "sell"</param>
        /// <param name="type">Required The type of the order "limit", "stop", "marketIfTouched’ or "market"</param>
        /// <param name="expiry">Required If order type is "limit", "stop", or "marketIfTouched". The order expiration time in UTC. The value specified must be in a valid datetime format</param>
        /// <param name="price">Required If order type is "limit", "stop", or "marketIfTouched". The price where the order is set to trigger at</param>
        /// <param name="lowerBound">Optional The minimum execution price</param>
        /// <param name="upperBound">Optional The maximum execution price</param>
        /// <param name="takeProfit">Optional The take profit price</param>
        /// <param name="trailingStop">Optional The trailing stop distance in pips, up to one decimal place</param>
        /// <returns></returns>
        public async Task<Order> CreateOrder(string instrument, int units, OandaTypes.Side side,
            OandaTypes.OrderType type, DateTime? expiry, float? price, float? lowerBound, float? upperBound, int? takeProfit, int? trailingStop)
        {
            Dictionary<string, string> routeParams = new Dictionary<string, string>();
            routeParams.Add("accountId", _accountId.ToString());

            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("instrument", instrument);
            properties.Add("units", units);
            properties.Add("side", side);
            properties.Add("type", type);
            if (expiry != null) properties.Add("expiry", expiry.Value.ToString("o"));
            if (price != null) properties.Add("price", price);
            if (lowerBound != null) properties.Add("lowerBound", lowerBound);
            if (upperBound != null) properties.Add("upperBound", upperBound);
            if (takeProfit != null) properties.Add("takeProfit", takeProfit);
            if (trailingStop != null) properties.Add("trailingStop", trailingStop);

            if (type == OandaTypes.OrderType.market)
            {
                OrderMarket orderMarket = await Post<OrderMarket>(routeParams, properties, _ordersRoute);
                return orderMarket;
            }

            OrderMarketIfTouched orderMarketIfTouched = await Post<OrderMarketIfTouched>(routeParams, properties, _ordersRoute);
            return orderMarketIfTouched;
        }

        /// <summary>
        /// Create a new order
        /// </summary>
        /// <param name="instrument">Required Instrument to open the order on</param>
        /// <param name="units">Required The number of units to open order for</param>
        /// <param name="side">Required Direction of the order, either "buy" or "sell"</param>
        /// <returns></returns>
        public async Task<OrderMarket> CreateMarketOrder(string instrument, int units, OandaTypes.Side side)
        {
            Dictionary<string, string> routeParams = new Dictionary<string, string>();
            routeParams.Add("accountId", _accountId.ToString());

            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("instrument", instrument);
            properties.Add("units", units);
            properties.Add("side", side);
            properties.Add("market", OandaTypes.OrderType.market);

            OrderMarket orderMarket = await Post<OrderMarket>(routeParams, properties, _ordersRoute);
            return orderMarket;
        }

        /// <summary>
        /// Create a new order
        /// </summary>
        /// <param name="instrument">Required Instrument to open the order on</param>
        /// <param name="units">Required The number of units to open order for</param>
        /// <param name="side">Required Direction of the order, either "buy" or "sell"</param>
        /// <param name="expiry">Required If order type is "limit", "stop", or "marketIfTouched". The order expiration time in UTC. The value specified must be in a valid datetime format</param>
        /// <param name="price">Required If order type is "limit", "stop", or "marketIfTouched". The price where the order is set to trigger at</param>
        /// <returns></returns>
        public async Task<OrderMarketIfTouched> CreateMarketIfTouchedOrder(string instrument, int units, OandaTypes.Side side, DateTime expiry, float price)
        {
            Dictionary<string, string> routeParams = new Dictionary<string, string>();
            routeParams.Add("accountId", _accountId.ToString());

            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("instrument", instrument);
            properties.Add("units", units);
            properties.Add("side", side);
            properties.Add("type", OandaTypes.OrderType.marketIfTouched);
            properties.Add("expiry", expiry.ToString("o"));
            properties.Add("price", price);

            OrderMarketIfTouched orderMarketIfTouched = await Post<OrderMarketIfTouched>(routeParams, properties, _ordersRoute);
            return orderMarketIfTouched;
        }

        #endregion

        #region UpdateOrder



        #endregion

    }
}