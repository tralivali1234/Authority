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
namespace Authority.Runtime
{
    using System;
    using System.Threading.Tasks;


    public class BetaCondition<TLeft, TRight> :
        IBetaCondition<TLeft, TRight>
        where TLeft : class
        where TRight : class
    {
        readonly Func<ITuple<TLeft>, TRight, bool> _compare;

        public BetaCondition(Func<ITuple<TLeft>, TRight, bool> compare)
        {
            _compare = compare;
        }

        public Task<bool> Evaluate(SessionContext context, ITuple<TLeft> tuple, TRight fact)
        {
            return Task.FromResult(_compare(tuple, fact));
        }
    }
}