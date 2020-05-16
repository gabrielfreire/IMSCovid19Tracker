# IMS COVID-19
This is a simple COVID-19 mobile app available for iOS and Android

![App Images](Assets/app.jpg)

# Installation

Open the following links from your phone to install the app:
- **iOS (from v12.4)**: https://install.appcenter.ms/orgs/imsmaxims/apps/ims-covid-19-tracker-ios/distribution_groups/allpublic
- **Android (from v5)**: https://install.appcenter.ms/orgs/imsmaxims/apps/ims-covid-19-tracker-android/distribution_groups/allpublic
[Click here to install the App](https://appcenter.ms/orgs/imsmaxims/applications)

You should now be able to download the app directly onto your mobile

# Release Notes
  - **1.0.0** - First Release
  - **1.0.1** 
    - Fix bug where query for U.S.A provinces were incomplete
    - Change Death icon to something more sensitive
  - **1.0.2** 
    - Added auto-complete on search page
    - Fix bug in country search
  - **2.0.0** 
    - Include dynamic versioning in About page
    - Make main page country panels as editable widgets
  - **2.0.1** 
    - Fix collection bug on refresh
  - **2.0.2** 
    - Fix bug where widget values do not update properly
  - **2.0.3** 
    - Swipe left in a widget to display it bigger in a second page
    - Reset search criteria when leaving Search Page
  - **2.0.4** 
    - Bug fix where search box would crash app sometimes
  - **2.0.5** 
    - Fix small flickering when poping a page (Android only)
  - **2.1.0** 
    - More data and statistics being displayed
    - Added Donut Chart and Bar Chart on Widget Detail view
    - Added small tutorial toast to explain widget usage
    - Add "help" button to display widget tutorial
    - Add Sources information in the About page
    - Get total population, population per country and new calculation on deaths per million
  - **2.1.1** 
    - Fix bug where the population for US is wrong
  - **2.1.2** 
    - Fix population bugs and improve population by country API.
  - **2.1.3** 
    - Change country population API.
  - **2.2.0** 
    - Overall UI clean up and performance improvements.
    - Small country widgets are bigger and now have slide indicators and the country flag.
    - Got rid of third party country info/covid APIs and created new web scraper APIs
    - Remove web scraping code from Mobile sources and move it to a Web API server deployed in Azure.
  - **2.2.1**
    - Android performance improvements ( [github issue #1](https://github.com/gabrielfreire/IMSCovid19Tracker/issues/1) ) 
    - Fix Widget reload to default on refresh or closing/opening app when no widget was registered ( [github issue #3](https://github.com/gabrielfreire/IMSCovid19Tracker/issues/3) ) 
    - Enhancement where help shows on every launch, now it shows only on first launch ( [github issue #2](https://github.com/gabrielfreire/IMSCovid19Tracker/issues/2) ) 
    - Use Shell navigation ( [github issue #5](https://github.com/gabrielfreire/IMSCovid19Tracker/issues/5) ) 
    - View reference in ViewModel ( [github issue #4](https://github.com/gabrielfreire/IMSCovid19Tracker/issues/4) ) 
  - **2.2.2**
    - Small *(big)* improvements - Merged from @domagojmedo [PR #6](https://github.com/gabrielfreire/IMSCovid19Tracker/pull/6)
  - **2.2.3**
    - UI Improvements, image optimization and performance improvements in general
    - Better error handling

# License
IMS COVID-19 is MIT Licensed