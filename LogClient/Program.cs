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
                LogHelper.Info("提示去外地群多无", "提示去外地群多无");
                LogHelper.Error("提示去外地群多无", new Exception("提示去外地群多无").ToString());
            }
            Console.ReadKey();

        }

    }
}
