using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using P7MExtractor.Models;
using P7MExtractor.Others;

namespace P7MExtractor.ViewModels
{
    class ExtractorViewModel : BaseViewModel
    {
        SignedFileModel _signedFile { get; set; }
        public ExtractedModel SelectedItem { get; set; }
        Next _next;

        private ObservableCollection<ExtractedModel> _extractedObjects = new();
        public ObservableCollection<ExtractedModel> ExtractedObjects { get => _extractedObjects; set { _extractedObjects = value; OnPropertyChanged(nameof(ExtractedObjects)); } }

        public ExtractorViewModel(SignedFileModel signedFile, Next nextView)
        {
            _signedFile = signedFile;
            _next = nextView;
            SaveSelectedCommand = new GenericCommandAsync(SaveSelected, CanExecute_SaveSelected);

            ProcessFile();
        }

        bool CanExecute_SaveSelected()
        {
            return SelectedItem != null;
        }
        public void ProcessFile()
        {
            SignedCms s = new();
            s.Decode(new ContentInfo(File.ReadAllBytes(_signedFile.FilePath)).Content);

            _signedFile.CertificatesCollection = s.Certificates;
            _signedFile.ExtractedFileBytes = s.ContentInfo.Content;

            ExtractedObjects.Add(new(_signedFile.ExtractedFileBytes, _signedFile.FileNameWithoutP7M, 0));
            foreach (var cert in _signedFile.CertificatesCollection)
            {
                ExtractedObjects.Add(new(cert, cert.GetNameInfo(X509NameType.SimpleName, false), 1));
            }
        }

        public async Task SaveSelected()
        {
            SaveFileDialog dialog = new();

            if (SelectedItem.ExtractedObject as X509Certificate2 is not null)
            {
                dialog.Filter = "Cert|*.cer";
                dialog.DefaultExt = "*.cer";
                dialog.AddExtension = true;
                dialog.FileName = SelectedItem.DisplayName.Replace("/", "_").Replace(".", "_");

                if (dialog.ShowDialog().Value)
                    await SaveCert(dialog.FileName);

                return;
            }

            if (SelectedItem.DisplayName.Split(".").Length > 1)
            {
                dialog.AddExtension = true;
                dialog.DefaultExt = SelectedItem.DisplayName.Split(".")[^1];
            }

            dialog.FileName = SelectedItem.DisplayName;
            dialog.AddExtension = false;
            dialog.Filter = "*.*|*.*";

            if (dialog.ShowDialog().Value)
                await SaveFile(dialog.FileName);
        }

        async Task SaveFile(string path)
        {
            await File.WriteAllBytesAsync(path, _signedFile.ExtractedFileBytes);
        }

        async Task SaveCert(string path)
        {
            await File.AppendAllTextAsync(path, X509Certificate2ToPem((X509Certificate2)SelectedItem.ExtractedObject));
        }

        /// <summary>
        /// Convert <see cref="X509Certificate2"/> to pem string.
        /// </summary>
        /// <param name="certificate"><see cref="X509Certificate2"/> certificate</param>
        /// <returns></returns>
        static string X509Certificate2ToPem(X509Certificate2 certificate)
        {
            var pem = Convert.ToBase64String(certificate.RawData);
            var tmpArr = pem.Split(Environment.NewLine.ToCharArray()); //rozdělení po řádcích
            var tmpList = new List<string>();

            foreach (var str in tmpArr) //projde řádky
            {
                if (str.Length > 63) //pokud je řádek delší než 63 znaků
                {
                    for (int p = 0; p < str.Length;) //projde se po 63 znacích
                    {
                        tmpList.Add(str.Substring(p, p + 63 < str.Length ? 63 : str.Length - p)); //přidá se řádek do dočasného listu
                        p += 63;
                    }
                }
                else
                    tmpList.Add(str); //řádek, který není delší než 63 znaků se přidá rovnou do listu
            }
            var finalPem = new StringBuilder();
            finalPem.Append($"-----BEGIN CERTIFICATE-----{Environment.NewLine}");
            foreach (var line in tmpList)
            {
                finalPem.Append($"{line}{Environment.NewLine}");
            }
            finalPem.Append("-----END CERTIFICATE-----");
            return finalPem.ToString();
        }

        public ICommand SaveSelectedCommand { get; set; }
        public ICommand Next { get; set; }
    }
}
