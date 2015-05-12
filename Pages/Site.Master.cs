﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CubeReportingModule.Pages
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        public bool navBarVisible = true;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.FindControl("NavBarPlaceholder").Visible = navBarVisible;
        }
    }
}