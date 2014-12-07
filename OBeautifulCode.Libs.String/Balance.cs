// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Balance.cs" company="OBeautifulCode">
//   Copyright 2014 OBeautifulCode
// </copyright>
// <summary>
//   Provides methods to check a strings for balanced parentheses, other characters, strings, tags, etc.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Libs.String
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using CuttingEdge.Conditions;

    /// <summary>
    /// Provides methods to check a strings for balanced parentheses, 
    /// other characters, strings, tags, etc.
    /// </summary>
    public class Balance
    {
        #region Fields (Private)

        /// <summary>
        /// 
        /// </summary>
        private readonly Stack<int> bookMarks = new Stack<int>();

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Public Methods

        /// <summary>
        /// Checks for balanced characters in a string.
        /// </summary>
        /// <param name="source">The string to search for blanced characters</param>
        /// <param name="open">the opening character to search for ( i.e. '(' )</param>
        /// <param name="close">the closing character to search for ( i.e. ')' )</param>
        /// <returns><b>True</b> if character is balanced in the string, <b>False</b> if not</returns>
        public bool IsBalanced(string source, char open, char close)
        {
            Condition.Requires(source, "source").IsNotNullOrWhiteSpace();            
            int unbalancedPosition;
            return (IsBalanced(source.ToCharArray(), open, close, out unbalancedPosition));
        }

        /// <summary>
        /// Checks for balanced characters in a string.
        /// </summary>
        /// <param name="source">The string to search for blanced characters</param>
        /// <param name="open">the opening character to search for ( i.e. '(' )</param>
        /// <param name="close">the closing character to search for ( i.e. ')' )</param>
        /// <param name="unbalancedPosition">returns <b>-1</b> if the string is balanced, otherwise returns the zero-based position in the string where the first unbalanced character was found.</param>
        /// <returns><b>True</b> if character is balanced in the string, <b>False</b> if not</returns>
        public bool IsBalanced(string source, char open, char close, out int unbalancedPosition)
        {
            Condition.Requires(source, "source").IsNotNullOrWhiteSpace();
            return (IsBalanced(source.ToCharArray(), open, close, out unbalancedPosition));
        }

        /// <summary>
        /// Checks for balanced strings (i.e. a tag) in another string.
        /// </summary>
        /// <param name="source">The string being searched</param>
        /// <param name="open">the opening string to search for (i.e. &lt;html&gt;)</param>
        /// <param name="close">the closing string to search for (i.e. &lt;/html&gt;)</param>
        /// <returns><b>True</b> if opening and closing strings are is balanced in the string being searched, <b>False</b> if not</returns> 
        public bool IsBalanced(string source, string open, string close)
        {
            Condition.Requires(source, "source").IsNotNullOrWhiteSpace();
            Condition.Requires(open, "open").IsNotNullOrWhiteSpace();
            Condition.Requires(close, "close").IsNotNullOrWhiteSpace();
            int unbalancedPosition;
            return (IsBalanced(source.ToCharArray(), open.ToCharArray(),
                close.ToCharArray(), out unbalancedPosition));
        }

        /// <summary>
        /// Checks for balanced strings (i.e. a tag) in another string.
        /// </summary>
        /// <param name="source">The string being searched</param>
        /// <param name="open">the opening string to search for (i.e. &lt;html&gt;)</param>
        /// <param name="close">the closing string to search for (i.e. &lt;/html&gt;)</param>
        /// <param name="unbalancedPosition">returns <b>-1</b> if the string is balanced, otherwise returns the zero-based position in the string where the first unbalanced string was found.</param>
        /// <returns><b>True</b> if opening and closing strings are is balanced in the string being searched, <b>False</b> if not</returns> 
        public bool IsBalanced(string source, string open, string close, out int unbalancedPosition)
        {
            Condition.Requires(source, "source").IsNotNullOrWhiteSpace();
            Condition.Requires(open, "open").IsNotNullOrWhiteSpace();
            Condition.Requires(close, "close").IsNotNullOrWhiteSpace();
            return (IsBalanced(source.ToCharArray(), open.ToCharArray(),
                close.ToCharArray(), out unbalancedPosition));
        }

        /// <summary>
        /// Checks that a string is balanced given a set of opening and closing marker characters.
        /// </summary>
        /// <param name="source">String to check for balancing</param>
        /// <param name="open">An ICollection(T) of opening character markers</param>
        /// <param name="close">An ICollection(T) of closing character markers.  The position in the colleciton will be matched up against the same position in open to get the open-close pair</param>
        /// <returns><b>True</b> if opening and closing markers are balanced in the string being searched, <b>False</b> if not</returns>
        /// <remarks>
        /// If opening = "(" and "["  and corresponding closing characters are ")" and "]" then
        /// this[is(definately)]balanced
        /// but[this(is]not)
        /// </remarks>
        public bool IsBalancedMultipleMarkers(string source, ICollection<char> open, ICollection<char> close)
        {
            Condition.Requires(source, "source").IsNotNullOrWhiteSpace();
            Condition.Requires(open, "open").IsNotEmpty();
            Condition.Requires(close, "close").IsNotEmpty();
            int unbalancedPosition;
            return IsBalancedMultipleMarkers(source.ToCharArray(), open.ToArray(), close.ToArray(), out unbalancedPosition);
        }

        /// <summary>
        /// Checks that a string is balanced given a set of opening and closing marker characters.
        /// </summary>
        /// <param name="source">String to check for balancing</param>
        /// <param name="open">An ICollection(T) of opening character markers</param>
        /// <param name="close">An ICollection(T) of closing character markers.  The position in the colleciton will be matched up against the same position in open to get the open-close pair</param>
        /// <param name="unbalancedPosition">returns <b>-1</b> if the string is balanced, otherwise returns the zero-based position in the string where the first unbalanced character was found.</param>
        /// <returns><b>True</b> if opening and closing markers are balanced in the string being searched, <b>False</b> if not</returns>
        /// <remarks>
        /// If opening = "(" and "["  and corresponding closing characters are ")" and "]" then
        /// this[is(definately)]balanced
        /// but[this(is]not)
        /// </remarks>
        public bool IsBalancedMultipleMarkers(String source, ICollection<char> open, ICollection<char> close, out int unbalancedPosition)
        {
            Condition.Requires(source, "source").IsNotNullOrWhiteSpace();
            Condition.Requires(open, "open").IsNotEmpty();
            Condition.Requires(close, "close").IsNotEmpty();
            return this.IsBalancedMultipleMarkers(source.ToCharArray(), open.ToArray(), close.ToArray(), out unbalancedPosition);
        }

        #endregion

        #region Internal Methods

        #endregion

        #region Protected Methods

        #endregion

        #region Private Methods

        private static bool CompareArrays(Char[] source, Char[] targetChars, int startPos)
        {
            bool isEqual = true;

            // ReSharper disable LoopCanBeConvertedToQuery
            for (int index = 0; index < targetChars.Length; index++)
            {
                if (targetChars[index] != source[startPos + index])
                {
                    isEqual = false;
                    break;
                }
            }
            // ReSharper restore LoopCanBeConvertedToQuery

            return (isEqual);
        }

        /// <summary>
        /// Checks for balanced characters in a character array. 
        /// </summary>
        /// <param name="source">The character array to search for balanced characters</param>
        /// <param name="open">the opening character to search for ( i.e. '(' )</param>
        /// <param name="close">the closing character to search for ( i.e. ')' )</param>
        /// <param name="unbalancedPosition">returns <b>-1</b> if the character array is balanced, otherwise returns the zero-based position in the character array where the first unbalanced character was found.</param>
        /// <returns><b>True</b> if character is balanced in the character array, <b>False</b> if not</returns>
        private bool IsBalanced(Char[] source, char open, char close, out int unbalancedPosition)
        {
            Condition.Requires(source, "source").IsNotEmpty();
            this.bookMarks.Clear();

            for (int index = 0; index < source.Length; index++)
            {
                if (source[index] == open)
                {
                    this.bookMarks.Push(index);
                }
                else if (source[index] == close)
                {
                    if (this.bookMarks.Count <= 0)
                    {
                        unbalancedPosition = index;
                        return false;
                    }
                    this.bookMarks.Pop();
                }
            }

            if (this.bookMarks.Count > 0)
            {
                unbalancedPosition = this.bookMarks.Pop();
                return false;
            }
            unbalancedPosition = -1;
            return true;
        }

        /// <summary>
        /// Checks for balanced character array (i.e. a tag) in another character array.
        /// </summary>
        /// <param name="source">The character array being searched</param>
        /// <param name="open">the opening character array to search for (i.e. &lt;html&gt;)</param>
        /// <param name="close">the closing character array to search for (i.e. &lt;/html&gt;)</param>
        /// <param name="unbalancedPosition">returns <b>-1</b> if the character array is balanced, otherwise returns the zero-based position in the character array where the first unbalanced character array was found.</param>
        /// <returns><b>True</b> if opening and closing character array are balanced in the character array being searched, <b>False</b> if not</returns> 
        private bool IsBalanced(Char[] source, Char[] open, Char[] close, out int unbalancedPosition)
        {
            Condition.Requires(source, "source").IsNotEmpty();
            Condition.Requires(open, "open").IsNotEmpty();
            Condition.Requires(close, "close").IsNotEmpty();
            this.bookMarks.Clear();

            for (int index = 0; index < source.Length; index++)
            {
                if (source[index] == open[0])
                {
                    if (CompareArrays(source, open, index))
                    {
                        this.bookMarks.Push(index);
                    }
                }

                if (source[index] == close[0])
                {
                    if (CompareArrays(source, close, index))
                    {
                        if (this.bookMarks.Count <= 0)
                        {
                            unbalancedPosition = index;
                            return (false);
                        }
                        this.bookMarks.Pop();
                    }
                }
            }

            if (this.bookMarks.Count > 0)
            {
                unbalancedPosition = this.bookMarks.Pop();
                return false;
            }
            unbalancedPosition = -1;
            return true;
        }

        /// <summary>
        /// Checks that a char array is balanced given a set of opening and closing marker characters.
        /// </summary>
        /// <param name="source">Char array to check for balancing</param>
        /// <param name="open">An ICollection(T) of opening character markers</param>
        /// <param name="close">An ICollection(T) of closing character markers.  The position in the colleciton will be matched up against the same position in open to get the open-close pair</param>
        /// <param name="unbalancedPosition">returns <b>-1</b> if the char array is balanced, otherwise returns the zero-based position in the char array where the first unbalanced character was found.</param>
        /// <returns><b>True</b> if opening and closing markers are balanced in the char array being searched, <b>False</b> if not</returns>
        private bool IsBalancedMultipleMarkers(Char[] source, Char[] open, Char[] close, out int unbalancedPosition)
        {
            // check params
            Condition.Requires(source, "source").IsNotEmpty();
            Condition.Requires(open, "open").IsNotEmpty();
            Condition.Requires(close, "close").IsNotEmpty();
            
            // every opening char needs a closing Char
            if (open.Length != close.Length)
            {                
                throw new ArgumentException("open and close must have the same number of elments.  Every opening character requires a matching closing character.");
            }

            // initialize stacks
            this.bookMarks.Clear();
            var charStack = new Stack<Char>();

            // create a lookup - key = closing character  value = opening character
            var openCloseLookup = new Dictionary<Char, Char>();
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
                    this.bookMarks.Push(index);
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
                    this.bookMarks.Pop();
                    charStack.Pop();
                }
            }

            // anything left on stack?
            if (charStack.Count > 0)
            {
                unbalancedPosition = this.bookMarks.Pop();
                return false;
            }
            unbalancedPosition = -1;
            return true;
        }

        #endregion
    }
}
