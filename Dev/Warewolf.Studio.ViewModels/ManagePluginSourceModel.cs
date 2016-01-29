﻿using System;
using System.Collections.Generic;
using Dev2.Common.Interfaces;
using Dev2.Common.Interfaces.ServerProxyLayer;

namespace Warewolf.Studio.ViewModels
{
    public class ManagePluginSourceModel : IManagePluginSourceModel
    {
        readonly IStudioUpdateManager _updateRepository;
        readonly IQueryManager _queryProxy;

        public ManagePluginSourceModel(IStudioUpdateManager updateRepository, IQueryManager queryProxy, string serverName)
        {
            _updateRepository = updateRepository;
            _queryProxy = queryProxy;

            ServerName = serverName;
            if (ServerName.Contains("("))
            {
                ServerName = serverName.Substring(0, serverName.IndexOf("(", StringComparison.Ordinal));
            }

        }

        #region Implementation of IManageDatabaseSourceModel

        public IList<string> GetComputerNames()
        {
            return _queryProxy.GetComputerNames();
        }

        public IList<string> TestDbConnection(IDbSource resource)
        {
            return _updateRepository.TestDbConnection(resource);
        }

        public void Save(IDbSource toDbSource)
        {
            _updateRepository.Save(toDbSource);
        }

        public string ServerName { get; private set; }

        public IList<IFileListing> GetDllListings(IFileListing listing)
        {
            return _queryProxy.GetDllListings(listing);
        }

        public void Save(IPluginSource source)
        {
            _updateRepository.Save(source);
        }

        #endregion
    }
}