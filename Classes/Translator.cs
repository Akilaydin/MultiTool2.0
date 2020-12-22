using GoogleTranslateFreeApi;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace SQLGenerator {
    static class Translator {
        private static readonly GoogleTranslator translator = new GoogleTranslator();
        public static string Translate (string text, string lang) {
            try
            {
                DialogResult result = System.Windows.Forms.MessageBox.Show("К сожалению, у нас отняли возможность бесплатного перевода без танцев с бубном.\n Вы можете воспользоваться веб-страницей Яндекс-переводчика. \n Открыть её?", "Ошибка перевода", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    Process.Start("https://translate.yandex.ru/");
                    return "";
                }
                else
                {
                    return "";
                }
            } catch
            {
                return "";

            }
           

            
        }

        public static string GetLangTranslate(string translateTo, string translateFrom) {
            switch (translateTo) {
                case "Русский":
                    translateTo = "ru";
                    break;
                case "Английский":
                    translateTo = "en";
                    break;
                case "Китайский":
                    translateTo = "zh";
                    break;
                case "Испанский":
                    translateTo = "es";
                    break;
            }

            switch (translateFrom) {
                case "Русский":
                    translateFrom = "ru";
                    break;
                case "Английский":
                    translateFrom = "en";
                    break;
                case "Китайский":
                    translateFrom = "zh";
                    break;
                case "Испанский":
                    translateFrom = "es";
                    break;
            }

            return translateTo + "-" + translateFrom;
        }


        private struct TranslateItem : ITranslatable
        {
            public string OriginalText { get; }
            public Language FromLanguage { get; }
            public Language ToLanguage { get; }


            public TranslateItem(string text)
            {
                OriginalText = text;
                FromLanguage = new Language("English", "en");
                ToLanguage = GoogleTranslator.GetLanguageByISO("fr");
            }
        }

    }
}
