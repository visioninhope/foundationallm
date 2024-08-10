using System.Security.Cryptography;
using System.Text;

namespace FoundationaLLM.Common.Extensions
{
    /// <summary>
    /// Extends the <see cref="System.String"/> interface with helper methods.
    /// </summary>
    public static class StringExtensions
    {

        /// <summary>
        /// Generates a valid resource name from the provided name.
        /// A good example use for this method is generating a valid
        /// resource name from an uploaded file name.
        /// </summary>
        /// <param name="name">The original resource name to convert.</param>
        /// <returns></returns>
        public static string GenerateValidResourceName(
            this string name)
        {
            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(name));
            var base64Hash = Convert.ToBase64String(hash);

            // Replace invalid characters.
            base64Hash = base64Hash.Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", ""); // Remove padding characters.

            // Prefix with a letter to ensure it starts with a letter.
            var validName = "a" + base64Hash;

            // Ensure the name length is within a reasonable limit, e.g., 50 characters.
            return validName.Length > 50 ? validName[..50] : validName;
        }

        /// <summary>
        /// Converts a UPN (User Principal Name) to a string that is better suited to be used as an identifier.
        /// </summary>
        /// <param name="upn">The original UPN (User Principal Name).</param>
        /// <returns>A string containing the normalized UPN (User Principal Name).</returns>
        public static string NormalizeUserPrincipalName(
            this string upn) =>
            upn
                .Replace('.', '_')
                .Replace("#", ".")
                .Replace("@", "_")
                .ToLower();
    }
}
