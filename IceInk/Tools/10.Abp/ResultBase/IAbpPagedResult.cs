#region << 文 件 说 明 >>
/*----------------------------------------------------------------
// 文件名称：IPagedResult
// 创 建 者：IceInk
// 创建时间：2021/3/26/周五 上午 10:05:46
// 文件版本：V1.0.0
// ===============================================================
// 功能描述：
//		
//
//----------------------------------------------------------------*/
#endregion

namespace IceInk.ABP
{
    /// <summary>
    /// 定义这个接口是为了标准化向服务端接收一页条目。
    /// </summary>
    /// <typeparam name="T">接收项的类型 <see cref="IAbpListResult{T}.Items"/> list</typeparam>
    public interface IAbpPagedResult<T> : IAbpListResult<T>, IAbpHasTotalCount
    {

    }
}