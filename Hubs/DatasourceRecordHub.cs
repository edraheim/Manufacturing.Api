﻿using System.Reflection;
using log4net;
using Manufacturing.Api.Data.Model;
using Manufacturing.Api.Hubs.ChannelResolvers;
using Manufacturing.Framework.Datasource;
using Manufacturing.Framework.Dto;
using Microsoft.AspNet.SignalR;

namespace Manufacturing.Api.Hubs
{
    public class DatasourceRecordHub : Hub
    {
        #region Fields

        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IChannelResolver<DatasourceRecord> _channelResolver;

        #endregion

        #region Constructors

        public DatasourceRecordHub() : this(new DatasourceRecordChannelResolver<DatasourceRecord>()) { }

        internal DatasourceRecordHub(IChannelResolver<DatasourceRecord> channelResolver)
        {
            _channelResolver = channelResolver;
        }

        #endregion

        #region public

        public void Notify(DatasourceRecord message)
        {
            // TODO: FILTER! Might be able to do group subscriptions instead of channel resolvers
            //var id = _channelResolver.GetChannelId(message);

            //_log.DebugFormat("Received a message from {0} with an ID of {1}", message.DatasourceId, id);

            //if(id == null)
            //{
            //    // log or something
            //    return;
            //}

            //Clients.Client(id).notify(message);

            // Convert to DataRecord (type mismatch???)
            var dataRecord = new DataRecord
            {
                DatasourceId = message.DatasourceId,
                EncodedDataType = message.EncodedDataType,
                IntervalSeconds = message.IntervalSeconds,
                Timestamp = message.Timestamp,
                Value = message.Value
            };

            Clients.All.notify(dataRecord);
        }

        public void Notify(DataRecord message)
        {
            Clients.All.notify(message);
        }

        public void Register(string id)
        {
            _channelResolver.SetChannelId(id, Context.ConnectionId);
        }

        #endregion
    }
}