﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using CubeReportingModule.Cache;

namespace CubeReportingModule.Pages
{
    public partial class Login : System.Web.UI.Page
    {
        MembershipCreateStatus status;

        protected void Page_Load(object sender, EventArgs e)
        {
            Membership.CreateUser("Bob", "password!", "Email@email.com", "Really?", "yes really", true, out status);
        }
    }
}