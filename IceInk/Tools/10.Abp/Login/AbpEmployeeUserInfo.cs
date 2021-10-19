using System;

namespace IceInkAbp.Login
{
    /// <summary>
    /// 登录获取当前登录用户信息，从 Employee 获取
    /// </summary>
    public class AbpEmployeeUserInfo
    {
        /// <summary>
        /// 部门编号
        /// </summary>
        public Guid DepartmentId { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PostId { get; set; }

        /// <summary>
        /// 职位名称
        /// </summary>
        public string PositionName { get; set; }

        /// <summary>
        /// 工作类型 ID
        /// </summary>
        public Guid WorkTypeId { get; set; }

        /// <summary>
        /// 工作类型名称
        /// </summary>
        public string WorkTypeName { get; set; }

        /// <summary>
        /// 员工编号
        /// </summary>
        public string EmployeeNo { get; set; }

        /// <summary>
        /// 员工名称
        /// </summary>
        public string EmployeeName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public string IDNumber { get; set; }

        /// <summary>
        /// 仓库ID
        /// </summary>
        public Guid? WarehouseId { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 状态描述
        /// </summary>
        public string StatusDesc { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string OpenPhoneNumber { get; set; }

        /// <summary>
        /// 是否管理员
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AvatarPhotoPath { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string IsManager { get; set; }

        /// <summary>
        /// 员工ID
        /// </summary>
        public Guid ID { get; set; }
    }
}