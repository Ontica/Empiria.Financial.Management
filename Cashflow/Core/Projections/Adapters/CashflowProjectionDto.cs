/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.CashFlow.Core.dll                  Pattern   : Output DTOs                             *
*  Type     : CashFlowProjectionDto                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTOs for cash flow projections.                                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Documents;
using Empiria.History;

namespace Empiria.CashFlow.Projections.Adapters {

  /// <summary>Output holder DTO used for a cash flow projection.</summary>
  public class CashFlowProjectionHolderDto {

    public CashFlowProjectionDto Projection {
      get; internal set;
    }

    //public FixedList<CashFlowProjectionEntryDescriptorDto> Entries {
    //  get; internal set;
    //}

    //public CashFlowProjectionEntriesByYearTableDto GroupedEntries {
    //  get; internal set;
    //}

    public FixedList<object> Entries {
      get;
    } = new FixedList<object>();


    public object GroupedEntries {
      get; internal set;
    } = new object();


    public FixedList<DocumentDto> Documents {
      get; internal set;
    }

    public FixedList<HistoryEntryDto> History {
      get; internal set;
    }

    public CashFlowProjectionActions Actions {
      get; internal set;
    }

  }  // class CashFlowProjectionHolderDto



  /// <summary>Action flags for cash flow projections.</summary>
  public class CashFlowProjectionActions : BaseActions {

    public bool CanAuthorize {
      get; internal set;
    }

    public bool CanClose {
      get; internal set;
    }

    public bool CanReject {
      get; internal set;
    }

    public bool CanSendToAuthorization {
      get; internal set;
    }

  }  // class CashFlowProjectionActions



  /// <summary>Output DTO used for cash flow projections.</summary>
  public class CashFlowProjectionDto {

    public string UID {
      get; internal set;
    }

    public NamedEntityDto Plan {
      get; internal set;
    }

    public NamedEntityDto Category {
      get; internal set;
    }

    public string ProjectionNo {
      get; internal set;
    }

    public NamedEntityDto BaseParty {
      get; internal set;
    }

    public NamedEntityDto BaseProject {
      get; internal set;
    }

    [Newtonsoft.Json.JsonProperty(PropertyName = "Classification")]
    public NamedEntityDto BaseProjectCategory {
      get; internal set;
    }

    public NamedEntityDto BaseAccount {
      get; internal set;
    }

    public NamedEntityDto OperationSource {
      get; internal set;
    }

    public string Description {
      get; internal set;
    }

    public string Justification {
      get; internal set;
    }

    public string[] Tags {
      get; internal set;
    }

    public DateTime ApplicationDate {
      get; internal set;
    }

    public NamedEntityDto AppliedBy {
      get; internal set;
    }

    public DateTime RecordingTime {
      get; internal set;
    }

    public NamedEntityDto RecordedBy {
      get; internal set;
    }

    public DateTime AuthorizationTime {
      get; internal set;
    }

    public NamedEntityDto AuthorizedBy {
      get; internal set;
    }

    public DateTime RequestedTime {
      get; internal set;
    }

    public NamedEntityDto RequestedBy {
      get; internal set;
    }

    public NamedEntityDto AdjustmentOf {
      get; internal set;
    }

    public decimal Total {
      get; internal set;
    }

    public NamedEntityDto Status {
      get; internal set;
    }

  }  // CashFlowProjectionDto



  /// <summary>Output DTO used to display cash flow projections in lists.</summary>
  public class CashFlowProjectionDescriptorDto {

    public string UID {
      get; internal set;
    }

    public string PlanName {
      get; internal set;
    }

    public string CategoryName {
      get; internal set;
    }

    public string ProjectionNo {
      get; internal set;
    }

    public string BaseProjectName {
      get; internal set;
    }

    [Newtonsoft.Json.JsonProperty(PropertyName = "ClassificationName")]
    public string BaseProjectCategoryName {
      get; internal set;
    }

    public string BaseAccountName {
      get; internal set;
    }

    public string BasePartyName {
      get; internal set;
    }

    public string OperationSourceName {
      get; internal set;
    }

    public DateTime AuthorizationTime {
      get; internal set;
    }

    public string AuthorizedByName {
      get; internal set;
    }

    public DateTime RecordingTime {
      get; internal set;
    }

    public string RecordedByName {
      get; internal set;
    }

    public decimal Total {
      get; internal set;
    }

    public string StatusName {
      get; internal set;
    }

  }  // CashFlowProjectionDescriptorDto

}  // namespace Empiria.CashFlow.Projections.Adapters
