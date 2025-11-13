// PDFsharp - A .NET library for processing PDF
// See the LICENSE file in the solution root for more information.

using System;
using System.Text;
using PdfSharp.Pdf.Internal;

namespace PdfSharp.Pdf
{
    /// <summary>
    /// Represents an XML Metadata stream.
    /// </summary>
    public sealed class PdfMetadata : PdfDictionary
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PdfMetadata"/> class.
        /// </summary>
        public PdfMetadata()
        {
            Elements.SetName(Keys.Type, "/Metadata");
            Elements.SetName(Keys.Subtype, "/XML");
            SetupStream();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PdfMetadata"/> class.
        /// </summary>
        /// <param name="document">The document that owns this object.</param>
        public PdfMetadata(PdfDocument document)
            : base(document)
        {
            document.Internals.AddObject(this);
            Elements.SetName(Keys.Type, "/Metadata");
            Elements.SetName(Keys.Subtype, "/XML");
            SetupStream();
        }

        void SetupStream()
        {
            const string begin = @"begin=""";

            var stream = GenerateXmp();

            // Preserve "ï»¿" if text is UTF8 encoded.
            var i = stream.IndexOf(begin, StringComparison.Ordinal);
            var pos = i + begin.Length;
            stream = stream[..pos] + "xxx" + stream[(pos + 3)..];

            byte[] bytes = Encoding.UTF8.GetBytes(stream);
            bytes[pos++] = (byte)'ï';
            bytes[pos++] = (byte)'»';
            bytes[pos] = (byte)'¿';

            CreateStream(bytes);
        }

        string GenerateXmp()
        {
            // Empty xmp
            var str = $"""
                       <?xpacket begin="ï»¿" id="W5M0MpCehiHzreSzNTczkc9d"?>
                         <x:xmpmeta xmlns:x="adobe:ns:meta/" />
                       <?xpacket end="w"?>
                       """;
            return str;
        }

        void Foo()
        {
            string s2 = $"""
                         <?xpacket begin="ï»¿" id="W5M0MpCehiHzreSzNTczkc9d"?>
                           <x:xmpmeta xmlns:x="adobe:ns:meta/" />
                         <?xpacket end="w"?>
                         """;
        }

        /// <summary>
        /// Predefined keys of this dictionary.
        /// </summary>
        internal class Keys : KeysBase
        {
            /// <summary>
            /// (Required) The type of PDF object that this dictionary describes; must be Metadata for a metadata stream.
            /// </summary>
            [KeyInfo(KeyType.Name | KeyType.Optional, FixedValue = "Metadata")]
            public const string Type = "/Type";

            /// <summary>
            /// (Required) The type of metadata stream that this dictionary describes; must be XML.
            /// </summary>
            [KeyInfo(KeyType.Name | KeyType.Optional, FixedValue = "XML")]
            public const string Subtype = "/Subtype";
        }
    }
}
