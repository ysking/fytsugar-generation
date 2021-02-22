using System;
using System.Linq;

namespace FytSugar.Builder
{
    public static class StringExtension
    {
        #region 数据库字段转换兼容，以及附加默认值

        /// <summary>
        /// 数据库类型，转换成实体类型
        /// </summary>
        /// <param name="DbType"></param>
        /// <returns></returns>
        public static string ConvertModelType(this string DbType)
        {
            return (DbType.ToLower()) switch
            {
                "varchar" => "string",
                "text" => "string",
                "longtext" => "string",
                "bit" => "bool",
                "bigint" => "Int64",
                "datetime" => "DateTime",
                "timestamp" => "DateTime",
                _ => DbType,
            };
        }

        /// <summary>
        /// 数据库类型，转换成实体类型
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string ModelDefaultValue(this string dbType, string defaultValue)
        {
            string str = string.Empty;
            if (string.IsNullOrEmpty(defaultValue))
            {
                return str;
            }
            return (dbType.ToLower()) switch
            {
                "int" => " = " + defaultValue + ";",
                "bit" => " = " + (defaultValue == "b'0'" ? "false" : "true") + ";",
                _ => str,
            };
        }

        /// <summary>
        /// 转换数据库名字和实体名字
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public static string TableName(this string Name)
        {
            if (!Name.Contains("_"))
            {
                return Name;
            }
            var tname = string.Empty;
            var str = Name.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in str)
            {
                tname += item.FirstCharToUpper();
            }
            return tname;
        }

        /// <summary>
        /// 首字母大写
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string FirstCharToUpper(this string input)
        {
            if (String.IsNullOrEmpty(input))
                return input;
            string str = input.First().ToString().ToUpper() + input[1..];
            return str;
        }
        #endregion
    }
}