using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace P7MExtractor.Models
{
    public class SignedFileModel
    {
        public string FilePath { get; set; }
        public string FileName { get => Path.GetFileName(FilePath); }
        public string FileNameWithoutP7M { get => string.Join(".", FileName.Split('.')?[0..^1]); }
        public byte[] ExtractedFileBytes { get; set; }
        public X509Certificate2Collection CertificatesCollection { get; set; }
        public X509Certificate2 LastCertInCollection { get => CertificatesCollection?[^1]; }
    }
}
