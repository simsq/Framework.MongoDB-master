using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogClient
{
    public static class EnvironmentConfig
    {
        /// <summary>
        /// 日志分组缓存Key
        /// </summary>
        public static readonly string LogGroupKey = "_log.group_";

        /// <summary>
        /// 当前服务器IP
        /// </summary>
        public static readonly string CurrLocalIp;

        /// <summary>
        /// 当前系统AppId
        /// </summary>
        public static readonly string AppId;

        /// <summary>
        /// 当前系统名称
        /// </summary>
        public static readonly string AppName;

        /// <summary>
        /// 当前系统环境
        /// </summary>
        public static readonly EnvironmentType Environment;

        /// <summary>
        ///  Redis.Server.Writer  Ip+":"+端口
        /// </summary>
        public static readonly string RedisServeWriter;

        /// <summary>
        ///  Redis.Server.Reader  Ip+":"+端口
        /// </summary>
        public static readonly string RedisServeReader;

        /// <summary>
        /// 静态资源root目录
        ///  Develop:  /statics/source/scripts/
        ///  Test:  /statics/source/dist/
        ///  Produce:  Host.OnlineStatic+EnvironmentConfig.AppName+"ext/"
        /// </summary>
        public static readonly string StaticRootDir;


        /// <summary>
        ///  日志分组ID
        /// </summary>
        public static string LogGroup { get { return new Guid().ToString(); } }

        /// <summary>
        /// 版本
        /// </summary>
        public static string Version { private set; get; }

        static EnvironmentConfig()
        {
            AppId = "appid";
            CurrLocalIp = "当前服务器IP";
        }

        /// <summary>
        /// 环境变量
        /// </summary>
        public enum EnvironmentType
        {
            /// <summary>
            /// 开发环境
            /// </summary>
            Develop = 1,

            /// <summary>
            /// 测试环境
            /// </summary>
            Test = 2,

            /// <summary>
            /// 生产环境
            /// </summary>
            Produce = 3
        }
    }
}
