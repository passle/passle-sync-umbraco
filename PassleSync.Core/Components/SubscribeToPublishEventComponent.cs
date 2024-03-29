﻿using Umbraco.Core.Composing;
using Umbraco.Core.Services.Implement;

namespace PassleSync.Core.Components
{
    public class SubscribeToPublishEventComponent : IComponent
    {
        public void Initialize()
        {
            ContentService.Saving += ContentService_Saving;
        }

        public void ContentService_Saving(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.ContentSavingEventArgs e)
        {
            foreach (var node in e.SavedEntities)
            {
                if (node.HasIdentity && node.ContentType.Alias == "passlePost")
                {
                    // Stop putting news article titles in upper case, so cancel publish
                    e.Cancel = true;

                    // Explain why the publish event is cancelled
                    e.Messages.Add(new Umbraco.Core.Events.EventMessage("Error", "Posts are not changeable!", Umbraco.Core.Events.EventMessageType.Error));
                }
            }

            foreach (var node in e.SavedEntities)
            {
                if (node.HasIdentity && node.ContentType.Alias == "passleAuthor")
                {
                    // Stop putting news article titles in upper case, so cancel publish
                    e.Cancel = true;

                    // Explain why the publish event is cancelled
                    e.Messages.Add(new Umbraco.Core.Events.EventMessage("Error", "This document is not changeable!", Umbraco.Core.Events.EventMessageType.Error));
                }
            }
        }

        public void Terminate()
        {
            //unsubscribe during shutdown
            ContentService.Saving -= ContentService_Saving;
        }
    }
}
