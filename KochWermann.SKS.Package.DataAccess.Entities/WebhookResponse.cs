using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace KochWermann.SKS.Package.DataAccess.Entities
{
    /// <summary>
    ///
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class WebhookResponse
    {
        /// <summary>
        /// ID of the Webhook.
        /// </summary>
        /// <value>ID of the Webhook.</value>
        [Required]
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// Gets or Sets TrackingId
        /// </summary>
        [Required]
        public string TrackingId { get; set; }

        /// <summary>
        /// Gets or Sets Url
        /// </summary>
        [Required]
        public string Url { get; set; }

        /// <summary>
        /// Gets or Sets CreatedAt
        /// </summary>
        public DateTime? CreatedAt { get; set; }
    }
}