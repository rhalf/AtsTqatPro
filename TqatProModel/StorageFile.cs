using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TqatProModel {
    public class StorageFile {


        public String Path {
            get;
            set;
        }

        public String Name {
            get;
            set;
        }

        public String getPath () {
            return Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + this.Path + "\\" + this.Name;
        }

        public void Append (Byte[] data) {
            using (BinaryWriter binaryWriter = new BinaryWriter(File.Open(this.getPath(), FileMode.OpenOrCreate))) {
                binaryWriter.Write(data);
            }
        }
    }
}
