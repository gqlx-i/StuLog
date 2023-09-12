using StuLog.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace StuLog.Common
{
    public abstract class ParameterBase : IParameter
    {
        public virtual string Path { get; set; } = ".\\Parameter";

        public virtual string FileName { get => base.GetType().Name + ".xml"; }

        public void Write()
        {
            try
            {
                string path = MakePath(Path, FileName);
                Write(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void Write(string path)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(base.GetType());
            FileInfo file = new FileInfo(path);
            FileStream stream = new FileStream(path, FileMode.Create);
            xmlSerializer.Serialize(stream, this);
            stream.Close();
        }
        public void Read()
        {
            try
            {
                string path = MakePath(Path, FileName);
                Read(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void Read(string path)
        {
            IParameter source = this;
            XmlSerializer xmlSerializer = new XmlSerializer(base.GetType());
            FileStream stream = new FileStream(path, FileMode.Open);
            source = (xmlSerializer.Deserialize(stream) as IParameter);
            stream.Close();
            this.Copy(source);
        }
        private string MakePath(string dir, string fileName)
        {
            string path;
            if (string.IsNullOrWhiteSpace(dir))
            {
                path = fileName;
            }
            else
            {
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                path = dir + "\\" + fileName;
            }
            return path;
        }

        public virtual void Copy(IParameter source)
        {

        }
    }
}
