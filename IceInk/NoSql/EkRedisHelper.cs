#region << 文 件 说 明 >>

/*----------------------------------------------------------------
// 文件名称：EkRedisHelper
// 创 建 者：IceInk
// 创建时间：2020/9/9 16:35:52
// 文件版本：V1.0.0
// ===============================================================
// 功能描述：
//		Redis操作帮助
//		需要的Nuget包
//            Newtonsoft.Json  
//            StackExchange.Redis
//  Redis 简介：
//      REmote DIctionary Server(Redis) 是一个由Salvatore Sanfilippo写的key-value存储系统。
//      Redis是一个开源的使用ANSI C语言编写、遵守BSD协议、支持网络、
//      可基于内存亦可持久化的日志型、Key-Value数据库，并提供多种语言的API。
//      它通常被称为数据结构服务器，
//      因为值（value）可以是 字符串(String), 哈希散列(Hash), 列表(list), 集合(sets) 和 有序集合(sorted sets)等类型。
//       
//       Redis 与其他 key - value 缓存产品有以下三个特点：
//           Redis支持数据的持久化，可以将内存中的数据保存在磁盘中，重启的时候可以再次加载进行使用。
//           Redis不仅仅支持简单的key-value类型的数据，同时还提供list，set，zset，hash等数据结构的存储。
//           Redis支持数据的备份，即master-slave模式的数据备份。
//       
//       Redis 优势
//          1.性能极高 – Redis能读的速度是110000次/s,写的速度是81000次/s 。
//          2.丰富的数据类型 – Redis支持二进制案例的 Strings, Lists, Hashes, Sets 及 Ordered Sets 数据类型操作。
//          3.原子 – Redis的所有操作都是原子性的，意思就是要么成功执行要么失败完全不执行。单个操作是原子性的。
                多个操作也支持事务，即原子性，通过MULTI和EXEC指令包起来。
//          4.丰富的特性 – Redis还支持 publish/subscribe, 通知, key 过期等等特性。
//          
//       Redis与其他key-value存储有什么不同
//          1.Redis有着更为复杂的数据结构并且提供对他们的原子性操作，这是一个不同于其他数据库的进化路径。
                Redis的数据类型都是基于基本数据结构的同时对程序员透明，无需进行额外的抽象。
//          2.Redis运行在内存中但是可以持久化到磁盘，所以在对不同数据集进行高速读写时需要权衡内存，
                因为数据量不能大于硬件内存。在内存数据库方面的另一个优点是，相比在磁盘上相同的复杂的数据结构，
                在内存中操作起来非常简单，这样Redis可以做很多内部复杂性很强的事情。
                同时，在磁盘格式方面他们是紧凑的以追加的方式产生的，因为他们并不需要进行随机访问。   
//          
//        String(字符串)	二进制安全	可以包含任何数据,比如jpg图片或者序列化的对象,一个键最大能存储512M	
//        Hash(字典)	键值对集合,即编程语言中的Map类型	适合存储对象,并且可以像数据库中update一个属性一样只修改某一项属性值(Memcached中需要取出整个字符串反序列化成对象修改完再序列化存回去)	
                [使用场景]存储、读取、修改用户属性
//        List(列表)	链表(双向链表)	增删快,提供了操作某一段元素的API	 
                [使用场景]1,最新消息排行等功能(比如朋友圈的时间线) 2,消息队列
//        Set(集合)	哈希表实现,元素不重复	1、添加、删除,查找的复杂度都是O(1) 2、为集合提供了求交集、并集、差集等操作	 
                [使用场景]1、共同好友 2、利用唯一性,统计访问网站的所有独立ip 3、好友推荐时,根据tag求交集,大于某个阈值就可以推荐
//        Sorted Set(有序集合)	将Set中的元素增加一个权重参数score,元素按score有序排列	数据插入集合时,已经进行天然排序	
                [使用场景]1、排行榜 2、带权重的消息队列
//----------------------------------------------------------------*/

