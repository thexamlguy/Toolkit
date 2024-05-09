namespace Toolkit.UI.Controls.Avalonia;

internal class PersonPictureInitialsGenerator
{
    public static PersonPictureCharacterType GetCharacterType(string str)
    {

        PersonPictureCharacterType result = PersonPictureCharacterType.Other;
        for (int i = 0; i < 3; i++)
        {
            if ((i >= str.Length) || (str[i] == '\0') || (str[i] == 0xFEFF))
            {
                break;
            }

            char character = str[i];
            PersonPictureCharacterType evaluationResult = GetCharacterType(character);

            switch (evaluationResult)
            {
                case PersonPictureCharacterType.Glyph:
                    result = PersonPictureCharacterType.Glyph;
                    break;
                case PersonPictureCharacterType.Symbolic:
                    if (result != PersonPictureCharacterType.Glyph)
                    {
                        result = PersonPictureCharacterType.Symbolic;
                    }
                    break;
                case PersonPictureCharacterType.Standard:
                    if ((result != PersonPictureCharacterType.Glyph) && (result != PersonPictureCharacterType.Symbolic))
                    {
                        result = PersonPictureCharacterType.Standard;
                    }
                    break;
                default:
                    break;
            }
        }
        return result;
    }

    public static PersonPictureCharacterType GetCharacterType(char character)
    {

        // IPA Extensions
        if ((character >= 0x0250) && (character <= 0x02AF))
        {
            return PersonPictureCharacterType.Glyph;
        }
        // Arabic
        if ((character >= 0x0600) && (character <= 0x06FF))
        {
            return PersonPictureCharacterType.Glyph;
        }
        // Arabic Supplement
        if ((character >= 0x0750) && (character <= 0x077F))
        {
            return PersonPictureCharacterType.Glyph;
        }
        // Arabic Extended-A
        if ((character >= 0x08A0) && (character <= 0x08FF))
        {
            return PersonPictureCharacterType.Glyph;
        }
        // Arabic Presentation Forms-A
        if ((character >= 0xFB50) && (character <= 0xFDFF))
        {
            return PersonPictureCharacterType.Glyph;
        }
        // Arabic Presentation Forms-B
        if ((character >= 0xFE70) && (character <= 0xFEFF))
        {
            return PersonPictureCharacterType.Glyph;
        }
        // Devanagari
        if ((character >= 0x0900) && (character <= 0x097F))
        {
            return PersonPictureCharacterType.Glyph;
        }
        // Devanagari Extended
        if ((character >= 0xA8E0) && (character <= 0xA8FF))
        {
            return PersonPictureCharacterType.Glyph;
        }
        // Bengali
        if ((character >= 0x0980) && (character <= 0x09FF))
        {
            return PersonPictureCharacterType.Glyph;
        }
        // Gurmukhi
        if ((character >= 0x0A00) && (character <= 0x0A7F))
        {
            return PersonPictureCharacterType.Glyph;
        }
        // Gujarati
        if ((character >= 0x0A80) && (character <= 0x0AFF))
        {
            return PersonPictureCharacterType.Glyph;
        }
        // Oriya
        if ((character >= 0x0B00) && (character <= 0x0B7F))
        {
            return PersonPictureCharacterType.Glyph;
        }
        // Tamil
        if ((character >= 0x0B80) && (character <= 0x0BFF))
        {
            return PersonPictureCharacterType.Glyph;
        }
        // Telugu
        if ((character >= 0x0C00) && (character <= 0x0C7F))
        {
            return PersonPictureCharacterType.Glyph;
        }
        // Kannada
        if ((character >= 0x0C80) && (character <= 0x0CFF))
        {
            return PersonPictureCharacterType.Glyph;
        }
        // Malayalam
        if ((character >= 0x0D00) && (character <= 0x0D7F))
        {
            return PersonPictureCharacterType.Glyph;
        }
        // Sinhala
        if ((character >= 0x0D80) && (character <= 0x0DFF))
        {
            return PersonPictureCharacterType.Glyph;
        }
        // Thai
        if ((character >= 0x0E00) && (character <= 0x0E7F))
        {
            return PersonPictureCharacterType.Glyph;
        }
        // Lao
        if ((character >= 0x0E80) && (character <= 0x0EFF))
        {
            return PersonPictureCharacterType.Glyph;
        }
        // SYMBOLIC
        //
        // CJK Unified Ideographs
        if ((character >= 0x4E00) && (character <= 0x9FFF))
        {
            return PersonPictureCharacterType.Symbolic;
        }
        // CJK Unified Ideographs Extension 
        if ((character >= 0x3400) && (character <= 0x4DBF))
        {
            return PersonPictureCharacterType.Symbolic;
        }
        // CJK Unified Ideographs Extension B
        if ((character >= 0x20000) && (character <= 0x2A6DF))
        {
            return PersonPictureCharacterType.Symbolic;
        }
        // CJK Unified Ideographs Extension C
        if ((character >= 0x2A700) && (character <= 0x2B73F))
        {
            return PersonPictureCharacterType.Symbolic;
        }
        // CJK Unified Ideographs Extension D
        if ((character >= 0x2B740) && (character <= 0x2B81F))
        {
            return PersonPictureCharacterType.Symbolic;
        }
        // CJK Radicals Supplement
        if ((character >= 0x2E80) && (character <= 0x2EFF))
        {
            return PersonPictureCharacterType.Symbolic;
        }
        // CJK Symbols and Punctuation
        if ((character >= 0x3000) && (character <= 0x303F))
        {
            return PersonPictureCharacterType.Symbolic;
        }
        // CJK Strokes
        if ((character >= 0x31C0) && (character <= 0x31EF))
        {
            return PersonPictureCharacterType.Symbolic;
        }
        // Enclosed CJK Letters and Months
        if ((character >= 0x3200) && (character <= 0x32FF))
        {
            return PersonPictureCharacterType.Symbolic;
        }
        // CJK Compatibility
        if ((character >= 0x3300) && (character <= 0x33FF))
        {
            return PersonPictureCharacterType.Symbolic;
        }
        // CJK Compatibility Ideographs
        if ((character >= 0xF900) && (character <= 0xFAFF))
        {
            return PersonPictureCharacterType.Symbolic;
        }
        // CJK Compatibility Forms
        if ((character >= 0xFE30) && (character <= 0xFE4F))
        {
            return PersonPictureCharacterType.Symbolic;
        }
        // CJK Compatibility Ideographs Supplement
        if ((character >= 0x2F800) && (character <= 0x2FA1F))
        {
            return PersonPictureCharacterType.Symbolic;
        }
        // Greek and Coptic
        if ((character >= 0x0370) && (character <= 0x03FF))
        {
            return PersonPictureCharacterType.Symbolic;
        }
        // Hebrew
        if ((character >= 0x0590) && (character <= 0x05FF))
        {
            return PersonPictureCharacterType.Symbolic;
        }
        // Armenian
        if ((character >= 0x0530) && (character <= 0x058F))
        {
            return PersonPictureCharacterType.Symbolic;
        }
        // LATIN
        //
        // Basic Latin
        if ((character > 0x0000) && (character <= 0x007F))
        {
            return PersonPictureCharacterType.Standard;
        }
        // Latin-1 Supplement
        if ((character >= 0x0080) && (character <= 0x00FF))
        {
            return PersonPictureCharacterType.Standard;
        }
        // Latin Extended-A
        if ((character >= 0x0100) && (character <= 0x017F))
        {
            return PersonPictureCharacterType.Standard;
        }
        // Latin Extended-B
        if ((character >= 0x0180) && (character <= 0x024F))
        {
            return PersonPictureCharacterType.Standard;
        }
        // Latin Extended-C
        if ((character >= 0x2C60) && (character <= 0x2C7F))
        {
            return PersonPictureCharacterType.Standard;
        }
        // Latin Extended-D
        if ((character >= 0xA720) && (character <= 0xA7FF))
        {
            return PersonPictureCharacterType.Standard;
        }
        // Latin Extended-E
        if ((character >= 0xAB30) && (character <= 0xAB6F))
        {
            return PersonPictureCharacterType.Standard;
        }
        // Latin Extended Additional
        if ((character >= 0x1E00) && (character <= 0x1EFF))
        {
            return PersonPictureCharacterType.Standard;
        }
        // Cyrillic
        if ((character >= 0x0400) && (character <= 0x04FF))
        {
            return PersonPictureCharacterType.Standard;
        }
        // Cyrillic Supplement
        if ((character >= 0x0500) && (character <= 0x052F))
        {
            return PersonPictureCharacterType.Standard;
        }
        // Combining Diacritical Marks
        if ((character >= 0x0300) && (character <= 0x036F))
        {
            return PersonPictureCharacterType.Standard;
        }
        return PersonPictureCharacterType.Other;
    }

