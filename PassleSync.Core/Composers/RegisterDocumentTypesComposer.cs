﻿using Umbraco.Core.Composing;
using Umbraco.Core;
using PassleSync.Core.Components;

namespace PassleSync.Core.Composers
{
    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    public class RegisterDocumentTypesComposer : ComponentComposer<RegisterDocumentTypesComponent>
    {
    }
}
