namespace Web.Infrastructure.Notifications
{
    using System;
    using System.Collections.Generic;

    public class Notifier
    {
        public Func<Notification, bool> Sender;

        public void Send(string templateId, object values)
        {
            if (Sender == null)
                throw new InvalidOperationException("Sender function not set.");

            var notification = new Notification();
            // todo get template

            // do replacement

            Sender(notification);
        }
    }

    public class Notification
    {
        public string Content { get; set; }

        public IEnumerable<KeyValuePair<string, string>> To { get; set; }
    }
}