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
namespace Authority.Builders
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Microsoft.Extensions.Logging;
    using Rules.Facts;
    using Runtime;


    public class RuntimeBuilder :
        IRuntimeBuilder
    {
        readonly ILogger<RuntimeBuilder> _logger;
        readonly ILoggerFactory _loggerFactory;
        readonly Network _network;

        public RuntimeBuilder(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;

            _logger = loggerFactory.CreateLogger<RuntimeBuilder>();
            _network = new Network(loggerFactory);
        }

//
//        TerminalNode BuildTerminalNode(ReteBuilderContext context, IEnumerable<Declaration> ruleDeclarations)
//        {
//            if (context.AlphaSource != null)
//                BuildJoinNode(context);
//            var factIndexMap = IndexMap.CreateMap(ruleDeclarations, context.Declarations);
//            var terminalNode = new TerminalNode(context.BetaSource, factIndexMap);
//            return terminalNode;
//        }
//
//        protected override void VisitAnd(ReteBuilderContext context, AndElement element)
//        {
//            foreach (var childElement in element.ChildElements)
//            {
//                if (context.AlphaSource != null)
//                    BuildJoinNode(context);
//                Visit(context, childElement);
//            }
//        }
//
//        protected override void VisitOr(ReteBuilderContext context, OrElement element)
//        {
//            throw new InvalidOperationException("Group Or element must be normalized");
//        }
//
//        protected override void VisitForAll(ReteBuilderContext context, ForAllElement element)
//        {
//            throw new InvalidOperationException("ForAll element must be normalized");
//        }
//
//        protected override void VisitNot(ReteBuilderContext context, NotElement element)
//        {
//            BuildSubnet(context, element.Source);
//            BuildNotNode(context);
//        }
//
//        protected override void VisitExists(ReteBuilderContext context, ExistsElement element)
//        {
//            BuildSubnet(context, element.Source);
//            BuildExistsNode(context);
//        }
//
//        protected override void VisitAggregate(ReteBuilderContext context, AggregateElement element)
//        {
//            BuildSubnet(context, element.Source);
//            BuildAggregateNode(context, element);
//        }
//
//        protected override void VisitPattern(ReteBuilderContext context, PatternElement element)
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
//
//        void BuildSubnet(ReteBuilderContext context, RuleElement element)
//        {
//            var subnetContext = new ReteBuilderContext(context);
//            Visit(subnetContext, element);
//
//            if (subnetContext.AlphaSource == null)
//            {
//                var adapter = subnetContext.BetaSource
//                    .Sinks.OfType<ObjectInputAdapter>()
//                    .SingleOrDefault();
//                if (adapter == null)
//                    adapter = new ObjectInputAdapter(subnetContext.BetaSource);
//                subnetContext.AlphaSource = adapter;
//                context.HasSubnet = true;
//            }
//            context.AlphaSource = subnetContext.AlphaSource;
//        }
//
//        void BuildJoinNode(ReteBuilderContext context, IEnumerable<ConditionElement> conditions = null)
//        {
//            var betaConditions = new List<BetaCondition>();
//            if (conditions != null)
//                foreach (var condition in conditions)
//                {
//                    var factIndexMap = IndexMap.CreateMap(condition.References, context.Declarations);
//                    var betaCondition = new BetaCondition(condition.Expression, factIndexMap);
//                    betaConditions.Add(betaCondition);
//                }
//
//            var node = context.BetaSource
//                .Sinks.OfType<JoinNode>()
//                .FirstOrDefault(x =>
//                    (x.RightSource == context.AlphaSource) &&
//                        (x.LeftSource == context.BetaSource) &&
//                        ConditionComparer.AreEqual(x.Conditions, betaConditions));
//            if (node == null)
//            {
//                node = new JoinNode(context.BetaSource, context.AlphaSource);
//                if (context.HasSubnet)
//                    node.Conditions.Insert(0, new SubnetCondition());
//                foreach (var betaCondition in betaConditions)
//                    node.Conditions.Add(betaCondition);
//            }
//            BuildBetaMemoryNode(context, node);
//            context.ResetAlphaSource();
//        }
//
//        void BuildNotNode(ReteBuilderContext context)
//        {
//            var node = context.AlphaSource
//                .Sinks.OfType<NotNode>()
//                .FirstOrDefault(x =>
//                    (x.RightSource == context.AlphaSource) &&
//                        (x.LeftSource == context.BetaSource));
//            if (node == null)
//            {
//                node = new NotNode(context.BetaSource, context.AlphaSource);
//                if (context.HasSubnet)
//                    node.Conditions.Insert(0, new SubnetCondition());
//            }
//            BuildBetaMemoryNode(context, node);
//            context.ResetAlphaSource();
//        }
//
//        void BuildExistsNode(ReteBuilderContext context)
//        {
//            var node = context.AlphaSource
//                .Sinks.OfType<ExistsNode>()
//                .FirstOrDefault(x =>
//                    (x.RightSource == context.AlphaSource) &&
//                        (x.LeftSource == context.BetaSource));
//            if (node == null)
//            {
//                node = new ExistsNode(context.BetaSource, context.AlphaSource);
//                if (context.HasSubnet)
//                    node.Conditions.Insert(0, new SubnetCondition());
//            }
//            BuildBetaMemoryNode(context, node);
//            context.ResetAlphaSource();
//        }
//
//        void BuildAggregateNode(ReteBuilderContext context, AggregateElement element)
//        {
//            var node = context.AlphaSource
//                .Sinks.OfType<AggregateNode>()
//                .FirstOrDefault(x =>
//                    (x.RightSource == context.AlphaSource) &&
//                        (x.LeftSource == context.BetaSource) &&
//                        (x.Name == element.Name) &&
//                        ExpressionMapComparer.AreEqual(x.ExpressionMap, element.ExpressionMap));
//            if (node == null)
//            {
//                node = new AggregateNode(context.BetaSource, context.AlphaSource, element.Name, element.ExpressionMap, element.AggregatorFactory);
//                if (context.HasSubnet)
//                    node.Conditions.Insert(0, new SubnetCondition());
//            }
//            BuildBetaMemoryNode(context, node);
//            context.ResetAlphaSource();
//        }
//
//        void BuildBetaMemoryNode(ReteBuilderContext context, BetaNode betaNode)
//        {
//            if (betaNode.MemoryNode == null)
//                betaNode.MemoryNode = new BetaMemoryNode();
//            context.BetaSource = betaNode.MemoryNode;
//        }
//
//
//        void BuildAlphaMemoryNode(ReteBuilderContext context)
//        {
//            AlphaMemoryNode memoryNode = context.CurrentAlphaNode.MemoryNode;
//
//            if (memoryNode == null)
//            {
//                memoryNode = new AlphaMemoryNode();
//                context.CurrentAlphaNode.MemoryNode = memoryNode;
//            }
//
//            context.AlphaSource = memoryNode;
//        }
        public BuilderContext CreateContext()
        {
            return new RuntimeBuilderContext();
        }

        public ITypeNode<T> BuildTypeNode<T>(BuilderContext context)
            where T : class
        {
            using (_logger.BeginScope($"{nameof(BuildTypeNode)}<{typeof(T).Name}>"))
            {
                TypeNode<T> typeNode = _network.GetTypeNode<T, TypeNode<T>>();

                context.CurrentAlphaNode = typeNode;
                context.AlphaSource = typeNode.MemoryNode;

                return typeNode;
            }
        }

        public ISelectionNode<T> BuildSelectionNode<T>(BuilderContext context, Expression<Func<T, bool>> conditionExpression)
            where T : class
        {
            using (_logger.BeginScope($"{nameof(BuildSelectionNode)}<{typeof(T).Name}>"))
            {
                var alphaCondition = new AlphaCondition<T>(conditionExpression);

                _logger.LogDebug($"Condition: {alphaCondition}");

                var alphaNode = context.CurrentAlphaNode ?? BuildTypeNode<T>(context);

                SelectionNode<T> selectionNode = alphaNode.GetChildNodes<SelectionNode<T>>()
                    .FirstOrDefault(x => x.Condition.Equals(alphaCondition));
                if (selectionNode == null)
                    using (_logger.BeginScope("Create"))
                    {
                        _logger.LogDebug($"Creating selection node: {typeof(T).Name}");

                        selectionNode = new SelectionNode<T>(_loggerFactory, alphaCondition);
                        alphaNode.AddChild(selectionNode);
                    }

                context.CurrentAlphaNode = selectionNode;
                context.AlphaSource = selectionNode.MemoryNode;

                return selectionNode;
            }
        }

        public IBetaNode<T, T> BuildJoinNode<T>(BuilderContext context)
            where T : class
        {
            using (_logger.BeginScope($"{nameof(BuildJoinNode)}<{typeof(T).Name}>"))
            {
                var betaSource = (context.BetaSource ?? new DummyNode<T>()) as ITupleSource<T>;
                var alphaSource = context.AlphaSource as IFactSource<T>;

                _logger.LogDebug($"Creating join node: {typeof(T).Name}");

                var node = new JoinNode<T, T>(betaSource, alphaSource, new BetaCondition<T, T>((x, y) => true));

                context.BetaSource = node.MemoryNode;

                context.ClearAlphaSource();

                return node;
            }
        }

        public ITerminalNode<T> BuildTerminalNode<T>(BuilderContext context, FactDeclaration<T> factDeclaration)
            where T : class
        {
            using (_logger.BeginScope($"{nameof(BuildTerminalNode)}<{typeof(T).Name}>"))
            {
                if (context.AlphaSource != null)
                    BuildJoinNode<T>(context);

                var factIndexMap = context.CreateIndexMap(factDeclaration);

                var betaSource = context.BetaSource as ITupleSource<T>;

                _logger.LogDebug($"Creating terminal node: {typeof(T).Name}");

                return new TerminalNode<T>(betaSource, factIndexMap);
            }
        }

        public AlphaBuilderContext<T> CreateContext<T>(FactDeclaration<T> declaration) where T : class
        {
            using (_logger.BeginScope($"{nameof(CreateContext)}<{typeof(T).Name}>"))
            {
                TypeNode<T> typeNode = _network.GetTypeNode<T, TypeNode<T>>();

                return new RuntimeAlphaBuilderContext<T>(declaration, typeNode);
            }
        }

        public AlphaBuilderContext<T> BuildSelectionNode<T>(AlphaBuilderContext<T> context, Expression<Func<T, bool>> conditionExpression)
            where T : class
        {
            using (_logger.BeginScope($"{nameof(BuildSelectionNode)}<{typeof(T).Name}>"))
            {
                var alphaCondition = new AlphaCondition<T>(conditionExpression);

                _logger.LogDebug($"Condition: {alphaCondition}");

                IAlphaNode<T> alphaNode = context.CurrentNode;

                SelectionNode<T> selectionNode = alphaNode.GetChildNodes<SelectionNode<T>>()
                    .FirstOrDefault(x => x.Condition.Equals(alphaCondition));
                if (selectionNode == null)
                    using (_logger.BeginScope("Create"))
                    {
                        _logger.LogDebug($"Creating selection node: {typeof(T).Name}");

                        selectionNode = new SelectionNode<T>(_loggerFactory, alphaCondition);
                        alphaNode.AddChild(selectionNode);
                    }

                return new RuntimeAlphaBuilderContext<T>(context.Declaration, selectionNode);
            }
        }

        public BetaBuilderContext<T, T> BuildJoinNode<T>(AlphaBuilderContext<T> context)
            where T : class
        {
            using (_logger.BeginScope($"{nameof(BuildJoinNode)}<{typeof(T).Name}>"))
            {
                var betaSource = new DummyNode<T>() as ITupleSource<T>;
                var alphaSource = context.CurrentSource as IFactSource<T>;

                _logger.LogDebug($"Creating join node: {typeof(T).Name}");

                var node = new JoinNode<T, T>(betaSource, alphaSource, new BetaCondition<T, T>((x, y) => true));

                return new RuntimeBetaBuilderContext<T, T>(context.Declaration, node);
            }
        }

        public BetaBuilderContext<TLeft, T> BuildJoinNode<T, TLeft>(AlphaBuilderContext<T> context, BetaBuilderContext<TLeft> betaContext)
            where T : class 
            where TLeft : class
        {
            using (_logger.BeginScope($"{nameof(BuildJoinNode)}<{typeof(T).Name}>"))
            {
                var betaSource = betaContext.CurrentSource as ITupleSource<TLeft>;
                var alphaSource = context.CurrentSource as IFactSource<T>;

                _logger.LogDebug($"Creating join node: {typeof(T).Name}");

                var node = new JoinNode<TLeft, T>(betaSource, alphaSource, new BetaCondition<TLeft, T>((x, y) => true));

                return new RuntimeBetaBuilderContext<TLeft, T>(context.Declaration, node);
            }
        }

        public ITerminalNode<T> BuildTerminalNode<T>(AlphaBuilderContext<T> context)
            where T : class
        {
            using (_logger.BeginScope($"{nameof(BuildTerminalNode)}<{typeof(T).Name}>"))
            {
                BetaBuilderContext<T> betaContext = BuildJoinNode(context);

                var factIndexMap = betaContext.CreateIndexMap(betaContext.Declaration);

                _logger.LogDebug($"Creating terminal node: {typeof(T).Name}");

                return new TerminalNode<T>(betaContext.CurrentSource, factIndexMap);
            }
        }

//        public IEnumerable<ITerminalNode> AddRule(IRuleDefinition ruleDefinition)
//        {
//            List<Declaration> ruleDeclarations = ruleDefinition.LeftHandSide.Declarations.ToList();
//            var terminals = new List<ITerminalNode>();
//            ruleDefinition.LeftHandSide.Match(
//                and =>
//                {
//                    var context = new ReteBuilderContext(_dummyNode);
//                    Visit(context, and);
//                    var terminalNode = BuildTerminalNode(context, ruleDeclarations);
//                    terminals.Add(terminalNode);
//                },
//                or =>
//                {
//                    foreach (var childElement in or.ChildElements)
//                    {
//                        var context = new ReteBuilderContext(_dummyNode);
//                        Visit(context, childElement);
//                        var terminalNode = BuildTerminalNode(context, ruleDeclarations);
//                        terminals.Add(terminalNode);
//                    }
//                });
//            return terminals;
//        }
//
        public INetwork Build()
        {
            return _network;
        }
    }
}