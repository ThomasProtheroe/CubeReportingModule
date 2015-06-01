﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CubeReportingModule.Models;
using System.Web.Security;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace CubeReportingModule.Pages
{
    public partial class ManageReports : System.Web.UI.Page
    {
        Repository repo = new Repository();

        protected void Page_Load(object sender, EventArgs e)
        {
            //MembershipUser currentUser = Membership.GetUser();
            //string username = currentUser.UserName;

            //if ((Roles.IsUserInRole("Admin")) || (Roles.IsUserInRole("SysAdmin")))
            //{
            //    Display.AutoGenerateDeleteButton = true;
            //}
        }

        public bool SetModifyVisibility(string creator)
        {
            if ((Roles.IsUserInRole("Admin")) || (Roles.IsUserInRole("SysAdmin")))
            {
                return true;
            }

            return false;

            //if (creator == null)
            //{
            //    return false;
            //}

            //string currentUsername = Membership.GetUser().UserName;

            //bool visible = creator.Equals(currentUsername);

            //return visible;
        }

        public bool SetDeleteVisibility(string creator)
        {
            if ((Roles.IsUserInRole("Admin")) || (Roles.IsUserInRole("SysAdmin")))
            {
                return true;
            }

            if (creator == null)
            {
                return false;
            }

            string currentUsername = Membership.GetUser().UserName;

            bool visible = creator.Equals(currentUsername);

            return visible;
        }

        public IQueryable<Report> GetReportsAsQuery()
        {
            IQueryable<Report> allReports = repo.Reports.AsQueryable<Report>().OrderBy(report => report.Name);
            return allReports;
        }

        public IEnumerable<Report> GetReports()
        {
            IEnumerable<Report> allReports = repo.Reports.OrderBy(report => report.Name);
            return allReports;
        }

        protected void Display_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Display.PageIndex = e.NewPageIndex;
            DataBind();
        }

        // The id parameter name should allMatches the DataKeyNames value set on the control
        public void Display_UpdateItem(int? id)
        {
            if (id == null)
            {
                return;
            }

            AppContext db = new AppContext();
            Report toModify = db.Reports.Where(report => report.ReportId == id).FirstOrDefault();
            IEnumerable<GRAReportOption> allOptions = db.GRAReportOptions.Where(option => option.ReportId == id);

            List<string> keywords = GetTablesInFromClause(toModify.FromClause);

            //debug
            string fakeFrom = @"Org_Company Join AccessLogs join Reports 1=1";
            Debug.WriteLine("From: " + fakeFrom);
            BreakStringOnKeywords(fakeFrom, keywords);
            //end debug

            //Session["Step"] = 2;
            //Session["ReportName"] = toModify.Name;
            //Session["TableNames"];
            //Session["ColumnNames"] = toModify.SelectClause;
            //Session["Options"];
            //Session["Restrictions"];

            //Response.Redirect("CreateReport.aspx");
        }

        private List<string> GetColumnsInSelectClause(string selectClause)
        {
            List<string> keywords = new List<string>();
            keywords.Add(", ");

            List<string> allTableNames = BreakStringOnKeywords(selectClause, keywords);
            return allTableNames;
        }

        private List<string> GetTablesInFromClause(string fromClause)
        {
            List<string> keywords = new List<string>();
            keywords.Add(" join ");
            keywords.Add(" 1=1");

            List<string> allTableNames = BreakStringOnKeywords(fromClause, keywords);
            return allTableNames;
        }

        // The id parameter name should allMatches the DataKeyNames value set on the control
        public void Display_DeleteItem(int? id)
        {
            if (id == null)
            {
                return;
            }

            AppContext db = new AppContext();
            Report toDelete = db.Reports.Where(report => report.ReportId == id).FirstOrDefault();
            db.Reports.Remove(toDelete);
            db.SaveChanges();
        }

        protected void Modify_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            int reportId = Convert.ToInt32(Server.HtmlDecode(button.CommandArgument));
            Display_UpdateItem(reportId);
        }

        protected void Delete_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            int reportId = Convert.ToInt32(Server.HtmlDecode(button.CommandArgument));
            Display_DeleteItem(reportId);
        }

        protected List<string> BreakStringOnKeywords(string input, List<string> allKeywordsToIgnoreList)
        {
            Queue<string> allKeywordsToIgnoreQueue = new Queue<string>(allKeywordsToIgnoreList);
            string regex = GetRegex(allKeywordsToIgnoreQueue);
            Debug.WriteLine("Regex: " + regex);

            //string regex = @"([\w\-]+)(?: join | 1=1|$)";
            MatchCollection allMatches = Regex.Matches(input, regex, RegexOptions.IgnoreCase);

            if (allMatches.Count == 0)
            {
                return null;
            }

            List<string> fragments = new List<string>();
            int tableNumber = 1;

            foreach (Match match in allMatches)
            {
                string value = match.Groups[1].Value;
                Debug.WriteLine("Table {0}: ({1})", tableNumber, value);
                tableNumber++;
                fragments.Add(value);
            }

            return fragments;
        }

        private static string GetRegex(Queue<string> allKeywordsToIgnore)
        {
            string regex = @"([\w\-]+)(?:";
            if (allKeywordsToIgnore.Count == 0)
            {
                regex += @"$)";
                return regex;
            }

            string keyword = allKeywordsToIgnore.Dequeue();
            regex += keyword;

            while (allKeywordsToIgnore.Count > 0)
            {
                keyword = allKeywordsToIgnore.Dequeue();
                regex += @"|" + keyword;
            }
            regex += @"|$)";
            return regex;
        }
    }
}