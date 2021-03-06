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
namespace Authority.Rules.Conditions
{
    using System;
    using System.Linq.Expressions;
    using Builders;
    using Facts;


    public class RuleCondition<T> :
        IRuleCondition<T>
        where T : class
    {
        public RuleCondition(FactDeclaration<T> factDeclaration, Expression<Func<T, bool>> conditionExpression)
        {
            FactDeclaration = factDeclaration;
            ConditionExpression = conditionExpression;
        }

        public static IRuleConditionFactory<T> Factory { get; } = new DefaultRuleConditionFactory<T>();

        public FactDeclaration<T> FactDeclaration { get; }

        public Expression<Func<T, bool>> ConditionExpression { get; }

        public void Apply(IRuntimeBuilder builder, BuilderContext context)
        {
//            context.AddParameter(_factDeclaration.);

            builder.BuildSelectionNode(context, ConditionExpression);

//            protected override void VisitPattern(ReteBuilderContext context, PatternElement element)
//        {
//            if (element.Source == null)
//            {
//                context.CurrentAlphaNode = _root;
//                context.RegisterDeclaration(element.Declaration);
//
//                BuildTypeNode(context, element.ValueType);
//                List<ConditionElement> alphaConditions = element.Conditions.Where(x => x.References.Count() == 1).ToList();
//                foreach (var alphaCondition in alphaConditions)
//                    BuildSelectionNode(context, alphaCondition);
//                BuildAlphaMemoryNode(context);
//
//                List<ConditionElement> betaConditions = element.Conditions.Where(x => x.References.Count() > 1).ToList();
//                if (betaConditions.Count > 0)
//                    BuildJoinNode(context, betaConditions);
//            }
//            else
//            {
//                if (element.Conditions.Any())
//                {
//                    BuildSubnet(context, element.Source);
//                    context.RegisterDeclaration(element.Declaration);
//
//                    BuildJoinNode(context, element.Conditions);
//                }
//                else
//                {
//                    Visit(context, element.Source);
//                    context.RegisterDeclaration(element.Declaration);
//                }
//            }
//        }
        }
    }
}