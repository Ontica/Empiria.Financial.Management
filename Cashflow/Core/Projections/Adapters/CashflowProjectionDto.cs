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

namespace Empiria.Cashflow.Projections.Adapters {

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

    public DateTime RecordingTime {
      get; internal set;
    }

    public string RecordedByName {
      get; internal set;
    }

    public DateTime AuthorizationTime {
      get; internal set;
    }

    public string AuthorizedByName {
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
