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

    public FixedList<CashFlowProjectionEntryDescriptor> Entries {
      get; internal set;
    }

    //public CashFlowProjectionEntriesByYearTableDto GroupedEntries {
    //  get; internal set;
    //}

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

    [Newtonsoft.Json.JsonProperty(PropertyName = "ProjectionType")]
    public NamedEntityDto ProjectionTypeCategory {
      get; internal set;
    }

    public string ProjectionNo {
      get; internal set;
    }

    public NamedEntityDto Party {
      get; internal set;
    }

    public NamedEntityDto Project {
      get; internal set;
    }

    [Newtonsoft.Json.JsonProperty(PropertyName = "ProjectType")]
    public NamedEntityDto ProjectTypeCategory {
      get; internal set;
    }

    public NamedEntityDto Account {
      get; internal set;
    }

    public NamedEntityDto Source {
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

    [Newtonsoft.Json.JsonProperty(PropertyName = "ProjectionTypeName")]
    public string ProjectionTypeCategoryName {
      get; internal set;
    }

    public string ProjectionNo {
      get; internal set;
    }

    public string ProjectName {
      get; internal set;
    }

    [Newtonsoft.Json.JsonProperty(PropertyName = "ProjectTypeName")]
    public string ProjectTypeCategoryName {
      get; internal set;
    }

    public string AccountName {
      get; internal set;
    }

    public string PartyName {
      get; internal set;
    }

    public string SourceName {
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
