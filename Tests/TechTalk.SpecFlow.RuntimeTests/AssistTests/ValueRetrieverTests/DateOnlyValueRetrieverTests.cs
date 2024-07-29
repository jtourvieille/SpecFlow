#if NET6_0_OR_GREATER
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Xunit;
using FluentAssertions;
using TechTalk.SpecFlow.Assist.ValueRetrievers;

namespace TechTalk.SpecFlow.RuntimeTests.AssistTests.ValueRetrieverTests
{
    public class DateOnlyValueRetrieverTests
    {
        private const string IrrelevantKey = "Irrelevant";
        private readonly Type IrrelevantType = typeof(object);

        public DateOnlyValueRetrieverTests()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
        }

        [Theory]
        [InlineData(typeof(DateOnly), true)]
        [InlineData(typeof(DateOnly?), true)]
        [InlineData(typeof(int), false)]
        public void CanRetrieve(Type type, bool expectation)
        {
            var retriever = new DateOnlyValueRetriever();
            var result = retriever.CanRetrieve(new KeyValuePair<string, string>(IrrelevantKey, IrrelevantKey), IrrelevantType, type);
            result.Should().Be(expectation);
        }

        private void Retrieve_correct_value(string value, DateOnly expectation)
        {
            var retriever = new DateOnlyValueRetriever();
            var result = (DateOnly)retriever.Retrieve(new KeyValuePair<string, string>(IrrelevantKey, value), IrrelevantType, typeof(DateOnly));
            result.Should().Be(expectation);
        }

        private void Retrieve_correct_nullable_value(string value, DateOnly? expectation)
        {
            var retriever = new DateOnlyValueRetriever();
            var result = (DateOnly?)retriever.Retrieve(new KeyValuePair<string, string>(IrrelevantKey, value), IrrelevantType, typeof(DateOnly?));
            result.Should().Be(expectation);
        }

        [Fact]
        public void Returns_MinValue_when_the_value_is_null()
        {
            Retrieve_correct_value(null, DateOnly.MinValue);
        }

        [Fact]
        public void Returns_null_when_the_value_is_null_and_type_nullable()
        {
            Retrieve_correct_nullable_value(null, null);
        }

        [Fact]
        public void Returns_MinValue_when_the_value_is_empty()
        {
            Retrieve_correct_value(string.Empty, DateOnly.MinValue);
        }

        [Fact]
        public void Returns_null_when_the_value_is_empty_and_type_nullable()
        {
            Retrieve_correct_nullable_value(string.Empty, null);
        }

        [Fact]
        public void Returns_the_date_when_value_represents_a_valid_date()
        {
            Retrieve_correct_value("1/1/2011", new DateOnly(2011, 1, 1));
            Retrieve_correct_nullable_value("1/1/2011", new DateOnly(2011, 1, 1));
            Retrieve_correct_value("12/31/2015", new DateOnly(2015, 12, 31));
            Retrieve_correct_nullable_value("12/31/2015", new DateOnly(2015, 12, 31));
        }

        [Fact]
        public void Returns_MinValue_when_the_value_is_not_a_valid_dateonly()
        {
            Retrieve_correct_value("xxxx", DateOnly.MinValue);
            Retrieve_correct_nullable_value("xxxx", DateOnly.MinValue);
            Retrieve_correct_value("this is not a date", DateOnly.MinValue);
            Retrieve_correct_nullable_value("this is not a date", DateOnly.MinValue);
            Retrieve_correct_value("Thursday", DateOnly.MinValue);
            Retrieve_correct_nullable_value("Thursday", DateOnly.MinValue);
        }
    }
}
#endif