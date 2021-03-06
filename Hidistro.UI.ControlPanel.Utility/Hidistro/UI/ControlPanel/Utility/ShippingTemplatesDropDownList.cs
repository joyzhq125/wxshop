﻿namespace Hidistro.UI.ControlPanel.Utility
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Core;
    using System;
    using System.Data;
    using System.Globalization;
    using System.Web.UI.WebControls;

    public class ShippingTemplatesDropDownList : DropDownList
    {
        private bool allowNull = true;
        private string nullToDisplay = "";

        public override void DataBind()
        {
            this.Items.Clear();
            if (this.AllowNull)
            {
                base.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
            }
            foreach (DataRow row in SalesHelper.GetShippingAllTemplates().Rows)
            {
                this.Items.Add(new ListItem(Globals.HtmlDecode(row["TemplateName"].ToString()), row["TemplateId"].ToString()));
            }
        }

        public bool AllowNull
        {
            get
            {
                return this.allowNull;
            }
            set
            {
                this.allowNull = value;
            }
        }

        public string NullToDisplay
        {
            get
            {
                return this.nullToDisplay;
            }
            set
            {
                this.nullToDisplay = value;
            }
        }

        public int? SelectedValue
        {
            get
            {
                if (string.IsNullOrEmpty(base.SelectedValue))
                {
                    return null;
                }
                return new int?(int.Parse(base.SelectedValue, CultureInfo.InvariantCulture));
            }
            set
            {
                if (value.HasValue)
                {
                    base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.Value.ToString(CultureInfo.InvariantCulture)));
                }
            }
        }
    }
}

