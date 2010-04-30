﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using FluentAssertions;
using Ncqrs.Domain;
using Ncqrs.Domain.Mapping;
using NUnit.Framework;
using Rhino.Mocks;

namespace Ncqrs.Tests.Domain
{
    public class AggregateRootMappedByConventionTests
    {
        public class EventForPublicMethod : DomainEvent
        {}

        public class EventForProtectedMethod : DomainEvent
        {}

        public class EventForPrivateMethod : DomainEvent
        {}

        public class EventForNoEventHandlerMethod : DomainEvent
        {}

        public class EventForMethodWithWrongMethodName : DomainEvent
        {}

        public class TheAggregateRoot : AggregateRootMappedByConvention
        {
            public int OnEventForPublicMethodInvokedCount;
            public int OnEventForProtectedMethodInvokeCount;
            public int OnEventForPrivateMethodInvokeCount;
            public int OnEventForNoEventHandlerMethodInvokeCount;
            public int FooBarEventForMethodWithWrongMethodNameInvokeCount;

            public new void ApplyEvent(DomainEvent e)
            {
                base.ApplyEvent(e);
            }

            public virtual void OnEventForPublicMethod(EventForPublicMethod e)
            {
                OnEventForPublicMethodInvokedCount++;
            }

            public virtual void OnEventForProtectedMethod(EventForProtectedMethod e)
            {
                OnEventForProtectedMethodInvokeCount++;
            }

            public virtual void OnEventForPrivateMethod(EventForPrivateMethod e)
            {
                OnEventForPrivateMethodInvokeCount++;
            }

            public virtual void FooBarEventForMethodWithWrongMethodName(EventForMethodWithWrongMethodName e)
            {
                FooBarEventForMethodWithWrongMethodNameInvokeCount++;
            }

            [NoEventHandler]
            public virtual void OnEventForNoEventHandlerMethod(EventForNoEventHandlerMethod e)
            {
                OnEventForNoEventHandlerMethodInvokeCount++;
            }
        }

        [Test]
        public void Initializing_one_should_set_the_mapping_strategy_to_convention_based()
        {
            var aggregateRoot = MockRepository.GenerateMock<AggregateRootMappedByConvention>();
            var field = aggregateRoot.GetType().BaseType.BaseType.GetField("_mappingStrategy", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);

            var theStrategy = field.GetValue(aggregateRoot);
            theStrategy.Should().BeOfType<ConventionBasedDomainEventHandlerMappingStrategy>();
        }

        [Test]
        public void Public_event_handlers_should_be_mapped()
        {
            using (var work = NcqrsEnvironment.Get<IUnitOfWorkFactory>().CreateUnitOfWork())
            {
                var target = new TheAggregateRoot();

                target.ApplyEvent(new EventForPublicMethod());

                target.OnEventForPublicMethodInvokedCount.Should().Be(1);
            }
        }

        [Test]
        public void Protected_event_handlers_should_be_mapped()
        {
            using (var work = NcqrsEnvironment.Get<IUnitOfWorkFactory>().CreateUnitOfWork())
            {
                var target = new TheAggregateRoot();

                target.ApplyEvent(new EventForProtectedMethod());

                target.OnEventForProtectedMethodInvokeCount.Should().Be(1);
            }
        }

        [Test]
        public void Private_event_handlers_should_be_mapped()
        {
            using (var work = NcqrsEnvironment.Get<IUnitOfWorkFactory>().CreateUnitOfWork())
            {
                var target = new TheAggregateRoot();

                target.ApplyEvent(new EventForPrivateMethod());

                target.OnEventForPrivateMethodInvokeCount.Should().Be(1);
            }
        }

        [Test]
        public void Method_with_a_wrong_method_name_should_not_be_mapped()
        {
            using (var work = NcqrsEnvironment.Get<IUnitOfWorkFactory>().CreateUnitOfWork())
            {
                var target = new TheAggregateRoot();

                Action act = () => target.ApplyEvent(new EventForMethodWithWrongMethodName());

                act.ShouldThrow<EventNotHandledException>();
            }
        }

        [Test]
        public void Methods_marked_as_no_event_handler_should_not_be_mapped()
        {
            using (var work = NcqrsEnvironment.Get<IUnitOfWorkFactory>().CreateUnitOfWork())
            {
                var target = new TheAggregateRoot();

                Action act = () => target.ApplyEvent(new EventForNoEventHandlerMethod());

                act.ShouldThrow<EventNotHandledException>();
            }
        }
    }
}