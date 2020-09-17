#region << 文 件 说 明 >>

/*----------------------------------------------------------------
// 文件名称：EkRandomHelper
// 创 建 者：IceInk
// 创建时间：2020/8/12 14:29:28
// 文件版本：V1.0.0
// ===============================================================
// 功能描述：
//	随机数生成功能
//      Random类是一个产生伪随机数字的类，
//      它的构造函数有两种，一个是直接New Random()，
//      另外一个是New Random(Int32 Seed)，前者是根据触发那刻的系统时间做为种子，来产生一个随机数字，
//      后者可以自己设定触发的种子。 
//      New Random(Int32 Seed)一般都是用UnCheck((Int)DateTime.Now.Ticks)做为参数种子，
//      因此如果计算机运行速度很快，如果触发Random函数间隔时间很短，就有可能造成产生一样的随机数，
//      因为伪随机的数字，在Random的内部产生机制中还是有一定规律的，并非是真正意义上的完全随机。
//
//----------------------------------------------------------------*/

#endregion

using System;
using System.Threading;

namespace IceInk
{
    /// <summary>
    ///     随机数生成帮助工具,极大概率保证生成的随机数不重复
    /// </summary>
    public static class EkRandomHelper
    {
        /// <summary>
        ///     随机获取数字并等待1~2s
        /// </summary>
        /// <returns></returns>
        public static int GetRandomNumberDelay(int min, int max)
        {
            Thread.Sleep(GetRandomInt(500, 1000)); //随机休息一下
            return GetRandomInt(min, max);
        }

        /// <summary>
        ///     获取随机数(int)，解决重复问题
        /// </summary>
        /// <param name="min">随机区间最小值(包含)</param>
        /// <param name="max">随机区间最大值(不包含)</param>
        /// <returns>返回随机数(int)</returns>
        public static int GetRandomInt(int min, int max)
        {
            var random = new Random(CreateSeed());
            return random.Next(min, max);
        }

        /// <summary>
        ///     获取随机数(double)，解决重复问题
        /// </summary>
        /// <param name="min">随机区间最小值(包含)</param>
        /// <param name="max">随机区间最大值(不包含)</param>
        /// <returns>返回随机数(double)</returns>
        public static double GetRandomDouble(double min = 0.0, double max = 1.0)
        {
            var random = new Random(CreateSeed());
            return random.NextDouble() * (max - min);
        }

        /// <summary>
        ///     创建随机种子，极大概率保证同一时刻随机种子不相同
        /// </summary>
        /// <returns></returns>
        private static int CreateSeed()
        {
            var guid = Guid.NewGuid(); //每次都是全新的ID  全球唯一Id
            var sGuid = guid.ToString();
            var seed = DateTime.Now.Millisecond;
            //保证seed 在同一时刻 是不相同的
            foreach (var t in sGuid)
                switch (t)
                {
                    case 'a':
                    case 'b':
                    case 'c':
                    case 'd':
                    case 'e':
                    case 'f':
                    case 'g':
                        seed += 1;
                        break;
                    case 'h':
                    case 'i':
                    case 'j':
                    case 'k':
                    case 'l':
                    case 'm':
                    case 'n':
                        seed += 2;
                        break;
                    case 'o':
                    case 'p':
                    case 'q':
                    case 'r':
                    case 's':
                    case 't':
                        seed += 3;
                        break;
                    case 'u':
                    case 'v':
                    case 'w':
                    case 'x':
                    case 'y':
                    case 'z':
                        seed += 3;
                        break;
                    default:
                        seed += 4;
                        break;
                }

            return seed;
        }
    }
}