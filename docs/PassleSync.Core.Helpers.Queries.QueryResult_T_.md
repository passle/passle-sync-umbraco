#### [PassleSync.Core](index.md 'index')
### [PassleSync.Core.Helpers.Queries](PassleSync.Core.Helpers.Queries.md 'PassleSync.Core.Helpers.Queries')

## QueryResult<T> Class

The result of a query which has been executed, including the content returned by the query, and pagination details.

```csharp
public class QueryResult<T>
    where T : Umbraco.Core.Models.PublishedContent.PublishedContentModel
```
#### Type parameters

<a name='PassleSync.Core.Helpers.Queries.QueryResult_T_.T'></a>

`T`

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; QueryResult<T>

| Fields | |
| :--- | :--- |
| [CurrentPage](PassleSync.Core.Helpers.Queries.QueryResult_T_.CurrentPage.md 'PassleSync.Core.Helpers.Queries.QueryResult<T>.CurrentPage') | The current page of paginated results. |
| [ItemsPerPage](PassleSync.Core.Helpers.Queries.QueryResult_T_.ItemsPerPage.md 'PassleSync.Core.Helpers.Queries.QueryResult<T>.ItemsPerPage') | The items per page of paginated results. |
| [TotalItems](PassleSync.Core.Helpers.Queries.QueryResult_T_.TotalItems.md 'PassleSync.Core.Helpers.Queries.QueryResult<T>.TotalItems') | The total number of items before pagination was applied. |
| [TotalPages](PassleSync.Core.Helpers.Queries.QueryResult_T_.TotalPages.md 'PassleSync.Core.Helpers.Queries.QueryResult<T>.TotalPages') | The total number of pages of paginated results. |

| Properties | |
| :--- | :--- |
| [Items](PassleSync.Core.Helpers.Queries.QueryResult_T_.Items.md 'PassleSync.Core.Helpers.Queries.QueryResult<T>.Items') | The content returned by the query. |
