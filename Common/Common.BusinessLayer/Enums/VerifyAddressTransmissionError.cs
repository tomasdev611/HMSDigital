using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Common.BusinessLayer.Enums
{
    public enum VerifyAddressTransmissionError
    {
        /// <summary>
        /// Empty Request Structure.
        /// </summary>
        GE01 = 1,

        /// <summary>
        /// Empty Request Record Structure.
        /// </summary>
        GE02 = 2,

        /// <summary>
        /// The counted records sent more than the number of records allowed per request.
        /// </summary>
        GE03 = 3,

        /// <summary>
        /// The License Key is empty.
        /// </summary>
        GE04 = 4,

        /// <summary>
        /// The required license string is missing or invalid
        /// </summary>
        GE05 = 5,

        /// <summary>
        /// The License Key is disabled.
        /// </summary>
        GE06 = 6,

        /// <summary>
        /// The SOAP, JSON, or XML request is invalid.
        /// </summary>
        GE07 = 7,

        /// <summary>
        /// The License Key is invalid for this product or level.
        /// </summary>
        GE08 = 8,

        /// <summary>
        /// The Customer ID is not in mellisa system
        /// </summary>
        GE09 = 9,

        /// <summary>
        /// Customer License Disabled
        /// </summary>
        GE10 = 10,

        /// <summary>
        /// The Customer ID is disabled.
        /// </summary>
        GE11 = 11,

        /// <summary>
        /// IP Blacklisted
        /// </summary>
        GE12 = 12,

        /// <summary>
        /// IP Not Whitelisted
        /// </summary>
        GE13 = 13,

        /// <summary>
        /// The account has ran out of credits
        /// </summary>
        GE14 = 14,

        /// <summary>
        /// The Verify package was requested but is not active for the License Key.
        /// </summary>
        GE20 = 20,

        /// <summary>
        /// The Append package was requested but is not active for the License Key.
        /// </summary>
        GE21 = 21,

        /// <summary>
        /// The Move package was requested but is not active for the License Key.
        /// </summary>
        GE22 = 22,

        /// <summary>
        /// No valid action was requested by the service.
        /// </summary>
        GE23 = 23,

        /// <summary>
        /// The Demographics package was requested but is not active for the License Key.
        /// </summary>
        GE24 = 24,

        /// <summary>
        /// IP Columns requested but not active for the customer ID
        /// </summary>
        GE27 = 27,

        /// <summary>
        /// SSN Verification requested but not active for the customer ID
        /// </summary>
        GE28 = 28,

        /// <summary>
        /// The requested fields were not available for a credit license.
        /// </summary>
        GE29 = 29,
    }
}
