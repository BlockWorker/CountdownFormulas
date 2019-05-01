using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoundownFormulas {
    class Testing {
        public static string[] words;
        public static char[] allPickedLetters = "tnetennba".ToCharArray();

        public static string[] GetMaxLengthWords() {
            var possibleWords = words.Where(w => {
                var availableChars = new List<char>();
                availableChars.AddRange(allPickedLetters);
                return w.All(c => availableChars.Remove(c));
            });
            var maxLength = possibleWords.Max(w => w.Length);
            return possibleWords.Where(w => w.Length == maxLength).ToArray();
        }
    }
}
