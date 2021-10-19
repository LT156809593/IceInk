using System;

namespace IceInkAbp.Login
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class AbpUserProfile
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public string UserNo { get; set; }

        /// <summary>
        /// 性别 
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// QQ号码
        /// </summary>
        public string QqNumber { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string MobilePhone { get; set; }

        /// <summary>
        /// 短信接收手机号
        /// </summary>
        public string MessageTo { get; set; }

        /// <summary>
        /// 头像路径
        /// </summary>
        public string AvatarPhotoPath { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IDNumber { get; set; }

        /// <summary>
        /// 邮箱地址
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// 用户角色[0:普通用户、99:管理员]
        /// </summary>
        public byte UserRole { get; set; }

        /// <summary>
        /// 启用状态
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 最后一次登入时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 公司信息,如该人员是某一家公司的管理员
        /// </summary>
        public AbpCompanyDto AbpCompany { get; set; }
    }
}