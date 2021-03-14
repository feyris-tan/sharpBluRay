using System;
using System.Collections.Generic;

namespace moe.yo3explorer.sharpBluRay.Languages
{
    public static class LanguageManager
    {
        private static void InitializeLanguageManager()
        {
            if (_knownLanguages != null)
                return;

            _knownLanguages = new List<Language>
            {
                new Language(LanguageFamily.IndoEuropean, "English", "English", "en", "eng", "eng", "eng"),
                new Language(LanguageFamily.Japonic, "Japanese", "日本語 (にほんご)", "ja", "jpn", "jpn", "jpn")
            };
        }

        private static List<Language> _knownLanguages;


        public static Language GetLanguage(string code)
        {
            if (code.Length != 3)
                throw new ArgumentException("Length must be 3!", nameof(code));

            InitializeLanguageManager();
            Language language = _knownLanguages.Find(x => x.Iso6393.Equals(code));
            if (language == null)
            {
                throw new UnknownLanguageCodeException(code);
            }
            return language;
        }
    }
}
