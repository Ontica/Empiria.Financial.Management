/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.CashFlow.Core.dll                  Pattern   : Output DTO                              *
*  Type     : CashFlowProjectionEntryByYearDto           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO used to return cash flow projection's entries for a whole year.                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.CashFlow.Projections.Adapters {

  /// <summary>Output DTO used to return cash flow projection's entries for a whole year.</summary>
  public class CashFlowProjectionEntryByYearDto {

    public string UID {
      get; internal set;
    }

    public CashFlowProjectionEntryDtoType EntryType {
      get;
    } = CashFlowProjectionEntryDtoType.Annually;


    public string ProjectionUID {
      get; internal set;
    }

    public NamedEntityDto ProjectionColumn {
      get; internal set;
    }

    public NamedEntityDto CashFlowAccount {
      get; internal set;
    }

    public NamedEntityDto Product {
      get; internal set;
    }

    public string Description {
      get; internal set;
    }

    public NamedEntityDto ProductUnit {
      get; internal set;
    }

    public string Justification {
      get; internal set;
    }

    public int Year {
      get; internal set;
    }

    public NamedEntityDto Currency {
      get; internal set;
    }

    public FixedList<CashFlowProjectionMonthEntryDto> Amounts {
      get; internal set;
    }

  }  // CashFlowProjectionEntryByYearDto



  /// <summary>Output DTO used to return a cash flow projection month entry.</summary>
  public class CashFlowProjectionMonthEntryDto {

    public string EntryUID {
      get; internal set;
    }

    public int Month {
      get; internal set;
    }

    public decimal ProductQty {
      get; internal set;
    }

    public decimal Amount {
      get; internal set;
    }

  }  // class CashFlowProjectionMonthEntryDto

}  // namespace Empiria.CashFlow.Projections.Adapters
