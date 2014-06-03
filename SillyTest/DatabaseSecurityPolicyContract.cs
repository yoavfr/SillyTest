using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace DataSec.Proxy.Core.UnitTests
{
    /// <summary>
    /// 
    /// DatabaseSecurityPolicyContract - This file contains:
    ///     1. Definition of the security policy json interface between various components in the system
    ///     2. Sample data for these fields
    ///     
    /// DO NOT use the DatabaseSecurityPolicyContract class directly - it is a reference only. There will be different 
    ///        implementations for different applications. For specific application you may decide to implement a subset of the interface
    /// DO implement IDatabaseSecurityPolicyContract to make tracking of the contract implementations easy
    /// DO use the DatabaseSecurityPolicyConstants
    /// DO add new data members to this reference as the API evolves
    ///
    /// </summary>
    [DataContract]
    public class DatabaseSecurityPolicyContract : IDatabaseSecurityPolicyContract
    {
        /// <summary>
        /// When the policy was last modified
        /// </summary>
        [DataMember]
        public DateTimeOffset DateTimeModifiedUtc { get; set; }

        /// <summary>
        /// The auditing policy
        /// </summary>
        [DataMember]
        public AuditingPolicyContract AuditingPolicy { get; set; }

        /// <summary>
        /// Data masking policies. There can be more than one policy. Different policies will apply to different principals
        /// </summary>
        [DataMember]
        public ICollection<DataMaskingPolicyContract> DataMaskingPolicies { get; set; }

        /// <summary>
        /// Subscription id
        /// </summary>
        [DataMember]
        public string SubscriptionId { get; set; }

        /// <summary>
        /// Constuctor - populates with sample data not part of the contract
        /// </summary>
        public DatabaseSecurityPolicyContract()
        {
            DateTimeModifiedUtc = DateTime.UtcNow;
            SubscriptionId = "BE965ACA-E69D-4822-BFB4-A3B2136981EB";
            AuditingPolicy = new AuditingPolicyContract();
            DataMaskingPolicies = new List<DataMaskingPolicyContract>() { new DataMaskingPolicyContract() };
        }

        public string ToJson()
        {
            // Newtonsoft Json.Net can pretty print this
            //*
            return JsonConvert.SerializeObject(this, Formatting.Indented);
            /*/
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(DatabaseSecurityPolicyContract));
            using (Stream stream = new MemoryStream())
            {
                serializer.WriteObject(stream, this);
                stream.Position = 0;
                StreamReader reader = new StreamReader(stream);
                string jsonString = reader.ReadToEnd();
                return jsonString;
            }
            //*/
        }

        public static DatabaseSecurityPolicyContract FromJson(string jsonString)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(jsonString);
            using (Stream stream = new MemoryStream(bytes))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(DatabaseSecurityPolicyContract));
                DatabaseSecurityPolicyContract result = (DatabaseSecurityPolicyContract)serializer.ReadObject(stream);
                return result;
            }
        }
    }

    /// <summary>
    /// AuditingPolicy - defines what events get audited
    /// </summary>
    [DataContract]
    public class AuditingPolicyContract
    {
        /// <summary>
        /// Indicates whether auditing is enabled
        /// </summary>
        [DataMember]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// XStore Where audit logs should be stored to
        /// </summary>
        [DataMember]
        public string StorageAccountName { get; set; }

        /// <summary>
        /// Access key to the XStore. This should never be persisted.
        /// </summary>
        [DataMember]
        public string StorageAccountAccessKey { get; set; }

        /// <summary>
        /// How long audit logs should be retained in the XStore
        /// </summary>
        [DataMember]
        public int RetentionDays { get; set; }

        /// <summary>
        /// List of events tyoes that should be audited
        /// </summary>
        [DataMember]
        public ICollection<string> EventTypesToAudit { get; set; }

        /// <summary>
        /// Constuctor - populates with sample data not part of the contract
        /// </summary>
        public AuditingPolicyContract()
        {
            IsEnabled = false;
            RetentionDays = 90;
            StorageAccountName = "customerStorage";
            StorageAccountAccessKey = "gRbjsJ6qRfCT3PrgVvzhpwQrZPyCLM15cA0pgG1KX5Itt6jkvJDpitPX4R9X4Kr3FtVIVe62Kn/FjEQ+eiueJA==";
            EventTypesToAudit = new List<string>() 
            { 
                DatabaseSecurityPolicyConstants.EventTypeSchemaChanges, 
                DatabaseSecurityPolicyConstants.EventTypeDataAccess,
                DatabaseSecurityPolicyConstants.EventTypeDataChanges,
                DatabaseSecurityPolicyConstants.EventTypeSecurityExceptions,
                DatabaseSecurityPolicyConstants.EventTypeGrantRevokePermissions,
            };
        }
    }

    /// <summary>
    /// DataMaskingPolicy defines what and how to mask database elements
    /// </summary>
    [DataContract]
    public class DataMaskingPolicyContract
    {
        /// <summary>
        /// Whether data masking is enabled
        /// </summary>
        [DataMember]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Users or groups to which this policy applies 
        /// </summary>
        [DataMember]
        public ICollection<string> IncludedPrincipals { get; set; }

        /// <summary>
        /// Users or groups to which this policy should not apply
        /// </summary>
        [DataMember]
        public ICollection<string> ExcludedPrincipals { get; set; }

        /// <summary>
        /// Data masking rules define what entities should be masked and in what way
        /// </summary>
        [DataMember]
        public ICollection<DataMaskingPolicyRuleContract> DataMaskingPolicyRules { get; set; }

        /// <summary>
        /// Data mask maapings reflect how data masking is implemented - which entities replace which
        /// </summary>
        [DataMember]
        public ICollection<DataMaskingReplacementRuleContract> DataMaskingReplacementRules { get; set; }

        /// <summary>
        /// Constuctor - populates with sample data not part of the contract
        /// </summary>
        public DataMaskingPolicyContract()
        {
            DatabaseEntityContract dummyEntity = new DatabaseEntityContract()
            {
                Schema = "dbo",
                Name = "MyTable"
            };
            DataMaskingPolicyRules = new List<DataMaskingPolicyRuleContract>()
            {
                new DataMaskingPolicyRuleContract(){IsEnabled=true, Entity=dummyEntity, ColumnName="Column1", FieldType=DatabaseSecurityPolicyConstants.FieldTypeCreditCard, MaskingFunction = DatabaseSecurityPolicyConstants.DataMaskingFunctionFull},
                new DataMaskingPolicyRuleContract(){IsEnabled=true, Entity=dummyEntity, ColumnName="Column2", FieldType=DatabaseSecurityPolicyConstants.FieldTypeSocialSecurityNumber, MaskingFunction=DatabaseSecurityPolicyConstants.DataMaskingFunctionPartial},
                new DataMaskingPolicyRuleContract(){IsEnabled=true, Entity=dummyEntity, ColumnName="Column3", FieldType=DatabaseSecurityPolicyConstants.FieldTypeFullName, MaskingFunction=DatabaseSecurityPolicyConstants.DataMaskingFunctionRandomReplacement},
                new DataMaskingPolicyRuleContract(){IsEnabled=true, Entity=dummyEntity, ColumnName="Column4", FieldType=DatabaseSecurityPolicyConstants.FieldTypePhoneNumber, MaskingFunction=DatabaseSecurityPolicyConstants.DataMaskingFunctionPartial},
                new DataMaskingPolicyRuleContract(){IsEnabled=true, Entity=dummyEntity, ColumnName="Column5", FieldType=DatabaseSecurityPolicyConstants.FieldTypeEmailAddress, MaskingFunction=DatabaseSecurityPolicyConstants.DataMaskingFunctionRandomReplacement},
            };
            IncludedPrincipals = new List<string>() 
            {
                "giladeis",
            };
            ExcludedPrincipals = new List<string>()
            {
                "yosefd",
            };
            DataMaskingReplacementRules = new List<DataMaskingReplacementRuleContract>() { new DataMaskingReplacementRuleContract() };
        }
    }

    /// <summary>
    /// DataMaskingPolicyRules define what entities should be masked and in what way
    /// </summary>
    [DataContract]
    public class DataMaskingPolicyRuleContract
    {
        /// <summary>
        /// Whether this rule is enabled
        /// </summary>
        [DataMember]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// The name that the user has assigned to this policy
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// The entity to which this rule applies
        /// </summary>
        [DataMember]
        public DatabaseEntityContract Entity { get; set; }

        /// <summary>
        /// Column to which this rule applies
        /// </summary>
        [DataMember]
        public string ColumnName { get; set; }

        /// <summary>
        /// The type of field that is being masked. Use <see cref="DatabaseSecurityPolicyConstants"/>
        /// </summary>
        [DataMember]
        public string FieldType { get; set; }

        /// <summary>
        /// The kind of data masking that should be applied. Use <see cref="DatabaseSecurityPolicyConstants"/>
        /// </summary>
        [DataMember]
        public string MaskingFunction { get; set; }
    }

    /// <summary>
    /// DataMaskingReplacementRule reflects how data masking is implemented - which entities replace which
    /// </summary>
    [DataContract]
    public class DataMaskingReplacementRuleContract
    {
        /// <summary>
        /// The entity that is being mapped
        /// </summary>
        [DataMember]
        public DatabaseEntityContract From { get; set; }

        /// <summary>
        /// The entity that we map to
        /// </summary>
        [DataMember]
        public DatabaseEntityContract To { get; set; }

        /// <summary>
        /// Constuctor - populates with sample data not part of the contract
        /// </summary>
        public DataMaskingReplacementRuleContract()
        {
            To = new DatabaseEntityContract();
            From = new DatabaseEntityContract();
        }
    }

    /// <summary>
    /// A database entity - defined by schema and name
    /// </summary>
    [DataContract]
    public class DatabaseEntityContract
    {
        [DataMember]
        public string Schema { get; set; }

        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Constuctor - populates with sample data not part of the contract
        /// </summary>
        public DatabaseEntityContract()
        {
            Schema = "dbo";
            Name = "Customers";
        }
    }

}
