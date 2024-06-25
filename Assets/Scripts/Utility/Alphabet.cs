using System;
using UnityEngine;

namespace JustChess.Utility
{
    public static class Alphabet
    {
        public static string GetLetterAtIndex(int index)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index must be non-negative.");
            }

            const int alphabetLength = 26;
            int adjustedIndex = index % alphabetLength;
            char letter = (char) ('A' + adjustedIndex);
            int increment = index / alphabetLength;

            if (increment > 0)
            {
                return $"{letter}{increment}";
            }
            else
            {
                return $"{letter}";
            }
        }
    }
}