# Sitecore Experience Analytics Table Control Readme

> Note: This module has been tested on Sitecore 8.2+. If you find this does not work on prior versions, please submit an Issue, and I will try to add support.  You can also submit a pull request.

## Overview

Sitecore Experience Analytics reporting supports a list control that renders tabular data.  The data includes the **Key** along with *Visits, Value per visit, Average duration, Bounce Rate, Conversion Rate, and Page views per visit*.  Only standard analytics data can be displayed in the out of the box list control.  The columns are fixed and cannot be configured.

We had a requirement to customize the columns in the report.  We also had the requirement to display non-standard analytics data.  I did a bunch of research and did not find very many examples of how to achieve this.  I did find the following article: [Custom Dashboard Reports](https://community.sitecore.net/technical_blogs/b/integration_solution_team_blog/posts/custom-dashboard-reports).  It explains how to display aggregated data in a custom list. This is exactly what we needed.  The article was very helpful, but did not provide a working example.  I ended up needing to reflect into the [Sitecore Media Framework 2.1](https://dev.sitecore.net/Downloads/Sitecore_Media_Framework/21/Sitecore_Media_Framework_21.aspx) to find the missing pieces.

In an effort to help others, I am posting the complete and fully working code that we used to get this working.  I tweaked the code from the original [post](https://community.sitecore.net/technical_blogs/b/integration_solution_team_blog/posts/custom-dashboard-reports) to support using configurable types instead of SQL to provide the data.  This comes in handy if you want to display results from the **sitecore_analytics_index** Lucene index.

You can follow the original blog [post](https://community.sitecore.net/technical_blogs/b/integration_solution_team_blog/posts/custom-dashboard-reports) for an explanation of how this was built.  The *Sitecore Media Framework 2.1* that this example was based on is located here: [Sitecore Media Framework 2.1](https://dev.sitecore.net/Downloads/Sitecore_Media_Framework/21/Sitecore_Media_Framework_21.aspx).

![Basic Sample](https://raw.github.com/onenorth/sitecore-experience-analytics-table-control/master/img/basic-sample.png)

### Features

The Experience Analytics Table Control supports rendering custom data within the Sitecore Experience Analytics dashboard.  The data is displayed in tabular form.  The columns in the table are customizable.  The look and feel matches the existing List Control.  The data is provided via configurable types / methods.  The methods provide the data that is displayed in the table.

## Installation

Install the update package located here: https://github.com/onenorth/sitecore-experience-analytics-table-control/releases

> NOTE: You can include the source code directly in your solution.  This allows for further customizations.

## Configuration

The Sitecore Experience Analytics Table Control has a single configuration file **z.OneNorth.ExperienceAnalyticsTableControl.config**.  This file needs to be placed in the App_Config/Include folder. The primary purpose of the file is to register the MVC route used for the web service that provides the data to the Experience Analytics Table Control.

> Note: This configuration file must be placed after the *Sitecore.ExperienceAnalytics* config files for the route to be setup correctly.

## Samples / Examples

Samples that demonstrate usage are automatically installed if you installed the update package.  The samples appear in the Sitecore Experience Analytics Dashboard under **Table Control Samples**.

![Basic Sample](https://raw.github.com/onenorth/sitecore-experience-analytics-table-control/master/img/navigation.png)

## Usage

> NOTE: In general you should be using Sitecore Rocks to configure SPEAK Components.  All of the following examples use Sitecore Rocks. 

### ExperienceAnalyticsTableControl

The **ExperienceAnalyticsTableControl** is used in a similar fashion to the other Experience Analytics controls.  The control is defined in the core database under *sitecore/client/Applications/ExperienceAnalytics/Common/Layouts/Renderings*.  It has a Parameters template that defines the various settings that the control requires.

![Experience Analytics Table Control](https://raw.github.com/onenorth/sitecore-experience-analytics-table-control/master/img/experience-analytics-table-control.png)

### Layout

The **ExperienceAnalyticsTableControl** is included in layouts using Add Rendering.  Note, you can find the control by searching for it by name.  Once added to a layout, configure the **Placeholder** and define a **Data Source**.

![Layout](https://raw.github.com/onenorth/sitecore-experience-analytics-table-control/master/img/layout.png)

### DataSource

Create a Data Source item based on the **ExperienceAnalyticsTableControl Parameters** template.  Place this item under the Dashboard Page / **PageSettings**.  Specify the Type and Method that will be used to provide the data to the Table.  We will setup the Type and Method in the **Code** section below

![Data Source](https://raw.github.com/onenorth/sitecore-experience-analytics-table-control/master/img/datasource.png)

Define the columns that you want in the table and specify the order.  The columns are defined by adding child items under the DataSource item.  The child items are of type **ColumnField**. The insert options have been already setup to support this.  The **HeaderText** and **DataField** should be set appropriately.  The **DataField** value should match the property names returned by the type/method.  It is case sensitive.

![Columns](https://raw.github.com/onenorth/sitecore-experience-analytics-table-control/master/img/columns.png)

### Code

Now that we have the Items setup in Sitecore, we can code up our logic to return the data.  The return type of the method should be **ExperienceAnalyticsTableControlResponse**.  The actual data is contained within a **ExperienceAnalyticsTableControlData** collection.

**SampleBasicDataSource**
``` C#
using OneNorth.ExperienceAnalyticsTableControl.Api;
using System;

namespace OneNorth.ExperienceAnalyticsTableControl.DataSources
{
    public class SampleBasicDataSource
    {
        public ExperienceAnalyticsTableControlResponse Get(DateTime dateFrom, DateTime dateTo, string siteName)
        {
            var reportData = new ExperienceAnalyticsTableControlData<dynamic>();

            var random = new Random();

            var count = 10;
            for (var i = 0; i < count; i++)
            {
                var item = new
                {
                    index = i,
                    id = Guid.NewGuid().ToString(),
                    datefrom = dateFrom.ToShortDateString(),
                    dateto = dateTo.ToShortDateString(),
                    sitename = siteName,
                    random = random.Next(0, 1000)
                };

                reportData.AddItem(item);
            }
            var content = new ExperienceAnalyticsTableControlResponse()
            {
                Data = reportData,
                TotalRecordCount = count
            };

            return content;
        }
    }
}

```

The above code is just a sample.  Logic can be provided to retrieve data from various sources such as Lucene indexes or SQL databases.  The order of the Get method parameters does not matter.  The names of the parameters do matter.  The names are exact matched against **dateFrom**, **dateTo**, and **siteName**.  Additional parameters can be provided which will be populated from the **Parameters** field of the Data Source.  The parameter name must match the key in the parameters field.  You can take a look a the **Parameter** sample for an example.

The framework and logic can be extended to take data input from other sources such as other controls in the Layout.  This will require customizations to the Table Control.

## Development

To customize the Table Control, pull down the source code.  All Sitecore references come from the [Sitecore public NuGet package](https://doc.sitecore.net/sitecore_experience_platform/developing/developing_with_sitecore/sitecore_public_nuget_packages_faq) repository.  The appropriate NuGet feed needs to be added to Visual Studio for the project to compile properly

In order to generate the .update packages, TDS needs to be updated to reference the location of the Sitecore assemblies.
To do this, open the **Properties** of the **TDS.Core** project.
In the properties, navigate to the **Update Package** tab.
Enter the location of the bin folder for the Sitecore installation in the **Sitecore Assembly Path** field. 

#License

The associated code is released under the terms of the [MIT license](http://onenorth.mit-license.org).

The Sitecore Experience Analyzer Table Control was inspired by and has used code from:
* https://community.sitecore.net/technical_blogs/b/integration_solution_team_blog/posts/custom-dashboard-reports
* https://dev.sitecore.net/Downloads/Sitecore_Media_Framework/21/Sitecore_Media_Framework_21.aspx

