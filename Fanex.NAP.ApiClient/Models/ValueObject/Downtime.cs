namespace Fanex.NAP.Core.Models.ValueObject
{
    /// <summary>
    /// Downtime model.
    /// </summary>
    public class Downtime
    {
        public string From { get; set; }

        public string To { get; set; }

        public bool IsDowntime { get; set; }
    }
}