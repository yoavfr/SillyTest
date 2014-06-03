using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSec.Proxy.Core
{
    /// <summary>
    /// IDatabaseSecurityPolicyContract defines a loose json based data contract between 
    /// consumers and producers of database security policy.
    /// The full contract is maintained in <see cref="DataSec.Proxy.Core.Test.DatabaseSecurityPolicyContract"/>.
    /// Different implementations of IDatabaseSecurityPolicyContract may require different subsets of the contract.
    /// Mark all implementations of the contract with this interface to make it easy to find implementors.
    /// If you add a new implementation, please make sure to add an appropriate test in <see cref="DataSec.Proxy.Core.Test.DatabaseSecurityPolicyContractTest"/>
    /// to enforce compatibility with the contract
    /// </summary>
    public interface IDatabaseSecurityPolicyContract
    {
        string ToJson();
    }

    /// <summary>
    /// DatabaseSecurityPolicyConstants - constants used in the IDatabaseSecurityPolicyContract
    /// </summary>
    public static class DatabaseSecurityPolicyConstants
    {
        // Data masking field types
        public const string FieldTypeCreditCard = "CreditCard";
        public const string FieldTypeSocialSecurityNumber = "SocialSecurityNumber";
        public const string FieldTypePhoneNumber = "PhoneNumber";
        public const string FieldTypeFullName = "FullName";
        public const string FieldTypeEmailAddress = "EmailAddress";

        // Data masking functions
        public const string DataMaskingFunctionFull = "Full";
        public const string DataMaskingFunctionPartial = "Partial";
        public const string DataMaskingFunctionRandomReplacement = "RandomReplacement";

        // Auditable event types
        public const string EventTypeSchemaChanges = "SchemaChanges";
        public const string EventTypeDataAccess = "DataAccess";
        public const string EventTypeDataChanges = "DataChanges";
        public const string EventTypeSecurityExceptions = "SecurityExceptions";
        public const string EventTypeGrantRevokePermissions = "RevokePermissions";

    }
}
