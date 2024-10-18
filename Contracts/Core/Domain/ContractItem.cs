/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Domain Layer                            *
*  Assembly : Empiria.Contracts.Core.dll                 Pattern   : Information Holder                      *
*  Type     : ContractItem                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a contract item.                                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Contacts;
using Empiria.Json;
using Empiria.StateEnums;
using Empiria.Time;

using Empiria.Budgeting;
using Empiria.Projects;

using Empiria.Contracts.Adapters;
using Empiria.Contracts.Data;

namespace Empiria.Contracts {

  /// <summary>Represents a contract item.</summary>
  public class ContractItem : BaseObject {

    #region Constructors and parsers

    private ContractItem() {
      // Required by Empiria Framework.
    }

    public ContractItem(ContractItemFields fields) {
      Assertion.Require(fields, nameof(fields));

      Load(fields);
    }


    static internal ContractItem Parse(string contractItemUID) {
      return BaseObject.ParseKey<ContractItem>(contractItemUID);
    }

    #endregion Constructors and parsers

    #region Properties

    [DataField("CONTRACT_ID")]
    public Contract Contract {
      get; private set;
    }


    [DataField("CONTRACT_ITEM_PRODUCT_ID")]
    public ContractCucop Product {
      get; private set;
    }


    [DataField("CONTRACT_ITEM_DESCRIPTION")]
    public string Description {
      get; private set;
    }


    [DataField("CONTRACT_ITEM_UNIT_ID")]
    public ContractUnit UnitMeasure {
      get; private set;
    }

    [DataField("CONTRACT_ITEM_FROM_QTY", ConvertFrom = typeof(decimal))]
    public decimal FromQuantity {
      get; private set;
    }

    [DataField("CONTRACT_ITEM_TO_QTY", ConvertFrom = typeof(decimal))]
    public decimal ToQuantity {
      get; private set;
    }

    [DataField("CONTRACT_ITEM_UNIT_PRICE", ConvertFrom = typeof(decimal))]
    public decimal UnitPrice {
      get; private set;
    }

    [DataField("CONTRACT_ITEM_PROJECT_ID")]
    public Project Project {
      get; private set;
    }


    [DataField("CONTRACT_ITEM_PERIODICITY_ID")]
    public Periodicity PaymentsPeriodicity {
      get; private set;
    }


    [DataField("CONTRACT_ITEM_BDG_ACCOUNT_ID")]
    public BudgetAccount BudgetAccount {
      get; private set;
    }


    [DataField("CONTRACT_ITEM_EXTDATA")]
    private JsonObject ExtData {
      get; set;
    }


    public string Keywords {
      get {
        return EmpiriaString.BuildKeywords(this.Description);
      }
    }


    internal Contact LastUpdatedBy {
      get {
        return ExtData.Get<Contact>("lastUpdatedBy", Contact.Empty);
      }
      set {
        ExtData.Set("lastUpdatedBy", value.Id);
      }
    }


    internal DateTime LastUpdatedTime {
      get {
        return ExtData.Get<DateTime>("lastUpdatedTime", this.PostingTime);
      }
      set {
        ExtData.Set("lastUpdatedTime", value);
      }
    }


    [DataField("CONTRACT_ITEM_POSTED_BY_ID")]
    public Contact PostedBy {
      get; private set;
    }


    [DataField("CONTRACT_ITEM_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }


    [DataField("CONTRACT_ITEM_STATUS", Default = EntityStatus.Pending)]
    public EntityStatus Status {
      get; private set;
    } = EntityStatus.Pending;

    #endregion Properties

    #region Methods

    internal void Delete() {

      this.Status = EntityStatus.Deleted;
    }


    protected override void OnSave() {
      if (base.IsNew) {
        this.PostedBy = ExecutionServer.CurrentContact;
        this.PostingTime = DateTime.Now;
      }

      LastUpdatedBy = ExecutionServer.CurrentContact;
      LastUpdatedTime = DateTime.Now;

      ContractItemData.WriteContractItem(this, this.ExtData.ToString());
    }

    #endregion Methods

    #region Helpers

    internal void Load(ContractItemFields fields) {
      this.Contract = Contract.Parse(fields.ContractUID);
      this.Product = ContractCucop.Parse(fields.ProductUID);
      this.Description = fields.Description;
      this.UnitMeasure = ContractUnit.Parse(fields.UnitMeasureUID);
      this.FromQuantity = fields.FromQuantity;
      this.ToQuantity = fields.ToQuantity;
      this.UnitPrice = fields.UnitPrice;
      this.PaymentsPeriodicity = Periodicity.Parse(fields.PaymentPeriodicityUID);
      this.BudgetAccount = BudgetAccount.Parse(fields.BudgetAccountUID);
      this.LastUpdatedBy = Contact.Parse(ExecutionServer.CurrentUserId);
      this.PostingTime = DateTime.Now;
    }

    #endregion Helpers

  }  // class ContractItem

}  // namespace Empiria.Contracts
