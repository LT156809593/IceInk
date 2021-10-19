using System;

namespace IceInkAbp.Login
{
    /// <summary>
    /// 公司信息
    /// </summary>
    public class AbpCompanyDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 公司类型id
        /// </summary>
        public int CompanyTypeId { get; set; }

        /// <summary>
        /// 公司类型名称
        /// </summary>
        public string CompanyTypeName { get; set; }

        /// <summary>
        /// 公司编码
        /// </summary>
        public string CompanyNo { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 公司简称
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// 管理人员编码
        /// </summary>
        public string AdministratorCode { get; set; }

        /// <summary>
        /// 管理人员名称
        /// </summary>
        public string AdministratorName { get; set; }

        /// <summary>
        /// 统一社会信用代码
        /// </summary>
        public string CreditCode { get; set; }

        /// <summary>
        /// 打印抬头
        /// </summary>
        public string PrintTitle { get; set; }

        /// <summary>
        /// 系统名称
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// Logo路径
        /// </summary>
        public string LogoPath { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string Contact { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactPhoneNo { get; set; }

        /// <summary>
        /// 联系传真
        /// </summary>
        public string FaxNumber { get; set; }

        /// <summary>
        /// 联系地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 邮政编码
        /// </summary>
        public string ZipCode { get; set; }

        /// <summary>
        /// 开票信息：公司名称
        /// </summary>
        public string InvoiceCompany { get; set; }

        /// <summary>
        /// 开票信息：税号
        /// </summary>
        public string InvoiceTax { get; set; }

        /// <summary>
        /// 开票信息：注册地址
        /// </summary>
        public string InvoiceAddress { get; set; }

        /// <summary>
        /// 开票信息：开户银行
        /// </summary>
        public string InvoiceAccountBank { get; set; }

        /// <summary>
        /// 开票信息：银行账号
        /// </summary>
        public string InvoiceAccountNo { get; set; }

        /// <summary>
        /// 开票信息：联系电话
        /// </summary>
        public string InvoiceTel { get; set; }

        /// <summary>
        /// 开票信息：企业类型
        /// </summary>
        public string InvoiceCategory { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string Note { get; set; }

        // /// <summary>
        // /// 单据状态
        // /// </summary>
        // public CompanyMixOutputStatus Status { get; set; }
        /// <summary>
        /// 状态说明
        /// </summary>
        public string StatusDesc { get; set; }

        /// <summary>
        /// 拒绝原因
        /// </summary>
        public string RejectReason { get; set; }

        /// <summary>
        /// 营业执照照片(路径)
        /// </summary>
        public string LicensePhotoPath { get; set; }

        /// <summary>
        /// 是否有变更申请[true:有、false:没有]
        /// </summary>
        public bool HasApplying { get; set; }
    }
}