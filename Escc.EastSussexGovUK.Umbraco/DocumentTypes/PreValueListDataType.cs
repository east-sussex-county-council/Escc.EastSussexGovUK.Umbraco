using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Umbraco.Core.Models;
using Umbraco.Inception.BL;

namespace Escc.EastSussexGovUK.Umbraco.DocumentTypes
{
    /// <summary>
    /// A data type based on a list of values, such as a dropdown list or checkbox list
    /// </summary>
    /// <seealso cref="Umbraco.Inception.BL.IPreValueProvider" />
    public abstract class PreValueListDataType : IPreValueProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PreValueListDataType"/> class.
        /// </summary>
        public PreValueListDataType(IEnumerable<string> values)
        {
            PreValues = CreatePreValues(values);
        }

        private static IDictionary<string, PreValue> CreatePreValues(IEnumerable<string> values)
        {
            IDictionary<string, PreValue> preValues = new Dictionary<string, PreValue>();

            // Use a SHA1 key for each prevalue because:
            // * we want prevalues to be accessible programatically
            // * it can be generated from the prevalue, in a way that remains consistent if more prevalues are added later
            // * SHA1 is short enough to fit the Umbraco database schema
            // * it doesn't need to be secure, just unique
            using (HashAlgorithm algorithm = SHA1.Create())
            {
                foreach (var value in values)
                {
                    preValues.Add(HashString(algorithm, value), new PreValue(-1, value));
                }
            }

            return preValues;
        }

        private static string HashString(HashAlgorithm algorithm, string wasteType)
        {
            var key = new StringBuilder();
            var data = algorithm.ComputeHash(Encoding.UTF8.GetBytes(wasteType));
            foreach (byte hashByte in data)
            {
                key.Append(hashByte.ToString("x2", CultureInfo.InvariantCulture));
            }
            return key.ToString();
        }

        /// <summary>
        /// Gets the values which can be selected
        /// </summary>
        /// <value>
        /// The pre values.
        /// </value>
        public IDictionary<string, PreValue> PreValues { get; private set; }
    }
}