#endregion

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace IceInk
{
    /// <summary>
    /// Redis操作帮助
    /// </summary>
    public class EkRedisHelper : IDisposable
    {
        private ConnectionMultiplexer _conn;

        /// <summary>
        /// 自定义键
        /// </summary>
        public string CustomKey;

        private int DbNum { get; }

        public static string DefaultConnectionString { get; set; } =
            ConfigurationManager.ConnectionStrings["RedisHosts"]?.ConnectionString ??
            "127.0.0.1:6379,allowadmin=true,abortConnect=false,connectTimeout=1000,connectRetry=1,syncTimeout=20000";

        /// <summary>
        /// 静态连接池
        /// </summary>
        public static ConcurrentDictionary<string, ConnectionMultiplexer> ConnectionCache { get; set; } =
            new ConcurrentDictionary<string, ConnectionMultiplexer>();

        //public override void Dispose(bool disposing)
        //{

        //}

        public void Dispose()
        {
            _conn?.Dispose();
            _conn = null;
        }


        #region 自定义事件

        /// <summary>
        /// 连接失败 ， 如果重新连接成功你将不会收到这个通知
        /// </summary>
        public event EventHandler<ConnectionFailedEventArgs> ConnectionFailed;

        /// <summary>
        /// 重新建立连接之前的错误
        /// </summary>
        public event EventHandler<ConnectionFailedEventArgs> ConnectionRestored;

        /// <summary>
        /// 发生错误时
        /// </summary>
        public event EventHandler<RedisErrorEventArgs> ErrorMessage;

        /// <summary>
        /// 配置更改时
        /// </summary>
        public event EventHandler<EndPointEventArgs> ConfigurationChanged;

        /// <summary>
        /// 更改集群时
        /// </summary>
        public event EventHandler<HashSlotMovedEventArgs> HashSlotMoved;

        /// <summary>
        /// redis类库错误时
        /// </summary>
        public event EventHandler<InternalErrorEventArgs> InternalError;

        #endregion


        #region 构造函数

        /// <summary>
        /// 构造函数，使用该构造函数需要先在config中配置链接字符串，连接字符串在config配置文件中的ConnectionStrings节下配置，name固定为RedisHosts，值的格式：127.0.0.1:6379,allowadmin=true，若未正确配置，将按默认值“127.0.0.1:6379,allowadmin=true,abortConnect=false”进行操作，如：
        /// <br />
        /// &lt;connectionStrings&gt;<br />
        /// &lt;add name = "RedisHosts" connectionString="127.0.0.1:6379,allowadmin=true,abortConnect=false"/&gt;<br />
        /// &lt;/connectionStrings&gt;
        /// </summary>
        /// <param name="dbNum">数据库编号</param>
        public EkRedisHelper(int dbNum = 0) : this(null, dbNum)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="readWriteHosts">Redis服务器连接字符串，格式：127.0.0.1:6379,allowadmin=true,abortConnect=false</param>
        /// <param name="dbNum">数据库的编号</param>
        public EkRedisHelper(string readWriteHosts, int dbNum = 0)
        {
            DbNum = dbNum;
            _conn = string.IsNullOrWhiteSpace(readWriteHosts)
                ? ConnectionMultiplexer.Connect(ConfigurationOptions.Parse(DefaultConnectionString))
                : ConnectionMultiplexer.Connect(ConfigurationOptions.Parse(readWriteHosts));
            //_conn.ConfigurationChanged += MuxerConfigurationChanged;
            _conn.ConfigurationChanged += ConfigurationChanged;
            //_conn.ConnectionFailed += MuxerConnectionFailed;
            _conn.ConnectionFailed += ConnectionFailed;
            //_conn.ConnectionRestored += MuxerConnectionRestored;
            _conn.ConnectionRestored += ConnectionRestored;
            //_conn.ErrorMessage += MuxerErrorMessage;
            _conn.ErrorMessage += ErrorMessage;
            //_conn.HashSlotMoved += MuxerHashSlotMoved;
            _conn.HashSlotMoved += HashSlotMoved;
            //_conn.InternalError += MuxerInternalError;
            _conn.InternalError += InternalError;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="readWriteHosts">Redis服务器连接字符串，格式：127.0.0.1:6379,allowadmin=true,abortConnect=false</param>
        /// <param name="dbNum">数据库的编号</param>
        /// <param name="_"></param>
        private EkRedisHelper(string readWriteHosts, int dbNum, int _)
        {
            DbNum = dbNum;
            readWriteHosts = string.IsNullOrWhiteSpace(readWriteHosts) ? DefaultConnectionString : readWriteHosts;
            _conn = ConnectionCache.GetOrAdd(readWriteHosts,
                ConnectionMultiplexer.Connect(ConfigurationOptions.Parse(readWriteHosts)));
            ////_conn.ConfigurationChanged += MuxerConfigurationChanged;
            //_conn.ConfigurationChanged += ConfigurationChanged;
            ////_conn.ConnectionFailed += MuxerConnectionFailed;
            //_conn.ConnectionFailed += ConnectionFailed;
            ////_conn.ConnectionRestored += MuxerConnectionRestored;
            //_conn.ConnectionRestored += ConnectionRestored;
            ////_conn.ErrorMessage += MuxerErrorMessage;
            //_conn.ErrorMessage += ErrorMessage;
            ////_conn.HashSlotMoved += MuxerHashSlotMoved;
            //_conn.HashSlotMoved += HashSlotMoved;
            ////_conn.InternalError += MuxerInternalError;
            //_conn.InternalError += InternalError;
        }

        /// <summary>
        /// 获取新实例
        /// </summary>
        /// <param name="db">数据库的编号</param>
        /// <returns></returns>
        public static EkRedisHelper GetInstance(int db = 0)
        {
            return new EkRedisHelper(db);
        }

        /// <summary>
        /// 获取单例
        /// </summary>
        /// <param name="db">数据库的编号</param>
        /// <returns></returns>
        public static EkRedisHelper GetSingleInstance(int db = 0)
        {
            return new EkRedisHelper(null, db, 0);
        }

        /// <summary>
        /// 从对象池获取默认实例
        /// </summary>
        /// <param name="conn">Redis服务器连接字符串，格式：127.0.0.1:6379,allowadmin=true,abortConnect=false</param>
        /// <param name="db">数据库的编号</param>
        /// <returns></returns>
        public static EkRedisHelper GetInstance(string conn, int db = 0)
        {
            return new EkRedisHelper(conn, db);
        }

        /// <summary>
        /// 获取单例
        /// </summary>
        /// <param name="conn">Redis服务器连接字符串，格式：127.0.0.1:6379,allowadmin=true,abortConnect=false</param>
        /// <param name="db">数据库的编号</param>
        /// <returns></returns>
        public static EkRedisHelper GetSingleInstance(string conn, int db = 0)
        {
            return new EkRedisHelper(conn, db, 0);
        }

        #endregion 构造函数

        #region String

        #region 同步方法

        /// <summary>
        /// 保存单个key value
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <param name="value">保存的值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns>是否保存成功</returns>
        public bool SetString(string key, string value, TimeSpan? expiry = default)
        {
            key = AddSysCustomKey(key);
            return Do(db => db.StringSet(key, value, expiry));
        }

        /// <summary>
        /// 保存多个key value
        /// </summary>
        /// <param name="keyValues">键值对</param>
        /// <returns>是否保存成功</returns>
        public bool SetString(List<KeyValuePair<RedisKey, RedisValue>> keyValues)
        {
            var newkeyValues = keyValues
                .Select(p => new KeyValuePair<RedisKey, RedisValue>(AddSysCustomKey(p.Key), p.Value)).ToList();
            return Do(db => db.StringSet(newkeyValues.ToArray()));
        }

        /// <summary>
        /// 保存一个对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="obj">值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns>是否保存成功</returns>
        public bool SetString<T>(string key, T obj, TimeSpan? expiry = default)
        {
            key = AddSysCustomKey(key);
            var json = ConvertJson(obj);
            return Do(db => db.StringSet(key, json, expiry));
        }

        /// <summary>
        /// 获取单个key的值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public string GetString(string key)
        {
            if (KeyExists(key))
            {
                key = AddSysCustomKey(key);
                return Do(db => db.StringGet(key));
            }

            return string.Empty;
        }

        /// <summary>
        /// 获取多个Key
        /// </summary>
        /// <param name="listKey">键集合</param>
        /// <returns>值集合</returns>
        public RedisValue[] GetString(List<string> listKey)
        {
            var newKeys = listKey.Select(AddSysCustomKey).ToList();
            return Do(db => db.StringGet(ConvertRedisKeys(newKeys)));
        }

        /// <summary>
        /// 获取一个key的对象
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <returns>实例对象</returns>
        public T GetString<T>(string key)
        {
            if (KeyExists(key))
            {
                key = AddSysCustomKey(key);
                return Do(db => ConvertObj<T>(db.StringGet(key)));
            }

            return default;
        }

        /// <summary>
        /// 为数字增长val
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        public double StringIncrement(string key, double val = 1)
        {
            key = AddSysCustomKey(key);
            return Do(db => db.StringIncrement(key, val));
        }

        /// <summary>
        /// 为数字减少val
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        public double StringDecrement(string key, double val = 1)
        {
            key = AddSysCustomKey(key);
            return Do(db => db.StringDecrement(key, val));
        }

        #endregion 同步方法

        #region 异步方法

        /// <summary>
        /// 保存单个key value
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <param name="value">保存的值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns>是否保存成功</returns>
        public async Task<bool> SetStringAsync(string key, string value, TimeSpan? expiry = default)
        {
            key = AddSysCustomKey(key);
            return await Do(async db => await db.StringSetAsync(key, value, expiry));
        }

        /// <summary>
        /// 保存多个key value
        /// </summary>
        /// <param name="keyValues">键值对</param>
        /// <returns>是否保存成功</returns>
        public async Task<bool> SetStringAsync(List<KeyValuePair<RedisKey, RedisValue>> keyValues)
        {
            var newkeyValues = keyValues
                .Select(p => new KeyValuePair<RedisKey, RedisValue>(AddSysCustomKey(p.Key), p.Value)).ToList();
            return await Do(async db => await db.StringSetAsync(newkeyValues.ToArray()));
        }

        /// <summary>
        /// 保存一个对象
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="obj">需要被缓存的对象</param>
        /// <param name="expiry">过期时间</param>
        /// <returns>是否保存成功</returns>
        public async Task<bool> SetStringAsync<T>(string key, T obj, TimeSpan? expiry = default)
        {
            key = AddSysCustomKey(key);
            var json = ConvertJson(obj);
            return await Do(async db => await db.StringSetAsync(key, json, expiry));
        }

        /// <summary>
        /// 获取单个key的值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public async Task<string> GetStringAsync(string key)
        {
            if (KeyExists(key))
            {
                key = AddSysCustomKey(key);
                return await Do(async db => await db.StringGetAsync(key));
            }

            return string.Empty;
        }

        /// <summary>
        /// 获取多个Key
        /// </summary>
        /// <param name="listKey">键集合</param>
        /// <returns>值集合</returns>
        public async Task<RedisValue[]> GetStringAsync(List<string> listKey)
        {
            var newKeys = listKey.Select(AddSysCustomKey).ToList();
            return await Do(async db => await db.StringGetAsync(ConvertRedisKeys(newKeys)));
        }

        /// <summary>
        /// 获取一个key的对象
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <returns>实例对象</returns>
        public async Task<T> GetStringAsync<T>(string key)
        {
            if (KeyExists(key))
            {
                key = AddSysCustomKey(key);
                string result = await Do(async db => await db.StringGetAsync(key));
                return ConvertObj<T>(result);
            }

            return default;
        }

        /// <summary>
        /// 为数字增长val
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        public async Task<double> IncrementStringAsync(string key, double val = 1)
        {
            key = AddSysCustomKey(key);
            return await Do(async db => await db.StringIncrementAsync(key, val));
        }

        /// <summary>
        /// 为数字减少val
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        public async Task<double> DecrementStringAsync(string key, double val = 1)
        {
            key = AddSysCustomKey(key);
            return await Do(async db => await db.StringDecrementAsync(key, val));
        }

        #endregion 异步方法

        #endregion String

        #region Hash

        #region 同步方法

        /// <summary>
        /// 判断某个数据是否已经被缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="dataKey">对象的字段</param>
        /// <returns>是否缓存成功</returns>
        public bool HashExists(string key, string dataKey)
        {
            key = AddSysCustomKey(key);
            return Do(db => db.HashExists(key, dataKey));
        }

        /// <summary>
        /// 存储数据到hash表
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="dataKey">对象的字段</param>
        /// <param name="t">对象实例</param>
        /// <returns>是否存储成功</returns>
        public bool SetHash<T>(string key, string dataKey, T t)
        {
            key = AddSysCustomKey(key);
            return Do(db =>
            {
                var json = ConvertJson(t);
                return db.HashSet(key, dataKey, json);
            });
        }

        /// <summary>
        /// 存储数据到hash表
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="dataKey">对象的字段</param>
        /// <param name="t">对象实例</param>
        /// <param name="expire">过期时间</param>
        /// <returns>是否存储成功</returns>
        public bool SetHash<T>(string key, string dataKey, T t, TimeSpan expire)
        {
            var b = SetHash(key, dataKey, t);
            Expire(key, expire);
            return b;
        }

        /// <summary>
        /// 移除hash中的某值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="dataKey">对象的字段</param>
        /// <returns>是否移除成功</returns>
        public bool DeleteHash(string key, string dataKey)
        {
            key = AddSysCustomKey(key);
            return Do(db => db.HashDelete(key, dataKey));
        }

        /// <summary>
        /// 移除hash中的多个值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="dataKeys">对象的字段集合</param>
        /// <returns>数量</returns>
        public long DeleteHash(string key, List<RedisValue> dataKeys)
        {
            key = AddSysCustomKey(key);
            return Do(db => db.HashDelete(key, dataKeys.ToArray()));
        }

        /// <summary>
        /// 从hash表获取数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="dataKey">对象的字段</param>
        /// <returns>对象实例</returns>
        public T GetHash<T>(string key, string dataKey)
        {
            if (KeyExists(key))
            {
                key = AddSysCustomKey(key);
                return Do(db =>
                {
                    string value = db.HashGet(key, dataKey);
                    return ConvertObj<T>(value);
                });
            }

            return default;
        }

        /// <summary>
        /// 为数字增长val
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="dataKey">对象的字段</param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        public double IncrementHash(string key, string dataKey, double val = 1)
        {
            key = AddSysCustomKey(key);
            return Do(db => db.HashIncrement(key, dataKey, val));
        }

        /// <summary>
        /// 为数字减少val
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="dataKey">对象的字段</param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        public double DecrementHash(string key, string dataKey, double val = 1)
        {
            key = AddSysCustomKey(key);
            return Do(db => db.HashDecrement(key, dataKey, val));
        }

        /// <summary>
        /// 获取hashkey所有Redis key
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <returns>数据集合</returns>
        public List<T> HashKeys<T>(string key)
        {
            key = AddSysCustomKey(key);
            return Do(db =>
            {
                var values = db.HashKeys(key);
                return ConvetList<T>(values);
            });
        }

        #endregion 同步方法

        #region 异步方法

        /// <summary>
        /// 判断某个数据是否已经被缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="dataKey">对象的字段</param>
        /// <returns>是否缓存成功</returns>
        public async Task<bool> ExistsHashAsync(string key, string dataKey)
        {
            key = AddSysCustomKey(key);
            return await Do(async db => await db.HashExistsAsync(key, dataKey));
        }

        /// <summary>
        /// 存储数据到hash表
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="dataKey">对象的字段</param>
        /// <param name="t">对象实例</param>
        /// <returns>是否存储成功</returns>
        public async Task<bool> SetHashAsync<T>(string key, string dataKey, T t)
        {
            key = AddSysCustomKey(key);
            return await Do(async db =>
            {
                var json = ConvertJson(t);
                return await db.HashSetAsync(key, dataKey, json);
            });
        }

        /// <summary>
        /// 存储数据到hash表
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="dataKey">对象的字段</param>
        /// <param name="t">对象实例</param>
        /// <param name="expire"></param>
        /// <returns>是否存储成功</returns>
        public async Task<bool> SetHashAsync<T>(string key, string dataKey, T t, TimeSpan expire)
        {
            var b = await SetHashAsync(key, dataKey, t);
            await ExpireAsync(key, expire);
            return b;
        }

        /// <summary>
        /// 移除hash中的某值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="dataKey">对象的字段</param>
        /// <returns>是否移除成功</returns>
        public async Task<bool> DeleteHashAsync(string key, string dataKey)
        {
            key = AddSysCustomKey(key);
            return await Do(async db => await db.HashDeleteAsync(key, dataKey));
        }

        /// <summary>
        /// 移除hash中的多个值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="dataKeys">对象的字段集合</param>
        /// <returns>数量</returns>
        public async Task<long> DeleteHashAsync(string key, List<RedisValue> dataKeys)
        {
            key = AddSysCustomKey(key);
            return await Do(async db => await db.HashDeleteAsync(key, dataKeys.ToArray()));
        }

        /// <summary>
        /// 从hash表获取数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="dataKey">对象的字段</param>
        /// <returns>对象实例</returns>
        public async Task<T> GetHashAsync<T>(string key, string dataKey)
        {
            if (KeyExists(key))
            {
                key = AddSysCustomKey(key);
                string value = await Do(async db => await db.HashGetAsync(key, dataKey));
                return ConvertObj<T>(value);
            }

            return default;
        }

        /// <summary>
        /// 为数字增长val
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="dataKey">对象的字段</param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        public async Task<double> IncrementHashAsync(string key, string dataKey, double val = 1)
        {
            key = AddSysCustomKey(key);
            return await Do(async db => await db.HashIncrementAsync(key, dataKey, val));
        }

        /// <summary>
        /// 为数字减少val
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="dataKey">对象的字段</param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        public async Task<double> DecrementHashAsync(string key, string dataKey, double val = 1)
        {
            key = AddSysCustomKey(key);
            return await Do(async db => await db.HashDecrementAsync(key, dataKey, val));
        }

        /// <summary>
        /// 获取hashkey所有Redis key
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <returns>数据集合</returns>
        public async Task<List<T>> HashKeysAsync<T>(string key)
        {
            key = AddSysCustomKey(key);
            var values = await Do(async db => await db.HashKeysAsync(key));
            return ConvetList<T>(values);
        }

        #endregion 异步方法

        #endregion Hash

        #region List

        #region 同步方法

        /// <summary>
        /// 移除指定ListId的内部List的值
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void RemoveList<T>(string key, T value)
        {
            key = AddSysCustomKey(key);
            Do(db => db.ListRemove(key, ConvertJson(value)));
        }

        /// <summary>
        /// 获取指定key的List
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <returns>数据集</returns>
        public List<T> ListRange<T>(string key)
        {
            if (KeyExists(key))
            {
                key = AddSysCustomKey(key);
                return Do(redis =>
                {
                    var values = redis.ListRange(key);
                    return ConvetList<T>(values);
                });
            }

            return new List<T>();
        }

        /// <summary>
        /// 入队
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void ListRightPush<T>(string key, T value)
        {
            key = AddSysCustomKey(key);
            Do(db => db.ListRightPush(key, ConvertJson(value)));
        }

        /// <summary>
        /// 出队
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public T ListRightPop<T>(string key)
        {
            if (KeyExists(key))
            {
                key = AddSysCustomKey(key);
                return Do(db =>
                {
                    var value = db.ListRightPop(key);
                    return ConvertObj<T>(value);
                });
            }

            return default;
        }

        /// <summary>
        /// 入栈
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void ListLeftPush<T>(string key, T value)
        {
            key = AddSysCustomKey(key);
            Do(db => db.ListLeftPush(key, ConvertJson(value)));
        }

        /// <summary>
        /// 出栈
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <returns>对象实例</returns>
        public T ListLeftPop<T>(string key)
        {
            if (KeyExists(key))
            {
                key = AddSysCustomKey(key);
                return Do(db =>
                {
                    var value = db.ListLeftPop(key);
                    return ConvertObj<T>(value);
                });
            }

            return default;
        }

        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>数量</returns>
        public long ListLength(string key)
        {
            key = AddSysCustomKey(key);
            return Do(redis => redis.ListLength(key));
        }

        #endregion 同步方法

        #region 异步方法

        /// <summary>
        /// 移除指定ListId的内部List的值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public async Task<long> RemoveListAsync<T>(string key, T value)
        {
            key = AddSysCustomKey(key);
            return await Do(async db => await db.ListRemoveAsync(key, ConvertJson(value)));
        }

        /// <summary>
        /// 获取指定key的List
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <returns>数据集合</returns>
        public async Task<List<T>> ListRangeAsync<T>(string key)
        {
            if (KeyExists(key))
            {
                key = AddSysCustomKey(key);
                var values = await Do(async redis => await redis.ListRangeAsync(key));
                return ConvetList<T>(values);
            }

            return new List<T>();
        }

        /// <summary>
        /// 入队
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public async Task<long> ListRightPushAsync<T>(string key, T value)
        {
            key = AddSysCustomKey(key);
            return await Do(async db => await db.ListRightPushAsync(key, ConvertJson(value)));
        }

        /// <summary>
        /// 出队
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <returns>对象实例</returns>
        public async Task<T> ListRightPopAsync<T>(string key)
        {
            if (KeyExists(key))
            {
                key = AddSysCustomKey(key);
                var value = await Do(async db => await db.ListRightPopAsync(key));
                return ConvertObj<T>(value);
            }

            return default;
        }

        /// <summary>
        /// 入栈
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public async Task<long> ListLeftPushAsync<T>(string key, T value)
        {
            key = AddSysCustomKey(key);
            return await Do(async db => await db.ListLeftPushAsync(key, ConvertJson(value)));
        }

        /// <summary>
        /// 出栈
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键</param>
        /// <returns>实例对象</returns>
        public async Task<T> ListLeftPopAsync<T>(string key)
        {
            if (KeyExists(key))
            {
                key = AddSysCustomKey(key);
                var value = await Do(async db => await db.ListLeftPopAsync(key));
                return ConvertObj<T>(value);
            }

            return default;
        }

        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>数量</returns>
        public async Task<long> ListLengthAsync(string key)
        {
            key = AddSysCustomKey(key);
            return await Do(async redis => await redis.ListLengthAsync(key));
        }

        #endregion 异步方法

        #endregion List

        #region SortedSet 有序集合

        #region 同步方法

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="score">排序号</param>
        public bool AddSortedSet<T>(string key, T value, double score)
        {
            key = AddSysCustomKey(key);
            return Do(redis => redis.SortedSetAdd(key, ConvertJson(value), score));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public bool RemoveSortedSet<T>(string key, T value)
        {
            key = AddSysCustomKey(key);
            return Do(redis => redis.SortedSetRemove(key, ConvertJson(value)));
        }

        /// <summary>
        /// 获取全部
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>数据集合</returns>
        public List<T> SetRangeSortedByRank<T>(string key)
        {
            if (KeyExists(key))
            {
                key = AddSysCustomKey(key);
                return Do(redis =>
                {
                    var values = redis.SortedSetRangeByRank(key);
                    return ConvetList<T>(values);
                });
            }

            return new List<T>();
        }

        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>数量</returns>
        public long SetSortedLength(string key)
        {
            key = AddSysCustomKey(key);
            return Do(redis => redis.SortedSetLength(key));
        }

        #endregion 同步方法

        #region 异步方法

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="score">排序号</param>
        public async Task<bool> SortedSetAddAsync<T>(string key, T value, double score)
        {
            key = AddSysCustomKey(key);
            return await Do(async redis => await redis.SortedSetAddAsync(key, ConvertJson(value), score));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public async Task<bool> SortedSetRemoveAsync<T>(string key, T value)
        {
            key = AddSysCustomKey(key);
            return await Do(async redis => await redis.SortedSetRemoveAsync(key, ConvertJson(value)));
        }

        /// <summary>
        /// 获取全部
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>数据集合</returns>
        public async Task<List<T>> SortedSetRangeByRankAsync<T>(string key)
        {
            if (KeyExists(key))
            {
                key = AddSysCustomKey(key);
                var values = await Do(async redis => await redis.SortedSetRangeByRankAsync(key));
                return ConvetList<T>(values);
            }

            return new List<T>();
        }

        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>数量</returns>
        public async Task<long> SortedSetLengthAsync(string key)
        {
            key = AddSysCustomKey(key);
            return await Do(async redis => await redis.SortedSetLengthAsync(key));
        }

        #endregion 异步方法

        #endregion SortedSet 有序集合

        #region key

        /// <summary>
        /// 删除单个key
        /// </summary>
        /// <param name="key">redis key</param>
        /// <returns>是否删除成功</returns>
        public bool DeleteKey(string key)
        {
            key = AddSysCustomKey(key);
            return Do(db => db.KeyDelete(key));
        }

        /// <summary>
        /// 删除单个key
        /// </summary>
        /// <param name="key">redis key</param>
        /// <returns>是否删除成功</returns>
        public async Task<bool> DeleteKeyAsync(string key)
        {
            key = AddSysCustomKey(key);
            return await Do(async db => await db.KeyDeleteAsync(key));
        }

        /// <summary>
        /// 删除多个key
        /// </summary>
        /// <param name="keys">rediskey</param>
        /// <returns>成功删除的个数</returns>
        public long DeleteKey(List<string> keys)
        {
            var newKeys = keys.Select(AddSysCustomKey).ToList();
            return Do(db => db.KeyDelete(ConvertRedisKeys(newKeys)));
        }

        /// <summary>
        /// 判断key是否存储
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>是否存储成功</returns>
        public bool KeyExists(string key)
        {
            key = AddSysCustomKey(key);
            return Do(db => db.KeyExists(key));
        }

        /// <summary>
        /// 重新命名key
        /// </summary>
        /// <param name="key">旧的键</param>
        /// <param name="newKey">新的键</param>
        /// <returns>处理结果</returns>
        public bool RenameKey(string key, string newKey)
        {
            key = AddSysCustomKey(key);
            return Do(db => db.KeyRename(key, newKey));
        }

        /// <summary>
        /// 设置Key的过期时间
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="expiry">过期时间</param>
        /// <returns>处理结果</returns>
        public bool Expire(string key, TimeSpan? expiry = default)
        {
            key = AddSysCustomKey(key);
            return Do(db => db.KeyExpire(key, expiry));
        }

        /// <summary>
        /// 设置Key的过期时间
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="expiry">过期时间</param>
        /// <returns>处理结果</returns>
        public async Task<bool> ExpireAsync(string key, TimeSpan? expiry = default)
        {
            key = AddSysCustomKey(key);
            return await Do(async db => await db.KeyExpireAsync(key, expiry));
        }

        #endregion key

        #region 发布订阅

        /// <summary>
        /// Redis发布订阅  订阅
        /// </summary>
        /// <param name="subChannel">订阅频道</param>
        /// <param name="handler">处理过程</param>
        public void Subscribe(string subChannel, Action<RedisChannel, RedisValue> handler = null)
        {
            var sub = _conn.GetSubscriber();
            sub.Subscribe(subChannel, (channel, message) =>
            {
                if (handler == null)
                    Console.WriteLine(subChannel + " 订阅收到消息：" + message);
                else
                    handler(channel, message);
            });
        }

        /// <summary>
        /// Redis发布订阅  发布
        /// </summary>
        /// <typeparam name="T">消息对象</typeparam>
        /// <param name="channel">发布频道</param>
        /// <param name="msg">消息</param>
        /// <returns>消息的数量</returns>
        public long Publish<T>(string channel, T msg)
        {
            var sub = _conn.GetSubscriber();
            return sub.Publish(channel, ConvertJson(msg));
        }

        /// <summary>
        /// Redis发布订阅  取消订阅
        /// </summary>
        /// <param name="channel">频道</param>
        public void Unsubscribe(string channel)
        {
            var sub = _conn.GetSubscriber();
            sub.Unsubscribe(channel);
        }

        /// <summary>
        /// Redis发布订阅  取消全部订阅
        /// </summary>
        public void UnsubscribeAll()
        {
            var sub = _conn.GetSubscriber();
            sub.UnsubscribeAll();
        }

        #endregion 发布订阅

        #region 其他

        /// <summary>
        /// 创建一个事务
        /// </summary>
        /// <returns>事务对象</returns>
        public ITransaction CreateTransaction()
        {
            return GetDatabase().CreateTransaction();
        }

        /// <summary>
        /// 获得一个数据库实例
        /// </summary>
        /// <returns>数据库实例</returns>
        public IDatabase GetDatabase()
        {
            return _conn.GetDatabase(DbNum);
        }

        /// <summary>
        /// 获得一个服务器实例
        /// </summary>
        /// <param name="hostAndPort">服务器地址</param>
        /// <returns>服务器实例</returns>
        public IServer GetServer(string hostAndPort = null)
        {
            hostAndPort = string.IsNullOrEmpty(hostAndPort) ? _conn.Configuration.Split(',')[0] : hostAndPort;
            return _conn.GetServer(hostAndPort);
        }

        /// <summary>
        /// 设置前缀
        /// </summary>
        /// <param name="customKey">前缀</param>
        public void SetSysCustomKey(string customKey)
        {
            CustomKey = customKey;
        }

        #endregion 其他

        #region 辅助方法

        private string AddSysCustomKey(string oldKey)
        {
            var prefixKey = CustomKey ?? string.Empty;
            return prefixKey + oldKey;
        }

        private T Do<T>(Func<IDatabase, T> func)
        {
            var database = _conn.GetDatabase(DbNum);
            return func(database);
        }

        #region 序列化 饭序列化

        private string ConvertJson<T>(T value)
        {
            return JsonConvert.SerializeObject(value);
        }

        private T ConvertObj<T>(RedisValue value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        private List<T> ConvetList<T>(RedisValue[] values)
        {
            return values.Select(ConvertObj<T>).ToList();
        }

        #endregion

        private RedisKey[] ConvertRedisKeys(List<string> redisKeys)
        {
            return redisKeys.Select(redisKey => (RedisKey) redisKey).ToArray();
        }

        #endregion 辅助方法

        #region 事件

        ///// <summary>
        ///// 配置更改时
        ///// </summary>
        ///// <param name="sender">触发者</param>
        ///// <param name="e">事件参数</param>
        //private static void MuxerConfigurationChanged(object sender, EndPointEventArgs e)
        //{
        //    Console.WriteLine("Configuration changed: " + e.EndPoint);
        //}

        ///// <summary>
        ///// 发生错误时
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private static void MuxerErrorMessage(object sender, RedisErrorEventArgs e)
        //{
        //    Console.WriteLine("ErrorMessage: " + e.Message);
        //}

        ///// <summary>
        ///// 重新建立连接之前的错误
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private static void MuxerConnectionRestored(object sender, ConnectionFailedEventArgs e)
        //{
        //    Console.WriteLine("ConnectionRestored: " + e.EndPoint);
        //}

        ///// <summary>
        ///// 连接失败 ， 如果重新连接成功你将不会收到这个通知
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private static void MuxerConnectionFailed(object sender, ConnectionFailedEventArgs e)
        //{
        //    Console.WriteLine("重新连接：Endpoint failed: " + e.EndPoint + ", " + e.FailureType + (e.Exception == null ? "" : (", " + e.Exception.Message)));
        //}

        ///// <summary>
        ///// 更改集群
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private static void MuxerHashSlotMoved(object sender, HashSlotMovedEventArgs e)
        //{
        //    Console.WriteLine("HashSlotMoved:NewEndPoint" + e.NewEndPoint + ", OldEndPoint" + e.OldEndPoint);
        //}

        ///// <summary>
        ///// redis类库错误
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private static void MuxerInternalError(object sender, InternalErrorEventArgs e)
        //{
        //    Console.WriteLine("InternalError:Message" + e.Exception.Message);
        //}

        #endregion 事件
    }
}