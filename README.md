# Tanka

[![Build status]
(https://ci.appveyor.com/api/projects/status/28080adn5nb7yxcc)]
(https://ci.appveyor.com/project/pekkah/tanka)

Prototype blogging engine with few twists


## Installation

Clone the repository
```
git clone https://github.com/pekkah/tanka.git
```

Restore packages by building the solution.

Update Web.config with your RavenDb connection string
```
<connectionStrings>
  <add name="RavenDB" connectionString="Url=http://localhost:8080;database=tanka" />
</connectionStrings>
```

Update Default.config
```
<!-- this is used during the first run setup admin user -->
<add key="tanka/installer/key" value="MyCuteLittleBlog" />

<!-- Your css style file -->
<add key="tanka/theme" value="/Content/themes/default/bootstrap.css" />

<!-- Code highlighter style file -->
<add key="tanka/hljs-theme" value="/Content/highlight/xcode.css" />

<!-- Your disqus short name -->
<add key="disqus/shortname" value="your-disqus" />``` 
```

Run the Web and setup admin user. Note that the redirection to https endpoint
does not work on IIS express so you need to manually type the correct address.
Default https://localhost:44301/installer