    public static string InitialsFromDisplayName(string contactDisplayName)
    {
        PersonPictureCharacterType type = GetCharacterType(contactDisplayName);
        if (type == PersonPictureCharacterType.Standard)
        {
            string displayName = contactDisplayName;
            StripTrailingBrackets(ref displayName);
            string[] words = Split(displayName, ' ');

            if (words.Length == 1)
            {
                string firstWord = words.First();
                string result = GetFirstFullCharacter(firstWord);

                return result.ToUpper();
            }
            else if (words.Length > 1)
            {
                string firstWord = words.First();
                string lastWord = words.Last();
                string result = GetFirstFullCharacter(firstWord);
                result += GetFirstFullCharacter(lastWord);

                return result.ToUpper();
            }
            else
            {
                return string.Empty;
            }
        }
        else
        {
            return string.Empty;
        }
    }
    private static string GetFirstFullCharacter(string str)
    {
        int start = 0;
        while (start < str.Length)
        {
            char character = str[start];
            // Omit ! " # $ % & ' ( ) * + , - . /
            if ((character >= 0x0021) && (character <= 0x002F))
            {
                start++;
                continue;
            }
            // Omit : ; < = > ? @
            if ((character >= 0x003A) && (character <= 0x0040))
            {
                start++;
                continue;
            }
            // Omit { | } ~
            if ((character >= 0x007B) && (character <= 0x007E))
            {
                start++;
                continue;
            }
            break;
        }

        if (start >= str.Length)
        {
            start = 0;
        }

        int index = start + 1;
        while (index < str.Length)
        {
            char character = str[index];

            if ((character < 0x0300) || (character > 0x036F))
            {
                break;
            }

            index++;
        }

        int strLength = index - start;
        return SafeSubstring(str, start, strLength);
    }

    private static string SafeSubstring(string value, int startIndex, int length)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        if (startIndex > value.Length)
        {
            return string.Empty;
        }

        if (length > value.Length - startIndex)
        {
            length = value.Length - startIndex;
        }

        return value.Substring(startIndex, length);
    }
    private static string[] Split(string source, char delim, int maxIterations = 25)
    {
        return source.Split(new[] { delim }, maxIterations);
    }

    private static void StripTrailingBrackets(ref string source)
    {
        string[] delimiters = { "{}", "()", "[]" };
        if (source.Length == 0)
        {
            return;
        }
        foreach (var delimiter in delimiters)
        {
            if (source[source.Length - 1] != delimiter[1])
            {
                continue;
            }
            var start = source.LastIndexOf(delimiter[0]);
            if (start == -1)
            {
                continue;
            }
            source = source.Remove(start);
            return;
        }
    }
}
