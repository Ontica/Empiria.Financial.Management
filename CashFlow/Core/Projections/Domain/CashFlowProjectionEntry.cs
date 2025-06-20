﻿/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                        Component : Domain Layer                            *
*  Assembly : Empiria.CashFlow.Core.dll                  Pattern   : Information Holder                      *
*  Type     : CashflowProjectionEntry                    License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : An entry in a cash flow projection.                                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Json;
using Empiria.Parties;
using Empiria.Products;
using Empiria.StateEnums;

using Empiria.Financial;
using Empiria.Financial.Projects;

using Empiria.CashFlow.Projections.Data;

namespace Empiria.CashFlow.Projections {

  /// <summary>An entry in a cash flow projection.</summary>
  public class CashFlowProjectionEntry : BaseObject {

    #region Fields

    static internal readonly string ENTRY_ACCOUNT_ROLE = "cash-flow-projection-entry-account";

    #endregion Fields

    #region Constructors and parsers

    private CashFlowProjectionEntry() {
      // Required by Empiria Framework.
    }

    internal CashFlowProjectionEntry(CashFlowProjection projection, CashFlowProjectionColumn projectionColumn,
                                     FinancialAccount account, int year, int month, decimal amount) {
      Assertion.Require(projection, nameof(projection));
      Assertion.Require(!projection.IsEmptyInstance, nameof(projection));
      Assertion.Require(account, nameof(account));
      Assertion.Require(!account.IsEmptyInstance, nameof(account));
      Assertion.Require(account.FinancialAccountType.PlaysRole(ENTRY_ACCOUNT_ROLE),
                $"{account.Name} is not enabled to be used for cash flow projection entries.");
      Assertion.Require(projection.Plan.IncludesYear(year), nameof(year));
      Assertion.Require(1 <= month && month <= 12, nameof(month));
      Assertion.Require(projection.Plan.IncludesMonth(year, month), nameof(month));
      Assertion.Require(amount > 0, nameof(amount));

      Projection = projection;
      CashFlowAccount = account;
      FinancialProject = account.Project;
      Currency = projection.Plan.BaseCurrency;

      ProjectionColumn = projectionColumn;
      Year = year;
      Month = month;

      this.OriginalAmount = amount;

      if (account.IsInflowAccount) {
        this.InflowAmount = amount;
      } else {
        this.OutflowAmount = amount;
      }
    }

    static public CashFlowProjectionEntry Parse(int id) => ParseId<CashFlowProjectionEntry>(id);

    static public CashFlowProjectionEntry Parse(string uid) => ParseKey<CashFlowProjectionEntry>(uid);

    static public CashFlowProjectionEntry Empty => ParseEmpty<CashFlowProjectionEntry>();

    #endregion Constructors and parsers

    #region Properties


    [DataField("CFW_PJC_ENTRY_PJC_ID")]
    public CashFlowProjection Projection {
      get; private set;
    }


    [DataField("CFW_PJC_ENTRY_ACCOUNT_ID")]
    public FinancialAccount CashFlowAccount {
      get; private set;
    }


    [DataField("CFW_PJC_ENTRY_OPERATION_TYPE_ID")]
    public int OperationTypeId {
      get; private set;
    } = -1;


    [DataField("CFW_PJC_ENTRY_PROJECT_ID")]
    public FinancialProject FinancialProject {
      get; private set;
    }


    [DataField("CFW_PJC_ENTRY_PRODUCT_ID")]
    public Product Product {
      get; private set;
    }


    [DataField("CFW_PJC_ENTRY_PRODUCT_UNIT_ID")]
    public ProductUnit ProductUnit {
      get; private set;
    }


    [DataField("CFW_PJC_ENTRY_PRODUCT_QTY")]
    public decimal ProductQty {
      get; private set;
    }


    [DataField("CFW_PJC_ENTRY_YEAR")]
    public int Year {
      get; private set;
    }


    [DataField("CFW_PJC_ENTRY_MONTH")]
    public int Month {
      get; private set;
    }


    public string MonthName {
      get {
        return new DateTime(Year, Month, 1).ToString("MMMM");
      }
    }


    [DataField("CFW_PJC_ENTRY_DAY")]
    public int Day {
      get; private set;
    }


    [DataField("CFW_PJC_ENTRY_PROJECTION_COLUMN_ID")]
    public CashFlowProjectionColumn ProjectionColumn {
      get; private set;
    }


    [DataField("CFW_PJC_ENTRY_CURRENCY_ID")]
    public Currency Currency {
      get; private set;
    }


