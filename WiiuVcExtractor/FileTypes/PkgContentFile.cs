using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

namespace WiiuVcExtractor.FileTypes
{
    // Individual file stored within a .pkg file
    public class PkgContentFile
    {
        string path;
        byte[] content;

        public string Path { get { return path; } }
        public byte[] Content { get { return content; } }

        public PkgContentFile(string path, byte[] contentBytes)
        {
            this.path = path;
            this.content = contentBytes;
        }

        // Writes the content file to a given path or to a relative path if not provided
        public void Write(string writePath = "")
        {
            if (String.IsNullOrEmpty(writePath))
            {
                writePath = path;
            }

            // Create the parent directory
            Directory.CreateDirectory(Directory.GetParent(writePath).ToString());

            using (BinaryWriter bw = new BinaryWriter(File.Open(writePath, FileMode.Create)))
            {
                Console.WriteLine("Writing content file {0} to {1}", path, writePath);
                bw.Write(content);
            }
        }

        public void Decompress()
        {
            // TODO: Make this actually functional

            // Attempt to decompress the content
            byte[] decompressedData;

            using (MemoryStream compressedStream = new MemoryStream(content))
            {
                using (DeflateStream deflateStream = new DeflateStream(compressedStream, CompressionMode.Decompress))
                {
                    using (MemoryStream decompressedStream = new MemoryStream())
                    {
                        deflateStream.CopyTo(decompressedStream);
                        decompressedData = decompressedStream.ToArray();
                    }
                }
            }

            // Write all of the decompressed data to the decompressedPath
            content = decompressedData;
        }
    }
}
