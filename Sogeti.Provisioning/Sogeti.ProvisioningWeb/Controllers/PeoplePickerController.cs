using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.UI.WebControls;
using Microsoft.SharePoint.ApplicationPages.ClientPickerQuery;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Utilities;
using Newtonsoft.Json;
using Sogeti.ProvisioningWeb.Models;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Microsoft.Online.SharePoint.TenantManagement;
using Sogeti.Provisioning.Domain;

namespace Sogeti.ProvisioningWeb.Controllers
{
    public class PeoplePickerController : Controller
    {
        private static int GroupID = -1;

        [SharePointContextFilter]
        [HttpPost]
        public ActionResult Find(string id)
        {
            string sPGroupName = "";
            
            try
            {
                
                var spContext = SharePointContextProvider.Current.GetSharePointContext(HttpContext);
                var clientContext = spContext.CreateUserClientContextForSPHost();
                var returnValue = GetPeoplePickerSearchData(id, sPGroupName, clientContext);
                return Json(new  {status = 200, success = true, data = returnValue});
            }
            catch (Exception e)
            {
                return Json(new { Success = false, Message = e.Message });
            }
        }

        public static string GetPeoplePickerSearchData(string SearchString, string SPGroupName, ClientContext context)
        {
            using (context)
            {
                //get searchstring and other variables
                var searchString = SearchString;
                int principalType = 1;
                string spGroupName = SPGroupName;

                ClientPeoplePickerQueryParameters queryParams = new ClientPeoplePickerQueryParameters();
                queryParams.AllowMultipleEntities = false;
                queryParams.MaximumEntitySuggestions = 2000;
                queryParams.PrincipalSource = PrincipalSource.All;
                queryParams.PrincipalType = (PrincipalType) principalType;
                queryParams.QueryString = searchString;

                if (!string.IsNullOrEmpty(spGroupName))
                {
                    if (GroupID == -1)
                    {
                        var group = context.Web.SiteGroups.GetByName(spGroupName);
                        if (group != null)
                        {
                            context.Load(group, p => p.Id);
                            context.ExecuteQuery();

                            GroupID = group.Id;

                            queryParams.SharePointGroupID = group.Id;
                        }
                    }
                    else
                    {
                        queryParams.SharePointGroupID = GroupID;
                    }
                }

                //execute query to Sharepoint
                ClientResult<string> clientResult =
                    Microsoft.SharePoint.ApplicationPages.ClientPickerQuery.ClientPeoplePickerWebServiceInterface
                        .ClientPeoplePickerSearchUser(context, queryParams);
                context.ExecuteQuery();

                return clientResult.Value;
                
            }
        }

        public static void FillPeoplePickerValue(HiddenField peoplePickerHiddenField, Microsoft.SharePoint.Client.User user)
        {
            List<O365User> O365Users = new List<O365User>(1);
            O365Users.Add(new O365User() { Name = user.Title, Email = user.Email, Login = user.LoginName });
            peoplePickerHiddenField.Value = JsonConvert.SerializeObject(O365Users);
        }

        public static void FillPeoplePickerValue(HiddenField peoplePickerHiddenField, Microsoft.SharePoint.Client.User[] users)
        {
            List<O365User> O365Users = new List<O365User>();
            foreach (var user in users)
            {
                O365Users.Add(new O365User() { Name = user.Title, Email = user.Email, Login = user.LoginName });
            }
            peoplePickerHiddenField.Value = JsonConvert.SerializeObject(O365Users);
        }

        public static List<O365User> GetValuesFromPeoplePicker(HiddenField peoplePickerHiddenField)
        {
            return JsonConvert.DeserializeObject<List<O365User>>(peoplePickerHiddenField.Value);
        }
    }
}