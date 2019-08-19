# NonProfitCRM

Simple CRM solution for non-profit sector originally developed for Hestia (hest.cz).
Main goal of solution is deliver open source platform (MIT license) for managing projects in Hestia for connecting commercial organizations and non-profit organizations and manage volunteers programs.

## Technical background
Solution is developed in .Net framework, Entity framework and uses Azure SQL database for data persistence.

## Roles / Users
Solution is using AAD roles/users, check these links for more informations:

* Create AAD application: https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-v2-aspnet-webapp
* Create Application roles for app: https://docs.microsoft.com/en-us/azure/architecture/multitenant-identity/app-roles

Snipped of XML configuration for FRD roles:

```xml
"appRoles": [
  {
    "allowedMemberTypes": [
      "User"
    ],
    "description": "Can manage customers, non-profit orgs and events.",
    "displayName": "FRD",
    "id": "1b4f816e-6eaf-48b9-8623-7923835634ad",
    "isEnabled": true,
    "value": "FRD"
  },
  {
    "allowedMemberTypes": [
      "User"
    ],
    "description": "Administrators can manage templates and search logs.",
    "displayName": "FRD_SYSTEM_ADMINISTRATOR",
    "id": "c20e145e-5759-4a6c-a174-b9407453cfe1",
    "isEnabled": true,
    "value": "FRD_SYSTEM_ADMINISTRATOR"
  }
],
```

## License

```
MIT License
	

	Copyright (c) 2019 Azure CZ
	

	Permission is hereby granted, free of charge, to any person obtaining a copy
	of this software and associated documentation files (the "Software"), to deal
	in the Software without restriction, including without limitation the rights
	to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
	copies of the Software, and to permit persons to whom the Software is
	furnished to do so, subject to the following conditions:
	

	The above copyright notice and this permission notice shall be included in all
	copies or substantial portions of the Software.
	

	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
	OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
	SOFTWARE.
```
