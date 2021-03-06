﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

namespace WMTA.Reporting
{
    public partial class BadgerChairFeeSummary : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                checkPermissions();

                loadYearDropdown();

                loadTeacherDropdown();
            }
        }

        /*
         * Pre:
         * Post: If the user is not logged in they will be redirected to the welcome screen
         */
        private void checkPermissions()
        {
            //if the user is not logged in, send them to login screen
            if (Session[Utility.userRole] == null)
                Response.Redirect("../Default.aspx");

            //if the user tries to access without permission, send them to login screen
            else
            {
                User user = (User)Session[Utility.userRole];

                if (!(user.permissionLevel.Contains("A") || user.permissionLevel.Contains("S")))
                    Response.Redirect("../Default.aspx");
            }
        }

        /*
         * Pre:
         * Post: Loads the appropriate years in the dropdown
         */
        private void loadYearDropdown()
        {
            int firstYear = DbInterfaceStudentAudition.GetFirstAuditionYear();

            for (int i = DateTime.Now.Year + 1; i >= firstYear; i--)
                ddlYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
        }

        /*
         * Pre:
         * Post: If the current user is a teacher, the teacher dropdown
         *       should only show the current user
         */
        private void loadTeacherDropdown()
        {
            if (HighestPermissionTeacher())
            {
                User user = (User)Session[Utility.userRole];
                Contact contact = DbInterfaceContact.GetContact(user.contactId);

                if (contact != null)
                {
                    ddlTeacher.Items.Add(new ListItem(contact.lastName + ", " + contact.firstName, user.contactId.ToString()));
                }
            }
        }

        /*
         * Pre:
         * Post: Update the list of available teachers
         */
        private void updateTeacherDropdown()
        {
            ddlTeacher.DataSource = null;
            ddlTeacher.DataBind();
            ddlTeacher.Items.Clear();

            int year = Convert.ToInt32(ddlYear.SelectedValue);

            DataTable table = DbInterfaceContact.GetTeachersForBadgerEvent(year);

            if (table != null)
            {
                //add empty item
                ddlTeacher.Items.Add(new ListItem("", ""));

                //add teachers from district
                ddlTeacher.DataSource = table;

                ddlTeacher.DataTextField = "ComboName";
                ddlTeacher.DataValueField = "ContactId";

                ddlTeacher.DataBind();
            }
            else
            {
                showErrorMessage("Error: The teachers for the selected event could not be retrieved.");
            }
        }

        /*
         * Pre:
         * Post: Determines whether or not the current user's highest permission level is Teacher
         * @returns true if the current user's highest permission level is Teacher and false otherwise
         */
        private bool HighestPermissionTeacher()
        {
            User user = (User)Session[Utility.userRole];
            bool teacherOnly = false;

            if (user.permissionLevel.Contains('T') && !(user.permissionLevel.Contains('D') || user.permissionLevel.Contains('S') || user.permissionLevel.Contains('A')))
            {
                teacherOnly = true;
            }

            return teacherOnly;
        }

        /*
         * Pre:
         * Post: If an event matching the search criteria is found, execute
         *       the reports for that audition
         */
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //get selected teacher
            int teacherId = 0;
            if (ddlTeacher.SelectedIndex >= 0 && !ddlTeacher.SelectedValue.Equals(""))
            {
                teacherId = Convert.ToInt32(ddlTeacher.SelectedValue);
            }

            showInfoMessage("Please allow several minutes for your reports to generate.");

            createReport("BadgerChairFeeSummary", rptBCFeeSummary, ddlYear.SelectedValue, teacherId);
        }

        /*
         * Pre:
         * Post: Create the input report in the specified report viewer
         */
        private void createReport(string rptName, ReportViewer rptViewer, string year, int teacherId)
        {
            try
            {
                rptViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
                rptViewer.ToolBarItemBorderColor = System.Drawing.Color.Black;
                rptViewer.ToolBarItemBorderStyle = BorderStyle.Double;

                rptViewer.ServerReport.ReportServerCredentials = new ReportCredentials(Utility.ssrsUsername, Utility.ssrsPassword, Utility.ssrsDomain);

                rptViewer.ServerReport.ReportServerUrl = new Uri(Utility.ssrsUrl);
                rptViewer.ServerReport.ReportPath = "/wismusta/" + rptName + Utility.reportSuffix;

                //set parameters
                List<ReportParameter> parameters = new List<ReportParameter>();
                parameters.Add(new ReportParameter("year", year));
                parameters.Add(new ReportParameter("teacherId", teacherId.ToString()));

                rptViewer.ServerReport.SetParameters(parameters);

                rptViewer.AsyncRendering = true;
            }
            catch (Exception e)
            {
                showErrorMessage("Error: An error occurred while generating reports.");

                Utility.LogError("BadgerChairFeeSummary", "createReport", "rptName: " + rptName +
                                 ", year: " + year, "Message: " + e.Message + "   Stack Trace: " + e.StackTrace, -1);
            }
        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!HighestPermissionTeacher())
                updateTeacherDropdown();
        }

        #region Messages

        /*
         * Pre:
         * Post: Displays the input error message in the top-left corner of the screen
         * @param message is the message text to be displayed
         */
        private void showErrorMessage(string message)
        {
            lblErrorMessage.InnerText = message;

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ShowError", "showMainError()", true);
        }

        /*
         * Pre: 
         * Post: Displays the input warning message in the top left corner of the screen
         * @param message is the message text to be displayed
         */
        private void showWarningMessage(string message)
        {
            lblWarningMessage.InnerText = message;

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ShowWarning", "showWarningMessage()", true);
        }

        /*
         * Pre: 
         * Post: Displays the input informational message in the top left corner of the screen
         * @param message is the message text to be displayed
         */
        private void showInfoMessage(string message)
        {
            lblInfoMessage.InnerText = message;

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ShowInfo", "showInfoMessage()", true);
        }

        #endregion
    }
}