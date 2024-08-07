﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensionsTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.String.Recipes.Test
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    using FakeItEasy;

    using FluentAssertions;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.AutoFakeItEasy;
    using OBeautifulCode.CodeAnalysis.Recipes;

    using Xunit;

    public static class StringExtensionsTest
    {
        [Fact]
        public static void AppendMissing_ParameterValueIsNull_ThrowsArgumentNullException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => StringExtensions.AppendMissing(null, string.Empty));
            Assert.Throws<ArgumentNullException>(() => StringExtensions.AppendMissing(null, "sometext"));
        }

        [Fact]
        public static void AppendMissing_ParameterShouldEndWithIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            const string Base1 = "some text";

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => Base1.AppendMissing(null));
        }

        [Fact]
        public static void AppendMissing_StringToAppendIsEmpty_AlwaysReturnBaseString()
        {
            // Arrange
            string base1 = string.Empty;
            const string Base2 = " ";
            const string Base3 = "   \r\n ";
            const string Base4 = "here is some\r\nmore text";

            // Act
            string actual1 = base1.AppendMissing(string.Empty);
            string actual2 = Base2.AppendMissing(string.Empty);
            string actual3 = Base3.AppendMissing(string.Empty);
            string actual4 = Base4.AppendMissing(string.Empty);

            // Assert
            Assert.Equal(base1, actual1);
            Assert.Equal(Base2, actual2);
            Assert.Equal(Base3, actual3);
            Assert.Equal(Base4, actual4);
        }

        [Fact]
        public static void AppendMissing_BaseStringAlreadyEndsWithStringToAppend_ReturnsBaseString()
        {
            // Arrange
            const string Base1 = " ";
            const string Base2 = "     ";
            const string Base3 = "  \r\n ";
            const string Base4 = "here is some\r\nmore text ";
            const string Base5 = "http://www.microsoft.com/";

            // Act
            string actual1 = Base1.AppendMissing(" ");
            string actual2 = Base2.AppendMissing(" ");
            string actual3 = Base3.AppendMissing(" ");
            string actual4a = Base4.AppendMissing(" ");
            string actual4b = Base4.AppendMissing("more text ");
            string actual4c = Base4.AppendMissing("\r\nmore text ");
            string actual4d = Base4.AppendMissing("some\r\nmore text ");
            string actual5 = Base5.AppendMissing("/");

            // Assert
            Assert.Equal(Base1, actual1);
            Assert.Equal(Base2, actual2);
            Assert.Equal(Base3, actual3);
            Assert.Equal(Base4, actual4a);
            Assert.Equal(Base4, actual4b);
            Assert.Equal(Base4, actual4c);
            Assert.Equal(Base4, actual4d);
            Assert.Equal(Base5, actual5);
        }

        [Fact]
        public static void AppendMissing_BaseStringDoesNotEndWithStringToAppend_ReturnsBaseStringWithEndingStringAppended()
        {
            // Arrange
            const string Base1 = " ";
            const string Base2 = "     ";
            const string Base3 = "  \r\n  ";
            const string Base4 = "here is some\r\nmore text";
            const string Base5 = "http://www.microsoft.com";

            const string Append1 = ".";
            const string Append2 = "%sometext#";
            const string Append3a = "+_+";
            const string Append3b = "\r\n";
            const string Append4a = "y";
            const string Append4b = "/";
            const string Append5 = "/IMissVista-JustKidding";

            // Act
            string actual1 = Base1.AppendMissing(Append1);
            string actual2 = Base2.AppendMissing(Append2);
            string actual3a = Base3.AppendMissing(Append3a);
            string actual3b = Base3.AppendMissing(Append3b);
            string actual4a = Base4.AppendMissing(Append4a);
            string actual4b = Base4.AppendMissing(Append4b);
            string actual5 = Base5.AppendMissing(Append5);

            // Assert
            Assert.Equal(Base1 + Append1, actual1);
            Assert.Equal(Base2 + Append2, actual2);
            Assert.Equal(Base3 + Append3a, actual3a);
            Assert.Equal(Base3 + Append3b, actual3b);
            Assert.Equal(Base4 + Append4a, actual4a);
            Assert.Equal(Base4 + Append4b, actual4b);
            Assert.Equal(Base5 + Append5, actual5);
        }

        [Fact]
        public static void FromCsv___Should_return_empty_collection___When_parameter_value_is_null()
        {
            // Arrange, Act
            var actual = StringExtensions.FromCsv(null);

            // Assert
            actual.Should().BeEmpty();
        }

        [Fact]
        public static void FromCsv___Should_return_collection_with_single_empty_element___When_parameter_value_is_the_empty_string()
        {
            // Arrange
            var value = string.Empty;

            // Act
            var actual = value.FromCsv();

            // Assert
            actual.Should().ContainSingle();
            actual.First().Should().Be(value);
        }

        [Fact]
        public static void FromCsv___Should_return_collection_with_single_null_element___When_parameter_value_is_the_empty_string_and_nullValueEncoding_is_the_empty_string()
        {
            // Arrange
            var value = string.Empty;

            // Act
            var actual = value.FromCsv(nullValueEncoding: string.Empty);

            // Assert
            actual.Should().ContainSingle();
            actual.First().Should().BeNull();
        }

        [Fact]
        public static void FromCsv___Should_return_collection_with_empty_string_elements___When_CSV_has_leading_or_trailing_commas()
        {
            // Arrange
            var tests = new[]
            {
                new { Csv = ",", Expected = new[] { string.Empty, string.Empty } },
                new { Csv = ",good,", Expected = new[] { string.Empty, "good", string.Empty } },
            };

            // Act, Assert
            foreach (var test in tests)
            {
                test.Csv.FromCsv().Should().Equal(test.Expected);
            }
        }

        [Fact]
        public static void FromCsv___Should_return_collection_with_null_elements___When_CSV_has_leading_or_trailing_commas_and_nullValueEncoding_is_empty_string()
        {
            // Arrange
            var tests = new[]
            {
                new { Csv = ",", Expected = new[] { (string)null, null } },
                new { Csv = ",good,", Expected = new[] { null, "good", null } },
            };

            // Act, Assert
            foreach (var test in tests)
            {
                test.Csv.FromCsv(nullValueEncoding: string.Empty).Should().Equal(test.Expected);
            }
        }

        [Fact]
        public static void FromCsv___Should_return_collection_empty_string_elements___When_CSV_has_leading_or_trailing_commas_and_nullValueEncoding_is_a_well_known_token()
        {
            // Arrange
            var tests = new[]
            {
                new { Csv = ",<null>", Expected = new[] { string.Empty, null } },
                new { Csv = ",<null>,", Expected = new[] { string.Empty, null, string.Empty } },
            };

            // Act, Assert
            foreach (var test in tests)
            {
                test.Csv.FromCsv(nullValueEncoding: "<null>").Should().Equal(test.Expected);
            }
        }

        [Fact]
        public static void FromCsv___Should_parse_CSV_encoded_string_into_individual_values_treating_empty_string_as_empty_string___When_parameter_nullValueEncoding_is_null()
        {
            // Arrange
            var tests = new[]
            {
                new { Original = " ", CsvSafe = "\" \"" },
                new { Original = "\"", CsvSafe = "\"\"\"\"" },
                new { Original = ",", CsvSafe = "\",\"" },
                new { Original = string.Empty, CsvSafe = string.Empty },
                new { Original = "Super, luxurious truck", CsvSafe = "\"Super, luxurious truck\"" },
                new { Original = "Super \"luxurious\" truck", CsvSafe = "\"Super \"\"luxurious\"\" truck\"" },
                new { Original = "Go get one now\r\nthey are going fast", CsvSafe = "\"Go get one now\r\nthey are going fast\"" },
                new { Original = "Go get one now\nthey are going fast", CsvSafe = "\"Go get one now\nthey are going fast\"" },
                new { Original = "Go get one now" + Environment.NewLine + "they are going fast", CsvSafe = "\"Go get one now" + Environment.NewLine + "they are going fast\"" },
                new { Original = "Super luxurious truck    ", CsvSafe = "\"Super luxurious truck    \"" },
                new { Original = "  Super luxurious truck", CsvSafe = "\"  Super luxurious truck\"" },
                new { Original = "  Super luxurious truck    ", CsvSafe = "\"  Super luxurious truck    \"" },
                new { Original = " Super, luxurious truck ", CsvSafe = "\" Super, luxurious truck \"" },
                new { Original = " Super, \"luxurious\" truck ", CsvSafe = "\" Super, \"\"luxurious\"\" truck \"" },
                new { Original = "something-boring", CsvSafe = "something-boring" },
            };

            var csv = tests.Select(_ => _.CsvSafe).Aggregate((working, next) => working + "," + next);

            // Act
            var actual = csv.FromCsv();

            // Assert
            actual.Should().Equal(tests.Select(_ => _.Original));
        }

        [Fact]
        public static void FromCsv___Should_parse_CSV_encoded_string_into_individual_values_treating_empty_string_as_null___When_parameter_nullValueEncoding_is_empty_string()
        {
            // Arrange
            var tests = new[]
            {
                new { Original = " ", CsvSafe = "\" \"" },
                new { Original = (string)null, CsvSafe = string.Empty },
                new { Original = "something-boring", CsvSafe = "something-boring" },
            };

            var csv = tests.Select(_ => _.CsvSafe).Aggregate((working, next) => working + "," + next);

            // Act
            var actual = csv.FromCsv(nullValueEncoding: string.Empty);

            // Assert
            actual.Should().Equal(tests.Select(_ => _.Original));
        }

        [Fact]
        public static void FromCsv___Should_parse_CSV_encoded_string_into_individual_values_treating_well_known_token_as_null___When_parameter_nullValueEncoding_is_a_well_known_token()
        {
            // Arrange
            var tests = new[]
            {
                new { Original = " ", CsvSafe = "\" \"" },
                new { Original = (string)null, CsvSafe = "<null>" },
                new { Original = "something-boring", CsvSafe = "something-boring" },
            };

            var csv = tests.Select(_ => _.CsvSafe).Aggregate((working, next) => working + "," + next);

            // Act
            var actual = csv.FromCsv(nullValueEncoding: "<null>");

            // Assert
            actual.Should().Equal(tests.Select(_ => _.Original));
        }

        [Fact]
        public static void IsAlphanumeric_StringToCheckIsNull_ThrowsArgumentNullException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => StringExtensions.IsAlphanumeric(null));
        }

        [Fact]
        public static void IsAlphanumeric_StringToCheckIsEmpty_ReturnsTrue()
        {
            // Arrange, Act, Assert
            Assert.True(string.Empty.IsAlphanumeric());
        }

        [Fact]
        public static void IsAlphanumericTest_StringIsNotAlphanumeric_ReturnsFalse()
        {
            // Arrange, Act, Assert
            Assert.False("q9*z".IsAlphanumeric());
            Assert.False("!q9z".IsAlphanumeric());
            Assert.False("q9@z".IsAlphanumeric());
            Assert.False("q9asdfz+".IsAlphanumeric());
            Assert.False("abCd29382-afjKsdf9209283".IsAlphanumeric());
            Assert.False("abCd29382-afjKsdf9209283".IsAlphanumeric(new[] { '_' }));

            for (int i = 0; i <= 47; i++)
            {
                Assert.False(Convert.ToString((char)i, CultureInfo.InvariantCulture).IsAlphanumeric());
                Assert.False(Convert.ToString((char)i, CultureInfo.InvariantCulture).IsAlphanumeric(new char[0]));
            }

            for (int i = 58; i <= 64; i++)
            {
                Assert.False(Convert.ToString((char)i, CultureInfo.InvariantCulture).IsAlphanumeric());
                Assert.False(Convert.ToString((char)i, CultureInfo.InvariantCulture).IsAlphanumeric(new char[0]));
            }

            for (int i = 91; i <= 96; i++)
            {
                Assert.False(Convert.ToString((char)i, CultureInfo.InvariantCulture).IsAlphanumeric());
                Assert.False(Convert.ToString((char)i, CultureInfo.InvariantCulture).IsAlphanumeric(new char[0]));
            }

            for (int i = 123; i <= 127; i++)
            {
                Assert.False(Convert.ToString((char)i, CultureInfo.InvariantCulture).IsAlphanumeric());
                Assert.False(Convert.ToString((char)i, CultureInfo.InvariantCulture).IsAlphanumeric(new char[0]));
            }
        }

        [Fact]
        public static void IsAlphanumericTest_StringIsAlphanumeric_ReturnsTrue()
        {
            // Arrange, Act, Assert
            Assert.True("A".IsAlphanumeric());
            Assert.True("a".IsAlphanumeric());
            Assert.True("ABcd".IsAlphanumeric());
            Assert.True("0".IsAlphanumeric());
            Assert.True("q9z".IsAlphanumeric());
            Assert.True("ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".IsAlphanumeric());
            Assert.True("1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ".IsAlphanumeric());
            Assert.True("abcdefghijklmnopqrstuvwxyz1234567890".IsAlphanumeric());
            Assert.True("1234567890abcdefghijklmnopqrstuvwxyz".IsAlphanumeric());
            Assert.True("+".IsAlphanumeric(new[] { '+' }));
            Assert.True("abcdefghijkl=34567890".IsAlphanumeric(new[] { '+', '=' }));
            Assert.True("abcdefghijkl=34567+890".IsAlphanumeric(new[] { '+', '=' }));
        }

        [Fact]
        public static void IsAlphabetic___Should_throw_ArgumentNullException___When_parameter_value_is_null()
        {
            // Arrange
            var actual = Record.Exception(() => StringExtensions.IsAlphabetic(null));

            // Act, Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.AsTest().Must().ContainString("value");
        }

        [Fact]
        public static void IsAlphabetic___Should_return_true___When_parameter_value_is_an_empty_string()
        {
            // Arrange, Act
            var actual = string.Empty.IsAlphabetic();

            // Assert
            actual.AsTest().Must().BeTrue();
        }

        [Fact]
        public static void IsAlphabetic___Should_return_false___When_value_is_not_alphabetic()
        {
            // Arrange, Act, Assert
            Assert.False("q9".IsAlphabetic());
            Assert.False("9q".IsAlphabetic());
            Assert.False("ab2cd".IsAlphabetic());
            Assert.False("123".IsAlphabetic());
            Assert.False("ab0gf".IsAlphabetic(new[] { '1' }));

            for (int i = 0; i <= 64; i++)
            {
                Assert.False(Convert.ToString((char)i, CultureInfo.InvariantCulture).IsAlphabetic());
                Assert.False(Convert.ToString((char)i, CultureInfo.InvariantCulture).IsAlphabetic(new char[0]));
            }

            for (int i = 91; i <= 96; i++)
            {
                Assert.False(Convert.ToString((char)i, CultureInfo.InvariantCulture).IsAlphabetic());
                Assert.False(Convert.ToString((char)i, CultureInfo.InvariantCulture).IsAlphabetic(new char[0]));
            }

            for (int i = 123; i <= 127; i++)
            {
                Assert.False(Convert.ToString((char)i, CultureInfo.InvariantCulture).IsAlphabetic());
                Assert.False(Convert.ToString((char)i, CultureInfo.InvariantCulture).IsAlphabetic(new char[0]));
            }
        }

        [Fact]
        public static void IsAlphabetic___Should_return_true___When_value_is_alphabetic()
        {
            // Arrange, Act, Assert
            Assert.True("A".IsAlphabetic());
            Assert.True("a".IsAlphabetic());
            Assert.True("ABcd".IsAlphabetic());
            Assert.True("ABCDEFGHIJKLMNOPQRSTUVWXYZ".IsAlphabetic());
            Assert.True("abcdefghijklmnopqrstuvwxyz".IsAlphabetic());
            Assert.True("_".IsAlphabetic(new[] { '_' }));
            Assert.True("ABCDEFGHIJK_MNOPQRSTUVWXYZ".IsAlphabetic(new[] { '+', '_' }));
            Assert.True("ABCDEFGHIJK_MNOPQRS+UVWXYZ".IsAlphabetic(new[] { '+', '_' }));
        }

        [Fact]
        public static void IsAsciiPrintable___Should_throw_ArgumentNullException___When_value_is_null()
        {
            // Arrange, Act
            var actual = Record.Exception(() => StringExtensions.IsAsciiPrintable(null));

            // Assert
            actual.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public static void IsAsciiPrintable___Should_return_true___When_value_is_the_empty_string()
        {
            // Arrange, Act
            var actual = string.Empty.IsAsciiPrintable();

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public static void IsAsciiPrintable___Should_return_true___When_value_contains_all_printable_characters()
        {
            // Arrange
            var value = @"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ !""#$%&'()*+,-./0123456789:;<=>?@[\]^_`{|}~";
            var actuals = new List<bool>();

            // Act
            var actual = value.IsAsciiPrintable();
            foreach (var character in value)
            {
                actuals.Add(character.ToString().IsAsciiPrintable());
            }

            // Assert
            actual.Should().BeTrue();
            actuals.Should().AllBeEquivalentTo(true);
        }

        [Fact]
        public static void IsAsciiPrintable___Should_return_false___When_value_contains_characters_not_in_the_printable_set()
        {
            // Arrange
            var value = $@"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ !""#$%&'()*+{Environment.NewLine},-./0123456789:;<=>?@[\]^_`{{|}}~";

            // Act
            var actual = value.IsAsciiPrintable();
            var actuals = new List<bool>();

            for (int i = 0; i <= 31; i++)
            {
                actuals.Add(Convert.ToChar(i).ToString().IsAsciiPrintable());
            }

            for (int i = 127; i <= 127; i++)
            {
                actuals.Add(Convert.ToChar(i).ToString().IsAsciiPrintable());
            }

            // Assert
            actual.Should().BeFalse();
            actuals.Should().AllBeEquivalentTo(false);
        }

        [Fact]
        public static void ReplaceCaseInsensitive_ParameterValueIsNull_ThrowsArgumentNullException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => StringExtensions.ReplaceCaseInsensitive(null, string.Empty, string.Empty));
            Assert.Throws<ArgumentNullException>(() => StringExtensions.ReplaceCaseInsensitive(null, "text", "EX"));
        }

        [Fact]
        public static void ReplaceCaseInsensitive_ParameterOldValueIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            const string Original = "my string";

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => Original.ReplaceCaseInsensitive(null, "some replacement"));
        }

        [Fact]
        public static void ReplaceCaseInsensitive_ParameterOldValueIsEmpty_ThrowsArgumentException()
        {
            // Arrange
            const string Original = "my string";

            // Act, Assert
            Assert.Throws<ArgumentException>(() => Original.ReplaceCaseInsensitive(string.Empty, "some replacement"));
        }

        [Fact]
        public static void ReplaceCaseInsensitive_OldValueNotFound_ReturnsStringWithNoReplacements()
        {
            // Arrange
            const string Original1 = "";
            const string Original2 = "   ";
            const string Original3 = "  \r\n   ";
            const string Original4 = "this is some text";
            const string Original5 = "this is \r\n some text";

            const string OldValue1 = "A";
            const string OldValue2 = "\r\n";
            const string OldValue3 = "text";
            const string OldValue4 = "ABCD";
            const string OldValue5 = @"//";

            // Act
            string actual1 = Original1.ReplaceCaseInsensitive(OldValue1, string.Empty);
            string actual2 = Original2.ReplaceCaseInsensitive(OldValue2, "some replacement");
            string actual3 = Original3.ReplaceCaseInsensitive(OldValue3, "ABCD");
            string actual4 = Original4.ReplaceCaseInsensitive(OldValue4, "...");
            string actual5 = Original5.ReplaceCaseInsensitive(OldValue5, "some other\r\nreplacement");

            // Assert
            Assert.Equal(Original1, actual1);
            Assert.Equal(Original2, actual2);
            Assert.Equal(Original3, actual3);
            Assert.Equal(Original4, actual4);
            Assert.Equal(Original5, actual5);
        }

        [Fact]
        public static void ReplaceCaseInsensitive_OldValueFoundAndNewValueIsNull_RemovesAllOccurrencesOfOldValue()
        {
            // Arrange
            const string Original1 = "   ";
            const string Original2 = "  \r\n   ";
            const string Original3 = "this is some text";
            const string Original4 = "this is \r\n some text";
            const string Original5 = "ThThisIsSomeisTeXtXT";
            const string Original6 = "appUkslsaAPPskskdsdksapP";

            const string OldValue1 = " ";
            const string OldValue2a = "\r\n";
            const string OldValue2b = " ";
            const string OldValue3a = "EXT";
            const string OldValue3b = "Thi";
            const string OldValue4a = "th";
            const string OldValue4b = "xT";
            const string OldValue4c = " \r\n ";
            const string OldValue5a = "th";
            const string OldValue5b = "XT";
            const string OldValue5c = "IS";
            const string OldValue6 = "ApP";

            const string Expected1 = "";
            const string Expected2a = "     ";
            const string Expected2b = "\r\n";
            const string Expected3a = "this is some t";
            const string Expected3b = "s is some text";
            const string Expected4a = "is is \r\n some text";
            const string Expected4b = "this is \r\n some te";
            const string Expected4c = "this issome text";
            const string Expected5a = "isIsSomeisTeXtXT";
            const string Expected5b = "ThThisIsSomeisTe";
            const string Expected5c = "ThThSomeTeXtXT";
            const string Expected6 = "Ukslsaskskdsdks";

            // Act
            string actual1 = Original1.ReplaceCaseInsensitive(OldValue1, null);
            string actual2a = Original2.ReplaceCaseInsensitive(OldValue2a, null);
            string actual2b = Original2.ReplaceCaseInsensitive(OldValue2b, null);
            string actual3a = Original3.ReplaceCaseInsensitive(OldValue3a, null);
            string actual3b = Original3.ReplaceCaseInsensitive(OldValue3b, null);
            string actual4a = Original4.ReplaceCaseInsensitive(OldValue4a, null);
            string actual4b = Original4.ReplaceCaseInsensitive(OldValue4b, null);
            string actual4c = Original4.ReplaceCaseInsensitive(OldValue4c, null);
            string actual5a = Original5.ReplaceCaseInsensitive(OldValue5a, null);
            string actual5b = Original5.ReplaceCaseInsensitive(OldValue5b, null);
            string actual5c = Original5.ReplaceCaseInsensitive(OldValue5c, null);
            string actual6 = Original6.ReplaceCaseInsensitive(OldValue6, null);

            // Assert
            Assert.Equal(Expected1, actual1);
            Assert.Equal(Expected2a, actual2a);
            Assert.Equal(Expected2b, actual2b);
            Assert.Equal(Expected3a, actual3a);
            Assert.Equal(Expected3b, actual3b);
            Assert.Equal(Expected4a, actual4a);
            Assert.Equal(Expected4b, actual4b);
            Assert.Equal(Expected4c, actual4c);
            Assert.Equal(Expected5a, actual5a);
            Assert.Equal(Expected5b, actual5b);
            Assert.Equal(Expected5c, actual5c);
            Assert.Equal(Expected6, actual6);
        }

        [Fact]
        public static void ReplaceCaseInsensitive_OldValueFoundAndNewValueIsEmpty_RemovesAllOccurrencesOfOldValue()
        {
            // Arrange
            const string Original1 = "   ";
            const string Original2 = "  \r\n   ";
            const string Original3 = "this is some text";
            const string Original4 = "this is \r\n some text";
            const string Original5 = "ThThisIsSomeisTeXtXT";
            const string Original6 = "appUkslsaAPPskskdsdksapP";

            const string OldValue1 = " ";
            const string OldValue2a = "\r\n";
            const string OldValue2b = " ";
            const string OldValue3a = "EXT";
            const string OldValue3b = "Thi";
            const string OldValue4a = "th";
            const string OldValue4b = "xT";
            const string OldValue4c = " \r\n ";
            const string OldValue5a = "th";
            const string OldValue5b = "XT";
            const string OldValue5c = "IS";
            const string OldValue6 = "ApP";

            const string Expected1 = "";
            const string Expected2a = "     ";
            const string Expected2b = "\r\n";
            const string Expected3a = "this is some t";
            const string Expected3b = "s is some text";
            const string Expected4a = "is is \r\n some text";
            const string Expected4b = "this is \r\n some te";
            const string Expected4c = "this issome text";
            const string Expected5a = "isIsSomeisTeXtXT";
            const string Expected5b = "ThThisIsSomeisTe";
            const string Expected5c = "ThThSomeTeXtXT";
            const string Expected6 = "Ukslsaskskdsdks";

            // Act
            string actual1 = Original1.ReplaceCaseInsensitive(OldValue1, string.Empty);
            string actual2a = Original2.ReplaceCaseInsensitive(OldValue2a, string.Empty);
            string actual2b = Original2.ReplaceCaseInsensitive(OldValue2b, string.Empty);
            string actual3a = Original3.ReplaceCaseInsensitive(OldValue3a, string.Empty);
            string actual3b = Original3.ReplaceCaseInsensitive(OldValue3b, string.Empty);
            string actual4a = Original4.ReplaceCaseInsensitive(OldValue4a, string.Empty);
            string actual4b = Original4.ReplaceCaseInsensitive(OldValue4b, string.Empty);
            string actual4c = Original4.ReplaceCaseInsensitive(OldValue4c, string.Empty);
            string actual5a = Original5.ReplaceCaseInsensitive(OldValue5a, string.Empty);
            string actual5b = Original5.ReplaceCaseInsensitive(OldValue5b, string.Empty);
            string actual5c = Original5.ReplaceCaseInsensitive(OldValue5c, string.Empty);
            string actual6 = Original6.ReplaceCaseInsensitive(OldValue6, string.Empty);

            // Assert
            Assert.Equal(Expected1, actual1);
            Assert.Equal(Expected2a, actual2a);
            Assert.Equal(Expected2b, actual2b);
            Assert.Equal(Expected3a, actual3a);
            Assert.Equal(Expected3b, actual3b);
            Assert.Equal(Expected4a, actual4a);
            Assert.Equal(Expected4b, actual4b);
            Assert.Equal(Expected4c, actual4c);
            Assert.Equal(Expected5a, actual5a);
            Assert.Equal(Expected5b, actual5b);
            Assert.Equal(Expected5c, actual5c);
            Assert.Equal(Expected6, actual6);
        }

        [Fact]
        public static void ReplaceTokens___Should_throw_ArgumentNullException___When_parameter_value_is_null()
        {
            // Arrange
            var tokenToReplacementValueMap = A.Dummy<IReadOnlyDictionary<string, string>>();

            // Act
            var actual = Record.Exception(() => StringExtensions.ReplaceTokens(null, tokenToReplacementValueMap));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.AsTest().Must().ContainString("value");
        }

        [Fact]
        public static void ReplaceTokens___Should_throw_ArgumentNullException___When_parameter_tokenToReplacementValueMap_is_null()
        {
            // Arrange
            var value = A.Dummy<string>();

            // Act
            var actual = Record.Exception(() => value.ReplaceTokens(null));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.AsTest().Must().ContainString("tokenToReplacementValueMap");
        }

        [Fact]
        public static void ReplaceTokens___Should_replace_tokens___When_called()
        {
            // Arrange
            var scenarios = new[]
            {
                new
                {
                    Value = string.Empty,
                    TokenToReplacementValueMap = new Dictionary<string, string>
                    {
                        { "[token1]", "value" },
                    },
                    Expected = string.Empty,
                },
                new
                {
                    Value = "does-not-contain-token",
                    TokenToReplacementValueMap = new Dictionary<string, string>
                    {
                        { "[token1]", "value" },
                    },
                    Expected = "does-not-contain-token",
                },
                new
                {
                    Value = "contains [token1] multiple [token2] token [token3] such [token4] as [token5]",
                    TokenToReplacementValueMap = new Dictionary<string, string>
                    {
                        { "[token1]", null },
                        { "[token2]", string.Empty },
                        { "[token3]", "some-value" },
                        { "[token4]", "some-other-value" },
                        { "[token6]", "not-there" },
                    },
                    Expected = "contains  multiple  token some-value such some-other-value as [token5]",
                },
            };

            // Act
            var actuals = scenarios.Select(_ => _.Value.ReplaceTokens(_.TokenToReplacementValueMap)).ToList();

            // Assert
            actuals.AsTest().Must().BeEqualTo(scenarios.Select(_ => _.Expected).ToList());
        }

        [Fact]
        public static void ReplaceCaseInsensitive_OldValueFoundAndNewValueIsNotEmpty_ReplacesAllOccurrencesOfOldValueWithNewValue()
        {
            // Arrange
            const string Original1 = "     ";
            const string OldValue1a = " ";
            const string NewValue1a = ".";
            const string Expected1a = ".....";
            const string OldValue1b = "  ";
            const string NewValue1b = "Ab";
            const string Expected1b = "AbAb ";
            const string OldValue1c = "   ";
            const string NewValue1c = "xyZ";
            const string Expected1c = "xyZ  ";
            const string OldValue1d = "  ";
            const string NewValue1d = "   ";
            const string Expected1d = "       ";

            const string Original2 = "  \r\n   ";
            const string OldValue2a = "\r\n";
            const string NewValue2a = "\r\n\r\n";
            const string Expected2a = "  \r\n\r\n   ";
            const string OldValue2b = " \r";
            const string NewValue2b = " \nO";
            const string Expected2b = "  \nO\n   ";

            const string Original3 = "this is some text";
            const string OldValue3a = "EXT";
            const string NewValue3a = "eXt";
            const string Expected3a = "this is some teXt";
            const string OldValue3b = "Thi";
            const string NewValue3b = "THI";
            const string Expected3b = "THIs is some text";

            const string Original4 = "this is \r\n some text";
            const string OldValue4a = "th";
            const string NewValue4a = "thth";
            const string Expected4a = "ththis is \r\n some text";
            const string OldValue4b = "xT";
            const string NewValue4b = "xtxt";
            const string Expected4b = "this is \r\n some textxt";
            const string OldValue4c = "is";
            const string NewValue4c = "is";
            const string Expected4c = "this is \r\n some text";

            const string Original5 = "ThThisIsSomeisTeXtXT";
            const string OldValue5a = "tH";
            const string NewValue5a = "ah";
            const string Expected5a = "ahahisIsSomeisTeXtXT";
            const string OldValue5b = "XT";
            const string NewValue5b = "%^";
            const string Expected5b = "ThThisIsSomeisTe%^%^";
            const string OldValue5c = "IS";
            const string NewValue5c = "YisO";
            const string Expected5c = "ThThYisOYisOSomeYisOTeXtXT";

            const string Original6 = "appUkslsaAPPskskdsdksapP";
            const string OldValue6 = "ApP";
            const string NewValue6 = "  ,*";
            const string Expected6 = "  ,*Ukslsa  ,*skskdsdks  ,*";

            // Act
            string actual1a = Original1.ReplaceCaseInsensitive(OldValue1a, NewValue1a);
            string actual1b = Original1.ReplaceCaseInsensitive(OldValue1b, NewValue1b);
            string actual1c = Original1.ReplaceCaseInsensitive(OldValue1c, NewValue1c);
            string actual1d = Original1.ReplaceCaseInsensitive(OldValue1d, NewValue1d);
            string actual2a = Original2.ReplaceCaseInsensitive(OldValue2a, NewValue2a);
            string actual2b = Original2.ReplaceCaseInsensitive(OldValue2b, NewValue2b);
            string actual3a = Original3.ReplaceCaseInsensitive(OldValue3a, NewValue3a);
            string actual3b = Original3.ReplaceCaseInsensitive(OldValue3b, NewValue3b);
            string actual4a = Original4.ReplaceCaseInsensitive(OldValue4a, NewValue4a);
            string actual4b = Original4.ReplaceCaseInsensitive(OldValue4b, NewValue4b);
            string actual4c = Original4.ReplaceCaseInsensitive(OldValue4c, NewValue4c);
            string actual5a = Original5.ReplaceCaseInsensitive(OldValue5a, NewValue5a);
            string actual5b = Original5.ReplaceCaseInsensitive(OldValue5b, NewValue5b);
            string actual5c = Original5.ReplaceCaseInsensitive(OldValue5c, NewValue5c);
            string actual6 = Original6.ReplaceCaseInsensitive(OldValue6, NewValue6);

            // Assert
            Assert.Equal(Expected1a, actual1a);
            Assert.Equal(Expected1b, actual1b);
            Assert.Equal(Expected1c, actual1c);
            Assert.Equal(Expected1d, actual1d);
            Assert.Equal(Expected2a, actual2a);
            Assert.Equal(Expected2b, actual2b);
            Assert.Equal(Expected3a, actual3a);
            Assert.Equal(Expected3b, actual3b);
            Assert.Equal(Expected4a, actual4a);
            Assert.Equal(Expected4b, actual4b);
            Assert.Equal(Expected4c, actual4c);
            Assert.Equal(Expected5a, actual5a);
            Assert.Equal(Expected5b, actual5b);
            Assert.Equal(Expected5c, actual5c);
            Assert.Equal(Expected6, actual6);
        }

        [Fact]
        public static void SplitIntoChunksOfLength___Should_throw_ArgumentNullException___When_parameter_value_is_null()
        {
            // Arrange
            var lengthPerChunk = A.Dummy<int>().ThatIs(_ => _ > 0);

            // Act
            var actual = Record.Exception(() => StringExtensions.SplitIntoChunksOfLength(null, lengthPerChunk));

            // Assert
            actual.Should().BeOfType<ArgumentNullException>();
            actual.Message.Should().Contain("value");
        }

        [Fact]
        public static void SplitIntoChunksOfLength___Should_throw_ArgumentOutOfRangeException___When_parameter_lengthPerChunk_is_less_than_or_equal_to_0()
        {
            // Arrange
            var value = A.Dummy<string>();
            var lengthPerChunk = A.Dummy<int>().ThatIs(_ => _ < 0);

            // Act
            var actual1 = Record.Exception(() => value.SplitIntoChunksOfLength(0));
            var actual2 = Record.Exception(() => value.SplitIntoChunksOfLength(lengthPerChunk));

            // Assert
            actual1.Should().BeOfType<ArgumentOutOfRangeException>();
            actual1.Message.Should().Contain("lengthPerChunk");

            actual2.Should().BeOfType<ArgumentOutOfRangeException>();
            actual2.Message.Should().Contain("lengthPerChunk");
        }

        [Fact]
        public static void SplitIntoChunksOfLength___Should_return_empty_list___When_value_is_an_empty_string()
        {
            // Arrange
            var lengthPerChunk = A.Dummy<int>().ThatIs(_ => _ > 0);

            // Act
            var actual = string.Empty.SplitIntoChunksOfLength(lengthPerChunk);

            // Assert
            actual.Should().BeEmpty();
        }

        [Fact]
        public static void SplitIntoChunksOfLength___Should_return_chunks___When_length_of_value_divides_evenly_by_lengthPerChunk()
        {
            // Arrange
            var value = " some brown cows  ";

            var lengthPerChunk1 = 1;
            var expected1 = new[] { " ", "s", "o", "m", "e", " ", "b", "r", "o", "w", "n", " ", "c", "o", "w", "s", " ", " " };

            var lengthPerChunk2 = 6;
            var expected2 = new[] { " some ", "brown ", "cows  " };

            var lengthPerChunk3 = 9;
            var expected3 = new[] { " some bro", "wn cows  " };

            var lengthPerChunk4 = 18;
            var expected4 = new[] { value };

            // Act
            var actual1 = value.SplitIntoChunksOfLength(lengthPerChunk1);
            var actual2 = value.SplitIntoChunksOfLength(lengthPerChunk2);
            var actual3 = value.SplitIntoChunksOfLength(lengthPerChunk3);
            var actual4 = value.SplitIntoChunksOfLength(lengthPerChunk4);

            // Assert
            actual1.Should().Equal(expected1);
            actual2.Should().Equal(expected2);
            actual3.Should().Equal(expected3);
            actual4.Should().Equal(expected4);
        }

        [Fact]
        public static void SplitIntoChunksOfLength___Should_return_chunks___When_length_of_value_does_not_divide_evenly_by_lengthPerChunk()
        {
            // Arrange
            var value = " some brown cows ";

            var lengthPerChunk1 = 3;
            var expected1 = new[] { " so", "me ", "bro", "wn ", "cow", "s " };

            var lengthPerChunk2 = 5;
            var expected2 = new[] { " some", " brow", "n cow", "s " };

            var lengthPerChunk3 = 9;
            var expected3 = new[] { " some bro", "wn cows " };

            var lengthPerChunk4 = 16;
            var expected4 = new[] { " some brown cows", " " };

            var lengthPerChunk5 = 18;
            var expected5 = new[] { value };

            var lengthPerChunk6 = int.MaxValue;
            var expected6 = new[] { value };

            // Act
            var actual1 = value.SplitIntoChunksOfLength(lengthPerChunk1);
            var actual2 = value.SplitIntoChunksOfLength(lengthPerChunk2);
            var actual3 = value.SplitIntoChunksOfLength(lengthPerChunk3);
            var actual4 = value.SplitIntoChunksOfLength(lengthPerChunk4);
            var actual5 = value.SplitIntoChunksOfLength(lengthPerChunk5);
            var actual6 = value.SplitIntoChunksOfLength(lengthPerChunk6);

            // Assert
            actual1.Should().Equal(expected1);
            actual2.Should().Equal(expected2);
            actual3.Should().Equal(expected3);
            actual4.Should().Equal(expected4);
            actual5.Should().Equal(expected5);
            actual6.Should().Equal(expected6);
        }

        [Fact]
        public static void ToAlphanumeric___Should_throw_ArgumentNullException___When_parameter_value_is_null()
        {
            // Arrange, Act
            var actual = Record.Exception(() => StringExtensions.ToLowerTrimmed(null));

            // Assert
            actual.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public static void ToAlphanumeric___Should_return_value_with_non_alphanumeric_characters_removed___When_called()
        {
            // Arrange
            var tests = new[]
            {
                new { Value = string.Empty, Expected = string.Empty },
                new { Value = " ", Expected = string.Empty },
                new { Value = " asdf ", Expected = "asdf" },
                new { Value = " as***df ", Expected = "asdf" },
                new { Value = "someletters==-", Expected = "someletters" },
            };

            // Act
            var actuals = tests.Select(_ => _.Value.ToAlphanumeric()).ToList();

            // Assert
            actuals.Should().Equal(tests.Select(_ => _.Expected));
        }

        [Fact]
        public static void ToAsciiBytes_ValueToConvertIsNull_ThrowsArgumentNullException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => StringExtensions.ToAsciiBytes(null));
        }

        [Fact]
        public static void ToAsciiBytes_StringToEncodeIsEmpty_ReturnsEmptyArray()
        {
            // Arrange, Act, Assert
            Assert.Empty(string.Empty.ToAsciiBytes());
        }

        [Fact]
        public static void ToAsciiBytes_StringToEncodeIsNotEmpty_ReturnsAsciiBytes()
        {
            // Arrange
            const string ToEncode = "Ascii";
            const string Expected = "[65][115][99][105][105]";

            // Act
            byte[] asciiBytes = ToEncode.ToAsciiBytes();

            // Assert
            var bytesPrintout = new StringBuilder();
            foreach (byte b in asciiBytes)
            {
                bytesPrintout.Append("[" + b.ToString(CultureInfo.CurrentCulture) + "]");
            }

            Assert.Equal(Expected, bytesPrintout.ToString());
        }

        [Fact]
        public static void ToBytes_ValueToConvertIsNull_ThrowsArgumentNullException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => StringExtensions.ToBytes(null, new UnicodeEncoding()));
        }

        [Fact]
        public static void ToBytes_EncodingIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            const string ToEncode = "my string";

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => ToEncode.ToBytes(null));
        }

        [Fact]
        public static void ToBytes_StringToEncodeIsEmpty_ReturnsEmptyArray()
        {
            // Arrange, Act, Assert
            Assert.Empty(string.Empty.ToBytes(new UnicodeEncoding()));
        }

        [Fact]
        public static void ToBytes_StringToEncodeIsNotEmptyEncodingIsUtf8_ReturnsUtf8Bytes()
        {
            // Arrange
            const string ToEncode = "This unicode string contains two characters " +
                                         "with codes outside an 8-bit code range, " +
                                         "Pi (\u03a0) and Sigma (\u03a3).";
            const string Expected = "[84][104][105][115][32][117][110][105][99][111][100][101][32][115][116][114][105][110][103][32][99][111][110][116][97][105][110][115][32][116][119][111][32][99][104][97][114][97][99][116][101][114][115][32][119][105][116][104][32][99][111][100][101][115][32][111][117][116][115][105][100][101][32][97][110][32][56][45][98][105][116][32][99][111][100][101][32][114][97][110][103][101][44][32][80][105][32][40][206][160][41][32][97][110][100][32][83][105][103][109][97][32][40][206][163][41][46]";

            // Act
            byte[] utf8Bytes = ToEncode.ToBytes(new UTF8Encoding());

            // Assert
            var bytesPrintout = new StringBuilder();
            foreach (byte b in utf8Bytes)
            {
                bytesPrintout.Append("[" + b.ToString(CultureInfo.CurrentCulture) + "]");
            }

            Assert.Equal(Expected, bytesPrintout.ToString());
        }

        [Fact]
        public static void ToCsvSafe_StringIsNull_ThrowsArgumentNullException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => StringExtensions.ToCsvSafe(null));
        }

        [Fact]
        public static void ToCsvSafe_StringIsEmpty_ReturnsEmptyString()
        {
            // Arrange, Act, Assert
            Assert.Equal(string.Empty, string.Empty.ToCsvSafe());
        }

        [Fact]
        public static void ToCsvSafe___Should_return_original_string___When_value_is_already_CSV_safe()
        {
            // Arrange
            var expected = new[] { "asdoiouwerzvkjdfm", "g       O", @"!@#$%^&*()_+-=';/?.`~_+\", string.Empty };

            // Act
            var actual = expected.Select(_ => _.ToCsvSafe());

            // Assert
            actual.Should().Equal(expected);
        }

        [Fact]
        public static void ToCsvSafe___Should_return_CSV_safe_version_of_value___When_value_is_not_CSV_safe()
        {
            // Arrange
            var tests = new[]
            {
                new { Original = " ", Expected = "\" \"" },
                new { Original = "\"", Expected = "\"\"\"\"" },
                new { Original = ",", Expected = "\",\"" },
                new { Original = "Super, luxurious truck", Expected = "\"Super, luxurious truck\"" },
                new { Original = "Super \"luxurious\" truck", Expected = "\"Super \"\"luxurious\"\" truck\"" },
                new { Original = "Go get one now\r\nthey are going fast", Expected = "\"Go get one now\r\nthey are going fast\"" },
                new { Original = "Go get one now\nthey are going fast", Expected = "\"Go get one now\nthey are going fast\"" },
                new { Original = "Go get one now" + Environment.NewLine + "they are going fast", Expected = "\"Go get one now" + Environment.NewLine + "they are going fast\"" },
                new { Original = "Super luxurious truck    ", Expected = "\"Super luxurious truck    \"" },
                new { Original = "  Super luxurious truck", Expected = "\"  Super luxurious truck\"" },
                new { Original = "  Super luxurious truck    ", Expected = "\"  Super luxurious truck    \"" },
                new { Original = " Super, luxurious truck ", Expected = "\" Super, luxurious truck \"" },
                new { Original = " Super, \"luxurious\" truck ", Expected = "\" Super, \"\"luxurious\"\" truck \"" },
            };

            var expected = tests.Select(_ => _.Expected);

            // Act,
            var actual = tests.Select(_ => _.Original.ToCsvSafe());

            // Assert
            actual.Should().Equal(expected);
        }

        [Fact]
        public static void ToLowerTrimmed___Should_throw_ArgumentNullException___When_parameter_value_is_null()
        {
            // Arrange, Act
            var actual = Record.Exception(() => StringExtensions.ToLowerTrimmed(null));

            // Assert
            actual.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public static void ToLowerTrimmed_cultureInfo___Should_throw_ArgumentNullException___When_parameter_value_is_null()
        {
            // Arrange, Act
            var actual = Record.Exception(() => StringExtensions.ToLowerTrimmed(null, CultureInfo.CurrentCulture));

            // Assert
            actual.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public static void ToLowerTrimmed_cultureInfo___Should_throw_ArgumentNullException___When_parameter_cultureInfo_is_null()
        {
            // Arrange, Act
            var actual = Record.Exception(() => A.Dummy<string>().ToLowerTrimmed(null));

            // Assert
            actual.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public static void ToLowerTrimmed___Should_return_empty_string___When_parameter_value_is_an_empty_string()
        {
            // Arrange, Act
            var actual = string.Empty.ToLowerTrimmed();

            // Assert
            actual.Should().BeEmpty();
        }

        [Fact]
        public static void ToLowerTrimmed_cultureInfo___Should_return_empty_string___When_parameter_value_is_an_empty_string()
        {
            // Arrange, Act
            var actual = string.Empty.ToLowerTrimmed(CultureInfo.CurrentCulture);

            // Assert
            actual.Should().BeEmpty();
        }

        [Fact]
        public static void ToLowerTrimmed___Should_return_same_string_passed_to_method___When_string_is_already_lowercase_and_trimmed()
        {
            // Arrange
            const string expected = @"a sdfj !@#$%^\&*()=+_?/.,mn2340-8938m  fkls d";

            // Act
            var actual = expected.ToLowerTrimmed();

            // Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public static void ToLowerTrimmed_cultureInfo___Should_return_same_string_passed_to_method___When_string_is_already_lowercase_and_trimmed()
        {
            // Arrange
            const string expected = @"a sdfj !@#$%^\&*()=+_?/.,mn2340-8938m  fkls d";

            // Act
            var actual = expected.ToLowerTrimmed(CultureInfo.CurrentCulture);

            // Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public static void ToLowerTrimmed___Should_convert_value_to_lowercase_and_remove_leading_and_trailing_white_space___When_called()
        {
            // Arrange
            const string value1 = "This Is Not%(#*&! lower CASE";
            const string expected1 = "this is not%(#*&! lower case";

            const string value2 = "  this needs a trim ";
            const string expected2 = "this needs a trim";

            const string value3 = " LoWer Trimmed   ";
            const string expected3 = "lower trimmed";

            // Act
            var actual1 = value1.ToLowerTrimmed();
            var actual2 = value2.ToLowerTrimmed();
            var actual3 = value3.ToLowerTrimmed();

            // Assert
            actual1.Should().Be(expected1);
            actual2.Should().Be(expected2);
            actual3.Should().Be(expected3);
        }

        [Fact]
        public static void ToLowerTrimmed_cultureInfo___Should_convert_value_to_lowercase_and_remove_leading_and_trailing_white_space___When_called()
        {
            // Arrange
            const string value1 = "This Is Not%(#*&! lower CASE";
            const string expected1 = "this is not%(#*&! lower case";

            const string value2 = "  this needs a trim ";
            const string expected2 = "this needs a trim";

            const string value3 = " LoWer Trimmed   ";
            const string expected3 = "lower trimmed";

            // Act
            var actual1 = value1.ToLowerTrimmed(CultureInfo.CurrentCulture);
            var actual2 = value2.ToLowerTrimmed(CultureInfo.CurrentCulture);
            var actual3 = value3.ToLowerTrimmed(CultureInfo.CurrentCulture);

            // Assert
            actual1.Should().Be(expected1);
            actual2.Should().Be(expected2);
            actual3.Should().Be(expected3);
        }

        [Fact]
        public static void ToUpperTrimmed___Should_throw_ArgumentNullException___When_parameter_value_is_null()
        {
            // Arrange, Act
            var actual = Record.Exception(() => StringExtensions.ToUpperTrimmed(null));

            // Assert
            actual.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public static void ToUpperTrimmed_cultureInfo___Should_throw_ArgumentNullException___When_parameter_value_is_null()
        {
            // Arrange, Act
            var actual = Record.Exception(() => StringExtensions.ToUpperTrimmed(null, CultureInfo.CurrentCulture));

            // Assert
            actual.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public static void ToUpperTrimmed_cultureInfo___Should_throw_ArgumentNullException___When_parameter_cultureInfo_is_null()
        {
            // Arrange, Act
            var actual = Record.Exception(() => A.Dummy<string>().ToUpperTrimmed(null));

            // Assert
            actual.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public static void ToUpperTrimmed___Should_return_empty_string___When_parameter_value_is_an_empty_string()
        {
            // Arrange, Act
            var actual = string.Empty.ToUpperTrimmed();

            // Assert
            actual.Should().BeEmpty();
        }

        [Fact]
        public static void ToUpperTrimmed_cultureInfo___Should_return_empty_string___When_parameter_value_is_an_empty_string()
        {
            // Arrange, Act
            var actual = string.Empty.ToUpperTrimmed(CultureInfo.CurrentCulture);

            // Assert
            actual.Should().BeEmpty();
        }

        [Fact]
        public static void ToUpperTrimmed___Should_return_same_string_passed_to_method___When_string_is_already_uppercase_and_trimmed()
        {
            // Arrange
            const string expected = @"A SDFJ !@#$%^\&*()=+_?/.,MN2340-8938M  FKLS D";

            // Act
            var actual = expected.ToUpperTrimmed();

            // Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public static void ToUpperTrimmed_cultureInfo___Should_return_same_string_passed_to_method___When_string_is_already_uppercase_and_trimmed()
        {
            // Arrange
            const string expected = @"A SDFJ !@#$%^\&*()=+_?/.,MN2340-8938M  FKLS D";

            // Act
            var actual = expected.ToUpperTrimmed(CultureInfo.CurrentCulture);

            // Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public static void ToUpperTrimmed___Should_convert_value_to_uppercase_and_remove_leading_and_trailing_white_space___When_called()
        {
            // Arrange
            const string value1 = "This Is Not%(#*&! upper CASE";
            const string expected1 = "THIS IS NOT%(#*&! UPPER CASE";

            const string value2 = "  THIS NEEDS A TRIM ";
            const string expected2 = "THIS NEEDS A TRIM";

            const string value3 = " uPpeR TrimMed   ";
            const string expected3 = "UPPER TRIMMED";

            // Act
            var actual1 = value1.ToUpperTrimmed();
            var actual2 = value2.ToUpperTrimmed();
            var actual3 = value3.ToUpperTrimmed();

            // Assert
            actual1.Should().Be(expected1);
            actual2.Should().Be(expected2);
            actual3.Should().Be(expected3);
        }

        [Fact]
        public static void ToUpperTrimmed_cultureInfo___Should_convert_value_to_uppercase_and_remove_leading_and_trailing_white_space___When_called()
        {
            // Arrange
            const string value1 = "This Is Not%(#*&! upper CASE";
            const string expected1 = "THIS IS NOT%(#*&! UPPER CASE";

            const string value2 = "  THIS NEEDS A TRIM ";
            const string expected2 = "THIS NEEDS A TRIM";

            const string value3 = " uPpeR TrimMed   ";
            const string expected3 = "UPPER TRIMMED";

            // Act
            var actual1 = value1.ToUpperTrimmed(CultureInfo.CurrentCulture);
            var actual2 = value2.ToUpperTrimmed(CultureInfo.CurrentCulture);
            var actual3 = value3.ToUpperTrimmed(CultureInfo.CurrentCulture);

            // Assert
            actual1.Should().Be(expected1);
            actual2.Should().Be(expected2);
            actual3.Should().Be(expected3);
        }

        [Fact]
        public static void ToLowerFirstCharacter___Should_throw_ArgumentNullException___When_parameter_value_is_null()
        {
            // Arrange, Act
            var actual = Record.Exception(() => StringExtensions.ToLowerFirstCharacter(null));

            // Assert
            actual.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public static void ToLowerFirstCharacter_cultureInfo___Should_throw_ArgumentNullException___When_parameter_value_is_null()
        {
            // Arrange, Act
            var actual = Record.Exception(() => StringExtensions.ToLowerFirstCharacter(null, CultureInfo.CurrentCulture));

            // Assert
            actual.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public static void ToLowerFirstCharacter_cultureInfo___Should_throw_ArgumentNullException___When_parameter_cultureInfo_is_null()
        {
            // Arrange, Act
            var actual = Record.Exception(() => A.Dummy<string>().ToLowerFirstCharacter(null));

            // Assert
            actual.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public static void ToLowerFirstCharacter___Should_return_empty_string___When_parameter_value_is_an_empty_string()
        {
            // Arrange, Act
            var actual = string.Empty.ToLowerFirstCharacter();

            // Assert
            actual.Should().BeEmpty();
        }

        [Fact]
        public static void ToLowerFirstCharacter_cultureInfo___Should_return_empty_string___When_parameter_value_is_an_empty_string()
        {
            // Arrange, Act
            var actual = string.Empty.ToLowerFirstCharacter(CultureInfo.CurrentCulture);

            // Assert
            actual.Should().BeEmpty();
        }

        [Fact]
        public static void ToLowerFirstCharacter___Should_return_same_string_passed_to_method___When_string_first_character_is_already_lowercase()
        {
            // Arrange
            const string expected1 = @"a";
            const string expected2 = @"a sdFj !@#$%^\&*()=+_?/.,MN2340-8938m";
            const string expected3 = @" ASDF";

            // Act
            var actual1 = expected1.ToLowerFirstCharacter();
            var actual2 = expected2.ToLowerFirstCharacter();
            var actual3 = expected3.ToLowerFirstCharacter();

            // Assert
            actual1.Should().Be(expected1);
            actual2.Should().Be(expected2);
            actual3.Should().Be(expected3);
        }

        [Fact]
        public static void ToLowerFirstCharacter_cultureInfo___Should_return_same_string_passed_to_method___When_string_first_character_is_already_lowercase()
        {
            // Arrange
            const string expected1 = @"a";
            const string expected2 = @"a sdFj !@#$%^\&*()=+_?/.,MN2340-8938m";
            const string expected3 = @" ASDF";

            // Act
            var actual1 = expected1.ToLowerFirstCharacter(CultureInfo.CurrentCulture);
            var actual2 = expected2.ToLowerFirstCharacter(CultureInfo.CurrentCulture);
            var actual3 = expected3.ToLowerFirstCharacter(CultureInfo.CurrentCulture);

            // Assert
            actual1.Should().Be(expected1);
            actual2.Should().Be(expected2);
            actual3.Should().Be(expected3);
        }

        [Fact]
        public static void ToLowerFirstCharacter___Should_convert_first_character_of_string_to_lowercase___When_called()
        {
            // Arrange
            const string value1 = "T";
            const string expected1 = "t";

            const string value2 = "Some String";
            const string expected2 = "some String";

            // Act
            var actual1 = value1.ToLowerFirstCharacter();
            var actual2 = value2.ToLowerFirstCharacter();

            // Assert
            actual1.Should().Be(expected1);
            actual2.Should().Be(expected2);
        }

        [Fact]
        public static void ToLowerFirstCharacter_cultureInfo___Should_convert_first_character_of_string_to_lowercase___When_called()
        {
            // Arrange
            const string value1 = "T";
            const string expected1 = "t";

            const string value2 = "Some String";
            const string expected2 = "some String";

            // Act
            var actual1 = value1.ToLowerFirstCharacter(CultureInfo.CurrentCulture);
            var actual2 = value2.ToLowerFirstCharacter(CultureInfo.CurrentCulture);

            // Assert
            actual1.Should().Be(expected1);
            actual2.Should().Be(expected2);
        }

        [Fact]
        public static void ToStringInvariantPreferred_object___Should_throw_ArgumentNullException___When_value_is_null_and_throwIfNull_is_true()
        {
            // Arrange
            object value = null;

            // Act
            var actual = Record.Exception(() => value.ToStringInvariantPreferred(throwIfNull: true));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.AsTest().Must().ContainString("value");
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "sbyte", Justification = ObcSuppressBecause.CA1704_IdentifiersShouldBeSpelledCorrectly_SpellingIsCorrectInContextOfTheDomain)]
        public static void ToStringInvariantPreferred_object___Should_return_null___When_value_is_null_and_throwIfNull_is_false()
        {
            // Arrange
            object value = null;

            // Act
            var actual = value.ToStringInvariantPreferred(throwIfNull: false);

            // Assert
            actual.AsTest().Must().BeNull();
        }

        [Fact]
        public static void ToStringInvariantPreferred_object___Should_get_string_representation___When_called()
        {
            // Arrange
            sbyte? value = 53;

            object test = value;

            test.ToStringInvariantPreferred();

            var valueAndExpected = new[]
            {
                new { Value = (object)"some-string", Expected = "some-string" },
                new { Value = (object)(sbyte?)10, Expected = "10" },
                new { Value = (object)(sbyte)11, Expected = "11" },
                new { Value = (object)(byte)12, Expected = "12" },
                new { Value = (object)(byte?)13, Expected = "13" },
                new { Value = (object)(short)14, Expected = "14" },
                new { Value = (object)(short?)15, Expected = "15" },
                new { Value = (object)(ushort)16, Expected = "16" },
                new { Value = (object)(ushort?)17, Expected = "17" },
                new { Value = (object)(int)18, Expected = "18" },
                new { Value = (object)(int?)19, Expected = "19" },
                new { Value = (object)20U, Expected = "20" },
                new { Value = (object)(uint?)21U, Expected = "21" },
                new { Value = (object)22L, Expected = "22" },
                new { Value = (object)(long?)23L, Expected = "23" },
                new { Value = (object)24UL, Expected = "24" },
                new { Value = (object)(ulong?)25UL, Expected = "25" },
                new { Value = (object)26.25F, Expected = "26.25" },
                new { Value = (object)(float?)27.25F, Expected = "27.25" },
                new { Value = (object)28.25d, Expected = "28.25" },
                new { Value = (object)(double?)29.25, Expected = "29.25" },
                new { Value = (object)30.25m, Expected = "30.25" },
                new { Value = (object)(decimal?)31.25m, Expected = "31.25" },
                new { Value = (object)true, Expected = "true" },
                new { Value = (object)(bool?)true, Expected = "true" },
                new { Value = (object)Guid.Parse("F950C64B-A629-4E65-9BC8-28DA7A2DFCBB"), Expected = "f950c64b-a629-4e65-9bc8-28da7a2dfcbb" },
                new { Value = (object)(Guid?)Guid.Parse("F950C64B-A629-4E65-9BC8-28DA7A2DFCBB"), Expected = "f950c64b-a629-4e65-9bc8-28da7a2dfcbb" },
                new { Value = (object)new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Utc).AddTicks(20), Expected = "2019-01-05T12:14:58.1920020Z" },
                new { Value = (object)(DateTime?)new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Utc).AddTicks(20), Expected = "2019-01-05T12:14:58.1920020Z" },
            };

            // Act
            var actuals = valueAndExpected.Select(_ => _.Value.ToStringInvariantPreferred(true)).ToList();

            // Assert
            actuals.AsTest().Must().BeEqualTo(valueAndExpected.Select(_ => _.Expected).ToList());
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "sbyte", Justification = ObcSuppressBecause.CA1704_IdentifiersShouldBeSpelledCorrectly_SpellingIsCorrectInContextOfTheDomain)]
        public static void ToStringInvariantPreferred_sbyte___Should_get_string_representation___When_called()
        {
            // Arrange
            sbyte value = 53;

            // Act
            var actual = value.ToStringInvariantPreferred();

            // Assert
            actual.AsTest().Must().BeEqualTo("53");
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "sbyte", Justification = ObcSuppressBecause.CA1704_IdentifiersShouldBeSpelledCorrectly_SpellingIsCorrectInContextOfTheDomain)]
        public static void ToStringInvariantPreferred_Nullable_sbyte___Should_throw_ArgumentNullException___When_value_is_null_and_throwIfNull_is_true()
        {
            // Arrange
            sbyte? value = null;

            // Act
            var actual = Record.Exception(() => value.ToStringInvariantPreferred(throwIfNull: true));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.AsTest().Must().ContainString("value");
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "sbyte", Justification = ObcSuppressBecause.CA1704_IdentifiersShouldBeSpelledCorrectly_SpellingIsCorrectInContextOfTheDomain)]
        public static void ToStringInvariantPreferred_Nullable_sbyte___Should_return_null___When_value_is_null_and_throwIfNull_is_false()
        {
            // Arrange
            sbyte? value = null;

            // Act
            var actual = value.ToStringInvariantPreferred(throwIfNull: false);

            // Assert
            actual.AsTest().Must().BeNull();
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "sbyte", Justification = ObcSuppressBecause.CA1704_IdentifiersShouldBeSpelledCorrectly_SpellingIsCorrectInContextOfTheDomain)]
        public static void ToStringInvariantPreferred_Nullable_sbyte___Should_get_string_representation___When_called()
        {
            // Arrange
            sbyte? value = 53;

            // Act
            var actual = value.ToStringInvariantPreferred();

            // Assert
            actual.AsTest().Must().BeEqualTo("53");
        }

        [Fact]
        public static void ToStringInvariantPreferred_byte___Should_get_string_representation___When_called()
        {
            // Arrange
            byte value = 53;

            // Act
            var actual = value.ToStringInvariantPreferred();

            // Assert
            actual.AsTest().Must().BeEqualTo("53");
        }

        [Fact]
        public static void ToStringInvariantPreferred_Nullable_byte___Should_throw_ArgumentNullException___When_value_is_null_and_throwIfNull_is_true()
        {
            // Arrange
            byte? value = null;

            // Act
            var actual = Record.Exception(() => value.ToStringInvariantPreferred(throwIfNull: true));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.AsTest().Must().ContainString("value");
        }

        [Fact]
        public static void ToStringInvariantPreferred_Nullable_byte___Should_return_null___When_value_is_null_and_throwIfNull_is_false()
        {
            // Arrange
            byte? value = null;

            // Act
            var actual = value.ToStringInvariantPreferred(throwIfNull: false);

            // Assert
            actual.AsTest().Must().BeNull();
        }

        [Fact]
        public static void ToStringInvariantPreferred_Nullable_byte___Should_get_string_representation___When_called()
        {
            // Arrange
            byte? value = 53;

            // Act
            var actual = value.ToStringInvariantPreferred();

            // Assert
            actual.AsTest().Must().BeEqualTo("53");
        }

        [Fact]
        public static void ToStringInvariantPreferred_short___Should_get_string_representation___When_called()
        {
            // Arrange
            short value = 53;

            // Act
            var actual = value.ToStringInvariantPreferred();

            // Assert
            actual.AsTest().Must().BeEqualTo("53");
        }

        [Fact]
        public static void ToStringInvariantPreferred_Nullable_short___Should_throw_ArgumentNullException___When_value_is_null_and_throwIfNull_is_true()
        {
            // Arrange
            short? value = null;

            // Act
            var actual = Record.Exception(() => value.ToStringInvariantPreferred(throwIfNull: true));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.AsTest().Must().ContainString("value");
        }

        [Fact]
        public static void ToStringInvariantPreferred_Nullable_short___Should_return_null___When_value_is_null_and_throwIfNull_is_false()
        {
            // Arrange
            short? value = null;

            // Act
            var actual = value.ToStringInvariantPreferred(throwIfNull: false);

            // Assert
            actual.AsTest().Must().BeNull();
        }

        [Fact]
        public static void ToStringInvariantPreferred_Nullable_short___Should_get_string_representation___When_called()
        {
            // Arrange
            short? value = 53;

            // Act
            var actual = value.ToStringInvariantPreferred();

            // Assert
            actual.AsTest().Must().BeEqualTo("53");
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ushort", Justification = ObcSuppressBecause.CA1704_IdentifiersShouldBeSpelledCorrectly_SpellingIsCorrectInContextOfTheDomain)]
        public static void ToStringInvariantPreferred_ushort___Should_get_string_representation___When_called()
        {
            // Arrange
            ushort value = 53;

            // Act
            var actual = value.ToStringInvariantPreferred();

            // Assert
            actual.AsTest().Must().BeEqualTo("53");
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ushort", Justification = ObcSuppressBecause.CA1704_IdentifiersShouldBeSpelledCorrectly_SpellingIsCorrectInContextOfTheDomain)]
        public static void ToStringInvariantPreferred_Nullable_ushort___Should_throw_ArgumentNullException___When_value_is_null_and_throwIfNull_is_true()
        {
            // Arrange
            ushort? value = null;

            // Act
            var actual = Record.Exception(() => value.ToStringInvariantPreferred(throwIfNull: true));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.AsTest().Must().ContainString("value");
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ushort", Justification = ObcSuppressBecause.CA1704_IdentifiersShouldBeSpelledCorrectly_SpellingIsCorrectInContextOfTheDomain)]
        public static void ToStringInvariantPreferred_Nullable_ushort___Should_return_null___When_value_is_null_and_throwIfNull_is_false()
        {
            // Arrange
            ushort? value = null;

            // Act
            var actual = value.ToStringInvariantPreferred(throwIfNull: false);

            // Assert
            actual.AsTest().Must().BeNull();
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ushort", Justification = ObcSuppressBecause.CA1704_IdentifiersShouldBeSpelledCorrectly_SpellingIsCorrectInContextOfTheDomain)]
        public static void ToStringInvariantPreferred_Nullable_ushort___Should_get_string_representation___When_called()
        {
            // Arrange
            ushort? value = 53;

            // Act
            var actual = value.ToStringInvariantPreferred();

            // Assert
            actual.AsTest().Must().BeEqualTo("53");
        }

        [Fact]
        public static void ToStringInvariantPreferred_int___Should_get_string_representation___When_called()
        {
            // Arrange
            int value = 53;

            // Act
            var actual = value.ToStringInvariantPreferred();

            // Assert
            actual.AsTest().Must().BeEqualTo("53");
        }

        [Fact]
        public static void ToStringInvariantPreferred_Nullable_int___Should_throw_ArgumentNullException___When_value_is_null_and_throwIfNull_is_true()
        {
            // Arrange
            int? value = null;

            // Act
            var actual = Record.Exception(() => value.ToStringInvariantPreferred(throwIfNull: true));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.AsTest().Must().ContainString("value");
        }

        [Fact]
        public static void ToStringInvariantPreferred_Nullable_int___Should_return_null___When_value_is_null_and_throwIfNull_is_false()
        {
            // Arrange
            int? value = null;

            // Act
            var actual = value.ToStringInvariantPreferred(throwIfNull: false);

            // Assert
            actual.AsTest().Must().BeNull();
        }

        [Fact]
        public static void ToStringInvariantPreferred_Nullable_int___Should_get_string_representation___When_called()
        {
            // Arrange
            int? value = 53;

            // Act
            var actual = value.ToStringInvariantPreferred();

            // Assert
            actual.AsTest().Must().BeEqualTo("53");
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "uint", Justification = ObcSuppressBecause.CA1704_IdentifiersShouldBeSpelledCorrectly_SpellingIsCorrectInContextOfTheDomain)]
        public static void ToStringInvariantPreferred_uint___Should_get_string_representation___When_called()
        {
            // Arrange
            uint value = 53;

            // Act
            var actual = value.ToStringInvariantPreferred();

            // Assert
            actual.AsTest().Must().BeEqualTo("53");
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "uint", Justification = ObcSuppressBecause.CA1704_IdentifiersShouldBeSpelledCorrectly_SpellingIsCorrectInContextOfTheDomain)]
        public static void ToStringInvariantPreferred_Nullable_uint___Should_throw_ArgumentNullException___When_value_is_null_and_throwIfNull_is_true()
        {
            // Arrange
            uint? value = null;

            // Act
            var actual = Record.Exception(() => value.ToStringInvariantPreferred(throwIfNull: true));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.AsTest().Must().ContainString("value");
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "uint", Justification = ObcSuppressBecause.CA1704_IdentifiersShouldBeSpelledCorrectly_SpellingIsCorrectInContextOfTheDomain)]
        public static void ToStringInvariantPreferred_Nullable_uint___Should_return_null___When_value_is_null_and_throwIfNull_is_false()
        {
            // Arrange
            uint? value = null;

            // Act
            var actual = value.ToStringInvariantPreferred(throwIfNull: false);

            // Assert
            actual.AsTest().Must().BeNull();
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "uint", Justification = ObcSuppressBecause.CA1704_IdentifiersShouldBeSpelledCorrectly_SpellingIsCorrectInContextOfTheDomain)]
        public static void ToStringInvariantPreferred_Nullable_uint___Should_get_string_representation___When_called()
        {
            // Arrange
            uint? value = 53;

            // Act
            var actual = value.ToStringInvariantPreferred();

            // Assert
            actual.AsTest().Must().BeEqualTo("53");
        }

        [Fact]
        public static void ToStringInvariantPreferred_long___Should_get_string_representation___When_called()
        {
            // Arrange
            long value = 53;

            // Act
            var actual = value.ToStringInvariantPreferred();

            // Assert
            actual.AsTest().Must().BeEqualTo("53");
        }

        [Fact]
        public static void ToStringInvariantPreferred_Nullable_long___Should_throw_ArgumentNullException___When_value_is_null_and_throwIfNull_is_true()
        {
            // Arrange
            long? value = null;

            // Act
            var actual = Record.Exception(() => value.ToStringInvariantPreferred(throwIfNull: true));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.AsTest().Must().ContainString("value");
        }

        [Fact]
        public static void ToStringInvariantPreferred_Nullable_long___Should_return_null___When_value_is_null_and_throwIfNull_is_false()
        {
            // Arrange
            long? value = null;

            // Act
            var actual = value.ToStringInvariantPreferred(throwIfNull: false);

            // Assert
            actual.AsTest().Must().BeNull();
        }

        [Fact]
        public static void ToStringInvariantPreferred_Nullable_long___Should_get_string_representation___When_called()
        {
            // Arrange
            long? value = 53;

            // Act
            var actual = value.ToStringInvariantPreferred();

            // Assert
            actual.AsTest().Must().BeEqualTo("53");
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ulong", Justification = ObcSuppressBecause.CA1704_IdentifiersShouldBeSpelledCorrectly_SpellingIsCorrectInContextOfTheDomain)]
        public static void ToStringInvariantPreferred_ulong___Should_get_string_representation___When_called()
        {
            // Arrange
            ulong value = 53;

            // Act
            var actual = value.ToStringInvariantPreferred();

            // Assert
            actual.AsTest().Must().BeEqualTo("53");
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ulong", Justification = ObcSuppressBecause.CA1704_IdentifiersShouldBeSpelledCorrectly_SpellingIsCorrectInContextOfTheDomain)]
        public static void ToStringInvariantPreferred_Nullable_ulong___Should_throw_ArgumentNullException___When_value_is_null_and_throwIfNull_is_true()
        {
            // Arrange
            ulong? value = null;

            // Act
            var actual = Record.Exception(() => value.ToStringInvariantPreferred(throwIfNull: true));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.AsTest().Must().ContainString("value");
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ulong", Justification = ObcSuppressBecause.CA1704_IdentifiersShouldBeSpelledCorrectly_SpellingIsCorrectInContextOfTheDomain)]
        public static void ToStringInvariantPreferred_Nullable_ulong___Should_return_null___When_value_is_null_and_throwIfNull_is_false()
        {
            // Arrange
            ulong? value = null;

            // Act
            var actual = value.ToStringInvariantPreferred(throwIfNull: false);

            // Assert
            actual.AsTest().Must().BeNull();
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "ulong", Justification = ObcSuppressBecause.CA1704_IdentifiersShouldBeSpelledCorrectly_SpellingIsCorrectInContextOfTheDomain)]
        public static void ToStringInvariantPreferred_Nullable_ulong___Should_get_string_representation___When_called()
        {
            // Arrange
            ulong? value = 53;

            // Act
            var actual = value.ToStringInvariantPreferred();

            // Assert
            actual.AsTest().Must().BeEqualTo("53");
        }

        [Fact]
        public static void ToStringInvariantPreferred_float___Should_get_string_representation___When_called()
        {
            // Arrange
            var value = 53.25F;

            // Act
            var actual = value.ToStringInvariantPreferred();

            // Assert
            actual.AsTest().Must().BeEqualTo("53.25");
        }

        [Fact]
        public static void ToStringInvariantPreferred_Nullable_float___Should_throw_ArgumentNullException___When_value_is_null_and_throwIfNull_is_true()
        {
            // Arrange
            float? value = null;

            // Act
            var actual = Record.Exception(() => value.ToStringInvariantPreferred(throwIfNull: true));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.AsTest().Must().ContainString("value");
        }

        [Fact]
        public static void ToStringInvariantPreferred_Nullable_float___Should_return_null___When_value_is_null_and_throwIfNull_is_false()
        {
            // Arrange
            float? value = null;

            // Act
            var actual = value.ToStringInvariantPreferred(throwIfNull: false);

            // Assert
            actual.AsTest().Must().BeNull();
        }

        [Fact]
        public static void ToStringInvariantPreferred_Nullable_float___Should_get_string_representation___When_called()
        {
            // Arrange
            float? value = 53.25F;

            // Act
            var actual = value.ToStringInvariantPreferred();

            // Assert
            actual.AsTest().Must().BeEqualTo("53.25");
        }

        [Fact]
        public static void ToStringInvariantPreferred_double___Should_get_string_representation___When_called()
        {
            // Arrange
            double value = 53.25;

            // Act
            var actual = value.ToStringInvariantPreferred();

            // Assert
            actual.AsTest().Must().BeEqualTo("53.25");
        }

        [Fact]
        public static void ToStringInvariantPreferred_Nullable_double___Should_throw_ArgumentNullException___When_value_is_null_and_throwIfNull_is_true()
        {
            // Arrange
            double? value = null;

            // Act
            var actual = Record.Exception(() => value.ToStringInvariantPreferred(throwIfNull: true));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.AsTest().Must().ContainString("value");
        }

        [Fact]
        public static void ToStringInvariantPreferred_Nullable_double___Should_return_null___When_value_is_null_and_throwIfNull_is_false()
        {
            // Arrange
            double? value = null;

            // Act
            var actual = value.ToStringInvariantPreferred(throwIfNull: false);

            // Assert
            actual.AsTest().Must().BeNull();
        }

        [Fact]
        public static void ToStringInvariantPreferred_Nullable_double___Should_get_string_representation___When_called()
        {
            // Arrange
            double? value = 53.25;

            // Act
            var actual = value.ToStringInvariantPreferred();

            // Assert
            actual.AsTest().Must().BeEqualTo("53.25");
        }

        [Fact]
        public static void ToStringInvariantPreferred_decimal___Should_get_string_representation___When_called()
        {
            // Arrange
            var value = 53.25m;

            // Act
            var actual = value.ToStringInvariantPreferred();

            // Assert
            actual.AsTest().Must().BeEqualTo("53.25");
        }

        [Fact]
        public static void ToStringInvariantPreferred_Nullable_decimal___Should_throw_ArgumentNullException___When_value_is_null_and_throwIfNull_is_true()
        {
            // Arrange
            decimal? value = null;

            // Act
            var actual = Record.Exception(() => value.ToStringInvariantPreferred(throwIfNull: true));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.AsTest().Must().ContainString("value");
        }

        [Fact]
        public static void ToStringInvariantPreferred_Nullable_decimal___Should_return_null___When_value_is_null_and_throwIfNull_is_false()
        {
            // Arrange
            decimal? value = null;

            // Act
            var actual = value.ToStringInvariantPreferred(throwIfNull: false);

            // Assert
            actual.AsTest().Must().BeNull();
        }

        [Fact]
        public static void ToStringInvariantPreferred_Nullable_decimal___Should_get_string_representation___When_called()
        {
            // Arrange
            decimal? value = 53.25m;

            // Act
            var actual = value.ToStringInvariantPreferred();

            // Assert
            actual.AsTest().Must().BeEqualTo("53.25");
        }

        [Fact]
        public static void ToStringInvariantPreferred_bool___Should_get_string_representation___When_called()
        {
            // Arrange
            bool value = true;

            // Act
            var actual = value.ToStringInvariantPreferred();

            // Assert
            actual.AsTest().Must().BeEqualTo("true");
        }

        [Fact]
        public static void ToStringInvariantPreferred_Nullable_bool___Should_throw_ArgumentNullException___When_value_is_null_and_throwIfNull_is_true()
        {
            // Arrange
            bool? value = null;

            // Act
            var actual = Record.Exception(() => value.ToStringInvariantPreferred(throwIfNull: true));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.AsTest().Must().ContainString("value");
        }

        [Fact]
        public static void ToStringInvariantPreferred_Nullable_bool___Should_return_null___When_value_is_null_and_throwIfNull_is_false()
        {
            // Arrange
            bool? value = null;

            // Act
            var actual = value.ToStringInvariantPreferred(throwIfNull: false);

            // Assert
            actual.AsTest().Must().BeNull();
        }

        [Fact]
        public static void ToStringInvariantPreferred_Nullable_bool___Should_get_string_representation___When_called()
        {
            // Arrange
            bool? value = true;

            // Act
            var actual = value.ToStringInvariantPreferred();

            // Assert
            actual.AsTest().Must().BeEqualTo("true");
        }

        [Fact]
        public static void ToStringInvariantPreferred_Guid___Should_get_string_representation___When_called()
        {
            // Arrange
            Guid value = Guid.Parse("F950C64B-A629-4E65-9BC8-28DA7A2DFCBB");

            // Act
            var actual = value.ToStringInvariantPreferred();

            // Assert
            actual.AsTest().Must().BeEqualTo("f950c64b-a629-4e65-9bc8-28da7a2dfcbb");
        }

        [Fact]
        public static void ToStringInvariantPreferred_Nullable_Guid___Should_throw_ArgumentNullException___When_value_is_null_and_throwIfNull_is_true()
        {
            // Arrange
            Guid? value = null;

            // Act
            var actual = Record.Exception(() => value.ToStringInvariantPreferred(throwIfNull: true));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.AsTest().Must().ContainString("value");
        }

        [Fact]
        public static void ToStringInvariantPreferred_Nullable_Guid___Should_return_null___When_value_is_null_and_throwIfNull_is_false()
        {
            // Arrange
            Guid? value = null;

            // Act
            var actual = value.ToStringInvariantPreferred(throwIfNull: false);

            // Assert
            actual.AsTest().Must().BeNull();
        }

        [Fact]
        public static void ToStringInvariantPreferred_Nullable_Guid___Should_get_string_representation___When_called()
        {
            // Arrange
            Guid? value = Guid.Parse("F950C64B-A629-4E65-9BC8-28DA7A2DFCBB");

            // Act
            var actual = value.ToStringInvariantPreferred();

            // Assert
            actual.AsTest().Must().BeEqualTo("f950c64b-a629-4e65-9bc8-28da7a2dfcbb");
        }

        [Fact]
        public static void ToStringInvariantPreferred_DateTime___Should_get_string_representation___When_called()
        {
            // Arrange
            var values = new[]
            {
                new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Utc).AddTicks(20),
                new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Unspecified).AddTicks(20),
                new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Local).AddTicks(20),
            };

            var expected = new[]
            {
                "2019-01-05T12:14:58.1920020Z",
                "2019-01-05T12:14:58.1920020",
                "2019-01-05T12:14:58.1920020" + DateTime.Now.ToString("%K"),
            };

            // Act
            var actual = values.Select(_ => _.ToStringInvariantPreferred()).ToArray();

            // Assert
            actual.AsTest().Must().BeEqualTo(expected);
        }

        [Fact]
        public static void ToStringInvariantPreferred_Nullable_DateTime___Should_throw_ArgumentNullException___When_value_is_null_and_throwIfNull_is_true()
        {
            // Arrange
            DateTime? value = null;

            // Act
            var actual = Record.Exception(() => value.ToStringInvariantPreferred(throwIfNull: true));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.AsTest().Must().ContainString("value");
        }

        [Fact]
        public static void ToStringInvariantPreferred_Nullable_DateTime___Should_return_null___When_value_is_null_and_throwIfNull_is_false()
        {
            // Arrange
            DateTime? value = null;

            // Act
            var actual = value.ToStringInvariantPreferred(throwIfNull: false);

            // Assert
            actual.AsTest().Must().BeNull();
        }

        [Fact]
        public static void ToStringInvariantPreferred_Nullable_DateTime___Should_get_string_representation___When_called()
        {
            // Arrange
            var values = new DateTime?[]
            {
                new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Utc).AddTicks(20),
                new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Unspecified).AddTicks(20),
                new DateTime(2019, 1, 5, 12, 14, 58, 192, DateTimeKind.Local).AddTicks(20),
            };

            var expected = new[]
            {
                "2019-01-05T12:14:58.1920020Z",
                "2019-01-05T12:14:58.1920020",
                "2019-01-05T12:14:58.1920020" + DateTime.Now.ToString("%K"),
            };

            // Act
            var actual = values.Select(_ => _.ToStringInvariantPreferred()).ToArray();

            // Assert
            actual.AsTest().Must().BeEqualTo(expected);
        }

        [Fact]
        public static void ToStringInvariantPreferred_Version___Should_get_string_representation___When_called()
        {
            // Arrange
            var valueAndExpected = new[]
            {
                new { Value = (Version)null, Expected = (string)null },
                new { Value = new Version(), Expected = "0.0" },
                new { Value = new Version(1, 2), Expected = "1.2" },
                new { Value = new Version(1, 2, 3), Expected = "1.2.3" },
                new { Value = new Version(1, 2, 3, 4), Expected = "1.2.3.4" },
            };

            // Act
            var actual = valueAndExpected.Select(_ => _.Value.ToStringInvariantPreferred()).ToList();

            // Assert
            actual.AsTest().Must().BeEqualTo(valueAndExpected.Select(_ => _.Expected).ToList());
        }

        [Fact]
        public static void ToUpperFirstCharacter___Should_throw_ArgumentNullException___When_parameter_value_is_null()
        {
            // Arrange, Act
            var actual = Record.Exception(() => StringExtensions.ToUpperFirstCharacter(null));

            // Assert
            actual.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public static void ToUpperFirstCharacter_cultureInfo___Should_throw_ArgumentNullException___When_parameter_value_is_null()
        {
            // Arrange, Act
            var actual = Record.Exception(() => StringExtensions.ToUpperFirstCharacter(null, CultureInfo.CurrentCulture));

            // Assert
            actual.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public static void ToUpperFirstCharacter_cultureInfo___Should_throw_ArgumentNullException___When_parameter_cultureInfo_is_null()
        {
            // Arrange, Act
            var actual = Record.Exception(() => A.Dummy<string>().ToUpperFirstCharacter(null));

            // Assert
            actual.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public static void ToUpperFirstCharacter___Should_return_empty_string___When_parameter_value_is_an_empty_string()
        {
            // Arrange, Act
            var actual = string.Empty.ToUpperFirstCharacter();

            // Assert
            actual.Should().BeEmpty();
        }

        [Fact]
        public static void ToUpperFirstCharacter_cultureInfo___Should_return_empty_string___When_parameter_value_is_an_empty_string()
        {
            // Arrange, Act
            var actual = string.Empty.ToUpperFirstCharacter(CultureInfo.CurrentCulture);

            // Assert
            actual.Should().BeEmpty();
        }

        [Fact]
        public static void ToUpperFirstCharacter___Should_return_same_string_passed_to_method___When_string_first_character_is_already_uppercase()
        {
            // Arrange
            const string expected1 = @"A";
            const string expected2 = @"A sdFj !@#$%^\&*()=+_?/.,MN2340-8938m";
            const string expected3 = @" ASDF";

            // Act
            var actual1 = expected1.ToUpperFirstCharacter();
            var actual2 = expected2.ToUpperFirstCharacter();
            var actual3 = expected3.ToUpperFirstCharacter();

            // Assert
            actual1.Should().Be(expected1);
            actual2.Should().Be(expected2);
            actual3.Should().Be(expected3);
        }

        [Fact]
        public static void ToUpperFirstCharacter_cultureInfo___Should_return_same_string_passed_to_method___When_string_first_character_is_already_uppercase()
        {
            // Arrange
            const string expected1 = @"A";
            const string expected2 = @"A sdFj !@#$%^\&*()=+_?/.,MN2340-8938m";
            const string expected3 = @" ASDF";

            // Act
            var actual1 = expected1.ToUpperFirstCharacter(CultureInfo.CurrentCulture);
            var actual2 = expected2.ToUpperFirstCharacter(CultureInfo.CurrentCulture);
            var actual3 = expected3.ToUpperFirstCharacter(CultureInfo.CurrentCulture);

            // Assert
            actual1.Should().Be(expected1);
            actual2.Should().Be(expected2);
            actual3.Should().Be(expected3);
        }

        [Fact]
        public static void ToUpperFirstCharacter___Should_convert_first_character_of_string_to_uppercase___When_called()
        {
            // Arrange
            const string value1 = "t";
            const string expected1 = "T";

            const string value2 = "some string";
            const string expected2 = "Some string";

            // Act
            var actual1 = value1.ToUpperFirstCharacter();
            var actual2 = value2.ToUpperFirstCharacter();

            // Assert
            actual1.Should().Be(expected1);
            actual2.Should().Be(expected2);
        }

        [Fact]
        public static void ToUpperFirstCharacter_cultureInfo___Should_convert_first_character_of_string_to_uppercase___When_called()
        {
            // Arrange
            const string value1 = "t";
            const string expected1 = "T";

            const string value2 = "some string";
            const string expected2 = "Some string";

            // Act
            var actual1 = value1.ToUpperFirstCharacter(CultureInfo.CurrentCulture);
            var actual2 = value2.ToUpperFirstCharacter(CultureInfo.CurrentCulture);

            // Assert
            actual1.Should().Be(expected1);
            actual2.Should().Be(expected2);
        }

        [Fact]
        public static void ToUnicodeBytes_ValueToConvertIsNull_ThrowsArgumentNullException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => StringExtensions.ToUnicodeBytes(null));
        }

        [Fact]
        public static void ToUnicodeBytes_StringToEncodeIsEmpty_ReturnsEmptyArray()
        {
            // Arrange, Act, Assert
            Assert.Empty(string.Empty.ToUnicodeBytes());
        }

        [Fact]
        public static void ToUnicodeBytes_StringToEncodeIsNotEmpty_ReturnsUnicodeBytes()
        {
            // Arrange
            const string ToEncode = "This unicode string contains two characters " +
                                         "with codes outside an 8-bit code range, " +
                                         "Pi (\u03a0) and Sigma (\u03a3).";
            const string Expected = "[84][0][104][0][105][0][115][0][32][0][117][0][110][0][105][0][99][0][111][0][100][0][101][0][32][0][115][0][116][0][114][0][105][0][110][0][103][0][32][0][99][0][111][0][110][0][116][0][97][0][105][0][110][0][115][0][32][0][116][0][119][0][111][0][32][0][99][0][104][0][97][0][114][0][97][0][99][0][116][0][101][0][114][0][115][0][32][0][119][0][105][0][116][0][104][0][32][0][99][0][111][0][100][0][101][0][115][0][32][0][111][0][117][0][116][0][115][0][105][0][100][0][101][0][32][0][97][0][110][0][32][0][56][0][45][0][98][0][105][0][116][0][32][0][99][0][111][0][100][0][101][0][32][0][114][0][97][0][110][0][103][0][101][0][44][0][32][0][80][0][105][0][32][0][40][0][160][3][41][0][32][0][97][0][110][0][100][0][32][0][83][0][105][0][103][0][109][0][97][0][32][0][40][0][163][3][41][0][46][0]";

            // Act
            byte[] unicodeBytes = ToEncode.ToUnicodeBytes();

            // Assert
            var bytesPrintout = new StringBuilder();
            foreach (byte b in unicodeBytes)
            {
                bytesPrintout.Append("[" + b.ToString(CultureInfo.CurrentCulture) + "]");
            }

            Assert.Equal(Expected, bytesPrintout.ToString());
        }

        [Fact]
        public static void ToUtf8Bytes_ValueToConvertIsNull_ThrowsArgumentNullException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => StringExtensions.ToUtf8Bytes(null));
        }

        [Fact]
        public static void ToUtf8Bytes_StringToEncodeIsEmpty_ReturnsEmptyArray()
        {
            // Arrange, Act, Assert
            Assert.Empty(string.Empty.ToUtf8Bytes());
        }

        [Fact]
        public static void ToUtf8Bytes_StringToEncodeIsNotEmpty_ReturnsUtf8Bytes()
        {
            // Arrange
            const string ToEncode = "This unicode string contains two characters " +
                                         "with codes outside an 8-bit code range, " +
                                         "Pi (\u03a0) and Sigma (\u03a3).";
            const string Expected = "[84][104][105][115][32][117][110][105][99][111][100][101][32][115][116][114][105][110][103][32][99][111][110][116][97][105][110][115][32][116][119][111][32][99][104][97][114][97][99][116][101][114][115][32][119][105][116][104][32][99][111][100][101][115][32][111][117][116][115][105][100][101][32][97][110][32][56][45][98][105][116][32][99][111][100][101][32][114][97][110][103][101][44][32][80][105][32][40][206][160][41][32][97][110][100][32][83][105][103][109][97][32][40][206][163][41][46]";

            // Act
            byte[] utf8Bytes = ToEncode.ToUtf8Bytes();

            // Assert
            var bytesPrintout = new StringBuilder();
            foreach (byte b in utf8Bytes)
            {
                bytesPrintout.Append("[" + b.ToString(CultureInfo.CurrentCulture) + "]");
            }

            Assert.Equal(Expected, bytesPrintout.ToString());
        }

        [Fact]
        public static void Truncate___Should_throw_ArgumentNullException___When_parameter_value_is_null()
        {
            // Arrange, Act
            var actual = Record.Exception(() => StringExtensions.Truncate(null, A.Dummy<ZeroOrPositiveInteger>()));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.AsTest().Must().ContainString("value");
        }

        [Fact]
        public static void Truncate___Should_throw_ArgumentOutOfRangeException___When_parameter_maxLength_is_negative()
        {
            // Arrange, Act
            var actual = Record.Exception(() => A.Dummy<string>().Truncate(A.Dummy<NegativeInteger>()));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentOutOfRangeException>();
            actual.Message.AsTest().Must().ContainString("maxLength is < 0.");
        }

        [Fact]
        public static void Truncate___Should_return_empty_string___When_value_is_empty_string()
        {
            // Arrange
            var value = string.Empty;

            // Act
            var actual = value.Truncate(A.Dummy<PositiveInteger>());

            // Assert
            actual.AsTest().Must().BeEqualTo(string.Empty);
        }

        [Fact]
        public static void Truncate___Should_return_empty_string___When_maxLength_is_0()
        {
            // Arrange
            var value1 = string.Empty;
            var value2 = A.Dummy<string>();

            // Act
            var actual1 = value1.Truncate(0);
            var actual2 = value2.Truncate(0);

            // Assert
            actual1.AsTest().Must().BeEqualTo(string.Empty);
            actual2.AsTest().Must().BeEqualTo(string.Empty);
        }

        [Fact]
        public static void Truncate___Should_return_value___When_maxLength_is_greater_than_or_equal_to_length_of_value()
        {
            // Arrange
            var value = A.Dummy<string>();

            // Act
            var actual1 = value.Truncate(value.Length);
            var actual2 = value.Truncate(value.Length + 1);

            // Assert
            actual1.AsTest().Must().BeEqualTo(value);
            actual2.AsTest().Must().BeEqualTo(value);
        }

        [Fact]
        public static void Truncate___Should_return_truncated_value___When_maxLength_is_less_than_length_of_value()
        {
            // Arrange
            var value1 = "ab";
            var value2 = "ab cd";

            // Act
            var actual1 = value1.Truncate(value1.Length - 1);
            var actual2 = value2.Truncate(value2.Length - 2);

            // Assert
            actual1.AsTest().Must().BeEqualTo("a");
            actual2.AsTest().Must().BeEqualTo("ab ");
        }
    }
}