using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace P7MExtractor.Resources
{
    static class ResourceManager
    {
        public static string FindResource(string key)
        {
            try
            {
                return MergedDictionary[key].ToString();
            }
            catch (NullReferenceException ex)
            {
                throw new KeyNotFoundException($"The key \"{key}\" was not found in application merged dictionaries.", ex);
            }
        }

        public static void LoadResourceDictionary(Uri dictionaryPath)
        {
            try
            {
                MergedDictionaries.Add(new ResourceDictionary()
                {
                    Source = dictionaryPath
                });
            }
            catch
            {
                throw;
            }
        }

        public static void LoadNewLanguageDictionary(string locale)
        {
            LoadNewLanguageDictionary(locale, true);
        }

        public static void LoadNewLanguageDictionary(string locale, bool loadAlsoDefaultDictionary)
        {
            ClearAllLanguageResourceDictionaries();

            var loadOnlyDefault = false;

            if (string.IsNullOrEmpty(locale) || locale.ToLower() == "default")
            {
                loadOnlyDefault = true;
            }

            if (loadAlsoDefaultDictionary || loadOnlyDefault)
                LoadResourceDictionary(new Uri($"pack://application:,,,/Resources/Texts/Language_default.xaml", UriKind.RelativeOrAbsolute)); //$"pack://application:,,,/Resources/Texts/Language_default.xaml", UriKind.RelativeOrAbsolute));

            if (!loadOnlyDefault)
                LoadResourceDictionary(new Uri($"pack://application:,,,/Resources/Texts/Language_{locale}.xaml", UriKind.RelativeOrAbsolute)); // $"pack://application:,,,/Resources/Texts/Language_{locale}.xaml", UriKind.RelativeOrAbsolute));

        }

        public static ResourceDictionary MergedDictionary;
        public static Collection<ResourceDictionary> MergedDictionaries => MergedDictionary.MergedDictionaries;

        public static void ClearAllLanguageResourceDictionaries()
        {
            var nonLanguageDictionaries = MergedDictionaries.Where(x => !x.Source.ToString().ToLower().Contains("language")).ToList();
            MergedDictionaries.Clear();

            foreach (var item in nonLanguageDictionaries)
            {
                MergedDictionaries.Add(item);
            }
        }
    }
}
