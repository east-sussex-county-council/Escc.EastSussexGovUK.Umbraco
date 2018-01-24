using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Escc.EastSussexGovUK.Umbraco.Forms
{
    /// <summary>
    /// Query Umbraco Forms for information about a file upload
    /// </summary>
    /// <remarks>Part of a workaround for http://issues.umbraco.org/issue/CON-1454 </remarks>
    internal class UmbracoFormsFileUploads
    {
        /// <summary>
        /// Gets the form identifier for uploaded file.
        /// </summary>
        /// <param name="uploadedFilePath">The uploaded file path.</param>
        /// <remarks>There is no API that could do this efficiently (you would have to loop through all records for all forms),
        /// so we have to look at the database directly.</remarks>
        /// <returns></returns>
        internal Guid? GetFormIdForUploadedFile(string uploadedFilePath)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["umbracoDbDSN"].ConnectionString))
            {
                using (var command = new SqlCommand("SELECT Form FROM UFRecordDataString [Data] " +
                                                    "INNER JOIN UFRecordFields[Fields] ON[Data].[Key] = [Fields].[Key] " +
                                                    "INNER JOIN UFRecords[FormInstance] ON[Fields].Record = [FormInstance].Id " +
                                                    "WHERE[Data].Value = @url", connection))
                {
                    command.Parameters.Add("@url", SqlDbType.NVarChar, 255).Value = uploadedFilePath;
                    try
                    {
                        connection.Open();
                        var formId = command.ExecuteScalar();
                        if (formId != null)
                        {
                            return new Guid(formId.ToString());
                        }
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            return null;
        }
    }
}