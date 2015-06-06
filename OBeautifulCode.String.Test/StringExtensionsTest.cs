// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensionsTest.cs" company="OBeautifulCode">
//   Copyright 2014 OBeautifulCode
// </copyright>
// <summary>
//   Tests the <see cref="StringExtensions"/> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.String.Test
{
    using System;
    using System.Globalization;
    using System.Text;

    using Xunit;

    /// <summary>
    /// Tests the <see cref="StringExtensions"/> class.
    /// </summary>
    public class StringExtensionsTest
    {
        // ReSharper disable InconsistentNaming

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
        public static void IsAlphaNumeric_StringToCheckIsNull_ThrowsArgumentNullException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => StringExtensions.IsAlphanumeric(null));
        }

        [Fact]
        public static void IsAlphaNumeric_StringToCheckIsEmpty_ReturnsTrue()
        {
            // Arrange, Act, Assert
            Assert.True(string.Empty.IsAlphanumeric());
        }

        [Fact]
        public static void IsAlphanumericTest_StringIsNotAlphaNumeric_ReturnsFalse()
        {
            // Arrange, Act, Assert
            Assert.False("q9*z".IsAlphanumeric());
            Assert.False("!q9z".IsAlphanumeric());
            Assert.False("q9@z".IsAlphanumeric());
            Assert.False("q9asdfz+".IsAlphanumeric());
            Assert.False("abCd29382-afjKsdf9209283".IsAlphanumeric());
            for (int i = 1; i <= 47; i++)
            {
                Assert.False(Convert.ToString((char)i).IsAlphanumeric());
            }

            for (int i = 58; i <= 64; i++)
            {
                Assert.False(Convert.ToString((char)i).IsAlphanumeric());
            }

            for (int i = 91; i <= 96; i++)
            {
                Assert.False(Convert.ToString((char)i).IsAlphanumeric());
            }

            for (int i = 123; i <= 127; i++)
            {
                Assert.False(Convert.ToString((char)i).IsAlphanumeric());
            }
        }

        [Fact]
        public static void IsAlphanumericTest_StringIsAlphaNumeric_ReturnsTrue()
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
        }

        [Fact]
        public static void Pluralize_StringToMakePluralIsNull_ReturnsNull()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => StringExtensions.Pluralize(null));
        }

        [Fact]
        public static void Pluralize_StringToMakePluralIsEmpty_ReturnsEmptyString()
        {
            // Arrange, Act, Assert
            Assert.Equal(string.Empty, string.Empty.Pluralize());
        }

        [Fact]
        public static void Pluralize_StringIsAlreadyPlural_ReturnsOriginalString()
        {
            // Arrange
            const string AlreadyPlural1 = "people";
            const string AlreadyPlural2 = "cars";
            const string AlreadyPlural3 = "fish";
            const string AlreadyPlural4 = "series";

            // Act
            string actual1 = AlreadyPlural1.Pluralize();
            string actual2 = AlreadyPlural2.Pluralize();
            string actual3 = AlreadyPlural3.Pluralize();
            string actual4 = AlreadyPlural4.Pluralize();

            // Assert
            Assert.Equal(AlreadyPlural1, actual1);
            Assert.Equal(AlreadyPlural2, actual2);
            Assert.Equal(AlreadyPlural3, actual3);
            Assert.Equal(AlreadyPlural4, actual4);
        }

        [Fact]
        public static void Pluralize_StringIsSingular_ReturnsPluralFormOfWord()
        {
            // Arrange
            const string Word1 = "person";
            const string Word2 = "car";
            const string Word3 = "axis";
            const string Word4 = "cargo";

            const string Expected1 = "people";
            const string Expected2 = "cars";
            const string Expected3 = "axes";
            const string Expected4 = "cargoes";

            // Act
            string actual1 = Word1.Pluralize();
            string actual2 = Word2.Pluralize();
            string actual3 = Word3.Pluralize();
            string actual4 = Word4.Pluralize();

            // Assert
            Assert.Equal(Expected1, actual1);
            Assert.Equal(Expected2, actual2);
            Assert.Equal(Expected3, actual3);
            Assert.Equal(Expected4, actual4);
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
        public static void ReplaceCaseInsensitive_OldValueFoundAndNewValueIsNull_RemovesAllOccurencesOfOldValue()
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
        public static void ReplaceCaseInsensitive_OldValueFoundAndNewValueIsEmpty_RemovesAllOccurencesOfOldValue()
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
        public static void ReplaceCaseInsensitive_OldValueFoundAndNewValueIsNotEmpty_ReplacesAllOccurencesOfOldValueWithNewValue()
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
        public static void ToAsciiBytes_ValueToConvertIsNull_ThrowsArgumentNullException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => StringExtensions.ToAsciiBytes(null));
        }

        [Fact]
        public static void ToAsciiBytes_StringToEncodIsEmpty_ReturnsEmptyArray()
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
        public static void ToCsvSafe_StringIsAlreadyCsvSafe_ReturnsOriginalString()
        {
            // Arrange
            const string Safe1 = "asdoiouwerzvkjdfm";
            const string Safe2 = "g       O";
            const string Safe3 = "g       O";
            const string Safe4 = @"!@#$%^&*()_+-=';/?.`~_+\";
            string safe5 = string.Empty;

            // Act
            string actual1 = Safe1.ToCsvSafe();
            string actual2 = Safe2.ToCsvSafe();
            string actual3 = Safe3.ToCsvSafe();
            string actual4 = Safe4.ToCsvSafe();
            string actual5 = safe5.ToCsvSafe();

            // Assert
            Assert.Equal(Safe1, actual1);
            Assert.Equal(Safe2, actual2);
            Assert.Equal(Safe3, actual3);
            Assert.Equal(Safe4, actual4);
            Assert.Equal(safe5, actual5);
        }

        [Fact]
        public static void ToCsvSafe_StringIsNotCsvSave_ReturnsCsvSafeVersionOfString()
        {
            // Arrange
            const string NotSafe1 = " ";
            const string Expected1 = "\" \"";

            const string NotSafe2 = "\"";
            const string Expected2 = "\"\"\"\"";

            const string NotSafe3 = ",";
            const string Expected3 = "\",\"";

            const string NotSafe4 = "Super, luxurious truck";
            const string Expected4 = "\"Super, luxurious truck\"";

            const string NotSafe5 = "Super \"luxurious\" truck";
            const string Expected5 = "\"Super \"\"luxurious\"\" truck\"";

            const string NotSafe6 = "Go get one now\r\nthey are going fast";
            const string Expected6 = "\"Go get one now\r\nthey are going fast\"";

            const string NotSafe7 = "Go get one now\nthey are going fast";
            const string Expected7 = "\"Go get one now\nthey are going fast\"";

            string notSafe8 = "Go get one now" + Environment.NewLine + "they are going fast";
            string expected8 = "\"Go get one now" + Environment.NewLine + "they are going fast\"";

            const string NotSafe9 = "Super luxurious truck    ";
            const string Expected9 = "\"Super luxurious truck    \"";

            const string NotSafe10 = "  Super luxurious truck";
            const string Expected10 = "\"  Super luxurious truck\"";

            const string NotSafe11 = "  Super luxurious truck    ";
            const string Expected11 = "\"  Super luxurious truck    \"";

            const string NotSafe12 = " Super, luxurious truck ";
            const string Expected12 = "\" Super, luxurious truck \"";

            const string NotSafe13 = " Super, \"luxurious\" truck ";
            const string Expected13 = "\" Super, \"\"luxurious\"\" truck \"";

            // Act
            string actual1 = NotSafe1.ToCsvSafe();
            string actual2 = NotSafe2.ToCsvSafe();
            string actual3 = NotSafe3.ToCsvSafe();
            string actual4 = NotSafe4.ToCsvSafe();
            string actual5 = NotSafe5.ToCsvSafe();
            string actual6 = NotSafe6.ToCsvSafe();
            string actual7 = NotSafe7.ToCsvSafe();
            string actual8 = notSafe8.ToCsvSafe();
            string actual9 = NotSafe9.ToCsvSafe();
            string actual10 = NotSafe10.ToCsvSafe();
            string actual11 = NotSafe11.ToCsvSafe();
            string actual12 = NotSafe12.ToCsvSafe();
            string actual13 = NotSafe13.ToCsvSafe();

            // Assert
            Assert.Equal(Expected1, actual1);
            Assert.Equal(Expected2, actual2);
            Assert.Equal(Expected3, actual3);
            Assert.Equal(Expected4, actual4);
            Assert.Equal(Expected5, actual5);
            Assert.Equal(Expected6, actual6);
            Assert.Equal(Expected7, actual7);
            Assert.Equal(expected8, actual8);
            Assert.Equal(Expected9, actual9);
            Assert.Equal(Expected10, actual10);
            Assert.Equal(Expected11, actual11);
            Assert.Equal(Expected12, actual12);
            Assert.Equal(Expected13, actual13);
        }

        [Fact]
        public static void ToLowerTrimmed_StringIsNull_ThrowsArgumentNullException()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => StringExtensions.ToLowerTrimmed(null));
        }

        [Fact]
        public static void ToLowerTrimmed_StringIsEmpty_ReturnsEmptyString()
        {
            // Arrange, Act, Assert
            Assert.Equal(string.Empty, string.Empty.ToLowerTrimmed());
        }

        [Fact]
        public static void ToLowerTrimmed_StringIsAlreadyLowerCaseAndTrimmed_ReturnsOriginalString()
        {
            // Arrange
            const string String1 = @"a sdfj !@#$%^\&*()=+_?/.,mn2340-8938m  fkls d";

            // Act
            string actual1 = String1.ToLowerTrimmed();

            // Assert
            Assert.Equal(String1, actual1);
        }

        [Fact]
        public static void ToLowerTrimmed_StringNotLowerCaseOrStringIsNotTrimmedOrBoth_ReturnsLowerCaseTrimmedString()
        {
            // Arrange
            const string String1 = "This Is Not%(#*&! lower CASE";
            const string Expected1 = "this is not%(#*&! lower case";

            const string String2 = "  this needs a trim ";
            const string Expected2 = "this needs a trim";

            const string String3 = " LoWer Trimmed   ";
            const string Expected3 = "lower trimmed";

            // Act
            string actual1 = String1.ToLowerTrimmed();
            string actual2 = String2.ToLowerTrimmed();
            string actual3 = String3.ToLowerTrimmed();

            // Assert
            Assert.Equal(Expected1, actual1);
            Assert.Equal(Expected2, actual2);
            Assert.Equal(Expected3, actual3);
        }
        
        // ReSharper restore InconsistentNaming        
    }
}