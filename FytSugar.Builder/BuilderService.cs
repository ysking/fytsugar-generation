using System;
using System.Collections.Generic;
using System.Linq;
using Masuit.Tools;
using Masuit.Tools.Files;
using Masuit.Tools.Strings;

namespace FytSugar.Builder
{
    public class BuilderService:IBuilderService
    {
        private readonly ISevenZipCompressor _sevenZipCompressor;
        public BuilderService(ISevenZipCompressor sevenZipCompressor)
        {
            _sevenZipCompressor = sevenZipCompressor;
        }

        /// <summary>
        /// 连接数据库，并返回当前连接下所有数据库名字
        /// </summary>
        /// <returns></returns>
        public ApiResult<List<string>> InitConnection(BuilderConnection param)
        {
            var result = JResult<List<string>>.Success();
            try
            {
                var db = new SugarInstance().GetInstance(param);
                result.Data=db.DbMaintenance.GetTableInfoList().Select(m=>m.Name).ToList();
                return result;
            }
            catch (Exception ex)
            {
                return JResult<List<string>>.Error(ex.Message);
            }
        }

        /// <summary>
        /// 生成代码
        /// </summary>
        /// <returns></returns>
        public ApiResult<string> CreateCode(BuilderModel createModel)
        {
            var result = JResult<string>.Success();
            try
            {
                var db = new SugarInstance().GetInstance(createModel.connection);
                //读取模板
                var strTemp = FileHelper.ReadFile("/Template/Model.html");
                foreach (var row in createModel.TableNames)
                {
                    var column = db.DbMaintenance.GetColumnInfosByTableName(row);
                    //构建属性
                    var attrStr = "";
                    foreach (var item in column)
                    {
                        attrStr += "        /// <summary>\r\n";
                        attrStr += "        /// " + item.ColumnDescription + "\r\n";
                        attrStr += "        /// <summary>\r\n";
                        if (item.IsPrimarykey)
                        {
                            attrStr += "        [SugarColumn(IsPrimaryKey = true)]\r\n";
                        }
                        attrStr += "        public " + item.DataType.ConvertModelType() + " " + item.DbColumnName + " { get; set; }" + item.DataType.ModelDefaultValue(item.DefaultValue) + "\r\n\r\n";
                    }
                    var modelName = row.TableName();
                    strTemp = strTemp
                       .Replace("{NameSpace}", "FytSoa.Domain.Models."+createModel.Namespace)
                       .Replace("{DataTable}", row)
                       .Replace("{TableName}", modelName)
                       .Replace("{AttributeList}", attrStr);
                    //写入文件
                    var path = "/wwwroot/generate/" + DateTime.Now.ToString("yyyyMMdd");
                    FileHelper.WriteFile(path + "/Model/", modelName + ".cs", strTemp);
                    result.Data = path;
                }
                return result;
            }
            catch (Exception ex)
            {
                return JResult<string>.Error(ex.Message);
            }
        }
    }
}
