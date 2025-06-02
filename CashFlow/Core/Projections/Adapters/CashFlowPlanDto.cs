/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.CashFlow.Core.dll                  Pattern   : Output DTO                              *
*  Type     : CashFlowPlanDto                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO for cash flow plan information                                                      *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.CashFlow.Projections.Adapters {

  /// <summary>Output DTO for cash flow plan information</summary>
  public class CashFlowPlanDto {

    public string UID {
      get; internal set;
    }

    public string Name {
      get; internal set;
    }

    public NamedEntityDto BaseCurrency {
      get; internal set;
    }

    public FixedList<NamedEntityDto> Categories {
      get; internal set;
    }

    public FixedList<NamedEntityDto> ProjectionsColumns {
      get; internal set;
    }

    public FixedList<int> Years {
      get; internal set;
    }

    public bool EditionAllowed {
      get; internal set;
    }

  }  // class CashFlowPlanDto

}  // namespace Empiria.CashFlow.Projections.Adapters
