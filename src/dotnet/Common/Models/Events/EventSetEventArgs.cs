using Azure.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoundationaLLM.Common.Models.Events
{
    /// <summary>
    /// Event arguments required for event set event delegates.
    /// </summary>
    public class EventSetEventArgs : EventArgs
    {
        /// <summary>
        /// The namespace associated with the event set.
        /// </summary>
        public required string Namespace { get; set; }

        /// <summary>
        /// The list of subjects associated with the event.
        /// </summary>
        public required IList<CloudEvent> Events { get; set; }
    }

    /// <summary>
    /// Delegate 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void EventSetEventDelegate(object sender, EventSetEventArgs e);
}
