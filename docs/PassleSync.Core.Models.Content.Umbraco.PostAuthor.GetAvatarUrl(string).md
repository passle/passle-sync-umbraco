#### [PassleSync.Core](index.md 'index')
### [PassleSync.Core.Models.Content.Umbraco](PassleSync.Core.Models.Content.Umbraco.md 'PassleSync.Core.Models.Content.Umbraco').[PostAuthor](PassleSync.Core.Models.Content.Umbraco.PostAuthor.md 'PassleSync.Core.Models.Content.Umbraco.PostAuthor')

## PostAuthor.GetAvatarUrl(string) Method

Get the URL of the author avatar, with a fallback URL if the author doesn't have an avatar.

```csharp
public string GetAvatarUrl(string fallbackUrl="https://images.passle.net/200x200/assets/images/no_avatar.png");
```
#### Parameters

<a name='PassleSync.Core.Models.Content.Umbraco.PostAuthor.GetAvatarUrl(string).fallbackUrl'></a>

`fallbackUrl` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

An optional custom fallback URL

Implements [GetAvatarUrl(string)](https://docs.microsoft.com/en-us/dotnet/api/PassleSync.Core.API.Models.IBasicAuthorDetails.GetAvatarUrl#PassleSync_Core_API_Models_IBasicAuthorDetails_GetAvatarUrl_System_String_ 'PassleSync.Core.API.Models.IBasicAuthorDetails.GetAvatarUrl(System.String)')

#### Returns
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')