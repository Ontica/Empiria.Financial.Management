/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Domain Layer                            *
*  Assembly : Empiria.Contracts.Core.dll                 Pattern   : Infomation Holder                       *
*  Type     : ContractMilestoneItem                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a contract milestone item.                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Contacts;
using Empiria.Json;
using Empiria.Products;
using Empiria.StateEnums;

namespace Empiria.Contracts {

  /// <summary>Represents a contract milestone item.</summary>
  public class ContractMilestoneItem : BaseObject {

    #region Constructors and parsers

    static internal ContractMilestoneItem Parse(int id) {
      return ParseId<ContractMilestoneItem>(id);
    }

    static internal ContractMilestoneItem Parse(string uid) {
      return ParseKey<ContractMilestoneItem>(uid);
    }

    static internal FixedList<ContractMilestoneItem> GetList() {
      return GetList<ContractMilestoneItem>().ToFixedList();
    }

    static internal ContractMilestoneItem Empty => ParseEmpty<ContractMilestoneItem>();

    #endregion Constructors and parsers

    #region Properties

    [DataField("MILESTONE_ID")]
    public ContractMilestone Milestone{
      get; private set;
    }


    [DataField("CONTRACT_ITEM_ID")]
    public ContractItem ContractItem {
      get; private set;
    }


    [DataField("CONTRACT_ITEM_PRODUCT_ID")]
    public Product Product {
      get; private set;
    }

    [DataField("MILESTONE_ITEM_QTY", ConvertFrom = typeof(decimal))]
    public decimal Quantity {
      get; private set;
    }


    [DataField("MILESTONE_ITEM_UNIT_PRICE", ConvertFrom = typeof(decimal))]
    public decimal UnitPrice {
      get; private set;
    }


    [DataField("MILESTONE_ITEM_EXT_DATA")]
    private JsonObject ExtData {
      get; set;
    }


    public string Keywords {
      get {
        return EmpiriaString.BuildKeywords(this.Status.GetName());
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


    [DataField("MILESTONE_ITEM_POSTED_BY_ID")]
    public Contact PostedBy {
      get; private set;
    }


    [DataField("MILESTONE_ITEM_POSTING_TIME")]
    public DateTime PostingTime {
      get; private set;
    }


    [DataField("MILESTONE_ITEM_STATUS", Default = EntityStatus.Pending)]
    public EntityStatus Status {
      get; private set;
    } = EntityStatus.Pending;

    #endregion Properties

  }  // class ContractMilestoneItem

}  // namespace Empiria.Contracts
