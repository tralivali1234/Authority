﻿// Copyright 2012-2016 Chris Patterson
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace Authority
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Runtime;


    public class SupremeAuthority :
        IAuthority,
        AuthorityContext
    {
        readonly INetwork _network;
        readonly ILoggerFactory _loggerFactory;

        public SupremeAuthority(INetwork network, ILoggerFactory loggerFactory)
        {
            _network = network;
            _loggerFactory = loggerFactory;
        }

        public Task<ISession> CreateSession()
        {
            return Task.FromResult<ISession>(new Session(this, _network, _loggerFactory));
        }

        ObserverHandle IObserverConnector.ConnectObserver<T>(IFactObserver<T> observer)
        {
            return _network.ConnectObserver(observer);
        }

        ObserverHandle IObserverConnector.ConnectObserver(IFactObserver observer)
        {
            return _network.ConnectObserver(observer);
        }

        public void Accept<TContext>(RuntimeVisitor<TContext> visitor, TContext context)
        {
            _network.Accept(visitor, context);
        }
    }
}