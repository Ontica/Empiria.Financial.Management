/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cashflow Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Cashflow.Core.dll                  Pattern   : Output DTOs                             *
*  Type     : CashflowProjectionDto                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTOs for cashflow projections.                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Documents;
using Empiria.History;

namespace Empiria.Cashflow.Projections.Adapters {

  /// <summary>Output holder DTO used for a cashflow projection.</summary>
  public class CashflowProjectionHolderDto {

    public CashflowProjectionDto Projection {
      get; internal set;
    }

    //public FixedList<CashflowProjectionEntryDescriptorDto> Entries {
    //  get; internal set;
    //}

    //public CashflowProjectionEntriesByYearTableDto GroupedEntries {
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

    public CashflowProjectionActions Actions {
      get; internal set;
    }

  }  // class CashflowProjectionHolderDto



  /// <summary>Action flags for cashflow projections.</summary>
  public class CashflowProjectionActions : BaseActions {

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

  }  // class CashflowProjectionActions



  /// <summary>Output DTO used for cashflow projections.</summary>
  public class CashflowProjectionDto {

    public string UID {
      get; internal set;
    }

    public NamedEntityDto Plan {
      get; internal set;
    }

    public NamedEntityDto Category {
      get; internal set;
    }

    public NamedEntityDto Classification {
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

    public NamedEntityDto BaseAccount {
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

  }  // CashflowProjectionDto



  /// <summary>Output DTO used to display cashflow projections in lists.</summary>
  public class CashflowProjectionDescriptorDto {

    public string UID {
      get; internal set;
    }

    public string PlanName {
      get; internal set;
    }

    public string CategoryName {
      get; internal set;
    }

    public string ClassificationName {
      get; internal set;
    }

    public string ProjectionNo {
      get; internal set;
    }

    public string BasePartyName {
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

  }  // CashflowProjectionDescriptorDto

}  // namespace Empiria.Cashflow.Projections.Adapters
