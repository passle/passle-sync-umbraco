#### [PassleSync.Core](index.md 'index')
### [PassleSync.Core.Helpers.Queries](PassleSync.Core.Helpers.Queries.md 'PassleSync.Core.Helpers.Queries')

## QueryBase<T,P> Class

The base class used by all query classes.

```csharp
public abstract class QueryBase<T,P>
    where T : PassleSync.Core.Helpers.Queries.QueryBase<T, P>
    where P : Umbraco.Core.Models.PublishedContent.PublishedContentModel
```
#### Type parameters

<a name='PassleSync.Core.Helpers.Queries.QueryBase_T,P_.T'></a>

`T`

<a name='PassleSync.Core.Helpers.Queries.QueryBase_T,P_.P'></a>

`P`

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; QueryBase<T,P>

Derived  
&#8627; [PassleAuthorQuery](PassleSync.Core.Helpers.Queries.PassleAuthorQuery.md 'PassleSync.Core.Helpers.Queries.PassleAuthorQuery')  
&#8627; [PasslePostQuery](PassleSync.Core.Helpers.Queries.PasslePostQuery.md 'PassleSync.Core.Helpers.Queries.PasslePostQuery')

| Methods | |
| :--- | :--- |
| [Execute()](PassleSync.Core.Helpers.Queries.QueryBase_T,P_.Execute().md 'PassleSync.Core.Helpers.Queries.QueryBase<T,P>.Execute()') | Execute the query, returning a [QueryResult&lt;T&gt;](PassleSync.Core.Helpers.Queries.QueryResult_T_.md 'PassleSync.Core.Helpers.Queries.QueryResult<T>') |
| [Search(string)](PassleSync.Core.Helpers.Queries.QueryBase_T,P_.Search(string).md 'PassleSync.Core.Helpers.Queries.QueryBase<T,P>.Search(string)') | Filter content using a text-based search. |
| [WithCurrentPage(int)](PassleSync.Core.Helpers.Queries.QueryBase_T,P_.WithCurrentPage(int).md 'PassleSync.Core.Helpers.Queries.QueryBase<T,P>.WithCurrentPage(int)') | Specify the current page that should be used for pagination. |
| [WithItemsPerPage(int)](PassleSync.Core.Helpers.Queries.QueryBase_T,P_.WithItemsPerPage(int).md 'PassleSync.Core.Helpers.Queries.QueryBase<T,P>.WithItemsPerPage(int)') | Specify the items per page that should be used for pagination. |
