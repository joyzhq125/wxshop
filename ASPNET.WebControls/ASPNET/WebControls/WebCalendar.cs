namespace ASPNET.WebControls
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class WebCalendar : TextBox
    {
        public WebCalendar()
        {
            this.CalendarType = ASPNET.WebControls.CalendarType.Default;
            this.BeginYear = 0x7c6;
            this.EndYear = 0x7e4;
            this.CalendarLanguage = ASPNET.WebControls.CalendarLanguage.zh_CN;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Attributes.Add("readonly", "readonly");
            base.Attributes.Add("onclick", string.Format("new Calendar({0}, {1}, {2}).show(this);", this.BeginYear, this.EndYear, (int) this.CalendarLanguage));
            string script = string.Format("<script src=\"{0}\" type=\"text/javascript\"></script>", this.Page.ClientScript.GetWebResourceUrl(base.GetType(), "ASPNET.WebControls.Calendar.calendar.js"));
            if (!this.Page.ClientScript.IsStartupScriptRegistered(base.GetType(), "WebCalendarScript"))
            {
                this.Page.ClientScript.RegisterStartupScript(base.GetType(), "WebCalendarScript", script, false);
            }
            base.Render(writer);
        }

        public int BeginYear { get; set; }

        public ASPNET.WebControls.CalendarLanguage CalendarLanguage { get; set; }

        public ASPNET.WebControls.CalendarType CalendarType { get; set; }

        public int EndYear { get; set; }

        public DateTime? SelectedDate
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Text))
                {
                    DateTime time;
                    string text = this.Text;
                    if (this.CalendarType == ASPNET.WebControls.CalendarType.StartDate)
                    {
                        text = text + " 00:00:00";
                    }
                    else if (this.CalendarType == ASPNET.WebControls.CalendarType.EndDate)
                    {
                        text = text + " 23:59:59";
                    }
                    if (DateTime.TryParse(text, out time))
                    {
                        return new DateTime?(time);
                    }
                }
                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.Text = value.Value.ToString("yyyy-MM-dd");
                }
            }
        }
    }
}

