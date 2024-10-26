/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Domain Layer                            *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Empiria Plain Object                    *
*  Type     : Bill                                       License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represent a bill according to SAT Mexico standard.                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Financial;
using Empiria.Json;
using Empiria.Parties;

namespace Empiria.Billing {

  /// <summary>Represent a bill according to SAT Mexico standard.</summary>
  internal class Bill : BaseObject {

    #region Constructors and parsers

    private Bill() {
      // Required by Empiria Framework.
    }

    static public Bill Parse(int id) => ParseId<Bill>(id);

    static public Bill Parse(string uid) => ParseKey<Bill>(uid);

    #endregion Constructors and parsers

    #region Properties

    [DataField("BILL_TYPE_ID")]
    public int BillType {
      get; private set;
    }


    [DataField("BILL_CATEGORY_ID")]
    public int BillCategory {
      get; private set;
    }


    [DataField("BILL_NO")]
    public string BillNo {
      get; private set;
    }


    [DataField("BILL_ISSUE_DATE")]
    public DateTime IssueDate {
      get; private set;
    }


    [DataField("BILL_ISSUED_BY_ID")]
    public Party IssuedBy {
      get; private set;
    }


    [DataField("BILL_ISSUED_TO_ID")]
    public Party IssuedTo {
      get; private set;
    }


    [DataField("BILL_SCHEMA_VERSION")]
    public string SchemaVersion {
      get; private set;
    }


    [DataField("BILL_IDENTIFICATORS")]
    public string Identificators {
      get; private set;
    }


    [DataField("BILL_TAGS")]
    public string Tags {
      get; private set;
    }


    [DataField("BILL_CURRENCY_ID")]
    public Currency Currency {
      get; private set;
    }


    [DataField("BILL_SUBTOTAL")]
    public decimal Subtotal {
      get; private set;
    }


    [DataField("BILL_DISCOUNT")]
    public decimal Discount {
      get; private set;
    }


    [DataField("BILL_TOTAL")]
    public decimal Total {
      get; private set;
    }


    [DataField("BILL_SCHEMA_EXT_DATA")]
    private JsonObject SchemaExtData {
      get; set;
    }


    [DataField("BILL_SECURITY_EXT_DATA")]
    private JsonObject SecurityExtData {
      get; set;
    }


    [DataField("BILL_EXT_DATA")]
    private JsonObject ExtData {
      get; set;
    }


    [DataField("BILL_POSTED_BY_ID")]
    public Party PostedBy {
      get; private set;
    }


    [DataField("BILL_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }


    [DataField("BILL_STATUS", Default = BillStatus.Pending)]
    public BillStatus Status {
      get; private set;
    }


    public BillSchemaData SchemaData {
      get {
        return new BillSchemaData(this.SchemaExtData);
      }
    }


    public BillSecurityData SecurityData {
      get {
        return new BillSecurityData(this.SecurityExtData);
      }
    }


    internal FixedList<BillConcept> Concepts {
      get; set;
    } = new FixedList<BillConcept>();


    public string Keywords {
      get {
        return EmpiriaString.BuildKeywords(BillNo);
      }
    }

    #endregion Properties

  } // class Bill

} // namespace Empiria.Billing
