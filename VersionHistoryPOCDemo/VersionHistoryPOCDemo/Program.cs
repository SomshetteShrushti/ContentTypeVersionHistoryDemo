using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using System.Net;
using Microsoft.SharePoint;

namespace VersionHistoryPOCDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //GetVersionHIstory();
            EnableandAddContentTypes();
        }

        private static void GetVersionHIstory()
        {
            ClientContext clientContext = new ClientContext("http://hr.zubaircorp.com/");
            clientContext.Credentials = new NetworkCredential("spqaadmin", "v%a&0t$17", "zubaircorp");
            List olist = clientContext.Web.Lists.GetByTitle("employeebasic");

            olist.EnableVersioning = false;

            // Minor Versioning
            olist.EnableMinorVersions = true;

            
            olist.Update();
            clientContext.ExecuteQuery();
            
        }

        private static void EnableandAddContentTypes()
        {
            using (ClientContext clientContext = new ClientContext("http://hr.zubaircorp.com/"))
            {
                clientContext.Credentials = new NetworkCredential("spqaadmin", "v%a&0t$17", "zubaircorp");
                Web web = clientContext.Web;
                List myLib = web.Lists.GetByTitle("Documents");
                clientContext.Load(myLib, lib => lib.ContentTypesEnabled);
                clientContext.ExecuteQuery();
                // check if contenttype is enabled if not then enable and update the list or Library
                if (!myLib.ContentTypesEnabled)
                {
                    myLib.ContentTypesEnabled = true;
                    myLib.Update();
                    clientContext.ExecuteQuery();
                }
                // Load content types from the rootweb
                ContentTypeCollection contentTypes = clientContext.Site.RootWeb.ContentTypes;
                clientContext.Load(contentTypes);
                clientContext.ExecuteQuery();
                // Add the exixting content type to the list and then update the list
                ContentType ctype = contentTypes.Where(c => c.Name == "Wiki Page").First();
                myLib.ContentTypes.AddExistingContentType(ctype);
                myLib.Update();
                clientContext.ExecuteQuery();

                Console.ReadKey();
            }
        }
    }
}
