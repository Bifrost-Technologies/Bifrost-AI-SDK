using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bifrost_AI_SDK.Plugins
{
    public class TemporalAwareness
    {
        /// <summary>
        /// Retrieves the current time in UTC.
        /// </summary>
        /// <returns>The current time in UTC. </returns>
        [KernelFunction("GetCurrentTime"), Description("Retrieves the current time in UTC.")]
        public string GetCurrentUtcTime()
            => DateTime.UtcNow.ToString("R");
    }
}
