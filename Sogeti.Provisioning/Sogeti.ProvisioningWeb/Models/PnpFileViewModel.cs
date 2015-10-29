using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using OfficeDevPnP.Core.Framework.Provisioning.Providers.Xml;
using Sogeti.Provisioning.Domain;

namespace Sogeti.ProvisioningWeb.Models
{
    public class PnpFileViewModel : PnpFile
    {

       public class ValidateFileAttribute : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                var allowedFileExtensions = new[] { ".xml" };

                var file = value as HttpPostedFileBase;

                if (file == null)
                    return false;
                else if (!allowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
                {
                    ErrorMessage = "Please upload file of type: " + string.Join(", ", allowedFileExtensions);
                    return false;
                }
                else
                    return true;
            }

        }


        public IEnumerable<SelectListItem> PnpFileList
        {
            get
            {
                var result = new List<SelectListItem>
                {
                    new SelectListItem {Value = "0", Text = "[Choose template or upload a new one]"}
                };

                result.AddRange(PnpFiles.Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.Filename
                }));

                return result;
            }
        }

        public IEnumerable<PnpFile> PnpFiles { get; set; }

        [Display(Name = "Site Templates File")]
        public string SelectedPnpFile { get; set; }

        [Display(Name = "Template File")]
        [ValidateFile]
        public HttpPostedFileBase file { get; set; }

        public bool PnpFileValid { get; set; }
        public string PnpFileError { get; set; }

        public PnpFile ToPnpFile()
        {
            var returnValue = new PnpFile()
            {
                Id = Id,
                Filename = file.FileName,
                PnpTemplate = ToPnpProvisioningTemplate()
            };

            return returnValue;
        }

        private ProvisioningTemplate PnpProvisioningTemplate { get; set; }



        public string IsTemplateValid()
        {
            var schemaFormatter = new XMLPnPSchemaFormatter();
            try
            {
                if (schemaFormatter.IsValid(file.InputStream))
                {
                    TemplateValid = true;
                    TemplateError = null;
                }
            }
            catch (Exception e)
            {
                TemplateError = e.Message;
                return e.Message;
            }

            return "OK";
        }

        public bool TemplateValid { get; set; }

        public string TemplateError { get; set; }


        private ProvisioningTemplate ToPnpProvisioningTemplate()
        {
            
                try
                {
                    var schemaFormatter = new XMLPnPSchemaFormatter();
                    if (schemaFormatter.IsValid(file.InputStream))
                    {
                        this.TemplateValid = true;
                        this.TemplateError = null;
                        var pnpProvisioningTemplate = schemaFormatter.ToProvisioningTemplate(file.InputStream);

                        return pnpProvisioningTemplate;
                    }
                    else
                    {
                        this.TemplateValid = false;
                        return null;
                    }
                }
                catch (Exception e)
                {
                    this.TemplateError = e.Message;
                    return null;
                }

            }
        
    }
}
