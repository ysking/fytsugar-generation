using System;
using System.Collections.Generic;

namespace FytSugar.Builder
{
    public interface IBuilderService
    {
        /// <summary>
        /// 连接数据库，并返回当前连接下所有数据库名字
        /// </summary>
        /// <returns></returns>
        ApiResult<List<string>> InitConnection(BuilderConnection param);

        /// <summary>
        /// 生成代码
        /// </summary>
        /// <returns></returns>
        ApiResult<string> CreateCode(BuilderModel createModel);
    }
}
