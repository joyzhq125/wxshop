namespace Hidistro.UI.Web.Admin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    [AdministerCheck(true)]
    public class RolePermissions : AdminPage
    {
        protected Button btnSet1;
        protected LinkButton btnSetTop;
        protected CheckBox cbAll;
        protected CheckBox cbBrandCategories;
        protected CheckBox cbClientActivy;
        protected CheckBox cbClientGroup;
        protected CheckBox cbClientNew;
        protected CheckBox cbClientSleep;
        protected CheckBox cbCountDown;
        protected CheckBox cbCoupons;
        protected CheckBox cbCRMmanager;
        protected CheckBox cbExpressComputerpes;
        protected CheckBox cbExpressPrint;
        protected CheckBox cbExpressTemplates;
        protected CheckBox cbGifts;
        protected CheckBox cbGroupBuy;
        protected CheckBox cbInStock;
        protected CheckBox cbManageCategories;
        protected CheckBox cbManageCategoriesAdd;
        protected CheckBox cbManageCategoriesDelete;
        protected CheckBox cbManageCategoriesEdit;
        protected CheckBox cbManageCategoriesView;
        protected CheckBox cbManageMembers;
        protected CheckBox cbManageMembersDelete;
        protected CheckBox cbManageMembersEdit;
        protected CheckBox cbManageMembersView;
        protected CheckBox cbManageOrder;
        protected CheckBox cbManageOrderConfirm;
        protected CheckBox cbManageOrderDelete;
        protected CheckBox cbManageOrderEdit;
        protected CheckBox cbManageOrderRemark;
        protected CheckBox cbManageOrderSendedGoods;
        protected CheckBox cbManageOrderView;
        protected CheckBox cbManageProducts;
        protected CheckBox cbManageProductsAdd;
        protected CheckBox cbManageProductsDelete;
        protected CheckBox cbManageProductsDown;
        protected CheckBox cbManageProductsEdit;
        protected CheckBox cbManageProductsUp;
        protected CheckBox cbManageProductsView;
        protected CheckBox cbManageUsers;
        protected CheckBox cbMarketing;
        protected CheckBox cbMemberArealDistributionStatistics;
        protected CheckBox cbMemberMarket;
        protected CheckBox cbMemberRanking;
        protected CheckBox cbMemberRanks;
        protected CheckBox cbMemberRanksAdd;
        protected CheckBox cbMemberRanksDelete;
        protected CheckBox cbMemberRanksEdit;
        protected CheckBox cbMemberRanksView;
        protected CheckBox cbOrderPromotion;
        protected CheckBox cbOrderRefundApply;
        protected CheckBox cbOrderReplaceApply;
        protected CheckBox cbOrderReturnsApply;
        protected CheckBox cbPageManger;
        protected CheckBox cbPaymentModes;
        protected CheckBox cbPictureMange;
        protected CheckBox cbProductBatchExport;
        protected CheckBox cbProductBatchUpload;
        protected CheckBox cbProductCatalog;
        protected CheckBox cbProductPromotion;
        protected CheckBox cbProductSaleRanking;
        protected CheckBox cbProductSaleStatistics;
        protected CheckBox cbProductTypes;
        protected CheckBox cbProductTypesAdd;
        protected CheckBox cbProductTypesDelete;
        protected CheckBox cbProductTypesEdit;
        protected CheckBox cbProductTypesView;
        protected CheckBox cbProductUnclassified;
        protected CheckBox cbSaleList;
        protected CheckBox cbSales;
        protected CheckBox cbSaleTargetAnalyse;
        protected CheckBox cbSaleTotalStatistics;
        protected CheckBox cbShipper;
        protected CheckBox cbShippingModes;
        protected CheckBox cbShippingTemplets;
        protected CheckBox cbShop;
        protected CheckBox cbSiteContent;
        protected CheckBox cbSubjectProducts;
        protected CheckBox cbSummary;
        protected CheckBox cbTotalReport;
        protected CheckBox cbUserIncreaseStatistics;
        protected CheckBox cbUserOrderStatistics;
        protected CheckBox cbVotes;
        protected Literal lblRoleName;
        private int roleId;

        protected RolePermissions() : base("", "")
        {
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            this.PermissionsSet(this.roleId);
            this.Page.Response.Redirect(Globals.GetAdminAbsolutePath(string.Format("/store/RolePermissions.aspx?roleId={0}&Status=1", this.roleId)));
        }

        private void LoadData(int roleId)
        {
            IList<int> privilegeByRoles = ManagerHelper.GetPrivilegeByRoles(roleId);
            this.cbSummary.Checked = privilegeByRoles.Contains(0x3e8);
            this.cbSiteContent.Checked = privilegeByRoles.Contains(0x3e9);
            this.cbVotes.Checked = privilegeByRoles.Contains(0x7d9);
            this.cbShippingTemplets.Checked = privilegeByRoles.Contains(0x3ee);
            this.cbExpressComputerpes.Checked = privilegeByRoles.Contains(0x3ef);
            this.cbPictureMange.Checked = privilegeByRoles.Contains(0x3f1);
            this.cbProductTypesView.Checked = privilegeByRoles.Contains(0xbc9);
            this.cbProductTypesAdd.Checked = privilegeByRoles.Contains(0xbca);
            this.cbProductTypesEdit.Checked = privilegeByRoles.Contains(0xbcb);
            this.cbProductTypesDelete.Checked = privilegeByRoles.Contains(0xbcc);
            this.cbManageCategoriesView.Checked = privilegeByRoles.Contains(0xbcd);
            this.cbManageCategoriesAdd.Checked = privilegeByRoles.Contains(0xbce);
            this.cbManageCategoriesEdit.Checked = privilegeByRoles.Contains(0xbcf);
            this.cbManageCategoriesDelete.Checked = privilegeByRoles.Contains(0xbd0);
            this.cbBrandCategories.Checked = privilegeByRoles.Contains(0xbd1);
            this.cbManageProductsView.Checked = privilegeByRoles.Contains(0xbb9);
            this.cbManageProductsAdd.Checked = privilegeByRoles.Contains(0xbba);
            this.cbManageProductsEdit.Checked = privilegeByRoles.Contains(0xbbb);
            this.cbManageProductsDelete.Checked = privilegeByRoles.Contains(0xbbc);
            this.cbInStock.Checked = privilegeByRoles.Contains(0xbbd);
            this.cbManageProductsUp.Checked = privilegeByRoles.Contains(0xbbe);
            this.cbManageProductsDown.Checked = privilegeByRoles.Contains(0xbbf);
            this.cbProductUnclassified.Checked = privilegeByRoles.Contains(0xbc2);
            this.cbProductBatchUpload.Checked = privilegeByRoles.Contains(0xbc4);
            this.cbProductBatchExport.Checked = privilegeByRoles.Contains(0xbd2);
            this.cbSubjectProducts.Checked = privilegeByRoles.Contains(0xbc3);
            this.cbClientGroup.Checked = privilegeByRoles.Contains(0x1b5f);
            this.cbClientActivy.Checked = privilegeByRoles.Contains(0x1b61);
            this.cbClientNew.Checked = privilegeByRoles.Contains(0x1b60);
            this.cbClientSleep.Checked = privilegeByRoles.Contains(0x1b62);
            this.cbMemberRanksView.Checked = privilegeByRoles.Contains(0x138c);
            this.cbMemberRanksAdd.Checked = privilegeByRoles.Contains(0x138d);
            this.cbMemberRanksEdit.Checked = privilegeByRoles.Contains(0x138e);
            this.cbMemberRanksDelete.Checked = privilegeByRoles.Contains(0x138f);
            this.cbManageMembersView.Checked = privilegeByRoles.Contains(0x1389);
            this.cbManageMembersEdit.Checked = privilegeByRoles.Contains(0x138a);
            this.cbManageMembersDelete.Checked = privilegeByRoles.Contains(0x138b);
            this.cbMemberArealDistributionStatistics.Checked = privilegeByRoles.Contains(0x2718);
            this.cbUserIncreaseStatistics.Checked = privilegeByRoles.Contains(0x2719);
            this.cbMemberRanking.Checked = privilegeByRoles.Contains(0x2717);
            this.cbManageOrderView.Checked = privilegeByRoles.Contains(0xfa1);
            this.cbManageOrderDelete.Checked = privilegeByRoles.Contains(0xfa2);
            this.cbManageOrderEdit.Checked = privilegeByRoles.Contains(0xfa3);
            this.cbManageOrderConfirm.Checked = privilegeByRoles.Contains(0xfa4);
            this.cbManageOrderSendedGoods.Checked = privilegeByRoles.Contains(0xfa5);
            this.cbExpressPrint.Checked = privilegeByRoles.Contains(0xfa6);
            this.cbManageOrderRemark.Checked = privilegeByRoles.Contains(0xfa8);
            this.cbExpressTemplates.Checked = privilegeByRoles.Contains(0xfa9);
            this.cbShipper.Checked = privilegeByRoles.Contains(0xfaa);
            this.cbPaymentModes.Checked = privilegeByRoles.Contains(0x3ec);
            this.cbShippingModes.Checked = privilegeByRoles.Contains(0x3ed);
            this.cbOrderRefundApply.Checked = privilegeByRoles.Contains(0xfac);
            this.cbOrderReturnsApply.Checked = privilegeByRoles.Contains(0xfae);
            this.cbOrderReplaceApply.Checked = privilegeByRoles.Contains(0xfad);
            this.cbSaleTotalStatistics.Checked = privilegeByRoles.Contains(0x2711);
            this.cbUserOrderStatistics.Checked = privilegeByRoles.Contains(0x2712);
            this.cbSaleList.Checked = privilegeByRoles.Contains(0x2713);
            this.cbSaleTargetAnalyse.Checked = privilegeByRoles.Contains(0x2714);
            this.cbProductSaleRanking.Checked = privilegeByRoles.Contains(0x2715);
            this.cbProductSaleStatistics.Checked = privilegeByRoles.Contains(0x2716);
            this.cbGifts.Checked = privilegeByRoles.Contains(0x1f41);
            this.cbGroupBuy.Checked = privilegeByRoles.Contains(0x1f45);
            this.cbCountDown.Checked = privilegeByRoles.Contains(0x1f46);
            this.cbCoupons.Checked = privilegeByRoles.Contains(0x1f47);
            this.cbProductPromotion.Checked = privilegeByRoles.Contains(0x1f42);
            this.cbOrderPromotion.Checked = privilegeByRoles.Contains(0x1f43);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.Page.Request.QueryString["roleId"]))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                int.TryParse(this.Page.Request.QueryString["roleId"], out this.roleId);
                this.btnSet1.Click += new EventHandler(this.btnSet_Click);
                this.btnSetTop.Click += new EventHandler(this.btnSet_Click);
                if (!this.Page.IsPostBack)
                {
                    RoleInfo role = ManagerHelper.GetRole(this.roleId);
                    this.lblRoleName.Text = role.RoleName;
                }
                if (this.Page.Request.QueryString["Status"] == "1")
                {
                    this.ShowMsg("设置部门权限成功", true);
                }
                this.LoadData(this.roleId);
            }
        }

        private void PermissionsSet(int roleId)
        {
            string str = string.Empty;
            if (this.cbSummary.Checked)
            {
                str = str + 0x3e8 + ",";
            }
            if (this.cbSiteContent.Checked)
            {
                str = str + 0x3e9 + ",";
            }
            if (this.cbVotes.Checked)
            {
                str = str + 0x7d9 + ",";
            }
            if (this.cbShippingTemplets.Checked)
            {
                str = str + 0x3ee + ",";
            }
            if (this.cbExpressComputerpes.Checked)
            {
                str = str + 0x3ef + ",";
            }
            if (this.cbPictureMange.Checked)
            {
                str = str + 0x3f1 + ",";
            }
            if (this.cbProductTypesView.Checked)
            {
                str = str + 0xbc9 + ",";
            }
            if (this.cbProductTypesAdd.Checked)
            {
                str = str + 0xbca + ",";
            }
            if (this.cbProductTypesEdit.Checked)
            {
                str = str + 0xbcb + ",";
            }
            if (this.cbProductTypesDelete.Checked)
            {
                str = str + 0xbcc + ",";
            }
            if (this.cbManageCategoriesView.Checked)
            {
                str = str + 0xbcd + ",";
            }
            if (this.cbManageCategoriesAdd.Checked)
            {
                str = str + 0xbce + ",";
            }
            if (this.cbManageCategoriesEdit.Checked)
            {
                str = str + 0xbcf + ",";
            }
            if (this.cbManageCategoriesDelete.Checked)
            {
                str = str + 0xbd0 + ",";
            }
            if (this.cbBrandCategories.Checked)
            {
                str = str + 0xbd1 + ",";
            }
            if (this.cbManageProductsView.Checked)
            {
                str = str + 0xbb9 + ",";
            }
            if (this.cbManageProductsAdd.Checked)
            {
                str = str + 0xbba + ",";
            }
            if (this.cbManageProductsEdit.Checked)
            {
                str = str + 0xbbb + ",";
            }
            if (this.cbManageProductsDelete.Checked)
            {
                str = str + 0xbbc + ",";
            }
            if (this.cbInStock.Checked)
            {
                str = str + 0xbbd + ",";
            }
            if (this.cbManageProductsUp.Checked)
            {
                str = str + 0xbbe + ",";
            }
            if (this.cbManageProductsDown.Checked)
            {
                str = str + 0xbbf + ",";
            }
            if (this.cbProductUnclassified.Checked)
            {
                str = str + 0xbc2 + ",";
            }
            if (this.cbProductBatchUpload.Checked)
            {
                str = str + 0xbc4 + ",";
            }
            if (this.cbProductBatchExport.Checked)
            {
                str = str + 0xbd2 + ",";
            }
            if (this.cbSubjectProducts.Checked)
            {
                str = str + 0xbc3 + ",";
            }
            if (this.cbClientGroup.Checked)
            {
                str = str + 0x1b5f + ",";
            }
            if (this.cbClientNew.Checked)
            {
                str = str + 0x1b60 + ",";
            }
            if (this.cbClientSleep.Checked)
            {
                str = str + 0x1b62 + ",";
            }
            if (this.cbClientActivy.Checked)
            {
                str = str + 0x1b61 + ",";
            }
            if (this.cbMemberRanksView.Checked)
            {
                str = str + 0x138c + ",";
            }
            if (this.cbMemberRanksAdd.Checked)
            {
                str = str + 0x138d + ",";
            }
            if (this.cbMemberRanksEdit.Checked)
            {
                str = str + 0x138e + ",";
            }
            if (this.cbMemberRanksDelete.Checked)
            {
                str = str + 0x138f + ",";
            }
            if (this.cbManageMembersView.Checked)
            {
                str = str + 0x1389 + ",";
            }
            if (this.cbManageMembersEdit.Checked)
            {
                str = str + 0x138a + ",";
            }
            if (this.cbManageMembersDelete.Checked)
            {
                str = str + 0x138b + ",";
            }
            if (this.cbMemberArealDistributionStatistics.Checked)
            {
                str = str + 0x2718 + ",";
            }
            if (this.cbUserIncreaseStatistics.Checked)
            {
                str = str + 0x2719 + ",";
            }
            if (this.cbMemberRanking.Checked)
            {
                str = str + 0x2717 + ",";
            }
            if (this.cbManageOrderView.Checked)
            {
                str = str + 0xfa1 + ",";
            }
            if (this.cbManageOrderDelete.Checked)
            {
                str = str + 0xfa2 + ",";
            }
            if (this.cbManageOrderEdit.Checked)
            {
                str = str + 0xfa3 + ",";
            }
            if (this.cbManageOrderConfirm.Checked)
            {
                str = str + 0xfa4 + ",";
            }
            if (this.cbManageOrderSendedGoods.Checked)
            {
                str = str + 0xfa5 + ",";
            }
            if (this.cbExpressPrint.Checked)
            {
                str = str + 0xfa6 + ",";
            }
            if (this.cbExpressTemplates.Checked)
            {
                str = str + 0xfa9 + ",";
            }
            if (this.cbShipper.Checked)
            {
                str = str + 0xfaa + ",";
            }
            if (this.cbPaymentModes.Checked)
            {
                str = str + 0x3ec + ",";
            }
            if (this.cbShippingModes.Checked)
            {
                str = str + 0x3ed + ",";
            }
            if (this.cbSaleTotalStatistics.Checked)
            {
                str = str + 0x2711 + ",";
            }
            if (this.cbUserOrderStatistics.Checked)
            {
                str = str + 0x2712 + ",";
            }
            if (this.cbSaleList.Checked)
            {
                str = str + 0x2713 + ",";
            }
            if (this.cbSaleTargetAnalyse.Checked)
            {
                str = str + 0x2714 + ",";
            }
            if (this.cbProductSaleRanking.Checked)
            {
                str = str + 0x2715 + ",";
            }
            if (this.cbProductSaleStatistics.Checked)
            {
                str = str + 0x2716 + ",";
            }
            if (this.cbOrderRefundApply.Checked)
            {
                str = str + 0xfac + ",";
            }
            if (this.cbOrderReplaceApply.Checked)
            {
                str = str + 0xfad + ",";
            }
            if (this.cbOrderReturnsApply.Checked)
            {
                str = str + 0xfae + ",";
            }
            if (this.cbGifts.Checked)
            {
                str = str + 0x1f41 + ",";
            }
            if (this.cbGroupBuy.Checked)
            {
                str = str + 0x1f45 + ",";
            }
            if (this.cbCountDown.Checked)
            {
                str = str + 0x1f46 + ",";
            }
            if (this.cbCoupons.Checked)
            {
                str = str + 0x1f47 + ",";
            }
            if (this.cbProductPromotion.Checked)
            {
                str = str + 0x1f42 + ",";
            }
            if (this.cbOrderPromotion.Checked)
            {
                str = str + 0x1f43 + ",";
            }
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Substring(0, str.LastIndexOf(","));
            }
            ManagerHelper.AddPrivilegeInRoles(roleId, str);
            ManagerHelper.ClearRolePrivilege(roleId);
        }
    }
}