    [DataField("CFW_PJC_ENTRY_ORIGINAL_AMOUNT")]
    public decimal OriginalAmount {
      get; private set;
    }


    public decimal Amount {
      get {
        return InflowAmount + OutflowAmount;
      }
    }


    [DataField("CFW_PJC_ENTRY_INFLOW_AMOUNT")]
    public decimal InflowAmount {
      get; private set;
    }


    [DataField("CFW_PJC_ENTRY_OUTFLOW_AMOUNT")]
    public decimal OutflowAmount {
      get; private set;
    }


    [DataField("CFW_PJC_ENTRY_EXCHANGE_RATE")]
    public decimal ExchangeRate {
      get; private set;
    }


    [DataField("CFW_PJC_ENTRY_DESCRIPTION")]
    public string Description {
      get; private set;
    }


    [DataField("CFW_PJC_ENTRY_JUSTIFICATION")]
    public string Justification {
      get; private set;
    }


    [DataField("CFW_PJC_ENTRY_TAGS")]
    private string _tags = string.Empty;

    public FixedList<string> Tags {
      get {
        return EmpiriaString.Tagging(_tags);
      }
    }


    [DataField("CFW_PJC_ENTRY_EXT_DATA")]
    internal protected JsonObject ExtensionData {
      get; private set;
    }


    [DataField("CFW_PJC_ENTRY_LINKED_PJC_ENTRY_ID")]
    internal int LinkedProjectionEntryId {
      get; private set;
    } = -1;


    [DataField("CFW_PJC_ENTRY_POSITION")]
    public int Position {
      get; private set;
    }


    [DataField("CFW_PJC_ENTRY_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }


    [DataField("CFW_PJC_ENTRY_POSTED_BY_ID")]
    public Party PostedBy {
      get; private set;
    }


    [DataField("CFW_PJC_ENTRY_STATUS", Default = TransactionStatus.Pending)]
    public TransactionStatus Status {
      get; private set;
    }


    public virtual string Keywords {
      get {
        return EmpiriaString.BuildKeywords(_tags, Description, CashFlowAccount.Keywords, Projection.Keywords,
                                           FinancialProject.Keywords, Product.Keywords);
      }
    }

    #endregion Properties

    #region Methods

    internal void Delete() {
      Status = TransactionStatus.Deleted;

      MarkAsDirty();
    }

    protected override void OnSave() {
      if (IsNew) {
        PostedBy = Party.ParseWithContact(ExecutionServer.CurrentContact);
        PostingTime = DateTime.Now;
      }
      CashFlowProjectionDataService.WriteProjectionEntry(this);
    }


    internal void Update(CashFlowProjectionEntryFields fields) {
      Assertion.Require(fields, nameof(fields));

      fields.EnsureValid();
      if (fields.Year.HasValue && !Projection.Plan.IncludesYear(fields.Year.Value)) {
        Assertion.RequireFail($"Invalid year for this projection's plan ({fields.Year.Value}).");
      }
      if (fields.Month.HasValue && !Projection.Plan.IncludesMonth(Patcher.Patch(fields.Year, Year),
                                                                  fields.Month.Value)) {
        Assertion.RequireFail($"Invalid month for this projection's plan ({fields.Month.Value}).");
      }

      CashFlowAccount = Patcher.Patch(fields.CashFlowAccountUID, CashFlowAccount);
      ProjectionColumn = Patcher.Patch(fields.ProjectionColumnUID, ProjectionColumn);
      Product = Patcher.Patch(fields.ProductUID, Product);
      ProductUnit = Patcher.Patch(fields.ProductUnitUID, ProductUnit);
      ProductQty = fields.ProductQty;
      Year = Patcher.Patch(fields.Year, Year);
      Month = Patcher.Patch(fields.Month, Month);

      Currency = Patcher.Patch(fields.CurrencyUID, Currency);
      OriginalAmount = Patcher.Patch(fields.Amount, Amount);

      if (CashFlowAccount.IsInflowAccount) {
        InflowAmount = OriginalAmount;
      } else {
        OutflowAmount = OriginalAmount;
      }
      ExchangeRate = Patcher.Patch(fields.ExchangeRate, ExchangeRate);

      Description = EmpiriaString.Clean(fields.Description);
      Justification = EmpiriaString.Clean(fields.Justification);
      _tags = string.Join(" ", fields.Tags);

      MarkAsDirty();
    }


    #endregion Methods

  }  // class CashflowProjectionEntry

}  // namespace Empiria.CashFlow.Projections
