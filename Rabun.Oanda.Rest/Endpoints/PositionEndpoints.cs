﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rabun.Oanda.Rest.Base;
using Rabun.Oanda.Rest.Models;

namespace Rabun.Oanda.Rest.Endpoints
{
    public class PositionEndpoints : Endpoint
    {

        private static String _positionsRoute = "/v1/accounts/:accountId/positions";
        private static String _positionRoute = "/v1/accounts/:accountId/positions/:instrument";
        private readonly int _accountId;

        public PositionEndpoints(string key, AccountType accountType, int accountId)
            : base(key, accountType)
        {
            _accountId = accountId;
        }

        #region GetPositions

        /// <summary>
        /// Get a list of all open positions
        /// </summary>
        /// <param name="accountId">accountId account id</param>
        /// <returns>list of positions</returns>
        public async Task<List<Position>> GetPositions(int accountId)
        {
            Dictionary<string, string> routeParams = new Dictionary<string, string>();
            routeParams.Add("accountId", _accountId.ToString());

            List<Position> result = await Get<List<Position>>(routeParams, null, _positionsRoute);
            return result;
        }

        /// <summary>
        /// Get the position for an instrument
        /// </summary>
        /// <param name="accountId">accountId account id</param>
        /// <param name="instrument">instrument</param>
        /// <returns>position</returns>
        public async Task<Position> GetPosition(int accountId, string instrument)
        {
            Dictionary<string, string> routeParams = new Dictionary<string, string>();
            routeParams.Add("accountId", _accountId.ToString());
            routeParams.Add("instrument", instrument);

            Position result = await Get<Position>(routeParams, null, _positionsRoute);
            return result;
        }

        /// <summary>
        /// Close an existing position
        /// </summary>
        /// <param name="accountId">accountId account id</param>
        /// <param name="instrument">instrument instrument</param>
        /// <returns></returns>
        public async Task<PositionClosed> ClosePosition(int accountId, string instrument)
        {
            Dictionary<string, string> routeParams = new Dictionary<string, string>();
            routeParams.Add("accountId", _accountId.ToString());
            routeParams.Add("instrument", instrument);

            PositionClosed positionClosed = await Delete<PositionClosed>(routeParams, _positionRoute);
            return positionClosed;
        }

        #endregion
    }
}