# Passle Sync for Umbraco

Passle Sync is a plugin for Umbraco which syncs your [Passle](https://home.passle.net/) posts and authors into your Umbraco instance.

Get started with the section below, or jump straight to the [API documentation](./docs/index.md).

A great example of how to use the plugin is our demo website:

- [📂 Website source](./PassleSync.Website/)
- [🌍 Live demo](http://mercierandveleztalkingpoints.com/)

## 🚀 Getting started

Get started by installing the plugin and activating it.

Once the plugin is installed, admin users can access the settings under **Settings > Passle Sync**.

### ⚙️ Configuration

On the first tab of the plugin settings page, you will find the following configuration options:

| Option                  | Description                                                                                                    |
| ----------------------- | -------------------------------------------------------------------------------------------------------------- |
| Passle API Key          | The API key generated in the Passle dashboard, used to fetch content from Passle.                              |
| Plugin API Key          | The API key Passle should use when calling the plugin webhooks after content is updated.                       |
| Passle Shortcodes       | A comma-separated list of the shortcodes of the Passles you want to sync content from.                         |
| Post Permalink Prefix   | The prefix that will be used for post permalink URLs. This needs to match what is set in the Passle backend.   |
| Person Permalink Prefix | The prefix that will be used for person permalink URLs. This needs to match what is set in the Passle backend. |
| Post Parent Node ID     | The ID of the parent node under which all Passle posts will be created.                                        |
| Author Parent Node ID   | The ID of the parent node under which all Passle posts will be created.                                        |

### 📙 Basic Usage

Once the plugin has been configured correctly, posts and people can be synced using the **Posts** and **People** tabs under **Settings > Passle Sync**.

**1. Fetch from API**

First, the plugin has to fetch posts and authors from the Passle API. Use the **Fetch Passle Posts** and **Fetch Passle People** buttons to do so. Once the plugin has done the initial fetches from the API, the API responses will be cached, so if you reload the page, the posts and authors you have fetched will be remembered.

**2. Sync to Umbraco**

To sync the posts and authors to Umbraco, use the **Sync All Posts** and **Sync All People** buttons. This will create a new content node for each post/author using the document types created by the plugin. Once all posts/authors have been synced, their statuses will update. Synced posts and authors can be viewed, but not edited, under the root nodes specified in the plugin settings.

**3. Webhooks**

Whenever a post or author is updated through the Passle interface, the Passle backend will make a call to a webhook exposed by the plugin with the shortcode of the item that was updated, and the plugin will automatically re-sync that item.

**4. Document Type Templates**

To display Passle posts and authors, you should create templates associated with the Passle Post and Passle Author document types that the plugin creates automatically.

This plugin provides the [PassleHelperService](./docs/PassleSync.Core.Services.PassleHelperService.md) class, which can be accessed in your controller via DI. The service includes the methods `GetPosts` and `GetAuthors`, which provide new instances of the [PasslePostQuery]() and [PassleAuthorQuery]() classes. These allow easy access to filtering and paginating Passle posts and authors via Examine.

### 📰 Example Queries

Fetch the post featured on the Passle page:

```csharp
var featuredPost = _passleHelperService.GetPosts().FeaturedOnPasslePage(true).Execute().Items.FirstOrDefault();
```

Fetch all posts except the post featured on the Passle page, with 10 items per page:

```csharp
var currentPage = 1;

// The result includes the items, as well as pagination data
var queryResult = _passleHelperService.GetPosts().FeaturedOnPasslePage(false).WithCurrentPage(currentPage).WithItemsPerPage(10).Execute();
```

A full example can be found in our demo site's [HomePageController.cs](PassleSync.Website/Controllers/HomePageController.cs).

### 🤝 Helper Methods

The [PasslePost](./docs/PassleSync.Core.Models.Content.Umbraco.PasslePost.md) and [PassleAuthor](./docs/PassleSync.Core.Models.Content.Umbraco.PassleAuthor.md) models returned by the queries described above include various helper methods in addition to all the properties contained in the document type. Here are some examples:

```csharp
var featuredPost = _passleHelperService.GetPosts().FeaturedOnPasslePage(true).Execute().Items.FirstOrDefault();

// Accepts a standard .NET datetime formatting string
var formattedDate = featuredPost.GetDate("d MMMM yyyy");

var postAuthor = featuredPost.GetAuthors().FirstOrDefault();

// Will fall back on a default URL if the user doesn't have an avatar
var authorAvatar = postAuthor.GetAvatarUrl();
```

## 🔧 Requirements

- Umbraco 8, using an SQL db.

## 👨‍💻 Development

<details>
<summary>Prerequisites</summary>

- [NPM](https://www.npmjs.com/)
- Development environment running an Umbraco instance

</details>

<details>
<summary>Environment setup</summary>

To develop this plugin, first clone the repository:

```
git clone https://github.com/passle/passle-sync-umbraco-v2
```

Next, install all dependencies and build the frontend with the following commands:

```
npm install
npm run build
```

Then, ensure the `umbracoDbDSN` credentials in `PassleSync.Website/Web.config` match your Umbraco SQL db credentials.

Finally, once you have built the solution and logged into the Umbraco backoffice, you will need to create a few document types and nodes for the demo site to work.

Create the following document types:

- Home Page (with template, allow as root)
- Insights Page (with template, allow as root)
- Passle Authors (allow as root)
- Passle Posts (allow as root)

They don't need to contain and groups/properties. Once they have been created, ensure the document types with templates been populated with the correct code (from `PassleSync.Website/Views`).

Once you've created the document types, create the following root nodes, using the document types described above:

- Home
- Insights
- Passle Posts
- Passle Authors

Once the nodes are updated, please update the node IDs under **Settings > Passle Sync**.

</details>

## 💬 Contributing

If you'd like to request a feature or report a bug, please create a GitHub Issue.

## 📜 License

The Passle Sync plugin is released under the under terms of the [MIT License](./LICENSE).
