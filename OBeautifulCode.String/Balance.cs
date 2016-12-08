// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Balance.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.String
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Spritely.Recipes;

    /// <summary>
    /// Provides methods to check a strings for balanced parentheses,
    /// other characters, strings, tags, etc.
    /// </summary>
    public static class Balance
    {
        /// <summary>
        /// Checks for balanced characters in a string.
        /// </summary>
        /// <param name="source">The string to search for balanced characters.</param>
        /// <param name="open">The opening character to search for ( i.e. '(' ).</param>
        /// <param name="close">The closing character to search for ( i.e. ')' ).</param>
        /// <exception cref="ArgumentNullException">source is null.</exception>
        /// <exception cref="ArgumentException">source is whitespace.</exception>
        /// <exception cref="ArgumentException">open == close</exception>
        /// <returns>Returns true if character is balanced in the string, false if not</returns>
        public static bool IsBalanced(this string source, char open, char close)
        {
            int unbalancedPosition;
            return IsBalanced(source, open, close, out unbalancedPosition);
        }

        /// <summary>
        /// Checks for balanced characters in a string.
        /// </summary>
        /// <param name="source">The string to search for balanced characters.</param>
        /// <param name="open">The opening character to search for ( i.e. '(' ).</param>
        /// <param name="close">The closing character to search for ( i.e. ')' ).</param>
        /// <param name="unbalancedPosition">Returns -1 if the string is balanced, otherwise returns the zero-based position in the string where the first unbalanced character was found.</param>
        /// <exception cref="ArgumentNullException">source is null.</exception>
        /// <exception cref="ArgumentException">source is whitespace.</exception>
        /// <exception cref="ArgumentException">open == close</exception>
        /// <returns>Returns true if character is balanced in the string, false if not</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "3#", Justification = "This is a good use of an out parameter.")]
        public static bool IsBalanced(this string source, char open, char close, out int unbalancedPosition)
        {
            source.Named(nameof(source)).Must().NotBeNull().And().NotBeWhiteSpace().OrThrowFirstFailure();
            open.Named(nameof(open)).Must().NotBeEqualTo(close).OrThrow<ArgumentException>();

            return IsBalanced(source.ToCharArray(), open, close, out unbalancedPosition);
        }

        /// <summary>
        /// Checks for balanced strings (i.e. a tag) in another string.
        /// </summary>
        /// <param name="source">The string being searched.</param>
        /// <param name="open">the opening string to search for (i.e. &lt;html&gt;).</param>
        /// <param name="close">the closing string to search for (i.e. &lt;/html&gt;).</param>
        /// <exception cref="ArgumentNullException">source is null.</exception>
        /// <exception cref="ArgumentException">source is whitespace.</exception>
        /// <exception cref="ArgumentNullException">open is null.</exception>
        /// <exception cref="ArgumentException">open is whitespace.</exception>
        /// <exception cref="ArgumentNullException">close is null.</exception>
        /// <exception cref="ArgumentException">close is whitespace.</exception>
        /// <exception cref="ArgumentException">open == close</exception>
        /// <returns>Returns true if opening and closing strings are is balanced in the string being searched, false if not.</returns>
        public static bool IsBalanced(this string source, string open, string close)
        {
            int unbalancedPosition;
            return IsBalanced(source, open, close, out unbalancedPosition);
        }

        /// <summary>
        /// Checks for balanced strings (i.e. a tag) in another string.
        /// </summary>
        /// <param name="source">The string being searched</param>
        /// <param name="open">the opening string to search for (i.e. &lt;html&gt;)</param>
        /// <param name="close">the closing string to search for (i.e. &lt;/html&gt;)</param>
        /// <param name="unbalancedPosition">returns -1 if the string is balanced, otherwise returns the zero-based position in the string where the first unbalanced string was found.</param>
        /// <exception cref="ArgumentNullException">source is null.</exception>
        /// <exception cref="ArgumentException">source is whitespace.</exception>
        /// <exception cref="ArgumentNullException">open is null.</exception>
        /// <exception cref="ArgumentException">open is whitespace.</exception>
        /// <exception cref="ArgumentNullException">close is null.</exception>
        /// <exception cref="ArgumentException">close is whitespace.</exception>
        /// <exception cref="ArgumentException">open == close</exception>
        /// <returns>Returns true if opening and closing strings are is balanced in the string being searched, false if not</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "3#", Justification = "This is a good use of an out parameter.")]
        public static bool IsBalanced(this string source, string open, string close, out int unbalancedPosition)
        {
            source.Named(nameof(source)).Must().NotBeNull().And().NotBeWhiteSpace().OrThrowFirstFailure();
            open.Named(nameof(open)).Must().NotBeNull().And().NotBeWhiteSpace().OrThrowFirstFailure();
            close.Named(nameof(close)).Must().NotBeNull().And().NotBeWhiteSpace().OrThrowFirstFailure();
            open.Named(nameof(open)).Must().NotBeEqualTo(close).OrThrow<ArgumentException>();

            return IsBalanced(source.ToCharArray(), open.ToCharArray(), close.ToCharArray(), out unbalancedPosition);
        }

        /// <summary>
        /// Checks that a string is balanced given a set of opening and closing marker characters.
        /// </summary>
        /// <remarks>
        /// If opening = "(" and "["  and corresponding closing characters are ")" and "]" then
        /// this[is(definitely)]balanced
        /// but[this(is]not)
        /// </remarks>
        /// <param name="source">String to check for balancing.</param>
        /// <param name="open">An ICollection(T) of opening character markers.</param>
        /// <param name="close">An ICollection(T) of closing character markers.  The position in the collection will be matched up against the same position in open to get the open-close pair.</param>
        /// <exception cref="ArgumentNullException">source is null.</exception>
        /// <exception cref="ArgumentException">source is whitespace.</exception>
        /// <exception cref="ArgumentNullException">open is null.</exception>
        /// <exception cref="ArgumentException">open is empty.</exception>
        /// <exception cref="ArgumentNullException">close is null.</exception>
        /// <exception cref="ArgumentException">close is empty.</exception>
        /// <exception cref="ArgumentException">open.Length != close.Length</exception>
        /// <exception cref="ArgumentException">An opening marker is the same as the corresponding closing marker.</exception>
        /// <returns>Returns true if opening and closing markers are balanced in the string being searched, false if not.</returns>
        public static bool IsBalanced(this string source, ICollection<char> open, ICollection<char> close)
        {
            int unbalancedPosition;
            return IsBalanced(source, open, close, out unbalancedPosition);
        }

        /// <summary>
        /// Checks that a string is balanced given a set of opening and closing marker characters.
        /// </summary>
        /// <remarks>
        /// If opening = "(" and "["  and corresponding closing characters are ")" and "]" then
        /// this[is(definitely)]balanced
        /// but[this(is]not)
        /// </remarks>
        /// <param name="source">String to check for balancing.</param>
        /// <param name="open">An ICollection(T) of opening character markers.</param>
        /// <param name="close">An ICollection(T) of closing character markers.  The position in the collection will be matched up against the same position in open to get the open-close pair.</param>
        /// <param name="unbalancedPosition">returns -1 if the string is balanced, otherwise returns the zero-based position in the string where the first unbalanced character was found.</param>
        /// <exception cref="ArgumentNullException">source is null.</exception>
        /// <exception cref="ArgumentException">source is whitespace.</exception>
        /// <exception cref="ArgumentNullException">open is null.</exception>
        /// <exception cref="ArgumentException">open is empty.</exception>
        /// <exception cref="ArgumentNullException">close is null.</exception>
        /// <exception cref="ArgumentException">close is empty.</exception>
        /// <exception cref="ArgumentException">open.Count != close.Count</exception>
        /// <exception cref="ArgumentException">An opening marker is the same as the corresponding closing marker.</exception>
        /// <returns>Returns true if opening and closing markers are balanced in the string being searched, false if not</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "3#", Justification = "This is a good use of an out parameter.")]
        public static bool IsBalanced(this string source, ICollection<char> open, ICollection<char> close, out int unbalancedPosition)
        {
            source.Named(nameof(source)).Must().NotBeNull().And().NotBeWhiteSpace().OrThrowFirstFailure();
            open.Named(nameof(open)).Must().NotBeNull().And().NotBeEmptyString().OrThrowFirstFailure();
            close.Named(nameof(close)).Must().NotBeNull().And().NotBeEmptyString().OrThrowFirstFailure();

            // every opening char needs a closing char
            if (open.Count != close.Count)
            {
                throw new ArgumentException("open and close must have the same number of elments.  Every opening character requires a matching closing character.");
            }

            var openArray = open.ToArray();
            var closeArray = close.ToArray();
            if (openArray.Where((item, i) => item == closeArray[i]).Any())
            {
                throw new ArgumentException("An opening marker is the same as the corresponding closing marker.");
            }

            return IsBalancedMultipleMarkers(source.ToCharArray(), openArray, closeArray, out unbalancedPosition);
        }

        /// <summary>
        /// Determines if one array (to find) is found within another (source)
        /// starting at some fixed position within the source array.
        /// </summary>
        /// <remarks>
        /// This is not a traditional search in the sense that the array to find can be found at any
        /// position within the source array.  The match must happen starting at the given index within
        /// the source array.  If the first character of the array to find does not match the character
        /// at the given source index to start at in the source array, then this method will return false.
        /// </remarks>
        /// <param name="source">The array to search.</param>
        /// <param name="toFindWithinSource">The array to find within the source array.</param>
        /// <param name="sourceIndexToStartAt">The index of the source array to start matching at.</param>
        /// <returns>
        /// Returns true if the array to find is found within the source starting at the given source index position.
        /// </returns>
        private static bool IsOneArrayInAnother(char[] source, char[] toFindWithinSource, int sourceIndexToStartAt)
        {
            bool found = !toFindWithinSource.Where((t, index) => t != source[sourceIndexToStartAt + index]).Any();
            return found;
        }

        /// <summary>
        /// Checks for balanced characters in a character array.
        /// </summary>
        /// <param name="source">The character array to search for balanced characters.</param>
        /// <param name="open">the opening character to search for ( i.e. '(' ).</param>
        /// <param name="close">the closing character to search for ( i.e. ')' ).</param>
        /// <param name="unbalancedPosition">returns -1 if the character array is balanced, otherwise returns the zero-based position in the character array where the first unbalanced character was found.</param>
        /// <returns>Return true if character is balanced in the character array, false if not</returns>
        private static bool IsBalanced(char[] source, char open, char close, out int unbalancedPosition)
        {
            var bookMarks = new Stack<int>();

            for (int index = 0; index < source.Length; index++)
            {
                if (source[index] == open)
                {
                    bookMarks.Push(index);
                }
                else if (source[index] == close)
                {
                    if (bookMarks.Count <= 0)
                    {
                        unbalancedPosition = index;
                        return false;
                    }

                    bookMarks.Pop();
                }
            }

            if (bookMarks.Count > 0)
            {
                unbalancedPosition = bookMarks.Pop();
                return false;
            }

            unbalancedPosition = -1;
            return true;
        }

        /// <summary>
        /// Checks for balanced character array (i.e. a tag) in another character array.
        /// </summary>
        /// <param name="source">The character array being searched.</param>
        /// <param name="open">the opening character array to search for (i.e. &lt;html&gt;).</param>
        /// <param name="close">the closing character array to search for (i.e. &lt;/html&gt;).</param>
        /// <param name="unbalancedPosition">returns -1 if the character array is balanced, otherwise returns the zero-based position in the character array where the first unbalanced character array was found.</param>
        /// <returns>Returns true if opening and closing character array are balanced in the character array being searched, false if not</returns>
        private static bool IsBalanced(char[] source, char[] open, char[] close, out int unbalancedPosition)
        {
            var bookMarks = new Stack<int>();

            for (int index = 0; index < source.Length; index++)
            {
                if (source[index] == open[0])
                {
                    if (IsOneArrayInAnother(source, open, index))
                    {
                        bookMarks.Push(index);
                    }
                }

                if (source[index] == close[0])
                {
                    if (IsOneArrayInAnother(source, close, index))
                    {
                        if (bookMarks.Count <= 0)
                        {
                            unbalancedPosition = index;
                            return false;
                        }

                        bookMarks.Pop();
                    }
                }
            }

            if (bookMarks.Count > 0)
            {
                unbalancedPosition = bookMarks.Pop();
                return false;
            }

            unbalancedPosition = -1;
            return true;
        }

        /// <summary>
        /// Checks that a char array is balanced given a set of opening and closing marker characters.
        /// </summary>
        /// <param name="source">Char array to check for balancing.</param>
        /// <param name="open">An ICollection(T) of opening character markers.</param>
        /// <param name="close">An ICollection(T) of closing character markers.  The position in the collection will be matched up against the same position in open to get the open-close pair.</param>
        /// <param name="unbalancedPosition">returns -1 if the char array is balanced, otherwise returns the zero-based position in the char array where the first unbalanced character was found.</param>
        /// <exception cref="ArgumentException">open.Length != close.Length</exception>
        /// <returns>Returns true if opening and closing markers are balanced in the char array being searched, false if not</returns>
        private static bool IsBalancedMultipleMarkers(char[] source, char[] open, char[] close, out int unbalancedPosition)
        {
            // initialize stacks
            var bookMarks = new Stack<int>();
            var charStack = new Stack<char>();

            // create a lookup - key = closing character  value = opening character
            var openCloseLookup = new Dictionary<char, char>();
            for (int index = 0; index < open.Length; index++)
            {
                openCloseLookup.Add(close[index], open[index]);
            }

            // iterate through each character in the search string
            for (int index = 0; index < source.Length; index++)
            {
                char thischaracter = source[index];

                // is this character an opening character?
                if (openCloseLookup.ContainsValue(thischaracter))
                {
                    bookMarks.Push(index);
                    charStack.Push(thischaracter);
                }
                else if (openCloseLookup.ContainsKey(thischaracter))
                {
                    // closing character - check if stack is empty or if opening character on stack doens't match associated closing character
                    if ((charStack.Count <= 0) || (charStack.Peek() != openCloseLookup[thischaracter]))
                    {
                        unbalancedPosition = index;
                        return false;
                    }

                    bookMarks.Pop();
                    charStack.Pop();
                }
            }

            // anything left on stack?
            if (charStack.Count > 0)
            {
                unbalancedPosition = bookMarks.Pop();
                return false;
            }

            unbalancedPosition = -1;
            return true;
        }
    }
}
