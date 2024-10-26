/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Domain Layer                            *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Empiria Plain Object                    *
*  Type     : BillConcept                                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a bill concept with its tax entries.                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Json;
using Empiria.Parties;
using Empiria.Products;

namespace Empiria.Billing {

  /// <summary>Represents a bill concept with its tax entries.</summary>
  internal class BillConcept : BaseObject {

    #region Constructors and parsers

    private BillConcept() {
      // Required by Empiria Framework.
    }

    static public BillConcept Parse(int id) => ParseId<BillConcept>(id);

    static public BillConcept Parse(string uid) => ParseKey<BillConcept>(uid);

    #endregion Constructors and parsers

    #region Public properties


    [DataField("BILL_CONCEPT_BILL_ID")]
    public Bill Bill {
      get; private set;
    }


    [DataField("BILL_CONCEPT_PRODUCT_ID")]
    public Product Product {
      get; private set;
    }


    [DataField("BILL_CONCEPT_DESCRIPTION")]
    public string Description {
      get; set;
    }


    [DataField("BILL_CONCEPT_IDENTIFICATORS")]
    public string Identificators {
      get; private set;
    }


    [DataField("BILL_CONCEPT_TAGS")]
    public string Tags {
      get; private set;
    }


    [DataField("BILL_CONCEPT_QTY")]
    public decimal Quantity {
      get; private set;
    }


    [DataField("BILL_CONCEPT_UNIT_ID")]
    public ProductUnit QuantityUnit {
      get; private set;
    }


    [DataField("BILL_CONCEPT_UNIT_PRICE")]
    public decimal UnitPrice {
      get; private set;
    }


    [DataField("BILL_CONCEPT_SUBTOTAL")]
    public decimal Subtotal {
      get; private set;
    }


    [DataField("BILL_CONCEPT_DISCOUNT")]
    public decimal Discount {
      get; private set;
    }


    [DataField("BILL_CONCEPT_SCHEMA_EXT_DATA")]
    private JsonObject SchemaExtData {
      get; set;
    }


    [DataField("BILL_CONCEPT_EXT_DATA")]
    private JsonObject ExtData {
      get; set;
    }


    [DataField("BILL_CONCEPT_POSTED_BY_ID")]
    public Party PostedBy {
      get; private set;
    }


    [DataField("BILL_CONCEPT_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }

    public BillConceptSchemaData SchemaData {
      get {
        return new BillConceptSchemaData(this.SchemaExtData);
      }
    }

    public FixedList<BillTaxEntry> TaxEntries {
      get; set;
    } = new FixedList<BillTaxEntry>();


    #endregion Public properties

  } // class BillConcept

} // namespace Empiria.Billing
