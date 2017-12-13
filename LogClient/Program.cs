using System;
using System.Collections.Generic;

namespace LogClient
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 100; i++)
            { 
                LogHelper.Info("提示", "正常提示");
                LogHelper.Error("错误信息", new Exception("测试异常").ToString());
            }
            Console.ReadKey();

        }

    }
}
