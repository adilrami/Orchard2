﻿using System.Collections.Generic;
using Orchard.DependencyInjection;
using Microsoft.AspNet.Mvc.Localization;

namespace Orchard.DisplayManagement.Notify
{
    /// <summary>
    /// Notification manager for UI notifications
    /// </summary>
    /// <remarks>
    /// Where such notifications are displayed depends on the theme used. Default themes contain a 
    /// Messages zone for this.
    /// </remarks>
    public interface INotifier : IDependency
    {
        /// <summary>
        /// Adds a new UI notification
        /// </summary>
        /// <param name="type">
        /// The type of the notification (notifications with different types can be displayed differently)</param>
        /// <param name="message">A localized message to display</param>
        void Add(NotifyType type, LocalizedHtmlString message);

        /// <summary>
        /// Get all notifications added
        /// </summary>
        IList<NotifyEntry> List();
    }
}