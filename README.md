# Site-provisioning
The Sogeti Site Provisioning Suite helps Office 365 users create sites and site collection in an easy and consistent way. Via separation of activities on technical knowledge tasks and user capabilities are efficient positioned.

Three levels of technology are recognized in the Site Provisioning Suite.

1. The SharePoint Expert, who has knowledge about SharePoint capabilities and features. Experienced for making a PNP based SharePoint site template.
2.	The SharePoint user, knowledgeable about the site or site collection needs of the business. This user will prepare specific site templates for business scenario’s (based on a PNP based SharePoint site template). 
3.	The Business user, knows when a specific site or site collection is needed. Selects it from the available template list and creates site (collections) simply by giving it a name. 

![alt text](https://github.com/sogeti/Site-provisioning/blob/master/Resources/SogetiSP.png "Sogeti Site Provisioning")


Every role has specific knowledge and activities which makes the provisioning efficient.

# Technology.

The provisioning engine is based on the PnP Provisioning solution on GitHub. It adopts the best practice and future proof solution of remote site provisioning via SharePoint CSOM. 

The implementation is based on the provider hosted app (add-in) solution. Which is hosted as an Azure Web App. PNP Site templates (made by the expert, 1) are uploaded in Azure DocumentDB and available for the SharePoint user, 2 for rights, policy, looks and other business specific customizations. This Business specific site template is also stored in DocumentDB. Files which come with the Business specific site template are store in an Azure blob storage.

Business users, 3 in need for a site (collection) select the business specific site template and create request. The request is put in an Azure Storage Queue which triggers an Azure WebJob for processing. 
The WebJob does the heavy lifting and makes for most provisioning functionality use of the PNP Core provisioning module. 

Creation progress is reported back to the business user on the O365 portal via SignalR, provisioning steps are also stored in an Azure Table storage for logging information.

For a consistent user experience Office365 UI Fabric is used.

Next to the business user role there is also a public facing REST API for external systems to use the site provisioning engine.

GitHub.
The sources for this solution are available on GitHub for use under the MIT license (http://choosealicense.com/ ), free to use with attribution to Sogeti.

The solution isn’t finished we will continue to contribute on it.
