using System;

namespace moe.yo3explorer.sharpBluRay.Languages
{
    public class Language
    {
        internal Language(LanguageFamily languageFamily, string isoLanguageName, string endonym, string iso6391,
            string iso6392T, string iso6392B, string iso6393)
        {
            LanguageFamily = languageFamily;
            IsoLanguageName = isoLanguageName ?? throw new ArgumentNullException(nameof(isoLanguageName));
            Endonym = endonym ?? throw new ArgumentNullException(nameof(endonym));
            Iso6391 = iso6391 ?? throw new ArgumentNullException(nameof(iso6391));
            Iso6392T = iso6392T ?? throw new ArgumentNullException(nameof(iso6392T));
            Iso6392B = iso6392B ?? throw new ArgumentNullException(nameof(iso6392B));
            Iso6393 = iso6393 ?? throw new ArgumentNullException(nameof(iso6393));
        }

        public LanguageFamily LanguageFamily { get; }
        public string IsoLanguageName { get; }
        public string Endonym { get; }
        public string Iso6391 { get; }
        public string Iso6392T { get; }
        public string Iso6392B { get; }
        public string Iso6393 { get; }

        public override string ToString()
        {
            return $"{nameof(IsoLanguageName)}: {IsoLanguageName}, {nameof(Endonym)}: {Endonym}, {nameof(LanguageFamily)}: {LanguageFamily}";
        }
    }
}
