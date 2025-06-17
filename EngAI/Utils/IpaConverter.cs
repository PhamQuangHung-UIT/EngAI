using System.Text.RegularExpressions;

namespace EngAI.Utils;

public partial class IpaConveter
{
    // 1) Mapping IPA → ARPABET (expand as needed)
    private static readonly Dictionary<string, string> IpaToArpa = new Dictionary<string, string>
    {
        // stops
        { "p",   "P" },
        { "b",   "B" },
        { "t",   "T" },
        { "t\u032C", "D" },  // 't̬' (voiced diacritic) → D
        { "d",   "D" },
        { "k",   "K" },
        { "g",   "G" },
        { "ʔ",   "GL" },      // glottal stop

        // nasals
        { "m",   "M" },
        { "n",   "N" },
        { "ŋ",   "NG" },

        // fricatives
        { "f",   "F" },
        { "v",   "V" },
        { "θ",   "TH" },
        { "ð",   "DH" },
        { "s",   "S" },
        { "z",   "Z" },
        { "ʃ",   "SH" },
        { "ʒ",   "ZH" },
        { "h",   "HH" },

        // affricates
        { "tʃ", "CH" },
        { "dʒ", "JH" },

        // approximants & liquids
        { "l",   "L" },
        { "ɹ",   "R" },
        { "j",   "Y" },
        { "w",   "W" },

        // vowels (monophthongs)
        { "i",   "IY" },
        { "ɪ",   "IH" },
        { "e",   "EH" },
        { "ɛ",   "EH" },
        { "æ",   "AE" },
        { "ɑ",   "AA" },
        { "ɒ",   "AO" },
        { "ʌ",   "AH" },
        { "ɔ",   "AO" },
        { "ʊ",   "UH" },
        { "u",   "UW" },
        { "ə",   "AH" },
        { "ɜ",   "ER" },

        // vowels (diphthongs)
        { "aɪ", "AY" },
        { "aʊ", "AW" },
        { "ɔɪ", "OY" },
        { "eɪ", "EY" },
        { "oʊ", "OW" },
    };

    // List of IPA tokens sorted longest-first for greedy matching
    private static readonly List<string> IpaTokens = IpaToArpa
        .Keys
        .OrderByDescending(s => s.Length)
        .ToList();

    // IPA stress markers
    private const char PrimaryStress = 'ˈ';
    private const char SecondaryStress = 'ˌ';

    /// <summary>
    /// Converts an IPA string (with ˈ/ˌ stress markers) into a CMUDict-style ARPABET
    /// transcription (with 0/1/2 stress on vowels).
    /// </summary>
    public static string IpaToCmudict(string ipaWord)
    {
        if (string.IsNullOrWhiteSpace(ipaWord))
            return string.Empty;

        var ipa = ipaWord.Trim();
        var result = new List<string>();
        string? pendingStress = null;  // "1" or "2"
        int i = 0;

        while (i < ipa.Length)
        {
            var c = ipa[i];

            // Handle stress markers
            if (c == PrimaryStress)
            {
                pendingStress = "1";
                i++;
                continue;
            }
            else if (c == SecondaryStress)
            {
                pendingStress = "2";
                i++;
                continue;
            }

            // Greedy match IPA tokens
            bool matched = false;
            foreach (var tok in IpaTokens)
            {
                if (i + tok.Length <= ipa.Length && ipa.Substring(i, tok.Length) == tok)
                {
                    var arpa = IpaToArpa[tok];

                    // If this ARPABET symbol is a vowel, append stress marker
                    if (VowelRegex().IsMatch(arpa))
                    {
                        if (pendingStress != null)
                        {
                            arpa += pendingStress;
                            pendingStress = null;
                        }
                        else
                        {
                            arpa += "0";
                        }
                    }

                    result.Add(arpa);
                    i += tok.Length;
                    matched = true;
                    break;
                }
            }

            if (!matched)
            {
                throw new ArgumentException(
                    $"Unknown IPA symbol at position {i}: '{ipa.Substring(i)}'"
                );
            }
        }

        return string.Join(" ", result);
    }

    [GeneratedRegex(@"^(AA|AE|AH|AO|AW|AY|EH|ER|EY|IH|IY|OW|OY|UH|UW)", RegexOptions.Compiled)]
    private static partial Regex VowelRegex();
}