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
namespace Authority.RuleCompiler
{
    using System;
    using System.Linq.Expressions;


    public class RuleParameter<T> :
        IRuleParameter<T>,
        IEquatable<RuleParameter<T>>
        where T : class
    {
        readonly ParameterExpression _parameter;

        public RuleParameter(ParameterExpression parameter)
        {
            _parameter = parameter;
        }

        public bool Equals(RuleParameter<T> other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return _parameter.Equals(other._parameter);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != GetType())
                return false;
            return Equals((RuleParameter<T>)obj);
        }

        public override int GetHashCode()
        {
            return _parameter.GetHashCode();
        }
    }
}