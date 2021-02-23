using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace FytSugar.Builder
{
    public class FileHelper
    {

        private static IHostingEnvironment _hostingEnvironment = new HttpContextAccessor().HttpContext.RequestServices.GetService(typeof(IHostingEnvironment)) as IHostingEnvironment;
        /// <summary>
        /// 目录分隔符
        /// windows "\" OSX and Linux  "/"
        /// </summary>
        private static string DirectorySeparatorChar = Path.DirectorySeparatorChar.ToString();
        /// <summary>
        /// 包含应用程序的目录的绝对路径
        /// </summary>
        private static string _ContentRootPath = _hostingEnvironment.ContentRootPath;

        /// <summary>
        /// 获取文件绝对路径
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static string MapPath(string path)
        {
            return Path.Combine(_ContentRootPath, path.TrimStart('~', '/').Replace("/", DirectorySeparatorChar));
        }

        #region 创建目录

        /// <summary>
        /// 创建.删除目录
        /// </summary>
        /// <param name="path">路径</param>
        public static void CreateFiles(string path)
        {
            try
            {
                if (IsDirectory(MapPath(path)))
                    Directory.CreateDirectory(MapPath(path));
                else
                    File.Create(MapPath(path)).Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 直接删除指定目录下的所有文件及文件夹(保留目录)
        /// </summary>
        /// <param name="file"></param>
        public static void DeleteDir(string file)
        {
            //去除文件夹和子文件的只读属性
            //去除文件夹的只读属性
            DirectoryInfo fileInfo = new DirectoryInfo(file)
            {
                Attributes = FileAttributes.Normal & FileAttributes.Directory
            };
            //去除文件的只读属性
            File.SetAttributes(file, FileAttributes.Normal);
            //判断文件夹是否还存在
            if (Directory.Exists(file))
            {
                foreach (string f in Directory.GetFileSystemEntries(file))
                {
                    if (File.Exists(f))
                    {
                        //如果有子文件删除文件
                        File.Delete(f);
                    }
                    else
                    {
                        //循环递归删除子文件夹
                        DeleteDir(f);
                    }
                }
                //删除空文件夹
                Directory.Delete(file);
            }
        }

        /// <summary>
        /// 检测指定路径是否存在
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static bool IsExist(string path)
        {
            return IsDirectory(MapPath(path)) ? Directory.Exists(MapPath(path)) : File.Exists(MapPath(path));
        }
        /// <summary>
        /// 是否为目录或文件夹
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static bool IsDirectory(string path)
        {
            if (path.EndsWith(DirectorySeparatorChar))
                return true;
            else
                return false;
        }
        #endregion

        #region 读取/写入文件
        public static string ReadFile(string fullName)
        {
            //  Encoding code = Encoding.GetEncoding(); //Encoding.GetEncoding("gb2312");
            string temp = MapPath("/") + fullName;
            string str = "";
            if (!File.Exists(temp))
            {
                return str;
            }
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(temp);
                str = sr.ReadToEnd(); // 读取文件
            }
            catch { }
            sr?.Close();
            sr?.Dispose();
            return str;
        }

        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="path">路径 </param>
        /// <param name="fileName">文件名</param>
        /// <param name="content">写入的内容</param>
        /// <param name="appendToLast">是否将内容添加到未尾,默认不添加</param>
        public static void WriteFile(string path, string fileName, string content, bool appendToLast = false)
        {
            path = MapPath(path);
            if (!Directory.Exists(path))//如果不存在就创建file文件夹
                Directory.CreateDirectory(path);

            using (FileStream stream = File.Open(path + fileName, FileMode.OpenOrCreate, FileAccess.Write))
            {
                byte[] by = Encoding.Default.GetBytes(content);
                if (appendToLast)
                {
                    stream.Position = stream.Length;
                }
                else
                {
                    stream.SetLength(0);
                }
                stream.Write(by, 0, by.Length);
            }
        }
        #endregion
    }
}
